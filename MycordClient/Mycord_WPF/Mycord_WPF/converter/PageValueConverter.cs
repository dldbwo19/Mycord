using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Mycord_WPF
{
    public class PageValueConverter : BaseValueConverter<PageValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            
            // Find the appropriate page
            switch ((EPageNavigation)value)
            {
                case EPageNavigation.Login:
                    return ViewFactory.GetView(typeof(LoginPage));

                case EPageNavigation.Chat:
                    return ViewFactory.GetView(typeof(ChatPage));

                case EPageNavigation.Insert:
                    return ViewFactory.GetView(typeof(InsertPage));

                case EPageNavigation.SideMenuBar:
                    return ViewFactory.GetView(typeof(SideButtonControl));

                case EPageNavigation.None:
                default:
                    return null;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

