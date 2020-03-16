using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace MyCordLib
{
    /// <summary>
    /// 전반적인 네트워크 통신을 위해 기반이 되는 코드가 포함되는 클래스이며,
    /// 서버 네트워크 모듈의 출발점이다.
    /// </summary>
    public class CNetworkService
    {
        private CListener client_listener;

        private SocketAsyncEventArgsPool receive_event_args_pool;
        private SocketAsyncEventArgsPool send_event_args_pool;

        private BufferManager buffer_manager;

        public delegate void SessionHandler(CUserToken token);
        public SessionHandler session_created_callback { get; set; }

        private int connected_count;
        private int max_connections;
        private int buffer_size;
        private const int BUFFER_COUNT = 2;

        public CNetworkService()
        {
            this.connected_count = 0;
            this.session_created_callback = null;
        }

        public void initialize()
        {
            this.max_connections = 10000;
            this.buffer_size = 1024;

            this.buffer_manager = new BufferManager(this.max_connections * BUFFER_COUNT * buffer_size, buffer_size);
            this.receive_event_args_pool = new SocketAsyncEventArgsPool(this.max_connections);
            this.send_event_args_pool = new SocketAsyncEventArgsPool(this.max_connections);

            this.buffer_manager.initBuffer();

            SocketAsyncEventArgs args;

            // 두 가지 방식을 사용할 수 있다.
            // 첫 번째는 아래와 같이 미리 최대 연결에 대한 토큰과 버퍼를 확보하고 사용하는 방법
            // 두 번째는 연결이 올 때 토큰과 버퍼를 확보하고 로그아웃 시 버퍼를 해제하여 사용하는 방법
            for (int i = 0; i < this.max_connections; i++)
            {
                CUserToken token = new CUserToken();

                // receive pool
                {
                    args = new SocketAsyncEventArgs();
                    args.Completed += new EventHandler<SocketAsyncEventArgs>(receive_completed);
                    args.UserToken = token;

                    this.buffer_manager.SetBuffer(args);

                    this.receive_event_args_pool.Push(args);
                }

                // send pool
                {
                    args = new SocketAsyncEventArgs();
                    args.Completed += new EventHandler<SocketAsyncEventArgs>(send_completed);
                    args.UserToken = token;

                    this.buffer_manager.SetBuffer(args);

                    this.send_event_args_pool.Push(args);
                }
            }
        }

        /// <summary>
        /// CListener 생성을 위한 메서드
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="backlog"></param>
        public void listen(string host, int port, int backlog)
        {
            this.client_listener = new CListener();
            this.client_listener.callback_on_new_client += on_new_client_connected;
            this.client_listener.start(host, port, backlog);
        }

        /// <summary>
        /// 클라이언트에서 서버와의 통신을 위한 풀 할당
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="token"></param>
        public void on_new_server_connected(Socket socket, CUserToken token)
        {
            // SocketAsyncEventArgsPool에서 빼오지 않고 그때 그때 할당해서 사용한다.
            // 풀은 서버에서 클라이언트와의 통신용으로만 쓰려고 만든것이기 때문이다.
            // 클라이언트 입장에서 서버와 통신을 할 때는 접속한 서버당 두개의 EventArgs만 있으면 되기 때문에 그냥 new해서 쓴다.
            // 서버간 연결에서도 마찬가지이다.
            // 풀링처리를 하려면 c->s로 가는 별도의 풀을 만들어서 써야 한다.
            SocketAsyncEventArgs receive_event_arg = new SocketAsyncEventArgs();
            receive_event_arg.Completed += new EventHandler<SocketAsyncEventArgs>(receive_completed);
            receive_event_arg.UserToken = token;
            receive_event_arg.SetBuffer(new byte[1024], 0, 1024);

            SocketAsyncEventArgs send_event_arg = new SocketAsyncEventArgs();
            send_event_arg.Completed += new EventHandler<SocketAsyncEventArgs>(send_completed);
            send_event_arg.UserToken = token;
            send_event_arg.SetBuffer(new byte[1024], 0, 1024);

            begin_receive(socket, receive_event_arg, send_event_arg);
        }

        /// <summary>
        /// CListener로부터 새로운 클라이언트 접속이 성공적으로 완료된 경우 호출된다.
        /// </summary>
        /// <param name="client_socket"></param>
        /// <param name="token"></param>
        private void on_new_client_connected(Socket client_socket, object token)
        {
            Interlocked.Increment(ref this.connected_count);

            Console.WriteLine(string.Format($"[{Thread.CurrentThread.ManagedThreadId}] A client connected. handle {client_socket.Handle}, count {this.connected_count}"));

            SocketAsyncEventArgs receive_args = this.receive_event_args_pool.Pop();
            SocketAsyncEventArgs send_args = this.send_event_args_pool.Pop();

            CUserToken user_token = null;

            if (this.session_created_callback != null)
            {
                user_token = receive_args.UserToken as CUserToken;
                this.session_created_callback(user_token);
            }

            begin_receive(client_socket, receive_args, send_args);
        }

        private void begin_receive(Socket socket, SocketAsyncEventArgs receive_args, SocketAsyncEventArgs send_args)
        {
            CUserToken token = receive_args.UserToken as CUserToken;
            token.set_event_args(receive_args, send_args);

            token.socket = socket;

            bool pending = socket.ReceiveAsync(receive_args);
            if (!pending)
            {
                process_receive(receive_args);
            }
        }

        private void receive_completed(object sender, SocketAsyncEventArgs args)
        {
            if(args.LastOperation == SocketAsyncOperation.Receive)
            {
                process_receive(args);
                return;
            }

            Console.WriteLine("The last operation completed on the socket was not a receive");
        }

        private void send_completed(object sender, SocketAsyncEventArgs args)
        {
            CUserToken token = args.UserToken as CUserToken;
            token.process_send(args);
        }

        private void process_receive(SocketAsyncEventArgs args)
        {
            CUserToken token = args.UserToken as CUserToken;
            if(args.BytesTransferred > 0 && args.SocketError == SocketError.Success)
            {
                token.on_receive(args.Buffer, args.Offset, args.BytesTransferred);

                bool pending = token.socket.ReceiveAsync(args);
                if (!pending)
                {
                    process_receive(args);
                }
            }
            else
            {
                Console.WriteLine($"error {args.SocketError}, transferred {args.BytesTransferred}");
                close_clientsocket(token);
            }
        }

        public void close_clientsocket(CUserToken token)
        {
            token.on_removed();

            // Free the SocketAsyncEventArg so they can be reused by another client
            // 버퍼는 반환할 필요가 없다. SocketAsyncEventArg가 버퍼를 물고 있기 때문에
            // 이것을 재사용 할 때 물고 있는 버퍼를 그대로 사용하면 되기 때문이다.
            if (this.receive_event_args_pool != null)
            {
                this.receive_event_args_pool.Push(token.receive_event_args);
            }

            if (this.send_event_args_pool != null)
            {
                this.send_event_args_pool.Push(token.send_event_args);
            }
        }
    }
}
