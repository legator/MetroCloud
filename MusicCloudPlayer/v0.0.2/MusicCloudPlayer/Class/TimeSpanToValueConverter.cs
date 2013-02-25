using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace MusicCloudPlayer.Class
{
    public class TimeSpanToValueConverter : IValueConverter
    {
        public TimeSpanToValueConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TimeSpan timeSpan = (TimeSpan)value;
            object totalSeconds = timeSpan.TotalSeconds;
            return totalSeconds;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object obj = TimeSpan.FromSeconds((double)value);
            return obj;
        }
    }
}
