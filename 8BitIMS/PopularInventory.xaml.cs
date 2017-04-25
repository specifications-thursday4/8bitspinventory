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
    /// Interaction logic for PopularInventory.xaml
    /// </summary>
    public partial class PopularInventory : Page
    {
        private static string DATABASE = "Data Source = inventory.db";
        private int timeFrame;
        public PopularInventory()
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
            Label conName = new Label();
            Label platCount = new Label();
            Label priceLabel = new Label();

            gameColLabel.Content = "Game";
            conName.Content = "Console";
            platCount.Content = "Quantity Sold";
            priceLabel.Content = "Total Value";


            gameColLabel.FontWeight = FontWeights.ExtraBold;
            conName.FontWeight = FontWeights.ExtraBold;
            platCount.FontWeight = FontWeights.ExtraBold;
            priceLabel.FontWeight = FontWeights.ExtraBold;

            GameColumn.Children.Add(gameColLabel);
            Count.Children.Add(conName);
            Qty.Children.Add(platCount);
            Price.Children.Add(priceLabel);

            command.CommandText = "SELECT s.game, s.plat, t.quantity, CASE WHEN s.price < 0 THEN 0"
                + " ELSE s.price END from transactions t "
                + " inner join (SELECT g.name game, b.name plat, b.price, b.game_id, b.platform_id "
                + " FROM games g INNER JOIN(SELECT p.name, m.price, game_id, platform_id from "
                + " multiplat_games m inner join platforms p on p.id = m.platform_id) b on "
                + " g.id = b.game_id) s on s.game_id = t.game_id and s.platform_id = t.platform_id"
                + " where julianday('now')-julianday(t.time) < " + timeFrame + " order by t.quantity DESC;";

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
                Qty.Children.Add(platcount);
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



            conn.Close();
        }
    }
}
