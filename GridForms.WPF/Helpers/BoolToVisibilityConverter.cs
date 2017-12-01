using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GridForms.Helpers
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue && boolValue) return Visibility.Visible;
            else return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visValue && visValue == Visibility.Visible) return true;
            else return false;
        }
    }
}
