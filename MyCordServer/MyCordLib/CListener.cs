
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace MyCordLib
{
    /// <summary>
    /// 클라이언트의 접속을 받아들이기 위한 클래스이며,
    /// IP주소와 포트 그리고 백로그(보류 중인 연결 큐의 최대 길이)를 start()의 인자로 받아서
    /// 소켓을 개설하고 비동기 접속 메서드를 통해 접속을 받아들인다.
    /// </summary>
    internal class CListener
    {
        // SocketAsyncEventArgs 클래스
        // 닷넷에서 비동기 소켓에서 사용하는 개념으로 비동기 소켓 메서드를 호출할 때 마다 반드시 필요한 객체
        // Object -> EvnetArgs -> SocketAsyncEventArgs
        private SocketAsyncEventArgs accept_args;

        private Socket listen_socket;

        private AutoResetEvent flow_control_event;

        public delegate void NewClientHandler(Socket client_socket, object token);
        public NewClientHandler callback_on_new_client;

        public CListener()
        {
            this.callback_on_new_client = null;
        }

        public void start(string host, int port, int backlog)
        {
            this.listen_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            
            IPAddress address;
            if(host == "0.0.0.0")
            {
                address = IPAddress.Any;
            }

            else
            {
                address = IPAddress.Parse(host);
            }
            
            IPEndPoint endpoint = new IPEndPoint(address, port);

            try
            {
                // 만들어둔 tcp 소켓에 인자로 받은 ip주소와 포트를 가지는 종단점 설정한다.
                this.listen_socket.Bind(endpoint);
                // 소켓을 수신 상태로 둔다.
                this.listen_socket.Listen(backlog);

                this.accept_args = new SocketAsyncEventArgs();
                // 받기 완료 시 이벤트
                this.accept_args.Completed += new EventHandler<SocketAsyncEventArgs>(on_accept_completed);

                // 별도의 스레드를 생성하여 accept를 처리한다.
                // 메인 스레드가 어떠한 이유로 블로킹이 되어있을 경우 accept가 처리가 되지 않는 버그를 회피할 수 있다.
                Thread listen_thread = new Thread(do_listen);
                listen_thread.Start();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void do_listen()
        {
            // AutoResetEvent 클래스(자동) <=> ManualResetEvent 클래스(수동)
            // 비동기 스레드를 파이프라인에 흐르는 물처럼 순차적으로 실행시킬 수 있게 해주는 클래스이다.
            // Set() => 신호 상태로 바꾸어 실행할 수 있는 상태로 전환
            // WaitOne() => 신호 상태가 되길 기다리는 대기 상태
            // Reset() => 신호 상태에서 비신호 상태로 직접 전환. AutoResetEvent는 자동으로 한 개의 스레드가 접근하면
            // 비신호 상태로 만들어 준다.
            // 사용되는 클래스로써 인자값이 true면 쓰레드를 실행시킬 수 있는 상태, false면 실행시킬 수 없는 상태이다.
            this.flow_control_event = new AutoResetEvent(false);

            while (true)
            {
                this.accept_args.AcceptSocket = null;

                bool pending = true;

                try
                {
                    // 비동기 accept를 호출하여 클라이언트의 접속을 받아들인다.
                    // 비동기 메소드이지만 동기적(= 즉시완료)으로 수행이 완료될 경우도 있으니
                    // 리턴값을 확인하여야 한다.
                    pending = this.listen_socket.AcceptAsync(this.accept_args);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }

                // 동기적으로 실행 시 false 값이 반환되는데
                // 이벤트가 발생하지 않으므로 콜백 메서드를 직접 호출해야 한다.
                if (!pending)
                {
                    on_accept_completed(null, this.accept_args);
                }

                this.flow_control_event.WaitOne();
            }
        }

        private void on_accept_completed(object sender, SocketAsyncEventArgs e)
        {
            if(e.SocketError == SocketError.Success)
            {
                Socket client_socket = e.AcceptSocket;

                this.flow_control_event.Set();


                if (this.callback_on_new_client != null)
                {
                    this.callback_on_new_client(client_socket, e.UserToken);
                }

                return;
            }
            else
            {
                Console.WriteLine("Failed to accept client");
            }

            this.flow_control_event.Set();
        }
    }
}
