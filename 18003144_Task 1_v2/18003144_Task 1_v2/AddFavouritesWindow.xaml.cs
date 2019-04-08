using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace _18003144_Task_1_v2
{
    /// <summary>
    /// Interaction logic for EditFavouritesWindow.xaml
    /// </summary>
    public partial class AddFavouritesWindow : Window
    {
        Dictionary<string, City> NameCityDict;

        public AddFavouritesWindow()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;


        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            chooseBackground();
            NameCityDict = CityUtilities.getNameCityDict();

            txtCity.Focus();
        }

        private void chooseBackground()
        {
            string directoryPath = Directory.GetCurrentDirectory() + "/BackgroundImages/";
            int fileCount = Directory.GetFiles(directoryPath, "*", SearchOption.TopDirectoryOnly).Length;
            string imageName = Directory.GetFiles(directoryPath, "*", SearchOption.TopDirectoryOnly)[new Random().Next(fileCount)];
            ImageBrush backgroundBrush = new ImageBrush();
            Image image = new Image();
            image.Source = new BitmapImage(new Uri(@imageName));
            backgroundBrush.ImageSource = image.Source;
            grdMain.Background = backgroundBrush;
            backgroundBrush.Opacity = 0.3;

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            lstCities.Items.Clear();
            if (!txtCity.Text.Equals(""))
            {
                List<City> matchedCities = CityUtilities.SearchCities(txtCity.Text);
                foreach (var city in matchedCities)
                {
                    lstCities.Items.Add(city);
                }
            }
        }

        private void LstCities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!alreadyFavourite(((City)lstCities.SelectedItem).id))
            {
                string ids = "";
                using (StreamReader file = new StreamReader("Favourites.txt"))
                {
                    string line = file.ReadLine();
                    while (line != null)
                    {
                        ids += line;
                        line = file.ReadLine();
                    }
                }

                string newLine = (ids.Length > 0) ? ids + "," + ((City)lstCities.SelectedItem).id : ((City)lstCities.SelectedItem).id + "";
                using (StreamWriter file = new StreamWriter("Favourites.txt", false))
                {
                    file.Write(newLine);
                }
            }


            this.Close();
        }

        private bool alreadyFavourite(int id)
        {
            string ids = "";
            using (StreamReader file = new StreamReader("Favourites.txt"))
            {
                string line = file.ReadLine();
                while (line != null)
                {
                    ids += line;
                    line = file.ReadLine();
                }
            }

            return ids.Split(',').Contains(id+"");
        }
    }
}
