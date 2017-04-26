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

using System.Data.SQLite;
using System.Data;

namespace _8BitIMS
{
    /// <summary>
    /// Interaction logic for UpdateInventory.xaml
    /// </summary>
    public partial class UpdateInventory : Page
    {
        private static string DATABASE = "Data Source = inventory.db";
        private string[] restrictions = new string[4];
        public UpdateInventory()
        {
            InitializeComponent();
            TabCreation();
        }

        private void TabCreation()
        {
            restrictions[2] = "platforms";

            SQLiteConnection conn = new SQLiteConnection(DATABASE);
            conn.Open();
            var command = conn.CreateCommand();

            command.CommandText = "SELECT name FROM platforms ORDER by name ASC";
            SQLiteDataReader sdr = command.ExecuteReader();


            while (sdr.Read())
            {
                Button btn = new Button();
                btn.Width = 305;
                btn.Height = 50;
                btn.Background = Brushes.LightSkyBlue;
                btn.Click += new RoutedEventHandler(Category_Click);

                btn.Content = sdr.GetString(0);
                SystemsCategories.Children.Add(btn);
            }
            sdr.Close();


            conn.Close();
        }

        private void Category_Click(object sender, RoutedEventArgs e)
        {
            Button btnSent = (Button)sender;
            MainWindow.tableNameFromButtonClickedInPage = btnSent.Content.ToString();
            this.NavigationService.Navigate(new Uri("InventoryPage.xaml", UriKind.Relative));
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("IMSMainPage.xaml", UriKind.Relative));
        }
    }

}
