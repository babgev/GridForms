using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace GridForms.WPF.Core
{
    [ContentProperty("Entries")]
    public class GridForm : UserControl
    {
        #region Dependency Properties

        public static readonly DependencyPropertyKey EntriesProperty =
            DependencyProperty.RegisterReadOnly(nameof(Entries),
                typeof(UIElementCollection),
                typeof(GridForm),
                new PropertyMetadata());
        
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
        public UIElementCollection Entries
        {
            get => (UIElementCollection)GetValue(EntriesProperty.DependencyProperty);
            private set => SetValue(EntriesProperty, value);
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

            Entries = new UIElementCollection(grid, grid);
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
                var castedEntry = entry as Entry;              
                
                var label = castedEntry.Label;
                if (label == null)
                {
                    var labelContentBinding = BindingOperations.GetBinding(castedEntry, Entry.LabelContentProperty);
                    if(labelContentBinding != null)
                    {
                        label = new Label();
                        BindingOperations.SetBinding(label, Label.ContentProperty, labelContentBinding);
                    }                   
                    else if(castedEntry.LabelContent != null)
                    {
                        label = new Label
                        {
                            Content = castedEntry.LabelContent
                        };
                    }
                }

                if(label != null)
                {
                    grid.Children.Add(label);
                    Grid.SetColumn(label, 0);
                }

                var presenter = castedEntry.Presenter;
                grid.Children.Add(presenter);
                Grid.SetColumn(presenter, 2);

                grid.RowDefinitions.Add(new RowDefinition { Height = castedEntry.RowHeight });

                int uiRow;
                if (HasSeparator)
                {
                    // ReSharper disable once PossibleInvalidOperationException => HasSeparator checks for null
                    grid.RowDefinitions.Add(new RowDefinition { Height = SeparatorHeight });
                    uiRow=  i * 2;
                }
                else
                {
                    uiRow = i;
                }

                SetRows(label, presenter, uiRow);
                ApplyVisibility();

                void ApplyVisibility()
                {
                    var targetRow = grid.RowDefinitions[uiRow];
                    var separatorRow = grid.RowDefinitions[uiRow + 1];

                    var uiRowConverter = new VisibilityToGridLengthConverter(castedEntry.RowHeight);
                    var separatorRowConverter = new VisibilityToGridLengthConverter(SeparatorHeight);

                    if (BindingOperations.GetBinding(entry, VisibilityProperty) != null)
                    {
                        var uiRowBinding = new Binding() { Source = castedEntry, Path = new PropertyPath(nameof(Visibility)), Converter = uiRowConverter };
                        BindingOperations.SetBinding(targetRow, RowDefinition.HeightProperty, uiRowBinding);

                        var separatorRowBinding =  new Binding() { Source = castedEntry, Path = new PropertyPath(nameof(Visibility)), Converter = separatorRowConverter };
                        BindingOperations.SetBinding(separatorRow, RowDefinition.HeightProperty, separatorRowBinding);
                    }
                    else
                    {
                        targetRow.Height = (GridLength)uiRowConverter.Convert(castedEntry.Visibility, typeof(GridLength),null, null);
                        separatorRow.Height = (GridLength)separatorRowConverter.Convert(castedEntry.Visibility, typeof(GridLength), null, null);
                    }                 
                }
            }
        }
        
        private static void SetRows(Label label, UIElement presenter, int uiRow)
        {
            if (label != null) Grid.SetRow(label, uiRow);
            Grid.SetRow(presenter, uiRow);           
        }
    }
}