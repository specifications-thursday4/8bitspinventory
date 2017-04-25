using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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
    /// Interaction logic for POSMainPage.xaml
    /// </summary>
    public partial class POSMainPage : Page
    {
        List<ImageKey> imageKeyList = new List<ImageKey>();
        List<string> logoKeyList = new List<string>();
       static int b1 = 0;
       static int b2 = 1;
       static int b3 = 2;
        //SoundPlayer player = new SoundPlayer("fireball.wav"); // fireball haha
        //SoundPlayer player2 = new SoundPlayer("kick.wav"); // fireball haha

        public POSMainPage()
        {
            InitializeComponent();

            // resourcekey for the image , which are in the xaml file under windows.resource
            imageKeyList.Add(new ImageKey("pc", "PC (Microsoft Windows)"));
            imageKeyList.Add(new ImageKey("wii", "Wii"));
            imageKeyList.Add(new ImageKey("wiiu", "Wii U"));
            imageKeyList.Add(new ImageKey("xbox360", "Xbox 360"));
            imageKeyList.Add(new ImageKey("xboxone", "Xbox One"));
            imageKeyList.Add(new ImageKey("ps", "PlayStation"));
            imageKeyList.Add(new ImageKey("ps2", "PlayStation 2"));
            imageKeyList.Add(new ImageKey("ps3", "PlayStation 3"));
            imageKeyList.Add(new ImageKey("ps4", "PlayStation 4"));
            imageKeyList.Add(new ImageKey("psv", "PlayStation Vita"));
            imageKeyList.Add(new ImageKey("psp", "PlayStation Portable"));
            imageKeyList.Add(new ImageKey("ngc", "Nintendo GameCube"));
            imageKeyList.Add(new ImageKey("n3ds", "Nintendo 3DS"));
            imageKeyList.Add(new ImageKey("n64", "Nintendo 64"));
            imageKeyList.Add(new ImageKey("ios", "iOS"));
            imageKeyList.Add(new ImageKey("xbox","Xbox")); 
            
         //initialize the buttopns
            button1.Content = FindResource(imageKeyList[b1].getImageKey());
            button2.Content = FindResource(imageKeyList[b2].getImageKey());
            button3.Content = FindResource(imageKeyList[b3].getImageKey());


            for(int i=1; i <= 12; i++) {
                logoKeyList.Add("logo" + i.ToString());
            }

           

        }
        private void ViewPop(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("PopularInventory.xaml", UriKind.Relative));
        }

        private void ClickViewCart(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("ViewCartPage.xaml", UriKind.Relative));
        }

        private void ClickCategory(object sender, RoutedEventArgs e)
        {
            Button btnClicked = (Button)sender;
            MainWindow.tableNameFromButtonClickedInPage = (String)btnClicked.Content;

            this.NavigationService.Navigate(new Uri("ViewCategoryPage.xaml", UriKind.Relative));
        }
        // mouse listener can be implemented in the future 
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //player2.Play();
            MainWindow.tableNameFromButtonClickedInPage = imageKeyList[b1].getConsoleName();
            this.NavigationService.Navigate(new Uri("ViewCategoryPage.xaml", UriKind.Relative));
          
        
    }

        private void button1_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void button1_MouseLeave(object sender, MouseEventArgs e)
        {

        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            //player2.Play();
            MainWindow.tableNameFromButtonClickedInPage = imageKeyList[b2].getConsoleName();
            this.NavigationService.Navigate(new Uri("ViewCategoryPage.xaml", UriKind.Relative));
           
        }

        private void button2_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void button2_MouseLeave(object sender, MouseEventArgs e)
        {

        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            //player2.Play();
            MainWindow.tableNameFromButtonClickedInPage = imageKeyList[b3].getConsoleName();
            this.NavigationService.Navigate(new Uri("ViewCategoryPage.xaml", UriKind.Relative));
            
        }

        private void button3_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void button3_MouseLeave(object sender, MouseEventArgs e)
        {

        }

        private void Canvas_MouseWheel_1(object sender, MouseWheelEventArgs e)
        {
            //player.Play();
            if (e.Delta < 0)      // scroll toward user
            {
                logorotateCC();
                b1 = (imageKeyList.Count + (b1-1)) % imageKeyList.Count;
                b2 = (imageKeyList.Count + (b2-1)) % imageKeyList.Count;
                b3 = (imageKeyList.Count + (b3-1)) % imageKeyList.Count;
                button1.Content = FindResource(imageKeyList[b1].getImageKey());
                button2.Content = FindResource(imageKeyList[b2].getImageKey());
                button3.Content = FindResource(imageKeyList[b3].getImageKey());

            }
            else       // scroll away user
            {
                logorotateC();
                b1 = (imageKeyList.Count + (b1+1)) % imageKeyList.Count;
                b2 = (imageKeyList.Count + (b2+1)) % imageKeyList.Count;
                b3 = (imageKeyList.Count + (b3+1)) % imageKeyList.Count;
                button1.Content = FindResource(imageKeyList[b1].getImageKey());
                button2.Content = FindResource(imageKeyList[b2].getImageKey());
                button3.Content = FindResource(imageKeyList[b3].getImageKey());

            }


        }

        private void logo_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("MainPage.xaml", UriKind.Relative));
        }

        private void logorotateC()                          //clockwise
        {
            int index = 0;
            foreach (string str in logoKeyList)
            {
                if (logo.Content.GetHashCode() == FindResource(str).GetHashCode())
                {
                    index = logoKeyList.IndexOf(str);
                }

            }
            if (index == 11)
            {
                index = 0;
                logo.Content = FindResource(logoKeyList[index]);
            }
            else
            {
                logo.Content = FindResource(logoKeyList[index + 1]);
            }


        }

        private void logorotateCC()                     //counterclockwise
        {
            int index = 0;
            foreach (string str in logoKeyList)
            {
                if (logo.Content.GetHashCode() == FindResource(str).GetHashCode())
                {
                    index = logoKeyList.IndexOf(str);

                }


            }
            if (index == 0)
            {
                index = 12;
                logo.Content = FindResource(logoKeyList[index - 1]);
            }
            else
            {
                logo.Content = FindResource(logoKeyList[index - 1]);
            }
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("MainPage.xaml", UriKind.Relative));
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
    public class ImageKey {
        private string imageKey;
        private string ConsoleName;
        public ImageKey(string imageKey, string ConsoleName)
        {
            this.imageKey = imageKey;
            this.ConsoleName = ConsoleName;
        }
        
        public string getImageKey()
        {
            return this.imageKey;
        }
        public string getConsoleName()
        {
            return this.ConsoleName;
        }
    }


}
