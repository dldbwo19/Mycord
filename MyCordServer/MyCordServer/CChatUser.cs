using System;
using System.Collections.Generic;
using System.Text;
using MyCordLib;

namespace MyCordServer
{
    public class CChatUser : IPeer
    {
        private CUserToken token;
        public string UserName { get; private set; } = "UNKNOWN";
        public Dictionary<string, CChatRoom> RoomList { get; set; }

        public CChatUser(CUserToken token)
        {
            this.RoomList = new Dictionary<string, CChatRoom>();
            this.token = token;
            this.token.set_peer(this);
        }

        public void on_message(Const<byte[]> buffer)
        {
            byte[] clone = new byte[1024];
            Array.Copy(buffer.Value, clone, buffer.Value.Length);
            CPacket message = new CPacket(clone, this);
            Program.server_main.enqueue_packet(message, this);
        }

        public void on_removed()
        {
            Console.WriteLine("The client disconnected.");
            Program.server_main.remove_name(this.UserName);

            if (RoomList.Count > 0)
            {
                int count;
                foreach (var item in RoomList)
                {
                    count = Program.server_main.exit_chat_room(item.Key, this);
                    if (count > 0)
                    {
                        CPacket packet = CPacket.create((short)PROTOCOL.EXIT_THE_ROOM_USER);
                        

                        packet.push(item.Key);
                        packet.push(this.UserName);
                        RoomList[item.Key].broadcast(packet);
                    }
                }
                RoomList = null;
            }

            Program.remove_user(this);
        }

        public void process_user_operation(CPacket message)
        {
            PROTOCOL protocol = (PROTOCOL)message.pop_protocol_id();
            Console.WriteLine($"protocol id {protocol}");
            CPacket response = null;
            switch (protocol)
            {
                //수정 완료
                case PROTOCOL.SET_NAME_REQ:
                    {
                        string name = message.pop_string();

                        if (!Program.server_main.set_name(name))
                        {
                            Console.WriteLine($"{name} failed set nickname.");

                            response = CPacket.create((short)PROTOCOL.SET_NAME_ERROR_EXISTED);
                            send(response);

                            break;
                        }

                        Console.WriteLine($"{name} succeeded set nickname.");

                        this.UserName = name;

                        response = CPacket.create((short)PROTOCOL.SET_NAME_ACK);
                        send(response);
                    }
                    break;

                //수정 완료
                case PROTOCOL.CHAT_MSG_REQ:
                    {
                        string RoomName = message.pop_string();
                        string Message = message.pop_string();
                        Console.WriteLine($"text {Message}");

                        if (!RoomList.ContainsKey(RoomName))
                        {
                            Console.WriteLine($"User dont have the Room");
                            return;
                        }

                        response = CPacket.create((short)PROTOCOL.CHAT_MSG_ACK);

                        response.push(RoomName);
                        response.push(this.UserName);
                        response.push(Message);

                        RoomList[RoomName].broadcast(response);
                    }
                    break;

                //수정 완료
                case PROTOCOL.CREATE_CHAT_ROOM_REQ:
                    {

                        Console.WriteLine($"{this.UserName} tries to create room");


                        string RoomName = message.pop_string();
                        string Initial = message.pop_string();

                        if (!Program.server_main.create_chat_room(RoomName, Initial, this))
                        {
                            Console.WriteLine($"{this.UserName} failed room creation");

                            response = CPacket.create((short)PROTOCOL.CREATE_CHAT_ROOM_ERROR);

                            send(response);

                            break;
                        }
                        this.RoomList.Add(RoomName, Program.server_main.get_chat_room(RoomName));

                        Console.WriteLine($"{this.UserName} succeeded room creation");

                        response = CPacket.create((short)PROTOCOL.CREATE_CHAT_ROOM_ACK);

                        response.push_int16((short)(Program.server_main.get_chat_room(RoomName).get_talker_list().Count));

                        foreach (var item in Program.server_main.get_chat_room(RoomName).get_talker_list())
                        {
                            response.push(item.UserName);
                        }

                        response.push(RoomName);
                        response.push(Initial);

                        send(response);


                    }
                    break;

                //수정완료
                case PROTOCOL.EXIT_THE_ROOM_REQ:
                    {
                        string RoomName = message.pop_string();

                        int count = Program.server_main.exit_chat_room(RoomName, this);

                        if (count > 0)
                        {
                            response = CPacket.create((short)PROTOCOL.EXIT_THE_ROOM_USER);
                            response.push(RoomName);
                            response.push(this.UserName);
                            RoomList[RoomName].broadcast(response);
                            RoomList.Remove(RoomName);
                        }
                        else if (count != -10 && count != -11)
                        {
                            RoomList.Remove(RoomName);
                        }
                        else
                        {
                            /*
                            response = CPacket.create((short)PROTOCOL.EXIT_THE_ROOM_ERROR);
                            send(response);
                            */
                        }
                    }
                    break;

                //수정 필요
                case PROTOCOL.JOIN_THE_ROOM_REQ:
                    {
                        string RoomName = message.pop_string();

                        if (RoomList.ContainsKey(RoomName))
                        {
                            return;
                        }
                        CChatRoom ChatRoom = Program.server_main.get_chat_room(RoomName);

                        if (ChatRoom == null)
                        {
                            response = CPacket.create((short)PROTOCOL.JOIN_THE_ROOM_ERROR);
                            send(response);
                        }
                        else if (Program.server_main.enter_chat_room(RoomName, this))
                        {
                            response = CPacket.create((short)PROTOCOL.JOIN_THE_ROOM_NEW_USER);
                            response.push(RoomName);
                            response.push(this.UserName);
                            ChatRoom.broadcast_Except_Me(response, this);

                            CPacket response2 = CPacket.create((short)PROTOCOL.JOIN_THE_ROOM_ACK);
                            response2.push_int16((short)(Program.server_main.get_chat_room(RoomName).get_talker_list().Count));
                            foreach (var item in Program.server_main.get_chat_room(RoomName).get_talker_list())
                            {
                                response2.push(item.UserName);
                            }
                            response2.push(RoomName);
                            response2.push(ChatRoom.Initial);
                            send(response2);

                            this.RoomList.Add(RoomName, ChatRoom);
                        }
                        else
                        {

                        }
                    }
                    break;

                //수정 필요
                case PROTOCOL.ADD_USER_IN_ROOM_REQ:
                    {
                        /*
                        string target_name = message.pop_string();

                        Console.WriteLine($"{UserName} try to invite {target_name} at the room");
                        CChatUser target_user = Program.server_main.get_waiting_user(target_name);
                        if (target_user == null)
                        {
                            Console.WriteLine("user is null");
                            response = CPacket.create((short)PROTOCOL.ADD_USER_IN_ROOM_ERROR);
                            break;
                        }

                        response = CPacket.create((short)PROTOCOL.ADD_USER_IN_ROOM_ACK);
                        response.push(this.chatroom.get_owner());
                        response.push(this.UserName);
                        target_user.send(response);
                        */
                    }
                    break;

                //수정 필요
                case PROTOCOL.ASK_USER_LIST_REQ:
                    {
                        /*
                        response = CPacket.create((short)PROTOCOL.ASK_USER_LIST_ACK);

                        if (Program.server_main.get_waiting_list() == null)
                        {

                        }
                        else
                        {
                            response.push_int16((short)((Program.server_main.get_waiting_list().Count) - 1));
                            foreach (var item in Program.server_main.get_waiting_list())
                            {
                                if (item.Key != this.UserName)
                                {
                                    response.push(item.Key);
                                }
                            }
                        }
                        this.send(response);
                        */
                    }
                    break;
            }
        }

        public void send(CPacket msg)
        {
            this.token.send(msg);
        }

        public void disconnect()
        {
            this.token.socket.Disconnect(false);
        }
    }
}
