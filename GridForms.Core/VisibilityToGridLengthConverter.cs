using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GridForms.Core
{
    class VisibilityToGridLengthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibilityValue && visibilityValue == Visibility.Visible) return GridLength.Auto;
            else return new GridLength(0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
