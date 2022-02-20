using System.Windows;
using MetarDecoder;

namespace MetarViewer
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

        private void downloadTest_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Hello world :)");
            UpdateMetars();
        }
        private void UpdateMetars()
        {
            Decoder decoder = new Decoder();
            decoder.ProcessMetars();
            dataGridTest.ItemsSource = decoder.MetarResults;
            dataGridTest.Columns[5].Visibility = Visibility.Collapsed;

            
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            dataGridTest.FontSize = this.Width / 40;
        }
    }
}
