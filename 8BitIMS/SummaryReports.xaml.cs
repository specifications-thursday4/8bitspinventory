using System;
using System.Collections.Generic;
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

namespace _8BitIMS
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class SummaryReports : Page
    {
        public SummaryReports()
        {
            InitializeComponent();
        }
        private void popular(object sender, RoutedEventArgs e)

        {
            this.NavigationService.Navigate(new Uri("PopularInventory.xaml", UriKind.Relative));
        }
        private void TotalInventory(object sender, RoutedEventArgs e)

        {
            this.NavigationService.Navigate(new Uri("TotalInventory.xaml", UriKind.Relative));
        }
    }
}
