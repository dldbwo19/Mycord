using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Mycord_WPF
{
    public class SideButtonControlVM : BaseViewModel
    {
        public ObservableCollection<SideItem> SideItems { get; set; }
        public ICommand CreateRoomCommand { get; set; }
        public ICommand EnterRoomCommand { get; set; }

        public bool IsRunning { get; set; } = false;

        public SideButtonControlVM()
        {
            SideItems = new ObservableCollection<SideItem>();
            CreateRoomCommand = new RelayCommand(() => CreateRoom());
            EnterRoomCommand = new RelayCommand(() => EnterRoom());
        }

        public void CreateRoom()
        {
            if (!IsRunning)
            {
                IsRunning = true;
                var messageBox = new CustomMessageBox();
                (messageBox.DataContext as CustomMessageBoxVM).returnCallback += ReturnSideItem;
                messageBox.Show();
            }
        }

        public void EnterRoom()
        {
            if (!IsRunning)
            {
                IsRunning = true;
                var messageBox = new CustomMessageBox2();
                (messageBox.DataContext as CustomMessageBox2VM).returnCallback += ReturnSideItem;
                messageBox.Show();
            }
        }

        public void ReturnSideItem(List<string> list, string name, string initial)
        {
            if (list == null || name == null || initial == null)
            {
                IsRunning = false;
                return;
            }

            ObservableCollection<User> observableCollection = new ObservableCollection<User>();
            foreach (var item in list)
            {
                observableCollection.Add(new User { Name = item });
            }

            AddItems(observableCollection, name, initial);

            IsRunning = false;
        }

        public void AddItems(ObservableCollection<User> list, string name, string initial)
        {
            App.Current.Dispatcher.BeginInvoke((Action)delegate ()
            {
                SideItems.Add(new SideItem(list,name,initial)); 
            });
        }

        public void RemoveItems(SideItem item)
        {
            App.Current.Dispatcher.BeginInvoke((Action)delegate ()
            {
                SideItems.Remove(item);
                ChatVMFactory.DeleteViewModel(item.RoomName);
            });
        }
    }
}
