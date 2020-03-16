using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Mycord_WPF
{
    /// <summary>
    /// CustomMessageBox2.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CustomMessageBox2 : Window
    {
        public CustomMessageBox2()
        {
            InitializeComponent();
            this.DataContext = new CustomMessageBox2VM(this);
        }
    }
}
