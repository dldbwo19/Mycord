using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Mycord_WPF
{
    public class FrameHistoryProperty
    {
        public static readonly DependencyProperty ValueProperty = DependencyProperty.RegisterAttached("Value", 
            typeof(bool), typeof(FrameHistoryProperty), new UIPropertyMetadata(new PropertyChangedCallback(OnValuePropertyChanged)));

        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var frame = (d as Frame);

            frame.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;

            frame.Navigated += (sender, args) => ((Frame)sender).NavigationService.RemoveBackEntry();
        }

        public static bool GetValue(DependencyObject d) => (bool)d.GetValue(ValueProperty);
        public static void SetValue(DependencyObject d, bool value) => d.SetValue(ValueProperty, value);


    }
}
