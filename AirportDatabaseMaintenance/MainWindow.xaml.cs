using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AirportDatabaseMaintenance
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
        private AirportDbContext dbc = new AirportDbContext();
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var airport = new Airport
            {
                AirportIATACode = airportIATATextBox.Text,
                AirportICAOCode = airportICAOTextBox.Text,
                Minima=Convert.ToInt32(airportMinimaTextBox.Text)
            };
            
            dbc.Airports.Add(airport);
            try
            {
                dbc.SaveChanges();
            }
            catch (Exception exception)
            {

                MessageBox.Show(exception.InnerException.Message);
            }
            
            airportList.Items.Refresh();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dbc.Airports.Load();
            CollectionViewSource airportSource = (CollectionViewSource)(this.FindResource("airportViewSource"));
            airportSource.Source = dbc.Airports.Local;
        }

        private void airportList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (Airport airport in e.AddedItems)
            {
                airportIATATextBox.Text = airport.AirportIATACode;
                airportICAOTextBox.Text = airport.AirportICAOCode;
                airportMinimaTextBox.Text = airport.Minima.ToString();
            }
            
        }
    }
}
