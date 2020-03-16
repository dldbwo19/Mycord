using System;
using System.Collections.Generic;
using System.Text;

namespace MyCordServer
{
    public class CChatRoomManager
    {
        private Dictionary<string, CChatRoom> roomlist;

        public CChatRoomManager()
        {
            roomlist = new Dictionary<string, CChatRoom>(100);
        }

        public bool create_chat_room(string roomName, string initial, CChatUser user)
        {
            if (roomlist.Count > 99 || roomlist.ContainsKey(roomName))
            {
                return false;
            }

            CChatRoom chatRoom = new CChatRoom(initial, user);
            roomlist.Add(roomName, chatRoom);

            return true;
        }

        public bool enter_the_room(string RoomName, CChatUser user)
        {
            if (!roomlist.ContainsKey(RoomName))
            {     
                return false;
            }

            return this.roomlist[RoomName].enter_chat_room(user);
        }

        public CChatRoom get_chat_room(string roomName)
        {
            return roomlist.ContainsKey(roomName) ? roomlist[roomName] : null;
        }

        public int exit_chat_room(string roomName, CChatUser user)
        {
            if (!roomlist.ContainsKey(roomName))
            {
                return -11;
            }
            int count = roomlist[roomName].exit_chat_room(user);

            if (count <= 0 && count != -10)
            {
                remove_room(roomName);
            }

            return count;
        }

        public void remove_room(string roomName)
        {
            roomlist.Remove(roomName);
        }

        /*
        
        public bool create_chat_room(CChatUser user, out CChatRoom room)
        {
            if (roomlist.Count < 100 && !roomlist.ContainsKey(user.UserName))
            {
                CChatRoom chat_room = new CChatRoom(user);
                roomlist.Add(user.UserName, chat_room);
                room = chat_room;
                return true;
            }
            room = null;
            return false;
        }
        

        public bool get_talker_list_in_selected_room(CChatRoom room, out List<CChatUser> talker_list)
        {
            if (roomlist.ContainsValue(room))
            {
                talker_list = room.get_talker_list();
                return true;
            }

            talker_list = null;
            return false;
        }

        public void destroy_room(CChatUser user)
        {
            if (!roomlist.ContainsKey(user.UserName))
            {
                return;
            }

            user.chatroom.destory_chat_room();
            roomlist.Remove(user.UserName);
        }

        public bool enter_the_room(string room_owner, CChatUser user, out CChatRoom chat_room)
        {
            if (!roomlist.ContainsKey(room_owner))
            {
                chat_room = null;
                return false;
            }

            chat_room = roomlist[room_owner];
            chat_room.enter_chat_room(user);
            Program.server_main.remove_waiting_list(user);

            return true;
        }

        */


    }
}
