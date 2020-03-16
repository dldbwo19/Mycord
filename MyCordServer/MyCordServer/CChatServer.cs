using System;
using System.Collections.Generic;
using System.Text;
using MyCordLib;
using System.Threading;

namespace MyCordServer
{
    internal class CChatServer
    {
        private List<string> UserList;

        private object operation_lock;
        private Queue<CPacket> user_operations_queue;

        //서버는 하나의 스레드로 순차 처리한다.
        private Thread thread;
        private AutoResetEvent loop_event;

        public CChatRoomManager room_manager { get; private set; }

        public CChatServer()
        {
            this.UserList = new List<string>();
            this.operation_lock = new object();
            this.loop_event = new AutoResetEvent(false);
            this.user_operations_queue = new Queue<CPacket>();

            this.room_manager = new CChatRoomManager();

            this.thread = new Thread(server_loop);
            this.thread.Start();
        }

        private void server_loop()
        {
            while (true)
            {
                CPacket message = null;
                lock (this.operation_lock)
                {
                    if (this.user_operations_queue.Count > 0)
                    {
                        message = this.user_operations_queue.Dequeue();
                    }
                }

                if (message != null)
                {
                    process_receive(message);
                }

                if (this.user_operations_queue.Count <= 0)
                {
                    this.loop_event.WaitOne();
                }
            }
        }

        public void enqueue_packet(CPacket message, CChatUser user)
        {
            lock (this.operation_lock)
            {
                this.user_operations_queue.Enqueue(message);
                this.loop_event.Set();
            }
        }

        private void process_receive(CPacket message)
        {
            message.owner.process_user_operation(message);
        }

        //--------------------------------------------------------------------------

        public bool set_name(string userName)
        {
            if (UserList.Contains(userName))
            {
                return false;
            }

            UserList.Add(userName);
            return true;
        }

        public bool create_chat_room(string roomName, string initial, CChatUser user)
        {
            return room_manager.create_chat_room(roomName, initial, user);
        }

        //
        public bool enter_chat_room(string roomName, CChatUser user)
        {
            return room_manager.enter_the_room(roomName, user);
        }

        public CChatRoom get_chat_room(string roomName)
        {
            return room_manager.get_chat_room(roomName);
        }

        public void remove_name(string userName)
        {
            if (!UserList.Contains(userName))
            {
                return;
            }
            UserList.Remove(userName);
        }

        public int exit_chat_room(string roomName, CChatUser user)
        {
            return room_manager.exit_chat_room(roomName, user);
        }
    }
}
