using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Data.SQLite;

namespace _8BitIMS
{
    /// <summary>
    /// DatabaseInit initializes the database by setting up all the necessary tables, and 
    /// then pulls data from IGDB and inserts the necessary platforms and games into the database.
    /// </summary>
    class DatabaseUpdate
    {
        private static string DATABASE = "Data Source = inventory.db";
        private static string URL_GAMES = "https://igdbcom-internet-game-database-v1.p.mashape.com/games/?fields=id,name&limit=50&offset=";
        private static string URL_PLATS = "https://igdbcom-internet-game-database-v1.p.mashape.com/platforms/?fields=id,name,games&limit=50&offset=";
        private static string APIKEY = "77DAy1j2rVmsh9SNYVKUuEQOjmpZp1yRFoqjsn93JPhTd4z7zk";
        private static string ACCEPT = "application/json";
        private List<Games> gamesList = new List<Games>();              // List of all game data from the IGDB
        private List<Platforms> platsList = new List<Platforms>();      // List of all platform data from IGDB


        public DatabaseUpdate()
        {
            gatherData();
            populateData();
        }

        /// <summary>
        /// Loops through each page from the Internet Games Database (IGDB) and pulls data in the form of JSON
        /// and parses it into C# objects to be queried into SQL database for use within the application. 
        /// </summary>
        private void gatherData()
        {


            WebClient wc = new WebClient();                         // Webclient is used to scrape from a URL                
            JObject data = null;                                    // Retains the data from the URL in the form of JTokens
            string response;                                        // Response from the URL
            wc.Headers.Add("X-Mashape-Key", APIKEY);                // Adding the Headers to the WebClient - API token
            wc.Headers.Add("Accept", ACCEPT);                       // Telling the WebClient what to accept from the URL



            const int MAX_PULL = 9900;
            const int MAX_OFFSET = 50;
            int x = MAX_OFFSET;                                     // Offset value for pagination while getting data from IGDB
            //int y = 0;                                              // Value for terminating loops

            do
            {
                response = wc.DownloadString(URL_PLATS + x);
                if (response.StartsWith("["))
                {
                    // y = 0;
                    //while (y < MAX_OFFSET)
                    for (int i = 0; i < MAX_OFFSET; i++)
                    {
                        try
                        {
                            Platforms platform = new Platforms();
                            data = JsonConvert.DeserializeObject<List<JObject>>(response)[i];
                            platform.id = (int)data["id"];
                            platform.name = (string)data["name"];
                            var tokenObj = data["games"];
                            platform.games = tokenObj.ToObject<List<int>>();
                            platsList.Add(platform);
                            //y++;


                            Console.WriteLine(data["name"]);
                        }
                        catch (ArgumentOutOfRangeException e)
                        {
                            break;
                        }
                    }
                }
                x += MAX_OFFSET;
            } while (!response.StartsWith("[]"));

            //x = y = 0;
            //while (x <= MAX_PULL)
            for (int i = 0; i <= MAX_PULL; i += MAX_OFFSET)
            {
                response = wc.DownloadString(URL_GAMES + i);
                if (response.StartsWith("["))
                {
                    //y = 0;
                    //while (y < MAX_OFFSET)
                    for (int j = 0; j < MAX_OFFSET; j++)
                    {
                        Games game = new Games();
                        data = JsonConvert.DeserializeObject<List<JObject>>(response)[j];
                        game.id = (int)data["id"];
                        game.name = (string)data["name"];
                        gamesList.Add(game);
                        //y++;

                        Console.WriteLine(data["name"]);
                    }
                    //x += MAX_OFFSET;

                }

            }

        }


        private void populateData()
        {
            SQLiteConnection conn = new SQLiteConnection(DATABASE); // Sets up a new database connection
            conn.Open();                                            // Opens the database for queries
            var command = conn.CreateCommand();                     // Creates a command variable for SQL commands

            Console.WriteLine("Inserting games into database");
            foreach (Games game in gamesList)
            {
                Console.WriteLine("Inserting " + game.name + " into games table");
                command.CommandText = "INSERT INTO games(id, name, quantity) VALUES("
                            + game.id + ",'" + game.name.Replace("'", "''") + "', 0);";
                command.ExecuteNonQuery();
            }

            Console.WriteLine("Inserting platforms and games-by-platfrom into database");
            foreach (Platforms plat in platsList)
            {
                Console.WriteLine("Inserting " + plat.name + " into platform table");
                command.CommandText = "INSERT INTO platforms(id, name, quantity) VALUES("
                    + plat.id + ",'" + plat.name.Replace("'", "''") + "', 0);";
                command.ExecuteNonQuery();

                foreach (int i in plat.games)
                    foreach (Games title in gamesList)
                    {
                        if (title.id == i)
                        {
                            Console.WriteLine("Inserting " + title.name + " into multiplat_games table under " + plat.name);
                            command.CommandText = "INSERT INTO multiplat_games(game_id, platform_id, quantity) VALUES("
                                + title.id + "," + plat.id + ", 0);";
                            command.ExecuteNonQuery();
                        }
                    }
            }

            conn.Close();

        }
    }




}
