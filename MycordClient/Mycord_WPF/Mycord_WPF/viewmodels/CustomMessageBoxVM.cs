using MyCordLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Mycord_WPF
{
    public class CustomMessageBoxVM : BaseViewModel
    {
        private Window mWindow;
        private string pattern = @"[a-zA-Z]*";
        private short return_success = -1;
        private List<string> userList;
        private string serverName;
        private string serverInitial;

        public delegate void ReturnCallbackHanler(List<string> UserList, string Name, string Initial);
        public ReturnCallbackHanler returnCallback;

        public bool IsCreating { get; set; }

        public ICommand CreateRoomCommand { get; set; }
        public ICommand CloseCommand { get; set; }

        public string ErrorText { get; set; } = string.Empty;
        public string ServerName { get; set; } = string.Empty;
        public string ServerInitial { get; set; } = string.Empty;

        public CustomMessageBoxVM(Window window)
        {
            mWindow = window;

            CreateRoomCommand = new RelayCommand(async() => await CreateRoomAsync());
            CloseCommand = new RelayCommand(() =>
            {
                if (IsCreating)
                {
                    return;
                }
                returnCallback(null, null, null);
                mWindow.Close();
            });
        }

        public void return_Result(short Id, List<string> UserList, string Name, string Initial)
        {
            return_success = (short)Id;
            this.userList = UserList;
            this.serverName = Name;
            this.serverInitial = Initial;
        }

        public async Task CreateRoomAsync()
        {
            await RunCommand(() => IsCreating, async () =>
            {
                await Task.Run(() => 
                {
                    if (ServerName == "" || !Regex.IsMatch(ServerName, pattern) || ServerName.Length < 2 || ServerName.Length > 12)
                    {
                        ErrorText = "서버 이름은 영문자 2 ~ 12로 지어주세요.";
                        return;
                    }
                    if (ServerInitial == "" || !Regex.IsMatch(ServerInitial, pattern) || ServerInitial.Length < 1 || ServerInitial.Length > 3)
                    {
                        ErrorText = "서버 이니셜은 영문자 1 ~ 3로 지어주세요.";
                        return;
                    }

                    (CClient.serverlist[0] as CServer).Create_Page_Callback += return_Result;
                    CPacket message = CPacket.create((short)EProtocol.CREATE_CHAT_ROOM_REQ);
                    message.push(ServerName);
                    message.push(ServerInitial);

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
                        if (return_success == (short)EProtocol.CREATE_CHAT_ROOM_ACK)
                        {
                            (CClient.serverlist[0] as CServer).Create_Page_Callback -= return_Result;
                            returnCallback(userList, serverName, serverInitial);
                            
                            this.mWindow.Dispatcher.BeginInvoke((Action)delegate 
                            {
                                this.returnCallback = null;
                                this.mWindow.Close();
                            });

                            this.ErrorText = string.Empty;
                            break;
                        }
                        else if (return_success == (short)EProtocol.CREATE_CHAT_ROOM_ERROR)
                        {
                            (CClient.serverlist[0] as CServer).Create_Page_Callback -= return_Result;
                            this.ErrorText = "해당 이름은 이미 사용중 입니다.";                         
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
