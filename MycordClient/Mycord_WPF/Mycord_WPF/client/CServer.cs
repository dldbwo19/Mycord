using MyCordLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mycord_WPF
{
    public class CServer : IPeer
    {
        public CUserToken token { get; private set; }

        public delegate void return_Callback_Handler(short protocolId);
        public return_Callback_Handler LoginPage_Return_Callback { get; set; }

        public delegate void return_Callback_Handler2(short protocolId , List<string> UserList, string serverName, string serverInitial);
        public return_Callback_Handler2 Create_Page_Callback { get; set; }

        public delegate void return_Callback_Handler3(EChatItem item, object obj);
        public Dictionary<string, return_Callback_Handler3> ChatPage_Return_Callback;

        public CServer(CUserToken token)
        {
            this.ChatPage_Return_Callback = new Dictionary<string, return_Callback_Handler3>();
            this.token = token;
            this.token.set_peer(this);
        }

        public void on_message(Const<byte[]> buffer)
        {
            byte[] clone = new byte[1024];
            Array.Copy(buffer.Value, clone, buffer.Value.Length);
            CPacket message = new CPacket(clone, this);
            CClient.client_main.enqueue_packet(message, this);
        }

        //  서버가 일방적으로 종료를 통보
        public void on_removed()
        {
            Console.WriteLine("Server removed.");
        }

        public void process_user_operation(CPacket message)
        {
            EProtocol protocol_id = (EProtocol)message.pop_protocol_id();

            switch (protocol_id)
            {
                #region receive from server

                case EProtocol.CHAT_MSG_ACK:
                    {
                        string RoomName = message.pop_string();
                        string UserName = message.pop_string();
                        string Message = message.pop_string();

                        if (!ChatPage_Return_Callback.ContainsKey(RoomName)) 
                        {
                            break;
                        }

                        ChatPage_Return_Callback[RoomName](EChatItem.Chat, new ChatItem { Name = UserName, Message = Message, SendTime=DateTime.UtcNow});
                    }
                    break;
                case EProtocol.SET_NAME_ACK:
                    {
                        if (LoginPage_Return_Callback == null) 
                        {
                            break;
                        }
                        LoginPage_Return_Callback((short)protocol_id);   
                    }
                    break;
                case EProtocol.SET_NAME_ERROR_EXISTED:
                    {
                        if (LoginPage_Return_Callback == null)
                        {
                            break;
                        }
                        LoginPage_Return_Callback((short)protocol_id);
                    }
                    break;
                case EProtocol.ASK_USER_LIST_ACK:
                    {

                    }
                    break;
                case EProtocol.CREATE_CHAT_ROOM_ACK:
                    {
                        if(Create_Page_Callback == null)
                        {
                            break;
                        }
                        var UserList = message.pop_string_list();
                        var ServerName = message.pop_string();
                        var ServerInitial = message.pop_string();
                        Create_Page_Callback((short)protocol_id, UserList, ServerName, ServerInitial);
                    }
                    break;

                case EProtocol.CREATE_CHAT_ROOM_ERROR:
                    {
                        if (Create_Page_Callback == null)
                        {
                            break;
                        }
                        Create_Page_Callback((short)protocol_id,null,null,null);
                    }
                    break;
                case EProtocol.EXIT_THE_ROOM_USER:
                    {
                        string RoomName = message.pop_string();
                        string UserName = message.pop_string();

                        if (!ChatPage_Return_Callback.ContainsKey(RoomName))
                        {
                            break;
                        }

                        ChatPage_Return_Callback[RoomName](EChatItem.UserExit, UserName);
                    }
                    break;

                case EProtocol.ADD_USER_IN_ROOM_ACK:
                    {
                        
                    }
                    break;
                case EProtocol.JOIN_THE_ROOM_ACK:
                    {
                        if (Create_Page_Callback == null)
                        {
                            break;
                        }
                        var UserList = message.pop_string_list();
                        var ServerName = message.pop_string();
                        var ServerInitial = message.pop_string();
                        Create_Page_Callback((short)protocol_id, UserList, ServerName, ServerInitial);
                    }
                    break;
                case EProtocol.JOIN_THE_ROOM_ERROR:
                    {
                        if (Create_Page_Callback == null)
                        {
                            break;
                        }
                        Create_Page_Callback((short)protocol_id, null, null, null);
                    }
                    break;
                case EProtocol.JOIN_THE_ROOM_NEW_USER:
                    {
                        string RoomName = message.pop_string();
                        string UserName = message.pop_string();

                        if (!ChatPage_Return_Callback.ContainsKey(RoomName))
                        {
                            break;
                        }

                        ChatPage_Return_Callback[RoomName](EChatItem.User, new User { Name = UserName });
                    }
                    break;


                    #endregion

            }
        }

        public void send(CPacket message)
        {
            this.token.send(message);
        }

        public void disconnect()
        {
            this.token.socket.Disconnect(false);
        }
    }
}

