using System;
using System.Collections.Generic;
using MyCordLib;

namespace MyCordServer
{
    class Program
    {
        // 모든 유저 리스트
        private static List<CChatUser> userlist;
        public static CChatServer server_main { get; private set; }

        static void Main(string[] args)
        {
            CPacketBufferManager.initialize(2000);

            userlist = new List<CChatUser>();
            server_main = new CChatServer();

            CNetworkService service = new CNetworkService();

            service.session_created_callback += on_session_created;

            service.initialize();
            service.listen("0.0.0.0", 7979, 100);

            Console.WriteLine("Started!");

            while (true)
            {
                System.Threading.Thread.Sleep(1000);
            }
        }

        static void on_session_created(CUserToken token)
        {
            CChatUser user = new CChatUser(token);
            lock (userlist)
            {
                userlist.Add(user);
            }
        }

        public static void remove_user(CChatUser user)
        {
            //server_main.room_manager.destroy_room(user);

            lock (userlist)
            {
                userlist.Remove(user);
            }
        }
    }
}
