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

namespace _8BitIMS
{
    /// <summary>
    /// Interaction logic for ViewCartPage.xaml
    /// </summary>
    public partial class ViewCartPage : Page
    {
        private static string DATABASE = "Data Source = inventory.db";
        private int total = 0;
        public ViewCartPage()
        {
           
            InitializeComponent();
            ReceiptGeneration();
            ReceiptHeader();
            TotalGenerate();
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("POSMainPage.xaml", UriKind.Relative));
        }

        private void ReceiptGeneration()
        {
            SQLiteConnection conn = new SQLiteConnection(DATABASE);
            conn.Open();

            

            foreach (var item in MainWindow.Cart.iList)
            {
                var command = conn.CreateCommand();
                command.CommandText = "SELECT g.name, m.price FROM " +
                    "games g INNER JOIN multiplat_games m " +
                    "ON g.id = m.game_id " +
                    "WHERE g.id = " + item.gameID;

                SQLiteDataReader sdr = command.ExecuteReader();

                while (sdr.Read())
                {
                    Label game = new Label();
                    Label quantity = new Label();
                    Label price = new Label();

                    game.Content = sdr.GetString(0);
                    price.Content = "$ " + sdr.GetInt32(1);
               

                    Name.Children.Add(game);
          
                    Price.Children.Add(price);
                    break;


                }
            }
            
            
        }

        private void ReceiptHeader()
        {
            Random rand = new Random();
            ReceiptHead.Text = "Check# " + (rand.Next()) % 450;
            ReceptHead2.Text += DateTime.Now;

        }

        private void TotalGenerate()
        {
            
            foreach (var i in MainWindow.Cart.iList)
            {
                total += i.price;
            }

            Total.Content = "Grand Total: $ " + total.ToString();
        }

        private void ExecuteTransaction(object sender, RoutedEventArgs e)
        {
            SQLiteConnection conn = new SQLiteConnection(DATABASE);
            conn.Open();
            
            MessageBoxResult result = MessageBox.Show("Finish check totalling $" + total + "?", "Confirmation"
                    , MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes) {
                var command = conn.CreateCommand();
                
                foreach (var item in MainWindow.Cart.iList)
                {
                  
                 command.CommandText = "INSERT INTO transactions (game_id, platform_id, quantity, time) VALUES("+
                           item.gameID + "," + item.platID + "," + item.quantity + ",'" + DateTime.Now + "');";
                 command.ExecuteNonQuery();

                 command.CommandText = "UPDATE multiplat_games " +
                          "SET quantity = quantity - " + item.quantity +
                          " WHERE game_id = " + item.gameID + " AND platform_id = " + item.platID;
                 command.ExecuteNonQuery();
                    
                }
                MessageBoxResult res = MessageBox.Show("Complete! Transaction has been recorded", "Confirmation", MessageBoxButton.OK);
                if (res == MessageBoxResult.OK)
                {
                    total = 0;
                    foreach (var i in MainWindow.Cart.iList.ToList())
                    {
                        MainWindow.Cart.iList.Remove(i);
                    }
                    this.NavigationService.Navigate(new Uri("POSMainPage.xaml", UriKind.Relative));
                }

                conn.Close();
            }
        }

        private void RefundItem(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you would like to refund" +
                " the amount of -$" + total + "?", "Confirm Refund", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                MessageBoxResult res = MessageBox.Show("Refund processed. Please give cash to customer" +
                    " if this was a cash transaction, otherwise, swipe card to disperse funds", "Complete"
                    , MessageBoxButton.OKCancel);

                if (res == MessageBoxResult.OK)
                {
                    total = 0;
                    foreach (var i in MainWindow.Cart.iList.ToList())
                    {
                        MainWindow.Cart.iList.Remove(i);
                    }
                    this.NavigationService.Navigate(new Uri("POSMainPage.xaml", UriKind.Relative));
                }
            }
        }
    }
}
