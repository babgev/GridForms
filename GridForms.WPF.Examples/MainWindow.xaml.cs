using GridForms.WPF.Examples.BindingsExample;
using System.Windows;

namespace GridForms.WPF.Examples
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new BindingsExampleView().Show();
        }

        private void LongForm_Click(object sender, RoutedEventArgs e)
        {
            new LongFormExample().Show();
        }
    }
}
