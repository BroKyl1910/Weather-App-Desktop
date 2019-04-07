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
    /// Interaction logic for EditForecastWindow.xaml
    /// </summary>
    public partial class EditForecastWindow : Window
    {
        Dictionary<string, City> NameCityDict;
        Dictionary<int, City> cityCodeDict;
        UserForecast loadedForecast;

        public EditForecastWindow(UserForecast forecast)
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            NameCityDict = new Dictionary<string, City>();
            cityCodeDict = new Dictionary<int, City>();

            loadedForecast = forecast;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            chooseBackground();
            NameCityDict = CityUtilities.getNameCityDict();
            cityCodeDict = CityUtilities.getCityCodeDict();
            txtCity.Focus();

            txtCity.Text = cityCodeDict[loadedForecast.CityID].name + ", " + cityCodeDict[loadedForecast.CityID].country;
            lstCities.SelectedItem = cityCodeDict[loadedForecast.CityID];
            lstCities.SelectedIndex = 0;
            dtpDate.SelectedDate = loadedForecast.ForecastDate;
            sldMin.Value = loadedForecast.MinimumTemp;
            sldMax.Value = loadedForecast.MaximumTemp;
            sldWind.Value = loadedForecast.WindSpeed;
            sldHumidity.Value = loadedForecast.Humidity;
            sldPrecip.Value = loadedForecast.Precipitation;
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

        private void TxtCity_TextChanged(object sender, TextChangedEventArgs e)
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
            txtCity.Text = ((City)lstCities.SelectedItem).name + ", " + ((City)lstCities.SelectedItem).country;
        }

        private void BtnHome_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Hide();
        }

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

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!checkValidInputsSave() || !unique()) return;
            //if (!checkValidInputsSave()) return;

            crdError.Visibility = Visibility.Hidden;

            var forecasts = FileUtilities.getForecastsFromFile();

            //int oldForecastIndex = forecasts.IndexOf(loadedForecast);

            //forecasts[oldForecastIndex] = new UserForecast(((City)lstCities.SelectedItem).id, (DateTime) dtpDate.SelectedDate, Convert.ToInt16(txtMin.Text), Convert.ToInt16(txtMax.Text), Convert.ToInt16(txtWind.Text), Convert.ToInt16(txtHumidity.Text), Convert.ToInt16(txtPrecip.Text));


            forecasts[forecasts.FindIndex(f => f.CityID == loadedForecast.CityID && f.ForecastDate.Date.Equals(loadedForecast.ForecastDate.Date))] = new UserForecast(((City)lstCities.SelectedItem).id, (DateTime)dtpDate.SelectedDate, Convert.ToInt16(txtMin.Text), Convert.ToInt16(txtMax.Text), Convert.ToInt16(txtWind.Text), Convert.ToInt16(txtHumidity.Text), Convert.ToInt16(txtPrecip.Text));

            using (StreamWriter file = new StreamWriter("Forecasts.txt", false))
            {
                foreach(var fc in forecasts)
                {
                    file.WriteLine(fc.GetTextFileFormat());
                }
                
                
            }
            MessageBox.Show("Forecast edited!");

            new ViewForecastsWindow().Show();
            this.Hide();

        }

        private bool unique()
        {
            List<UserForecast> fc = FileUtilities.getForecastsFromFile();
            DateTime selectedDate = ((DateTime)dtpDate.SelectedDate).Date;
            int selectedCityID = ((City)lstCities.SelectedItem).id;
            if (fc.Where(f => f.CityID == selectedCityID && f.ForecastDate.Date == selectedDate.Date && f.ForecastDate.Date != loadedForecast.ForecastDate.Date).ToList().Count > 0)
            {
                crdError.Visibility = Visibility.Visible;
                lblError.Text = "Already have a forecast for this city on this date";
                return false;
            }
            return true;
        }

        private bool checkValidInputsSave()
        {
            if (checkValidInputsAutofill())
            {
                if (dtpDate.SelectedDate == null)
                {
                    crdError.Visibility = Visibility.Visible;
                    lblError.Text = "Please select date";
                    return false;
                }

                return true;
            }

            return false;

        }

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

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var forecasts = FileUtilities.getForecastsFromFile();

            
            int index = forecasts.FindIndex(f => f.CityID == loadedForecast.CityID && f.ForecastDate.Date.Equals(loadedForecast.ForecastDate.Date));
            forecasts.RemoveAt(index);

            using (StreamWriter file = new StreamWriter("Forecasts.txt", false))
            {
                foreach (var fc in forecasts)
                {
                    file.WriteLine(fc.GetTextFileFormat());
                }


            }
            MessageBox.Show("Forecast deleted!");

            new ViewForecastsWindow().Show();
            this.Hide();
        }

        private void DtpDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            //dtpDate.SelectedDate = loadedForecast.ForecastDate;
        }
    }
}
