using MyCordLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyCordServer
{
    public class CChatRoom
    {
        public string Initial { get; private set; }

        private List<CChatUser> talker_list;

        public CChatRoom(string initial, CChatUser user)
        {
            talker_list = new List<CChatUser> { user };
            Initial = initial;
        }

        public List<CChatUser> get_talker_list()
        {
            return talker_list;
        }

        public bool enter_chat_room(CChatUser user)
        {
            if (talker_list.Contains(user))
            {
                this.talker_list.Remove(user);
            }

            this.talker_list.Add(user);
            return true;
        }

        public void broadcast(CPacket message)
        {
            this.talker_list.ForEach(talker => talker.send(message));
        }

        public void broadcast_Except_Me(CPacket message, CChatUser user)
        {
            foreach(var item in talker_list)
            {
                if(item != user)
                {
                    item.send(message);
                }
            }
        }

        public int exit_chat_room(CChatUser user)
        {
            if (!talker_list.Contains(user))
            {
                return -10;
            }
            talker_list.Remove(user);

            return talker_list.Count;
        }
        /*
        

        public void exit_chat_room(CChatUser user)
        {
            if (!talker_list.Contains(user))
            {
                return;
            }

            if (talker_list.IndexOf(user) == 0)
            {
                Program.server_main.room_manager.destroy_room(user);
            }
            else
            {
                exit_general_user(user);
            }
        }

        private void exit_general_user(CChatUser user)
        {
            this.talker_list.Remove(user);
            Program.server_main.add_waiting_list(user.UserName, user);

            CPacket message = CPacket.create((short)PROTOCOL.EXIT_THE_ROOM_USER);
            message.push(user.UserName);
            broadcast(message);

            return;
        }

        public void destory_chat_room()
        {
            CPacket message = CPacket.create((short)PROTOCOL.DESTROYED_CHAT_ROOM);
            broadcast(message);
            talker_list.Clear();
        }
        */
    }
}
