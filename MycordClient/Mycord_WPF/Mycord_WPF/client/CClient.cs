using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Windows;
using MyCordLib;

namespace Mycord_WPF
{
    public class CClient
    {
        public static List<IPeer> serverlist { get; private set; }
        public static List<string> waiting_userlist { get; set; }
        public static CClientProcess client_main { get; private set; }

        private static CClient client_instance;
        private static CNetworkService service;
        private static CConnector connector;

        //private static string myname = null;

        private IPEndPoint end_point;
        private int connection_count = 0;

        public delegate void OnConnectedHandler();
        public OnConnectedHandler on_connectedHandler { get; set; }
        

        private CClient()
        {
            CPacketBufferManager.initialize(2000);
            service = new CNetworkService();
            serverlist = new List<IPeer>();
            waiting_userlist = new List<string>();

            connector = new CConnector(service);

            connector.connected_callback += on_connected_server;
            connector.reconnection_callback += reconnect_server;

            end_point = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 7979);

            connect_server();
        }

        public static CClient get_CClient_instance()
        {
            //return client_instance == null ? client_instance = new CClient() : client_instance;
            return client_instance ?? (client_instance = new CClient());
        }

        private void connect_server()
        {
            connector.connect(end_point);
        }

        private void on_connected_server(CUserToken server_token)
        {
            lock (serverlist)
            {
                IPeer server = new CServer(server_token);
                serverlist.Add(server);
            }
            client_main = new CClientProcess();
            on_connectedHandler();
        }

        private void reconnect_server()
        {
            if (connection_count < 100)
            {
                ++connection_count;
                //Console.WriteLine($"connection_count : {connection_count} / 100 ");
                connect_server();
            }
            else
            {
                //Console.WriteLine("Stop");
            }
        }

        public void disconnect_server()
        {
            client_main.close_loop();
            serverlist[0].disconnect();
            serverlist.Clear();
        }
    }
}
