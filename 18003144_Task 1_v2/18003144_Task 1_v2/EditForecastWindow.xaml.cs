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

            loadedForecast = forecast;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            grdMain.Background = FileUtilities.ChooseBackground(); //Randomly select background
            NameCityDict = CityUtilities.getNameCityDict(); //Get dictionary of City Names to City Objects
            cityCodeDict = CityUtilities.getCityCodeDict(); //Get dictionary of City IDs to City Objects
            txtCity.Focus();

            //Pre load fields with values supplied from loaded forecast object
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

        // Search cities when text is changed
        private void TxtCity_TextChanged(object sender, TextChangedEventArgs e)
        {
            lstCities.Items.Clear();
            if (!txtCity.Equals(""))
            {
                List<City> matchedCities = CityUtilities.SearchCities(txtCity.Text);
                foreach (var city in matchedCities)
                {
                    lstCities.Items.Add(city);
                }
            }
        }

        //Auto fill textbox when listbox item selected
        private void LstCities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtCity.Text = ((City)lstCities.SelectedItem).name + ", " + ((City)lstCities.SelectedItem).country;
        }

        private void BtnHome_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Hide();
        }

        //Check if inputs are valid and then make API call to fill fields
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

        // Make API call and return current weather object
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

        #region  Slider and text box handling

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

        //Check if inputs are valid, then overwrite file with new values
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!checkValidInputsSave() || !unique()) return;

            crdError.Visibility = Visibility.Hidden;

            //Get forecasts and then replace the original forecast object with the new one made from the values in the fields on form
            var forecasts = FileUtilities.GetForecastsFromFile();
            forecasts[forecasts.FindIndex(f => f.CityID == loadedForecast.CityID && f.ForecastDate.Date.Equals(loadedForecast.ForecastDate.Date))] = new UserForecast(((City)lstCities.SelectedItem).id, (DateTime)dtpDate.SelectedDate, Convert.ToInt16(txtMin.Text), Convert.ToInt16(txtMax.Text), Convert.ToInt16(txtWind.Text), Convert.ToInt16(txtHumidity.Text), Convert.ToInt16(txtPrecip.Text));

            //Overwrite original file
            using (StreamWriter file = new StreamWriter("Forecasts.txt", false))
            {
                foreach (var fc in forecasts)
                {
                    file.WriteLine(fc.GetTextFileFormat());
                }
            }
            MessageBox.Show("Forecast edited!");

            new ViewForecastsWindow().Show();
            this.Hide();

        }

        //Check if there is already a forecast for city on selected date
        private bool unique()
        {
            List<UserForecast> fc = FileUtilities.GetForecastsFromFile();
            DateTime selectedDate = ((DateTime)dtpDate.SelectedDate).Date;
            int selectedCityID = ((City)lstCities.SelectedItem).id;
            //if there is a forecast one the same day for the same city which is not from this forecast, original forecast being edited still exists in file so if there is already
            //a forecast it might be this one
            if (fc.Where(f => f.CityID == selectedCityID && f.ForecastDate.Date == selectedDate.Date && f.ForecastDate.Date != loadedForecast.ForecastDate.Date).ToList().Count > 0)
            {
                crdError.Visibility = Visibility.Visible;
                lblError.Text = "Already have a forecast for this city on this date";
                return false;
            }
            return true;
        }

        //Check that city, date and min and max are valid
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

        //Check city is selected
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

        private void BtnViewForecasts_Click(object sender, RoutedEventArgs e)
        {
            new ViewForecastsWindow().Show();
            this.Hide();
        }

        //Remove forecast from file
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            //Get all forecasts
            var forecasts = FileUtilities.GetForecastsFromFile();

            //Remove this forecast from list
            int index = forecasts.FindIndex(f => f.CityID == loadedForecast.CityID && f.ForecastDate.Date.Equals(loadedForecast.ForecastDate.Date));
            forecasts.RemoveAt(index);

            //Rewrite list to file
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

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            new ViewForecastsWindow().Show();
            this.Hide();
        }
    }
}
