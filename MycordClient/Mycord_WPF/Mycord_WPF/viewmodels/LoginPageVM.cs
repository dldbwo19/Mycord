using MyCordLib;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Mycord_WPF
{
    public class LoginPageVM : BaseViewModel
    {
        private string pattern = @"[a-zA-Z]*";
        private short return_success = -1;

        public string NickName { get; set; } = null;
        public string ErrorText { get; set; } = string.Empty;

        public ICommand LoginCommand { get; set; }
        public ICommand QuestionCommand { get; set; }

        public bool Connection { get; set; } = false;
        public bool LoginIsRunning { get; set; }


        public LoginPageVM()
        {
            LoginCommand = new RelayParameterCommand(async (parameter) => await LoginAsync(parameter));

            Task.Run(() => Connect());
        }

        public async Task Connect()
        {
            await Task.Run(() => CClient.get_CClient_instance().on_connectedHandler += () =>
            {
                Connection = true;

                (CClient.serverlist[0] as CServer).LoginPage_Return_Callback += return_Result;

            });
        }

        public void return_Result(short Id)
        {
            return_success = (short)Id;
        }

        public async Task LoginAsync(object parameter)
        {
            await RunCommand(() => LoginIsRunning, async () =>
            {
                await Task.Delay(1000);
                Task.WaitAll();

                await Task.Run(() =>
                {
                    NickName = parameter as string;
                    if (NickName == "" || !Regex.IsMatch(NickName, pattern) || NickName.Length < 4 || NickName.Length > 15)
                    {
                        ErrorText = "닉네임이 올바르지 않습니다.";
                        return;
                    }

                    CPacket message = CPacket.create((short)EProtocol.SET_NAME_REQ);
                    message.push(NickName);

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
                        if (return_success == (short)EProtocol.SET_NAME_ACK)
                        {
                            (CClient.serverlist[0] as CServer).LoginPage_Return_Callback -= return_Result;

                            var VM = VMFactory.GetViewModel(typeof(MainWindowVM));
                            (VM as MainWindowVM).ContentPage = EPageNavigation.Insert; 

                            this.ErrorText = string.Empty;
                            break;
                        }
                        else if (return_success == (short)EProtocol.SET_NAME_ERROR_EXISTED)
                        {
                            this.ErrorText = "해당 이름은 이미 사용중 입니다.";
                            break;
                        }
                        else if (timeOver)
                        {
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


