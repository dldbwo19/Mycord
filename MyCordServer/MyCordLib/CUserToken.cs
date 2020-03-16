using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace MyCordLib
{
    public class CUserToken
    {
        public Socket socket { get; set; }

        public SocketAsyncEventArgs receive_event_args { get; private set; }
        public SocketAsyncEventArgs send_event_args { get; private set; }

        // 바이트형식의 메세지를 서버에 맞는 처리 형식으로 변환해주는 변환기
        private CMessageConverter message_converter;

        IPeer peer;

        // 전송할 패킷을 보관해놓는 큐.
        private Queue<CPacket> sending_queue;
        // sending_queue lock 처리에 사용되는 객체
        private object cs_sending_queue;

        public CUserToken()
        {
            this.cs_sending_queue = new object();

            this.message_converter = new CMessageConverter();
            this.peer = null;
            this.sending_queue = new Queue<CPacket>();
        }

        public void set_peer(IPeer peer)
        {
            this.peer = peer;
        }

        public void set_event_args(SocketAsyncEventArgs recevie_event_args, SocketAsyncEventArgs send_event_args)
        {
            this.receive_event_args = receive_event_args;
            this.send_event_args = send_event_args;
        }

        public void on_receive(byte[] buffer, int offset, int transfered)
        {
            this.message_converter.on_receive(buffer, offset, transfered, on_message);
        }

        private void on_message(Const<byte[]> buffer)
        {
            if (this.peer != null)
            {
                this.peer.on_message(buffer);
            }
        }

        public void on_removed()
        {
            this.sending_queue.Clear();

            if(this.peer != null)
            {
                this.peer.on_removed();
            }
        }

        public void send(CPacket message)
        {
            CPacket clone = new CPacket();
            message.copy_to(clone);

            lock (this.cs_sending_queue)
            {
                if(this.sending_queue.Count <= 0)
                {
                    this.sending_queue.Enqueue(clone);
                    start_send();
                    return;
                }

                Console.WriteLine($"Queue is not empty. Copy and Enqueue a message. protocol id : {message.protocol_id}");
                this.sending_queue.Enqueue(clone);
            }
        }

        private void start_send()
        {
            lock (this.cs_sending_queue)
            {
                CPacket message = this.sending_queue.Peek();

                message.record_size();

                this.send_event_args.SetBuffer(this.send_event_args.Offset, message.position);

                Array.Copy(message.buffer, 0, this.send_event_args.Buffer, this.send_event_args.Offset, message.position);

                bool pending = this.socket.SendAsync(this.send_event_args);
                if (!pending)
                {
                    process_send(this.send_event_args);
                }
            }
        }

        static int sent_count = 0;
        static object cs_count = new object();

        public void process_send(SocketAsyncEventArgs args)
        {
            if(args.BytesTransferred <= 0 || args.SocketError != SocketError.Success)
            {
                return;
            }

            lock (this.cs_sending_queue)
            {
                if(this.sending_queue.Count <= 0)
                {
                    Console.WriteLine("Sending queue count is less than zero");
                }

                int size = this.sending_queue.Peek().position;
                if(args.BytesTransferred != size)
                {
                    Console.WriteLine($"Need to send more! transferred {args.BytesTransferred}, packet size {size}");
                    return;
                }
                
                lock (cs_count)
                {
                    ++sent_count;
                    Console.WriteLine($"process send : {args.SocketError}, transferred {args.BytesTransferred}, sent count {sent_count}");
                }

                this.sending_queue.Dequeue();

                if(this.sending_queue.Count > 0)
                {
                    start_send();
                }
            }
        }

        public void disconnect()
        {
            // close the socket associated with the client
            try
            {
                this.socket.Shutdown(SocketShutdown.Send);
            }
            // throws if client process has already closed
            catch (Exception) { }
            this.socket.Close();
        }

        public void start_keepalive()
        {
            System.Threading.Timer keepalive = new System.Threading.Timer((object e) =>
            {
                CPacket msg = CPacket.create(0);
                msg.push(0);
                send(msg);
            }, null, 0, 3000);
        }
    }
}
