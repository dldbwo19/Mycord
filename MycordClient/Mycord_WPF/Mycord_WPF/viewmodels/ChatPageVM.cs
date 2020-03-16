using MyCordLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Mycord_WPF
{
    public class ChatPageVM : BaseViewModel
    {

        public string TextBoxText { get; set; }
        public ICommand MessageSendCommand { get; set; }
        public ICommand ExitCommand { get; set; }

        public SideItem SideItem { get; set; }
        public ObservableCollection<ChatItem> ChatList { get; set; }
        public ObservableCollection<User> UserList { get; set; }
        public string RoomName { get; set; }

        public ChatPageVM(string roomName, ObservableCollection<ChatItem> chatList, ObservableCollection<User> userList, SideItem sideItem)
        {
            ChatList = chatList;
            UserList = userList;
            SideItem = sideItem;
            RoomName = roomName;

            (CClient.serverlist[0] as CServer).ChatPage_Return_Callback.Add(RoomName, ReturnCallback);
            
            MessageSendCommand = new RelayParameterCommand(async (parameter) => await MessageSendAsync(parameter));
            ExitCommand = new RelayCommand(async () => await ExitAsync());
        }

        public async Task MessageSendAsync(object parameter)
        {
            await Task.Run(() =>
            {
                if ((parameter as string).Trim() == "")
                {
                    return;
                }

                CPacket message = CPacket.create((short)EProtocol.CHAT_MSG_REQ);
                message.push(RoomName);
                message.push((parameter as string));
                CClient.serverlist[0].send(message);

                TextBoxText = string.Empty;
            });
        }

        public async Task ExitAsync()
        {
            await Task.Run(() =>
            {
                CPacket message = CPacket.create((short)EProtocol.EXIT_THE_ROOM_REQ);
                message.push(RoomName);
                CClient.serverlist[0].send(message);

                (CClient.serverlist[0] as CServer).ChatPage_Return_Callback.Remove(RoomName);

                (VMFactory.GetViewModel(typeof(MainWindowVM)) as MainWindowVM).ContentPage = EPageNavigation.Insert;
                (VMFactory.GetViewModel(typeof(SideButtonControlVM)) as SideButtonControlVM).RemoveItems(SideItem);
            });
        }

        public void ReturnCallback(EChatItem item, object obj)
        {
            switch (item)
            {
                case EChatItem.Chat:
                    App.Current.Dispatcher.BeginInvoke((Action)delegate ()
                    {
                        ChatList.Add(obj as ChatItem);
                    });
                    break;

                case EChatItem.User:
                    App.Current.Dispatcher.BeginInvoke((Action)delegate ()
                    {
                        UserList.Add(obj as User);
                    });
                    break;
                case EChatItem.UserExit:
                    App.Current.Dispatcher.BeginInvoke((Action)delegate ()
                    {
                        User target = null;
                        foreach(var user in UserList)
                        {
                           if(user.Name == (string)obj)
                            {
                                target = user;
                                break;
                            }
                        }

                        if(target != null)
                        {
                            UserList.Remove(target);
                        }    
                    });
                    break;
                default:
                    Debugger.Break();
                    break;
            }
        }
    }
}
