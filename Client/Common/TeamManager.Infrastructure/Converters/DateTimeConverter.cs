using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TeamManager.Infrastructure.Converters
{
    public class DateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "";

            var dt = (DateTime) value;
            var format = parameter == null ? "dd MMMM, yyyy" : parameter.ToString();
            return dt.ToString(format, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var strValue = value as string;
            DateTime dateTime;
            return DateTime.TryParse(strValue, out dateTime) ? dateTime : DependencyProperty.UnsetValue;
        }
    }
}