using System.Windows;

namespace GridForms.WPF.Examples.BindingsExample
{
    public partial class BindingsExampleView : Window
    {
        public BindingsExampleView()
        {
            this.DataContext = new BindingsExampleViewModel();
            InitializeComponent();
        }
    }
}
