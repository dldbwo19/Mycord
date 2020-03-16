using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Mycord_WPF
{
    public class TimeFormatConverter : BaseValueConverter<TimeFormatConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var time = (DateTimeOffset)value;

            if(time.Date == DateTimeOffset.UtcNow.Date)
            {
                return time.ToLocalTime().ToString("HH:mm");
            }

            return time.ToLocalTime().ToString("yy MM HH:mm");
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
