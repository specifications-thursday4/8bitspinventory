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
    /// Interaction logic for TotalInventory.xaml
    /// </summary>
    public partial class TotalInventory : Page
    {
        private static string DATABASE = "Data Source = inventory.db";

        public TotalInventory()
        {
            InitializeComponent();
            InventoryFieldGeneration();
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void InventoryFieldGeneration()
        {
            SQLiteConnection conn = new SQLiteConnection(DATABASE);
            conn.Open();
            var command = conn.CreateCommand();


            Label gameColLabel = new Label();
            Label count = new Label();
            Label platCount = new Label();
            Label priceLabel = new Label();

            gameColLabel.Content = "Console";
            count.Content = "Game Count";
            platCount.Content = "Platform Count";
            priceLabel.Content = "Total Value";


            gameColLabel.FontWeight = FontWeights.ExtraBold;
            count.FontWeight = FontWeights.ExtraBold;
            platCount.FontWeight = FontWeights.ExtraBold;
            priceLabel.FontWeight = FontWeights.ExtraBold;

            GameColumn.Children.Add(gameColLabel);
            Count.Children.Add(count);
            CountCon.Children.Add(platCount);
            Price.Children.Add(priceLabel);


            command.CommandText = "SELECT name, quantity, CASE WHEN price < 0 THEN 0 ELSE price END, inBoxQuant, CASE WHEN inBoxPrice < 0 THEN 0 ELSE inBoxPrice END FROM platforms";
            SQLiteDataReader sdr = command.ExecuteReader();
            List<String> conNames = new List<String>();

            int platformValue = 0;
            int platformQty = 0;
            while (sdr.Read())
            {
                Label platcount = new Label();
                conNames.Add(sdr.GetString(0));
                platformQty += sdr.GetInt32(1);
                platformValue += sdr.GetInt32(2) * sdr.GetInt32(1);
                platformQty += sdr.GetInt32(3);
                platformValue += sdr.GetInt32(3) * sdr.GetInt32(4);
                platcount.Content = sdr.GetInt32(3) + sdr.GetInt32(1);
                CountCon.Children.Add(platcount);
            }
            sdr.Close();

            int gameTotal = 0;
            int priceTotal = platformValue;
            foreach (String consoleName in conNames)
            {
                Label gameLabel = new Label();
                gameLabel.Content = consoleName;
                GameColumn.Children.Add(gameLabel);
                command.CommandText = "SELECT m.quantity, CASE WHEN m.price < 0 THEN 0 ELSE m.price END FROM games g INNER JOIN ("
                     + " SELECT game_id, quantity, price FROM multiplat_games WHERE platform_id = ("
                     + " SELECT id FROM platforms WHERE name = '" + consoleName + "'"
                     + "))m ON g.id = m.game_id;";

                sdr = command.ExecuteReader();
                Label gamecount = new Label();
                Label price = new Label();
                int tempValue = 0;
                int tempCount = 0;
                while (sdr.Read())
                {
                    if (!sdr.IsDBNull(0))
                    {
                        tempCount += sdr.GetInt32(0);
                        tempValue += sdr.GetInt32(0) * sdr.GetInt32(1);
                    }
                }
                gamecount.Content = tempCount;
                price.Content = tempValue;
                gameTotal += tempCount;
                priceTotal += tempValue;
                Count.Children.Add(gamecount);
                Price.Children.Add(price);
                sdr.Close();
            }
            quantityLabel.Content = "Total Games: " + gameTotal;
            valueLabel.Content = "Total Value: " + priceTotal;
            consoleLabel.Content = "Total Consoles: " + platformQty;


            conn.Close();
        }
    }
}