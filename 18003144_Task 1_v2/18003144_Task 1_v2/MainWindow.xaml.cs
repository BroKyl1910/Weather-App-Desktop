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

        User user;

        public MainWindow(User user)
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            this.user = user;
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            populateFavouriteCityIds(); // Get favourite city IDs from file
            codeCityDict = CityUtilities.getCityCodeDict(); // Get dictionary of City Codes to City objects

            createButtonsForCities();
             
            // If there is no favourites, show placeholders that tell users to add favourites
            if(stckpnlFavourites.Children.Count > 0)
            {
                //By default show forecast for first favourite city
                string firstId = ((Button)stckpnlFavourites.Children[0]).Name.Substring(6);
                showForecast(firstId);
            }
            else
            {
                showPlaceholders();
            }

            Console.WriteLine();
        }

        // Placeholders to tell user to add favourites
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

        // Loop through ID list and add buttons to the stackpanel for each city
        private void createButtonsForCities()
        {
            stckpnlFavourites.Children.Clear();
            //Using the favCityIds list, get objects of each favourite city and order them by name
            List<City> favCities = favCityIds.Select(id => codeCityDict[Convert.ToInt32(id)]).OrderBy(o=> o.name).ToList();

            // Make buttons
            for (int i = 0; i < favCities.Count; i++)
            {
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
            new AddFavouritesWindow(user, favCityIds).Show();
        }

        //Right clicking removes city from favourites
        private void favCityButton_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Button clickedCity = (Button)sender;
            string clickedId = clickedCity.Name.Substring(6);

            var confirmResult = MessageBox.Show("Are you sure you want to remove " + clickedCity.Content + " from your favourites?",
                                     "Remove Favourite",
                                     System.Windows.MessageBoxButton.YesNo);
            if (confirmResult == System.Windows.MessageBoxResult.No) return;

            //Remove city from list
            favCityIds.Remove(favCityIds.Where(f=> f.Equals(clickedId)).ToList()[0]);

            //Get current lines in file
            List<string> lines = File.ReadAllLines("Favourites.txt").ToList();

            //Remove line for user
            lines.Remove(lines.Where(line => line.Split(',')[0].Equals(user.Username)).ToList()[0]);

            //Recreate new line as username,id,id,id...
            string newLine = user.Username;
            foreach (var id in favCityIds)
            {
                newLine += "," + id;
            }

            lines.Add(newLine);

            //Write new line back to file
            using (StreamWriter file = new StreamWriter("Favourites.txt", false))
            {
                foreach(var line in lines)
                {
                    file.WriteLine(line);
                }
            }

            Window_Loaded(null, null);

        }

        // When left clicked, the forecast for the city is displayed
        private void favCity_Click(object sender, RoutedEventArgs e)
        {

            Button clickedCity = (Button)sender;
            string id = clickedCity.Name.Substring(6);

            showForecast(id);
  
        }

        private void showForecast(string id)
        {

            //Using OpenWeatherMap API, get current weather in city with specified ID
            APICurrentWeather currentWeather = APICurrentWeather.GetCurrentWeather(id);

            lblCity.Text = currentWeather.name + ", " + currentWeather.sys.country;
            lblMin.Text = Math.Round(currentWeather.main.temp_min) + " °C";
            lblMax.Text = Math.Round(currentWeather.main.temp_max) + " °C";
            lblDesc.Text = currentWeather.weather[0].main + " - " + currentWeather.weather[0].description;

            lblDate.Text = DateTime.Now.Date.ToLongDateString();
            lblWindSpeed.Text = currentWeather.wind.speed + " km/h";
            lblHumidity.Text = currentWeather.main.humidity + " %";
            lblPrecipitation.Text = currentWeather.GetRain() + " mm in last 3 hours";

            //Set background image based on weather
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

        //Decides if the weather report says anything about clouds
        private bool cloudy(APICurrentWeather currentWeather)
        {
            string desc = currentWeather.weather[0].description;
            return desc.Contains("cloud") || desc.Contains("rain") || desc.Contains("shower") || desc.Contains("storm") || desc.Contains("drizzle");
        }

        // Read all city IDs from file into list
        private void populateFavouriteCityIds()
        {
            favCityIds = new List<string>();

            //Gets all users favourites
            string[] lines = File.ReadAllLines("Favourites.txt");

            //Gets line where the username is equal to the user's username
            favCityIds = lines.Where(line => line.Split(',')[0].Equals(user.Username)).ToList()[0].Split(',').ToList();
            //Removes username from line
            favCityIds.RemoveAt(0);

        }

        private void Window_Activated(object sender, EventArgs e)
        {
            Window_Loaded(null,null);
        }

        private void BtnViewForecasts_Click(object sender, RoutedEventArgs e)
        {
            new ViewForecastsWindow(user).Show();
            this.Hide();
        }

        private void BtnAddForecast_Click(object sender, RoutedEventArgs e)
        {
            new CreateForecastWindow(user).Show();
            this.Hide();
        }

    }
}
