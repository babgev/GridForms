using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GridForms.WPF.Core
{
    class VisibilityToGridLengthConverter : IValueConverter
    {
        public GridLength VisibileLength { get; set; }
        public GridLength CollapsedLength { get; set; } = new GridLength(0);

        public VisibilityToGridLengthConverter()
        {

        }
        public VisibilityToGridLengthConverter(GridLength visibileLength)
        {
            VisibileLength = visibileLength;
        }
        public VisibilityToGridLengthConverter(GridLength visibileLength, GridLength collapsedLength)
        {
            VisibileLength = visibileLength;
            CollapsedLength = collapsedLength;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibilityValue && visibilityValue == Visibility.Visible) return VisibileLength;
            else return CollapsedLength;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
