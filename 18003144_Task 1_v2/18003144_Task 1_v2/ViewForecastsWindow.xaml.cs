using System;
using System.Collections;
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
    /// Interaction logic for ViewForecastsWindowxaml.xaml
    /// </summary>
    public partial class ViewForecastsWindow : Window
    {
        Dictionary<int, City> codeCityDict;
        List<UserForecast> forecasts;

        User user;

        public ViewForecastsWindow(User user)
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            this.user = user;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (user.UserType == UserType.GeneralUser)
            {
                btnAddForecast.Visibility = Visibility.Hidden;
            }

            grdMain.Background = DataUtilities.ChooseBackground(); //Randomly select background
            codeCityDict = CityUtilities.getCityCodeDict(); //Get dictionary of City Codes to City Objects
            forecasts = DataUtilities.GetForecastsFromDB(); //Get forecasts from file
            populateListBox(); //Populate list box with cities

            //Set defaults
            lstCities.SelectedIndex = 0;
            dtpFrom.SelectedDate = DateTime.Now;
            dtpTo.SelectedDate = DateTime.Now;
        }

        // Populate list box with cities
        private void populateListBox()
        {
            lstCities.Items.Clear();
            List<City> cities = new List<City>();
            //Get all cities with forecasts, repeats allowed
            foreach (var forecast in forecasts)
            {
                cities.Add(CityUtilities.getCityCodeDict()[forecast.CityID]);
            }
            //Get distinct cities to add to listbox, no repeats
            List<City> distinctCities = cities.Distinct().OrderBy(o => o.name).ToList();
            foreach (var city in distinctCities)
            {
                lstCities.Items.Add(city);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void BtnAddForecast_Click(object sender, RoutedEventArgs e)
        {
            new CreateForecastWindow(user).Show();
            this.Hide();
        }

        private void BtnHome_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow(user).Show();
            this.Hide();
        }

        //Assume user wants a single date, and chooses from first, therefore make to = from, but from can be changed
        private void DtpFrom_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            dtpTo.SelectedDate = dtpFrom.SelectedDate;
        }

        //Get forecasts and display them
        //Each date has a tab in the tabcontrol and each forecast has a card in each tab
        private void BtnGetForecasts_Click(object sender, RoutedEventArgs e)
        {
            if (!validInputs()) return;

            crdError.Visibility = Visibility.Hidden;
            tbctrlForecasts.Items.Clear();

            //List of all forecasts with same city and date
            List<UserForecast> matchingForecasts = getMatchingForecasts();

            tbctrlForecasts.Visibility = Visibility.Visible;

            DateTime dt = (DateTime)dtpFrom.SelectedDate;

            string fileTimeStamp = DateTime.UtcNow.ToFileTimeUtc()+"";

            var writeReport = MessageBox.Show("Would you like to save this report to file?",
                                     "Save Report",
                                     System.Windows.MessageBoxButton.YesNo);

            if(user.UserType == UserType.Forecaster && writeReport == System.Windows.MessageBoxResult.Yes)
            {
                startWritingToReportFile(fileTimeStamp);
            }

            while (dt.Date.CompareTo(((DateTime)dtpTo.SelectedDate).Date) <= 0)
            {
                var date = dt;
                TabItem tabItem = new TabItem();
                tabItem.Padding = new Thickness(5);
                tabItem.Header = dt.ToLongDateString();

                tbctrlForecasts.Items.Add(tabItem);

                ScrollViewer scrollViewer = new ScrollViewer();
                tabItem.Content = scrollViewer;

                StackPanel stackPanel = new StackPanel();
                scrollViewer.Content = stackPanel;

                //List of forecasts which match date, decides which forecast to put on the card
                List<UserForecast> fc = matchingForecasts.Where(o => o.ForecastDate.Date.Equals(date.Date)).OrderBy(f => codeCityDict[f.CityID].name).ToList();

                //List of cities that dont have forecasts for selected dates
                List<City> nm = getSelectedCities().Where(o => forecasts.Where(f => f.ForecastDate.Date.Equals(date.Date) && f.CityID == o.id).ToList().Count == 0).OrderBy(o => o.name).ToList();

                #region Make card for each forecast
                foreach (UserForecast forecast in fc)
                {
                    //Make objects to store the forecasts with the highest/lowest values
                    UserForecast maxTempObj;
                    UserForecast minTempObj;
                    UserForecast maxWindObj;
                    UserForecast maxHumidityObj;
                    UserForecast maxPrecipObj;

                    //Get extremes
                    maxTempObj = fc.Where(o => o.MaximumTemp == fc.Max(f => f.MaximumTemp)).ToList()[0];
                    minTempObj = fc.Where(o => o.MinimumTemp == fc.Min(f => f.MinimumTemp)).ToList()[0];
                    maxWindObj = fc.Where(o => o.WindSpeed == fc.Max(f => f.WindSpeed)).ToList()[0];
                    maxHumidityObj = fc.Where(o => o.Humidity == fc.Max(f => f.Humidity)).ToList()[0];
                    maxPrecipObj = fc.Where(o => o.Precipitation == fc.Max(f => f.Precipitation)).ToList()[0];

                    //Make card
                    MaterialDesignThemes.Wpf.Card card = new MaterialDesignThemes.Wpf.Card();
                    card.Background = new SolidColorBrush(Color.FromArgb(51, 0, 0, 0));
                    card.Foreground = new SolidColorBrush(Colors.White);
                    card.Margin = new Thickness(10);
                    card.Padding = new Thickness(10, 10, 0, 50);

                    Grid grid = new Grid();
                    grid.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));

                    //nv = Non-value, not used to ouput values.....lbl used to output values
                    card.Content = grid;

                    //City output
                    TextBlock lblCity = new TextBlock();
                    lblCity.VerticalAlignment = VerticalAlignment.Top;
                    lblCity.HorizontalAlignment = HorizontalAlignment.Left;
                    lblCity.FontSize = 28;
                    lblCity.Margin = new Thickness(10, 10, 0, 0);
                    lblCity.TextAlignment = TextAlignment.Center;
                    lblCity.Text = codeCityDict[forecast.CityID].ToString();

                    //Date nv
                    TextBlock nvDate = new TextBlock();
                    nvDate.VerticalAlignment = VerticalAlignment.Top;
                    nvDate.HorizontalAlignment = HorizontalAlignment.Left;
                    nvDate.FontSize = 22;
                    nvDate.Margin = new Thickness(98, 97, 0, 0);
                    nvDate.Text = "Date:";

                    //Date output
                    TextBlock lblDate = new TextBlock();
                    lblDate.VerticalAlignment = VerticalAlignment.Top;
                    lblDate.HorizontalAlignment = HorizontalAlignment.Left;
                    lblDate.FontSize = 22;
                    lblDate.Margin = new Thickness(419, 97, 0, 0);
                    lblDate.Text = forecast.ForecastDate.ToLongDateString();
                    lblDate.FontStyle = FontStyles.Italic;


                    //Min value nv
                    TextBlock nvMin = new TextBlock();
                    nvMin.VerticalAlignment = VerticalAlignment.Top;
                    nvMin.HorizontalAlignment = HorizontalAlignment.Left;
                    nvMin.FontSize = 22;
                    nvMin.Margin = new Thickness(98, 129, 0, 0);
                    nvMin.Text = "Min:";

                    //Min temp output
                    TextBlock lblMin = new TextBlock();
                    lblMin.VerticalAlignment = VerticalAlignment.Top;
                    lblMin.HorizontalAlignment = HorizontalAlignment.Left;
                    lblMin.FontSize = 22;
                    lblMin.Margin = new Thickness(419, 129, 0, 0);
                    lblMin.Text = forecast.MinimumTemp + " °C";
                    lblMin.FontStyle = FontStyles.Italic;
                    lblMin.Foreground = (forecast.Equals(minTempObj) && fc.Count > 1) ? Brushes.Red : Brushes.White;


                    //Max value nv
                    TextBlock nvMax = new TextBlock();
                    nvMax.VerticalAlignment = VerticalAlignment.Top;
                    nvMax.HorizontalAlignment = HorizontalAlignment.Left;
                    nvMax.FontSize = 22;
                    nvMax.Margin = new Thickness(98, 160, 0, 0);
                    nvMax.Text = "Max:";

                    //Max temp output
                    TextBlock lblMax = new TextBlock();
                    lblMax.VerticalAlignment = VerticalAlignment.Top;
                    lblMax.HorizontalAlignment = HorizontalAlignment.Left;
                    lblMax.FontSize = 22;
                    lblMax.Margin = new Thickness(419, 160, 0, 0);
                    lblMax.Text = forecast.MaximumTemp + " °C";
                    lblMax.FontStyle = FontStyles.Italic;
                    lblMax.Foreground = (forecast.Equals(maxTempObj) && fc.Count > 1) ? Brushes.Red : Brushes.White;


                    //Wind nv
                    TextBlock nvWind = new TextBlock();
                    nvWind.VerticalAlignment = VerticalAlignment.Top;
                    nvWind.HorizontalAlignment = HorizontalAlignment.Left;
                    nvWind.FontSize = 22;
                    nvWind.Margin = new Thickness(98, 191, 0, 0);
                    nvWind.Text = "Wind Speed:";

                    //Wind speed output
                    TextBlock lblWind = new TextBlock();
                    lblWind.VerticalAlignment = VerticalAlignment.Top;
                    lblWind.HorizontalAlignment = HorizontalAlignment.Left;
                    lblWind.FontSize = 22;
                    lblWind.Margin = new Thickness(419, 191, 0, 0);
                    lblWind.Text = forecast.WindSpeed + " km/h";
                    lblWind.FontStyle = FontStyles.Italic;
                    lblWind.Foreground = (forecast.Equals(maxWindObj) && fc.Count > 1) ? Brushes.Red : Brushes.White;


                    //Humidity nv
                    TextBlock nvHumidity = new TextBlock();
                    nvHumidity.VerticalAlignment = VerticalAlignment.Top;
                    nvHumidity.HorizontalAlignment = HorizontalAlignment.Left;
                    nvHumidity.FontSize = 22;
                    nvHumidity.Margin = new Thickness(98, 222, 0, 0);
                    nvHumidity.Text = "Humidity:";

                    //Humidity output
                    TextBlock lblHumidity = new TextBlock();
                    lblHumidity.VerticalAlignment = VerticalAlignment.Top;
                    lblHumidity.HorizontalAlignment = HorizontalAlignment.Left;
                    lblHumidity.FontSize = 22;
                    lblHumidity.Margin = new Thickness(419, 222, 0, 0);
                    lblHumidity.Text = forecast.Humidity + " %";
                    lblHumidity.FontStyle = FontStyles.Italic;
                    lblHumidity.Foreground = (forecast.Equals(maxHumidityObj) && fc.Count > 1) ? Brushes.Red : Brushes.White;


                    //Precipitation nv
                    TextBlock nvPrecipitation = new TextBlock();
                    nvPrecipitation.VerticalAlignment = VerticalAlignment.Top;
                    nvPrecipitation.HorizontalAlignment = HorizontalAlignment.Left;
                    nvPrecipitation.FontSize = 22;
                    nvPrecipitation.Margin = new Thickness(98, 253, 0, 0);
                    nvPrecipitation.Text = "Precipitation:";

                    //Precipitation output
                    TextBlock lblPrecipitation = new TextBlock();
                    lblPrecipitation.VerticalAlignment = VerticalAlignment.Top;
                    lblPrecipitation.HorizontalAlignment = HorizontalAlignment.Left;
                    lblPrecipitation.FontSize = 22;
                    lblPrecipitation.Margin = new Thickness(419, 253, 0, 0);
                    lblPrecipitation.Text = forecast.Precipitation + " %";
                    lblPrecipitation.FontStyle = FontStyles.Italic;
                    lblPrecipitation.Foreground = (forecast.Equals(maxPrecipObj) && fc.Count > 1) ? Brushes.Red : Brushes.White;

                    //Edit button
                    Button btnEdit = new Button();
                    if (user.UserType == UserType.Forecaster)
                    {
                        btnEdit.VerticalAlignment = VerticalAlignment.Top;
                        btnEdit.HorizontalAlignment = HorizontalAlignment.Left;
                        btnEdit.FontSize = 22;
                        btnEdit.Padding = new Thickness(10, 10, 10, 10);
                        btnEdit.Margin = new Thickness(419, 10, 0, 0);
                        btnEdit.Height = double.NaN;
                        btnEdit.Content = "Edit Forecast";
                        btnEdit.Name = "btnEdit" + forecast.CityID + "_" + forecast.ForecastDate.ToShortDateString().Replace('/', '_');
                        btnEdit.Click += BtnEdit_Click;
                        btnEdit.Foreground = new SolidColorBrush(Colors.White);
                        btnEdit.Background = new SolidColorBrush(Color.FromArgb(51, 0, 0, 0));
                    }


                    //Adding controls to UI
                    grid.Children.Add(lblCity);
                    grid.Children.Add(lblDate);
                    grid.Children.Add(nvDate);
                    grid.Children.Add(lblMin);
                    grid.Children.Add(nvMin);
                    grid.Children.Add(lblMax);
                    grid.Children.Add(nvMax);
                    grid.Children.Add(lblWind);
                    grid.Children.Add(nvWind);
                    grid.Children.Add(lblHumidity);
                    grid.Children.Add(nvHumidity);
                    grid.Children.Add(lblPrecipitation);
                    grid.Children.Add(nvPrecipitation);
                    if (user.UserType == UserType.Forecaster)
                    {
                        grid.Children.Add(btnEdit);
                    }
                    stackPanel.Children.Add(card);
                }
                #endregion

                #region Make card for each city with no forecast
                foreach (City c in nm)
                {
                    //Make card
                    MaterialDesignThemes.Wpf.Card card = new MaterialDesignThemes.Wpf.Card();
                    card.Background = new SolidColorBrush(Color.FromArgb(51, 0, 0, 0));
                    card.Foreground = new SolidColorBrush(Colors.White);
                    card.Margin = new Thickness(10);
                    card.Padding = new Thickness(10, 10, 0, 50);

                    Grid grid = new Grid();
                    grid.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));

                    card.Content = grid;

                    //City output
                    TextBlock lblCity = new TextBlock();
                    lblCity.VerticalAlignment = VerticalAlignment.Top;
                    lblCity.HorizontalAlignment = HorizontalAlignment.Left;
                    lblCity.FontSize = 28;
                    lblCity.Margin = new Thickness(10, 10, 0, 0);
                    lblCity.TextAlignment = TextAlignment.Center;
                    lblCity.Text = c.ToString();

                    //No forecast message
                    TextBlock nvNoForecast = new TextBlock();
                    nvNoForecast.VerticalAlignment = VerticalAlignment.Top;
                    nvNoForecast.HorizontalAlignment = HorizontalAlignment.Left;
                    nvNoForecast.FontSize = 22;
                    nvNoForecast.Margin = new Thickness(98, 97, 0, 0);
                    nvNoForecast.Text = "No forecast for " + date.ToLongDateString();

                    //Add controls to UI
                    grid.Children.Add(lblCity);
                    grid.Children.Add(nvNoForecast);
                    stackPanel.Children.Add(card);

                }
                #endregion

                if (user.UserType == UserType.Forecaster && writeReport==System.Windows.MessageBoxResult.Yes)
                {
                    writeForecastToFile(fileTimeStamp, dt, fc, nm);
                }

                dt = dt.AddDays(1);
                Console.WriteLine("");
            }

            tbctrlForecasts.SelectedIndex = 0;
            if (user.UserType == UserType.Forecaster && writeReport == System.Windows.MessageBoxResult.Yes)
            {
                MessageBox.Show("Report saved as Report - " + fileTimeStamp + ".txt");
            }

        }

        private void startWritingToReportFile(string timestamp)
        {
            using (StreamWriter sw = new StreamWriter("Report - " + timestamp+".txt", false))
            {
                sw.WriteLine("Report generated on " + DateTime.Now.ToString());
                sw.WriteLine("Generated by: " + user.Username);
                string citiesString = "";
                foreach(var city in lstCities.SelectedItems)
                {
                    citiesString += city.ToString()+", ";
                }
                sw.WriteLine("For: " + citiesString.TrimEnd(new char[] { ',' , ' '}));
                sw.WriteLine("Dates: " + dtpFrom.SelectedDate.ToString() + " - " + dtpTo.SelectedDate.ToString());
                sw.WriteLine("============================================================");
            }
        }

        private void writeForecastToFile(string timestamp, DateTime dt, List<UserForecast> validForecasts, List<City> citiesWithNoForecasts)
        {
            using (StreamWriter sw = new StreamWriter("Report - " + timestamp + ".txt", true))
            {
                foreach (UserForecast fc in validForecasts)
                {
                    sw.WriteLine();
                    sw.WriteLine(fc.ForecastDate.ToShortDateString()+" - "+codeCityDict[fc.CityID].ToString());
                    sw.WriteLine("=========================================");
                    sw.WriteLine("Min: " + fc.MinimumTemp + " °C");
                    sw.WriteLine("Max: " + fc.MaximumTemp + " °C");
                    sw.WriteLine("Wind Speed: " + fc.WindSpeed+ " km/h");
                    sw.WriteLine("Humidity: " + fc.Humidity+ " %");
                    sw.WriteLine("Precipitation: " + fc.Precipitation+ " %");
                    sw.WriteLine("=========================================");

                }

                foreach(City city in citiesWithNoForecasts)
                {
                    sw.WriteLine();
                    sw.WriteLine(dt.ToShortDateString() + " - " + city.ToString());
                    sw.WriteLine("=========================================");
                    sw.WriteLine("No Forecasts");
                }

                sw.WriteLine();
                sw.WriteLine();
            }
        }


        //Handler for if user wants to edit forecast
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            Button editBtn = (Button)sender;
            //Useful bits refers to the city ID and date of the forecast
            string usefulBits = editBtn.Name.Substring(7);
            var usefulBitsArr = usefulBits.Split('_');
            int cityId = Convert.ToInt32(usefulBitsArr[0]);
            DateTime date = new DateTime(Convert.ToInt16(usefulBitsArr[1]), Convert.ToInt16(usefulBitsArr[2]), Convert.ToInt16(usefulBitsArr[3]));
            UserForecast clickedForecast = forecasts.Where(o => o.CityID == cityId && o.ForecastDate.Date.Equals(date.Date)).ToList()[0];

            new EditForecastWindow(clickedForecast, user).Show();
            this.Hide();
        }

        //Must select at least 1 city and a from and to date
        private bool validInputs()
        {
            if (lstCities.SelectedIndex < 0)
            {
                crdError.Visibility = Visibility.Visible;
                lblError.Text = "Please select at least 1 city from list";
                return false;
            }
            else if (dtpFrom.SelectedDate == null)
            {
                crdError.Visibility = Visibility.Visible;
                lblError.Text = "Please select begin date";
                dtpFrom.SelectedDate = DateTime.Now;
                return false;
            }
            else if (dtpTo.SelectedDate == null)
            {
                crdError.Visibility = Visibility.Visible;
                lblError.Text = "Please select end date";
                dtpTo.SelectedDate = DateTime.Now;
                return false;
            }

            return true;
        }

        //Returns all forecasts which match city and start and end date
        private List<UserForecast> getMatchingForecasts()
        {
            List<UserForecast> matchingForecasts = new List<UserForecast>();
            List<City> cities = getSelectedCities();
            List<int> cityIds = cities.Select(o => o.id).ToList();
            DateTime startDate = (DateTime)dtpFrom.SelectedDate;
            DateTime endDate = (DateTime)dtpTo.SelectedDate;

            foreach (UserForecast forecast in forecasts)
            {
                if (forecast.ForecastDate.Date.CompareTo(startDate.Date) >= 0 && forecast.ForecastDate.Date.CompareTo(endDate.Date) <= 0 && cityIds.IndexOf(forecast.CityID) > -1)
                {
                    matchingForecasts.Add(forecast);
                }
            }

            return matchingForecasts;
        }

        //Returns selected cities as objects
        private List<City> getSelectedCities()
        {
            List<City> cities = new List<City>();
            foreach (City c in lstCities.SelectedItems)
            {
                cities.Add(c);
            }
            return cities;
        }

        private void BtnViewForecasts_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new LoginWindow().Show();
        }
    }
}
