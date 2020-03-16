using System;
using System.ComponentModel;
using System.Windows;

namespace Mycord_WPF
{
    public class IsRunningProperty
    {
      
        public static readonly DependencyProperty ValueProperty = DependencyProperty.RegisterAttached("Value", 
            typeof(bool), typeof(IsRunningProperty), new UIPropertyMetadata(new PropertyChangedCallback(OnValuePropertyChanged)));

        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e){}

        public static bool GetValue(DependencyObject d) => (bool)d.GetValue(ValueProperty);
        public static void SetValue(DependencyObject d, bool value) => d.SetValue(ValueProperty, value);


    }
}
