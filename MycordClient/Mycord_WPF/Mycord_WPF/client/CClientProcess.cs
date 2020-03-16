using MyCordLib;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Mycord_WPF
{
    public class CClientProcess
    {
        private List<string> waiting_users;

        private object operation_lock;
        private Queue<CPacket> operations_queue;

        private Thread thread;
        private AutoResetEvent loop_event;

        private bool loop = true;

        public CClientProcess()
        {
            this.waiting_users = new List<string>();
            this.operation_lock = new object();
            this.loop_event = new AutoResetEvent(false);
            this.operations_queue = new Queue<CPacket>();

            this.thread = new Thread(client_loop);
            this.thread.IsBackground = true;
            this.thread.Start();
        }

        private void client_loop()
        {
            while (loop)
            {
                CPacket message = null;
                lock (this.operation_lock)
                {
                    if (this.operations_queue.Count > 0)
                    {
                        message = this.operations_queue.Dequeue();
                    }
                }

                if (message != null)
                {
                    process_receive(message);
                }

                if (this.operations_queue.Count <= 0)
                {
                    this.loop_event.WaitOne();
                }
            }
        }

        public void close_loop()
        {
            this.thread.Join();
            loop = false;

        }

        public void enqueue_packet(CPacket message, CServer server)
        {
            lock (this.operation_lock)
            {
                this.operations_queue.Enqueue(message);
                this.loop_event.Set();
            }
        }

        private void process_receive(CPacket message)
        {
            message.owner.process_user_operation(message);
        }
    }
}
