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
        private int timeFrame= 0;
        public PopularInventory()
        {
            InitializeComponent();
        }
        private void Back(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void Generate(object sender, RoutedEventArgs e)
        {
            int casevalue = yearSelect.SelectedIndex;
            switch (casevalue)
            {
                case 0:
                    timeFrame = 30;
                    break;
                case 1:
                    timeFrame = 60;
                    break;
                case 2:
                    timeFrame = 90;
                    break;
                case 3:
                    timeFrame = 365;
                    break;
                case 4:
                    timeFrame = 730;
                    break;
            }
            InventoryFieldGeneration();
        }

        private void InventoryFieldGeneration()
        {
            SQLiteConnection conn = new SQLiteConnection(DATABASE);
            conn.Open();
            var command = conn.CreateCommand();

            GameColumn.Children.Clear();
            Qty.Children.Clear();
            Count.Children.Clear();
            Price.Children.Clear();

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
                + " where julianday('now')-julianday(t.time) < " + timeFrame + " order by Game DESC;";

            SQLiteDataReader sdr = command.ExecuteReader();
            List<String> tempgameNames = new List<String>();
            List<String> tempconsoleNames = new List<String>();
            List<int> tempgameQuantity = new List<int>();
            List<int> tempgameValue = new List<int>();
            List<String> gameNames = new List<String>();
            List<String> consoleNames = new List<String>();
            List<int> gameQuantity = new List<int>();
            List<int> gameValue = new List<int>();

            while (sdr.Read())
            {
                tempgameNames.Add(sdr.GetString(0));
                tempconsoleNames.Add(sdr.GetString(1));
                tempgameQuantity.Add(sdr.GetInt32(2));
                tempgameValue.Add(sdr.GetInt32(3));
            }
            sdr.Close();
            int tempQty = 0;
            bool flag = false;
            for(int i =0; i<tempgameNames.Count()-1 ; i++)
            {
                if(i== tempgameNames.Count() - 2)
                {
                    if (tempgameNames[i].Equals(tempgameNames[i + 1]) && tempconsoleNames[i].Equals(tempconsoleNames[i + 1]))
                    {
                        tempQty += tempgameQuantity[i] + tempgameQuantity[i + 1];
                        gameNames.Add(tempgameNames[i]);
                        consoleNames.Add(tempconsoleNames[i]);
                        gameQuantity.Add(tempQty);
                        gameValue.Add((tempgameValue[i]));
                    }
                    else
                    {
                        if (flag)
                        {
                            gameQuantity.Add(tempQty);
                        }
                        else
                        {
                            gameQuantity.Add(tempgameQuantity[i]);
                        }
                        gameNames.Add(tempgameNames[i]);
                        consoleNames.Add(tempconsoleNames[i]);
                        gameValue.Add((tempgameValue[i]));
                        gameNames.Add(tempgameNames[i+1]);
                        consoleNames.Add(tempconsoleNames[i+1]);
                        gameQuantity.Add(tempgameQuantity[i+1]);
                        gameValue.Add((tempgameValue[i+1]));
                        break;
                    }
                }
                else if(tempgameNames[i].Equals(tempgameNames[i+1]) && tempconsoleNames[i].Equals(tempconsoleNames[i + 1]))
                {
                    flag = true;
                    tempQty += tempgameQuantity[i] + tempgameQuantity[i + 1];
                }
                else if (flag)
                {
                    gameNames.Add(tempgameNames[i]);
                    consoleNames.Add(tempconsoleNames[i]);
                    gameQuantity.Add(tempQty);
                    gameValue.Add((tempgameValue[i]));
                    flag = false;
                    tempQty = 0;
                }
                else
                {
                    gameNames.Add(tempgameNames[i]);
                    consoleNames.Add(tempconsoleNames[i]);
                    gameQuantity.Add(tempgameQuantity[i]);
                    gameValue.Add((tempgameValue[i]));

                }
            }

            int tmpQ = 0;
            int tmpP = 0;
            String tmpG;
            String tmpC;
            for (int k = 0; k < gameNames.Count()-1; k++)
            {
                for (int i = 0; i < gameNames.Count()-1; i++)
                {
                    if (gameQuantity[i + 1] > gameQuantity[i])
                    {
                        tmpQ = gameQuantity[i];
                        tmpP = gameValue[i];
                        tmpG = gameNames[i];
                        tmpC = consoleNames[i];

                        gameQuantity[i] = gameQuantity[i + 1];
                        gameValue[i] = gameValue[i + 1];
                        gameNames[i] = gameNames[i + 1];
                        consoleNames[i] = consoleNames[i + 1];

                        gameQuantity[i + 1] = tmpQ;
                        gameValue[i + 1] = tmpP;
                        gameNames[i + 1] = tmpG;
                        consoleNames[i + 1] = tmpC;
                    }
                }
            }

            for(int i =0; i<gameNames.Count; i++)
            {
                Label name = new Label();
                Label platform = new Label();
                Label quantity = new Label();
                Label price = new Label();

                name.Content = gameNames[i];
                platform.Content = consoleNames[i];
                quantity.Content = gameQuantity[i];
                price.Content = gameValue[i];

                GameColumn.Children.Add(name);
                Count.Children.Add(platform);
                Qty.Children.Add(quantity);
                Price.Children.Add(price);

            }

            conn.Close();
        }
    }
}
