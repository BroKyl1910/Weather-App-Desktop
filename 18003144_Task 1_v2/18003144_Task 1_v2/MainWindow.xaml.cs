using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

namespace _18003144_Task_1_v2
{
    public partial class MainWindow : Window
    {
        List<string> favCityIds;
        private Dictionary<int, City> codeCityDict;

        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

        }
        private void Window_Closed(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            populateFavouriteCityIds();
            codeCityDict = CityUtilities.getCityCodeDict();

            createButtonsForCities();

            if(stckpnlFavourites.Children.Count > 0)
            {
                string firstId = ((Button)stckpnlFavourites.Children[0]).Name.Substring(6);
                showForecast(firstId);
            }
            else
            {
                showPlaceholders();
            }

            Console.WriteLine();
        }

        private void showPlaceholders()
        {
            lblCity.Text = "Add Favourites";
            lblMin.Text = "NA";
            lblMax.Text = "NA";
            lblDesc.Text = "Weather Description";

            lblDate.Text = "NA";
            lblWindSpeed.Text = "NA";
            lblHumidity.Text = "NA";
            lblPrecipitation.Text = "NA";

            grdMain.Background = null;
        }

        private void createButtonsForCities()
        {
            stckpnlFavourites.Children.Clear();
            List<City> favCities = favCityIds.Select(id => codeCityDict[Convert.ToInt32(id)]).OrderBy(o=> o.name).ToList();

            for (int i = 0; i < favCities.Count; i++)
            {
                //int id = favCities[i].id;
                Button newButton = new Button();
                newButton.Content = favCities[i].name;
                newButton.Margin = new Thickness(10, 0, 0, 0);
                newButton.HorizontalAlignment = HorizontalAlignment.Left;
                newButton.Foreground = new SolidColorBrush(Colors.White);
                newButton.Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
                newButton.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 193, 193, 193));
                newButton.Height = double.NaN;

                newButton.Name = "btnFav" + favCities[i].id;
                newButton.Click += new RoutedEventHandler(favCity_Click);
                newButton.MouseRightButtonUp += favCityButton_MouseRightButtonUp;
                newButton.ToolTip = "Left click to view forecast or right-click to remove from favourites";
                stckpnlFavourites.Children.Add(newButton);
            }
                        

        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            new AddFavouritesWindow().Show();
        }

        private void favCityButton_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Button clickedCity = (Button)sender;
            string id = clickedCity.Name.Substring(6);

            var confirmResult = MessageBox.Show("Are you sure you want to remove " + clickedCity.Content + " from your favourites?",
                                     "Remove Favourite",
                                     System.Windows.MessageBoxButton.YesNo);
            if (confirmResult == System.Windows.MessageBoxResult.No) return;



            string[] linesInOldFile = System.IO.File.ReadAllLines("Favourites.txt");
            List<string> idsForNewFile = new List<string>();

            //Get list of IDs to write back
            foreach (string line in linesInOldFile)
            {
                var ids = line.Split(',');
                foreach (var item in ids)
                {
                    if (!item.Equals(id))
                    {
                        idsForNewFile.Add(item);
                    }
                }
            }

            using (System.IO.StreamWriter file = new System.IO.StreamWriter("Favourites.txt", false))
            {
                string line="";
                foreach (string _id in idsForNewFile)
                {
                    line+=_id+",";
                }
                if(line.Length > 0) line = line.Substring(0, line.Length - 1);
                file.Write(line);
            }

            Window_Loaded(null, null);

        }

        private void favCity_Click(object sender, RoutedEventArgs e)
        {

            Button clickedCity = (Button)sender;
            string id = clickedCity.Name.Substring(6);

            showForecast(id);
  
        }

        private void showForecast(string id)
        {
            lblLoading.Visibility = Visibility.Visible;

            APICurrentWeather currentWeather = GetCurrentWeather(id);

            lblLoading.Visibility = Visibility.Hidden;

            lblCity.Text = currentWeather.name + ", " + currentWeather.sys.country;
            lblMin.Text = Math.Round(currentWeather.main.temp_min) + " °C";
            lblMax.Text = Math.Round(currentWeather.main.temp_max) + " °C";
            lblDesc.Text = currentWeather.weather[0].main + " - " + currentWeather.weather[0].description;

            lblDate.Text = DateTime.Now.Date.ToLongDateString();
            lblWindSpeed.Text = currentWeather.wind.speed + " km/h";
            lblHumidity.Text = currentWeather.main.humidity + " %";
            lblPrecipitation.Text = currentWeather.GetRain() + " mm in last 3 hours";

            string src = "";
            if (cloudy(currentWeather))
            {
                src = "cloudy.jpg";
            }
            else
            {
                src = "sunny.jpg";
            }
            ImageBrush backgroundBrush = new ImageBrush();
            Image image = new Image();
            image.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/BackgroundImages/" + src));
            backgroundBrush.ImageSource = image.Source;
            grdMain.Background = backgroundBrush;
            backgroundBrush.Opacity = 0.3;

            this.UpdateLayout();
        }

        private bool cloudy(APICurrentWeather currentWeather)
        {
            string desc = currentWeather.weather[0].description;
            return desc.Contains("cloud") || desc.Contains("rain") || desc.Contains("shower") || desc.Contains("storm") || desc.Contains("drizzle");
        }

        private void populateFavouriteCityIds()
        {
            favCityIds = new List<string>();

            string[] lines = System.IO.File.ReadAllLines("Favourites.txt");

            foreach (string line in lines)
            {
                var ids = line.Split(',');
                foreach (var id in ids)
                {
                    favCityIds.Add(id);
                }
            }
        }

        private APICurrentWeather GetCurrentWeather(string id)
        {
            string responseString = string.Empty;
            Uri uri = new Uri(@"http://api.openweathermap.org/data/2.5/weather?id=" + id + "&units=metric&APPID=1146a3547ac18a07b0cdfe6894520297");

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip;
            request.UserAgent = "12345";

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                responseString = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<APICurrentWeather>(responseString);
            }
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            Window_Loaded(null,null);
        }

        private void BtnViewForecasts_Click(object sender, RoutedEventArgs e)
        {
            new ViewForecastsWindow().Show();
            this.Hide();
        }

        private void BtnAddForecast_Click(object sender, RoutedEventArgs e)
        {
            new CreateForecastWindow().Show();
            this.Hide();
        }

    }
}
