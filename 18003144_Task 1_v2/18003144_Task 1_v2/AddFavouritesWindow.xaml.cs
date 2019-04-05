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
            NameCityDict = new Dictionary<string, City>();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            chooseBackground();
            NameCityDict = CityUtilities.getNameCityDict();

            txtCity.Focus();
        }

        //private void populateDictionaryOfCities()
        //{
        //    CityFile cityFile;
        //    using (StreamReader file = File.OpenText("cities.json"))
        //    {
        //        JsonSerializer serializer = new JsonSerializer();
        //        cityFile = (CityFile)serializer.Deserialize(file, typeof(CityFile));
        //    }

        //    foreach (City city in cityFile.cities)
        //    {
        //        NameCityDict.Add(city.name + ", " + city.country + ", " + city.id, city);
        //    }

        //}

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
            List<City> matchedCities = SearchCities(txtCity.Text);
            lstCities.Items.Clear();
            foreach (var city in matchedCities)
            {
                lstCities.Items.Add(city);
            }
        }

        private List<City> SearchCities(string cityName)
        {
            return NameCityDict.Where(q => q.Key.ToLower().Contains(cityName.ToLower())).Select(q => q.Value).ToList<City>();
        }

        private void LstCities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string ids="";
            using (StreamReader file = new StreamReader("Favourites.txt"))
            {
                string line = file.ReadLine();
                while(line != null){
                    ids += line;
                    line = file.ReadLine();
                }
            }

            string newLine = (ids.Length > 0)?ids + "," + ((City)lstCities.SelectedItem).id:((City)lstCities.SelectedItem).id+"";
            using (StreamWriter file = new StreamWriter("Favourites.txt", false))
            {
                file.Write(newLine);
            }

            this.Close();
        }
    }
}
