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
    /// Interaction logic for InventoryPage.xaml
    /// </summary>
    public partial class InventoryPage : Page
    {
        private static string DATABASE = "Data Source = inventory.db";
        private TextBox[] countBoxQ;
        private TextBox[] countBoxP;
        private int platID;
        public InventoryPage()
        {
            InitializeComponent();
            InventoryFieldGeneration(MainWindow.tableNameFromButtonClickedInPage);
        }

        private void InventoryFieldGeneration(string tableName)
        {
            SQLiteConnection conn = new SQLiteConnection(DATABASE);
            conn.Open();
            var command = conn.CreateCommand();
            consoleLabel.Content = tableName;
            command.CommandText = "SELECT id FROM platforms WHERE name = '" + tableName + "'";
            platID = (int)command.ExecuteScalar();
            Random rand = new Random();


            Label gameColLabel = new Label();
            Label count = new Label();
            Label qty = new Label();
            Label priceLabel = new Label();
            Label CurrPrice = new Label();

            BG.Background = Brushes.CornflowerBlue;
            BG2.Background = MainWindow.colourArr[rand.Next() % MainWindow.colourArr.Length];

            gameColLabel.Content = "Console";
            count.Content = "Count";
            qty.Content = "Qty.";
            priceLabel.Content = "Price";
            CurrPrice.Content = "Price";

            gameColLabel.FontWeight = FontWeights.ExtraBold;
            count.FontWeight = FontWeights.ExtraBold;
            qty.FontWeight = FontWeights.ExtraBold;
            priceLabel.FontWeight = FontWeights.ExtraBold;
            CurrPrice.FontWeight = FontWeights.ExtraBold;

            GameColumn.Children.Add(gameColLabel);
            Count.Children.Add(count);
            Qty.Children.Add(qty);
            Price.Children.Add(priceLabel);
            CurrentPrice.Children.Add(CurrPrice);

            command.CommandText = "SELECT name, quantity, CASE WHEN price < 0 THEN 0 ELSE price END FROM platforms where name= '"
                + tableName + "';";
            SQLiteDataReader sdr = command.ExecuteReader();
            sdr.Read();
            Label gameLabel = new Label();
            Label quantity = new Label();
            Label price = new Label();

            gameLabel.Content = sdr.GetString(0);
            quantity.Content = sdr.GetInt32(1);
            price.Content = sdr.GetInt32(2);


            GameColumn.Children.Add(gameLabel);
            Qty.Children.Add(quantity);
            CurrentPrice.Children.Add(price);
            sdr.Close();

            command.CommandText = "SELECT g.name, m.quantity, CASE WHEN m.price < 0 THEN 0 ELSE m.price END FROM games g INNER JOIN ("
               + " SELECT game_id, quantity, price FROM multiplat_games WHERE platform_id = ("
               + " SELECT id FROM platforms WHERE name = '" + tableName + "'"
               + "))m ON g.id = m.game_id ORDER BY g.name ASC;";

            sdr = command.ExecuteReader();

            Label gametitleLabel = new Label();
            qty = new Label();
            CurrPrice = new Label();
            gametitleLabel.Content = "Game Title";
            qty.Content = "";
            CurrPrice.Content = "";

            gametitleLabel.FontWeight = FontWeights.ExtraBold;

            GameColumn.Children.Add(gametitleLabel);
            Qty.Children.Add(qty);
            CurrentPrice.Children.Add(CurrPrice);
            int fieldcount = 0;
            while (sdr.Read())
            {
                gameLabel = new Label();
                quantity = new Label();
                price = new Label();

                gameLabel.Content = sdr.GetString(0);
                quantity.Content = sdr.GetInt32(1);
                price.Content = sdr.GetInt32(2);


                GameColumn.Children.Add(gameLabel);
                Qty.Children.Add(quantity);
                CurrentPrice.Children.Add(price);
                fieldcount++;
            }
            sdr.Close();
            countBoxQ = new TextBox[fieldcount + 1];
            countBoxP = new TextBox[fieldcount + 1];


            for (int i = 0; i < countBoxQ.Length; i++)
            {

                countBoxQ[i] = new TextBox();
                countBoxP[i] = new TextBox();
                countBoxQ[i].Margin = new Thickness(0, 5, 0, 3);
                countBoxP[i].Margin = new Thickness(0, 5, 0, 3);

                Count.Children.Add(countBoxQ[i]);
                Price.Children.Add(countBoxP[i]);
                if (i == 0)
                {
                    priceLabel = new Label();
                    CurrPrice = new Label();
                    priceLabel.Content = "";
                    CurrPrice.Content = "";
                    Count.Children.Add(priceLabel);
                    Price.Children.Add(CurrPrice);
                }
            }


            conn.Close();
        }

        private void UpdateInventory()
        {
            SQLiteConnection conn = new SQLiteConnection(DATABASE);
            conn.Open();
            var command = conn.CreateCommand();

            IEnumerable<Label> indexedLabels = GameColumn.Children.Cast<Label>();
            Label[] labels = indexedLabels.ToArray<Label>();


            for (int i = 0, j = 1; i < countBoxQ.Length; i++, j++)
            {
                if (countBoxQ[i].Text != null)
                {
                    int result;
                    Int32.TryParse(countBoxQ[i].Text, out result);
                    if (result != 0)
                    {
                        if (i == 0)
                        {
                            command.CommandText = "UPDATE platforms"
                                + " SET quantity = quantity + " + result
                                + " WHERE id=" + platID + ";";
                            command.ExecuteNonQuery();
                        }
                        else
                        {
                            command.CommandText = "UPDATE multiplat_games"
                                + " SET quantity = quantity + " + result
                                + " WHERE game_id = ("
                                + " SELECT id FROM games WHERE name = '" + labels[j + 1].Content.ToString().Replace("'", "''") + "'"
                                + ") AND platform_id = " + platID + ";";
                            command.ExecuteNonQuery();
                        }
                    }
                }
                if (countBoxP[i].Text != null)
                {
                    int result;
                    Int32.TryParse(countBoxP[i].Text, out result);
                    if (result != 0)
                    {
                        if (i == 0)
                        {
                            command.CommandText = "UPDATE platforms"
                                + " SET price = " + result
                                + " WHERE id=" + platID + ";";
                            command.ExecuteNonQuery();
                        }
                        else
                        {
                            command.CommandText = "UPDATE multiplat_games"
                                + " SET price = " + result
                                + " WHERE game_id = ("
                                + " SELECT id FROM games WHERE name = '" + labels[j + 1].Content.ToString().Replace("'", "''") + "'"
                                + ") AND platform_id = " + platID + ";";
                            command.ExecuteNonQuery();
                        }
                    }
                }

            }
            RefreshPage();
            conn.Close();

        }

        private void Back(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("UpdateInventory.xaml", UriKind.Relative));
        }

        private void Submit(object sender, RoutedEventArgs e)
        {
            UpdateInventory();
        }

        private void RefreshPage()
        {
            GameColumn.Children.Clear();
            Qty.Children.Clear();
            Count.Children.Clear();
            Price.Children.Clear();
            CurrentPrice.Children.Clear();
            InventoryFieldGeneration(MainWindow.tableNameFromButtonClickedInPage);
        }




    }
}