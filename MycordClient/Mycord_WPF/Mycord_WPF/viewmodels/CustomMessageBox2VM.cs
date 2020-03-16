using MyCordLib;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Mycord_WPF
{
    public class CustomMessageBox2VM : BaseViewModel
    {
        private Window mWindow;

        private string pattern = @"[a-zA-Z]*";
        private short return_success = -1;
        private List<string> userList;
        private string serverName;
        private string serverInitial;

        public delegate void ReturnCallbackHanler(List<string> UserList, string Name, string Initial);
        public ReturnCallbackHanler returnCallback;

        public bool IsSearching { get; set; }

        public ICommand EnterRoomCommand { get; set; }
        public ICommand CloseCommand { get; set; }

        public string ErrorText { get; set; } = string.Empty;
        public string ServerName { get; set; } = string.Empty;

        public CustomMessageBox2VM(Window window)
        {
            mWindow = window;

            EnterRoomCommand = new RelayCommand(async () => await EnterRoomAsync());
            CloseCommand = new RelayCommand(() =>
            {
                if (IsSearching)
                {
                    return;
                }
                returnCallback(null, null, null);
                mWindow.Close();
            });
        }

        public void return_Result(short Id, List<string> UserList, string Name, string Initial)
        {
            this.return_success = (short)Id;
            this.userList = UserList;
            this.serverName = Name;
            this.serverInitial = Initial;
        }

        public async Task EnterRoomAsync()
        {
            await RunCommand(() => IsSearching, async () =>
            {
                await Task.Run(() =>
                {
                    if (ServerName == "" || !Regex.IsMatch(ServerName, pattern) || ServerName.Length < 2 || ServerName.Length > 12)
                    {
                        ErrorText = "서버 이름 양식이 잘못되었습니다.";
                        return;
                    }
                    (CClient.serverlist[0] as CServer).Create_Page_Callback += return_Result;
                    CPacket message = CPacket.create((short)EProtocol.JOIN_THE_ROOM_REQ);
                    message.push(ServerName);

                    CClient.serverlist[0].send(message);

                    bool timeOver = false;

                    var timer = new System.Timers.Timer(10000);

                    timer.Elapsed += (sender, e) =>
                    {
                        timeOver = true;
                    };

                    timer.Enabled = true;

                    while (true)
                    {
                        if (return_success == (short)EProtocol.JOIN_THE_ROOM_ACK)
                        {
                            this.ErrorText = string.Empty;
                            (CClient.serverlist[0] as CServer).Create_Page_Callback -= return_Result;
                            returnCallback(userList, serverName, serverInitial);

                            this.mWindow.Dispatcher.BeginInvoke((Action)delegate
                            {
                                this.returnCallback = null;
                                this.mWindow.Close();
                            });
                            break;
                        }
                        else if (return_success == (short)EProtocol.JOIN_THE_ROOM_ERROR)
                        {
                            (CClient.serverlist[0] as CServer).Create_Page_Callback -= return_Result;
                            this.ErrorText = "해당 서버에 입장하지 못하였습니다.";
                            break;
                        }
                        else if (timeOver)
                        {
                            (CClient.serverlist[0] as CServer).Create_Page_Callback -= return_Result;
                            timer.Dispose();
                            this.ErrorText = "서버와의 연결 상태가 좋지 않습니다.";
                            break;
                        }
                    }
                    return_success = -1;

                });
            });
        }
    }
}
