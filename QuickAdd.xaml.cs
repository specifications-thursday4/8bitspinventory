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
    /// Interaction logic for QuickAdd.xaml
    /// </summary>
    public partial class QuickAdd : Page
    {
        public static string typeToAddToInventory;

        public QuickAdd()
        {
            InitializeComponent();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            Button btnClicked = (Button)sender;
            typeToAddToInventory = btnClicked.Name;

            this.NavigationService.Navigate(new Uri("AddNewItem.xaml", UriKind.Relative));
        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("IMSMainPage.xaml", UriKind.Relative));
        }
    }
}
