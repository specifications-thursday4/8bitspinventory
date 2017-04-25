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
    /// Interaction logic for IMSMainPage.xaml
    /// </summary>
    public partial class IMSMainPage : Page
    {
        public IMSMainPage()
        {
            InitializeComponent();
        }

        private void Inventory_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("UpdateInventory.xaml", UriKind.Relative));
        }

        private void Add_Item(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("QuickAdd.xaml", UriKind.Relative));
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("MainPage.xaml", UriKind.Relative));
        }

        private void View_Detail(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("TotalInventory.xaml", UriKind.Relative));
        }
    }
}
