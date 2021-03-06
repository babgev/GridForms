﻿using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace GridForms.WPF.Core
{
    [ContentProperty("Presenter")]
    public class Entry : UserControl
    {
        #region Dependency Properties

        public static readonly DependencyProperty PresenterProperty =
            DependencyProperty.Register(
                nameof(Presenter),
                typeof(UIElement),
                typeof(Entry));

        public static readonly DependencyProperty RowHeightProperty =
            DependencyProperty.Register(
                nameof(RowHeight),
                typeof(GridLength),
                typeof(Entry),
                new PropertyMetadata(GridLength.Auto));

        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register(
                nameof(Label),
                typeof(Label),
                typeof(Entry));

        public static readonly DependencyProperty LabelContentProperty =
            DependencyProperty.Register(
                nameof(LabelContent),
                typeof(object),
                typeof(Entry));

        #endregion Dependency Properties

        public UIElement Presenter
        {
            get => (UIElement)GetValue(PresenterProperty);
            set => SetValue(PresenterProperty, value);
        }
        
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

        public object LabelContent
        {
            get => GetValue(LabelContentProperty);
            set => SetValue(LabelContentProperty, value);
        }
    }
}