using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Mycord_WPF
{
    public class SideItem : BaseViewModel
    {
        public ObservableCollection<ChatItem> ChatList { get; set; }
        public ObservableCollection<User> UserList { get; set; }
        public string RoomName { get; set; }
        public string Initial { get; set; }

        public ChatPageVM pageVM { get; set; }
        public ICommand Test2Command { get; set; }

        public SideItem(ObservableCollection<User> users, string serverName, string initial)
        {
            //Test2Command = new RelayParameterCommand(async (parameter) => await go2(parameter));
            Test2Command = new RelayCommand(async () => await GoToChatPage());
            ChatList = new ObservableCollection<ChatItem>();
            UserList = users;
            RoomName = serverName;
            Initial = initial;

            ChatVMFactory.PushViewModel(RoomName, new ChatPageVM(RoomName, ChatList, UserList, this));
            pageVM = ChatVMFactory.GetViewModel(RoomName);
        }

        public async Task GoToChatPage()
        {
            await Task.Run(() =>
            {
                App.Current.Dispatcher.BeginInvoke((Action)delegate ()
                {
                    (VMFactory.GetViewModel(typeof(MainWindowVM)) as MainWindowVM).ContentPage = EPageNavigation.Chat;
                    (ViewFactory.GetView(typeof(ChatPage)) as ChatPage).DataContext = pageVM;
                });
            });
        }
    }
}
