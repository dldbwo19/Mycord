using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace Mycord_WPF
{
    public class MainWindowVM : BaseViewModel
    {
        #region Private 멤버

        private Window mWindow;

        private int mOuterMarginSize = 0;

        private int mWindowRadius = 0;

        #endregion

        #region Public 멤버

        public double WindowMinimumWidth { get; set; } = 600;
        public double WindowMinimumHeight { get; set; } = 400;

        public int ResizeBorder { get; set; } = 2;
        public Thickness ResizeBorderThickness
        {
            get
            {
                return new Thickness(ResizeBorder + OuterMarginSize);
            }
        }

        public int OuterMarginSize
        {
            get
            {
                return mWindow.WindowState == WindowState.Maximized ? 7 : mOuterMarginSize;
            }
            set
            {
                mOuterMarginSize = value;
            }
        }
        public Thickness OuterMarginSizeThickness
        {
            get
            {
                return new Thickness(OuterMarginSize);
            }
        }

        public int WindowRadius
        {
            get
            {
                return mWindow.WindowState == WindowState.Maximized ? 0 : mWindowRadius;
            }
            set
            {
                mWindowRadius = value;
            }
        }
        public CornerRadius WindowCornerRadius
        {
            get
            {
                return new CornerRadius(WindowRadius);
            }
        }

        public int TitleHeight { get; set; } = 22;
        public GridLength TitleHeightGridLength
        {
            get
            {
                return new GridLength(TitleHeight);
            }
        }


        #endregion

        #region Command 멤버

        public ICommand MinimizeCommand { get; set; }
        public ICommand MaximizeCommand { get; set; }
        public ICommand CloseCommand { get; set; }

        #endregion

        public object ContentPage { get; set; }

        public object SideMenu { get; set; } 

        #region 생성자

        public MainWindowVM(Window window)
        {
            mWindow = window;

            VMFactory.PushViewModel(this.GetType(), this);

            this.ContentPage = EPageNavigation.Login;
            this.SideMenu = EPageNavigation.None;

            mWindow.StateChanged += (sender, e) =>
            {
                OnPropertyChanged(nameof(ResizeBorderThickness));
                OnPropertyChanged(nameof(OuterMarginSize));
                OnPropertyChanged(nameof(OuterMarginSizeThickness));
                OnPropertyChanged(nameof(WindowRadius));
                OnPropertyChanged(nameof(WindowCornerRadius));
            };

            MinimizeCommand = new RelayCommand(() => mWindow.WindowState = WindowState.Minimized);
            MaximizeCommand = new RelayCommand(() => mWindow.WindowState ^= WindowState.Maximized);
            CloseCommand = new RelayCommand(() => 
            {
                mWindow.Close();
                Application.Current.Shutdown();   
            });
        }

        #endregion
    }
}
