using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace GridForms.Core
{
    [ContentProperty("Presenter")]
    public class Entry : UserControl
    {
        #region Dependency Properties

        public static readonly DependencyProperty PresenterProperty =
            DependencyProperty.Register(
                "Presenter",
                typeof(UIElement),
                typeof(Entry));

        public static readonly DependencyProperty RowHeightProperty =
            DependencyProperty.Register(
                "RowHeight",
                typeof(GridLength),
                typeof(Entry),
                new PropertyMetadata(GridLength.Auto));

        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register(
                "Label",
                typeof(Label),
                typeof(Entry));
        #endregion Dependency Properties

        public UIElement Presenter
        {
            get => (UIElement)GetValue(PresenterProperty);
            set => SetValue(PresenterProperty, value);
        }

        [TypeConverter(typeof(StringToLabelConverter))]
        public Label Label
        {
            get => (Label)GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }
        
        public GridLength RowHeight
        {
            // ReSharper disable once PossibleNullReferenceException => has a default value of GridLength.Auto
            get => (GridLength)GetValue(RowHeightProperty);
            set => SetValue(RowHeightProperty, value);
        }
    }
}