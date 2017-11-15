using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace GridForms.WPF
{
    [Serializable]
    [ContentProperty("Entries")]
    public class GridForm : UserControl
    {
        #region Dependency Properties

        public static readonly DependencyProperty EntriesProperty =
            DependencyProperty.Register(nameof(Entries),
                typeof(List<Entry>),
                typeof(GridForm));

        public static readonly DependencyProperty SeparatorHeightProperty =
            DependencyProperty.Register(
                nameof(SeparatorHeight),
                typeof(GridLength),
                typeof(GridForm),
                new PropertyMetadata(new GridLength(5)));

        public static readonly DependencyProperty LabelColumnWidthProperty =
            DependencyProperty.Register(
                nameof(LabelColumnWidth),
                typeof(GridLength),
                typeof(GridForm),
                new FrameworkPropertyMetadata(GridLength.Auto, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        #endregion Dependency Properties

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public List<Entry> Entries
        {
            get => (List<Entry>)GetValue(EntriesProperty);
            set => SetValue(EntriesProperty, value);
        }

        public GridLength SeparatorHeight
        {
            get => (GridLength)GetValue(SeparatorHeightProperty);
            set => SetValue(SeparatorHeightProperty, value);
        }
        public GridLength LabelColumnWidth
        {
            get => (GridLength)GetValue(LabelColumnWidthProperty);
            set => SetValue(LabelColumnWidthProperty, value);
        }        

        private bool HasSeparator => SeparatorHeight.Value != 0;

        public GridForm()
        {
            var grid = CreateGrid();
            var scrollViewer = new ScrollViewer
            {
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
                Content = grid
            };
            grid.SizeChanged += ItemsContainer_OnSizeChanged;
            Content = scrollViewer;

            SetValue(EntriesProperty, new List<Entry>());
        }

        private Grid CreateGrid()
        {
            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Tag="LabelColumn", Width = LabelColumnWidth });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(10) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            return grid;
        }

        private void ItemsContainer_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var grid = sender as Grid;
            if (Entries.Count <= 0 || grid is null) return;
            InitializeForm(grid);
            grid.SizeChanged -= ItemsContainer_OnSizeChanged;
        }

        private void InitializeForm(Grid grid)
        {
            grid.RowDefinitions.Clear();

            grid.ColumnDefinitions.FirstOrDefault(x => x.Tag == "LabelColumn").Width = LabelColumnWidth;

            for (var i = 0; i < Entries.Count; i++)
            {
                var entry = Entries[i];
                var label = ResolveLabel(entry);

                grid.Children.Add(label);
                Grid.SetColumn(label, 0);

                var presenter = entry.Presenter;
                grid.Children.Add(presenter);
                Grid.SetColumn(presenter, 2);

                grid.RowDefinitions.Add(new RowDefinition { Height = entry.RowHeight });

                if (HasSeparator)
                {
                    // ReSharper disable once PossibleInvalidOperationException => HasSeparator checks for null
                    grid.RowDefinitions.Add(new RowDefinition { Height = SeparatorHeight });
                    SetRows(label, presenter, i * 2);
                }
                else
                {
                    SetRows(label, presenter, i);
                }

                grid.Children.Remove(entry);
            }
        }

        private static Label ResolveLabel(Entry entry)
        {
            var label = new Label { VerticalAlignment = VerticalAlignment.Center };

            var labelContentBinding = BindingOperations.GetBinding(entry, Entry.LabelProperty);
            if (labelContentBinding != null)
            {
                BindingOperations.SetBinding(label, ContentProperty, labelContentBinding);
            }
            else
            {
                label.Content = entry.Label;
            }

            var labelVisibilityBinding = BindingOperations.GetBinding(entry, Entry.LabelVisibilityProperty);
            if (labelVisibilityBinding != null)
            {
                BindingOperations.SetBinding(label, VisibilityProperty, labelVisibilityBinding);
            }

            return label;
        }

        private static void SetRows(Label label, UIElement presenter, int i)
        {
            Grid.SetRow(label, i);
            Grid.SetRow(presenter, i);
        }
    }
}