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
using System.Windows.Shapes;

namespace _18003144_Task_1_v2
{
    /// <summary>
    /// Interaction logic for CreateForecastWindow.xaml
    /// </summary>
    public partial class CreateForecastWindow : Window
    {
        Dictionary<string, City> NameCityDict;

        public CreateForecastWindow()
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
            chooseBackground(); //Randomly select background
            NameCityDict = CityUtilities.getNameCityDict(); // Get dictionary of City Name to City objects
            txtCity.Focus();
        }

        //Randomly select background
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

        //Search cities when text is changed
        private void TxtCity_TextChanged(object sender, TextChangedEventArgs e)
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

        //Auto fill textbox if city is selected
        private void LstCities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            City selectedCity = (City)lstCities.SelectedItem;
            if (selectedCity != null)
            {
                txtCity.Text = selectedCity.name + ", " + selectedCity.country;
                lstCities.SelectedIndex = 0;
            }
        }

        private void BtnHome_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Hide();
        }

        //Auto fill form with data from API
        private void BtnAutofill_Click(object sender, RoutedEventArgs e)
        {
            if (!checkValidInputsAutofill()) return;

            crdError.Visibility = Visibility.Hidden;

            APICurrentWeather currentWeather = GetCurrentWeather(((City)lstCities.SelectedItem).id + "");

            dtpDate.SelectedDate = DateTime.Now;
            sldMin.Value = currentWeather.main.temp_min;
            sldMax.Value = currentWeather.main.temp_max;
            sldWind.Value = currentWeather.wind.speed;
            sldHumidity.Value = currentWeather.main.humidity;
            sldPrecip.Value = currentWeather.GetRain();

        }

        //Make API call and return current weather object
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

        #region Slider and Textbox handling to make slider and textbox reflect each others values
        //Slider and text box handling
        private void SldMin_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            txtMin.Text = Math.Round(sldMin.Value) + "";
        }

        private void TxtMin_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!txtMin.Text.Equals(""))
            {
                sldMin.Value = Convert.ToInt16(txtMin.Text);
            }
            else
            {
                sldMin.Value = 0;
            }
        }

        private void SldMax_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            txtMax.Text = Math.Round(sldMax.Value) + "";
        }

        private void TxtMax_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!txtMax.Text.Equals(""))
            {
                sldMax.Value = Convert.ToInt16(txtMax.Text);
            }
            else
            {
                sldMax.Value = 0;
            }
        }

        private void SldWind_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            txtWind.Text = Math.Round(sldWind.Value) + "";
        }

        private void TxtWind_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!txtWind.Text.Equals(""))
            {
                sldWind.Value = Convert.ToInt16(txtWind.Text);
            }
            else
            {
                sldWind.Value = 0;
            }
        }

        private void SldHumidity_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            txtHumidity.Text = Math.Round(sldHumidity.Value) + "";
        }

        private void TxtHumidity_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!txtHumidity.Text.Equals(""))
            {
                sldHumidity.Value = Convert.ToInt16(txtHumidity.Text);
            }
            else
            {
                sldHumidity.Value = 0;
            }
        }

        private void SldPrecip_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            txtPrecip.Text = Math.Round(sldPrecip.Value) + "";
        }

        private void TxtPrecip_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!txtPrecip.Text.Equals(""))
            {
                sldPrecip.Value = Convert.ToInt16(txtPrecip.Text);
            }
            else
            {
                sldPrecip.Value = 0;
            }
        }
        #endregion 

        //If inputs are valid add forecast as new line in text file
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!checkValidInputsSave() || !unique()) return;

            crdError.Visibility = Visibility.Hidden;

            using (StreamWriter file = new StreamWriter("Forecasts.txt", true))
            {
                UserForecast forecast = new UserForecast(((City)lstCities.SelectedItem).id, (DateTime)dtpDate.SelectedDate, Convert.ToInt16(txtMin.Text), Convert.ToInt16(txtMax.Text), Convert.ToInt16(txtWind.Text), Convert.ToInt16(txtHumidity.Text), Convert.ToInt16(txtPrecip.Text));
                file.WriteLine(forecast.GetTextFileFormat());
                MessageBox.Show("Forecast added!");
                clearForm();
            }

        }

        //Check text file for forecast with same city and date
        private bool unique()
        {
            List<UserForecast> fc = FileUtilities.getForecastsFromFile();
            DateTime selectedDate = ((DateTime)dtpDate.SelectedDate).Date;
            int selectedCityID = ((City)lstCities.SelectedItem).id;
            //If there are any forecasts with the same city ID and forecast date
            if (fc.Where(f => f.CityID == selectedCityID && f.ForecastDate.Date == selectedDate).ToList().Count > 0)
            {
                crdError.Visibility = Visibility.Visible;
                lblError.Text = "Already have a forecast for this city on this date";
                return false;
            }
            return true;
        }

        //To save, there must be a city and date, and min < max
        private bool checkValidInputsSave()
        {
            //Checks city
            if (checkValidInputsAutofill())
            {
                if (dtpDate.SelectedDate == null)
                {
                    crdError.Visibility = Visibility.Visible;
                    lblError.Text = "Please select date";
                    return false;
                }
                if (sldMin.Value > sldMax.Value)
                {
                    crdError.Visibility = Visibility.Visible;
                    lblError.Text = "Minimum temperature cannot be higher than maximum temperature!";
                    return false;
                }

                return true;
            }

            return false;

        }

        //To autofill, there only needs to be a city selected
        private bool checkValidInputsAutofill()
        {
            if (lstCities.SelectedIndex < 0)
            {
                crdError.Visibility = Visibility.Visible;
                lblError.Text = "Please select city from list";
                return false;
            }
            return true;
        }

        //Rest form
        private void clearForm()
        {
            txtCity.Text = "";
            lstCities.SelectedIndex = -1;
            lstCities.Items.Clear();
            dtpDate.SelectedDate = DateTime.Now;
            sldMin.Value = 0;
            sldMax.Value = 0;
            sldWind.Value = 0;
            sldHumidity.Value = 0;
            sldPrecip.Value = 0;
        }

        private void BtnViewForecasts_Click(object sender, RoutedEventArgs e)
        {
            new ViewForecastsWindow().Show();
            this.Hide();
        }
    }
}
