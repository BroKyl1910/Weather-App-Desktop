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
    /// Interaction logic for ViewForecastsWindowxaml.xaml
    /// </summary>
    public partial class ViewForecastsWindow : Window
    {
        Dictionary<int, City> codeCityDict;
        List<UserForecast> forecasts;

        public ViewForecastsWindow()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            forecasts = new List<UserForecast>();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            chooseBackground();
            codeCityDict = CityUtilities.getCityCodeDict();
            forecasts = FileUtilities.getForecastsFromFile();
            populateListBox();

            dtpFrom.SelectedDate = DateTime.Now;
            dtpTo.SelectedDate = DateTime.Now;
        }

        private void populateListBox()
        {
            lstCities.Items.Clear();
            List<City> cities = new List<City>();
            foreach (var forecast in forecasts)
            {
                cities.Add(CityUtilities.getCityCodeDict()[forecast.CityID]);
            }
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

        private void BtnAddForecast_Click(object sender, RoutedEventArgs e)
        {
            new CreateForecastWindow().Show();
            this.Hide();
        }

        private void BtnHome_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Hide();
        }

        private void DtpFrom_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            dtpTo.SelectedDate = dtpFrom.SelectedDate;
        }

        private void BtnGetForecasts_Click(object sender, RoutedEventArgs e)
        {
            if (!validInputs()) return;

            crdError.Visibility = Visibility.Hidden;
            tbctrlForecasts.Items.Clear();

            Dictionary<int, City> cityCodeDict = CityUtilities.getCityCodeDict();

            //List of all forecasts with same city and date
            List<UserForecast> matchingForecasts = getMatchingForecasts();

            tbctrlForecasts.Visibility = Visibility.Visible;

            DateTime dt = (DateTime)dtpFrom.SelectedDate;

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
                List<UserForecast> fc = matchingForecasts.Where(o => o.ForecastDate.Date.Equals(date.Date)).OrderBy(f => cityCodeDict[f.CityID].name).ToList();

                //List of cities that dont have forecasts for selected dates
                List<City> nm = getSelectedCities().Where(o => forecasts.Where(f => f.ForecastDate.Date.Equals(date.Date) && f.CityID == o.id).ToList().Count == 0).OrderBy(o => o.name).ToList();

                //Make card for each forecast
                foreach (UserForecast forecast in fc)
                {
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
                    //lblCity.TextDecorations = TextDecorations.Underline;
                    lblCity.Text = cityCodeDict[forecast.CityID].name + ", " + cityCodeDict[forecast.CityID].country;

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

                    //Edit button
                    Button btnEdit = new Button();
                    btnEdit.VerticalAlignment = VerticalAlignment.Top;
                    btnEdit.HorizontalAlignment = HorizontalAlignment.Left;
                    btnEdit.FontSize = 22;
                    btnEdit.Padding = new Thickness(10,10,10,10);
                    btnEdit.Margin = new Thickness(419, 10, 0, 0);
                    btnEdit.Height = double.NaN;
                    btnEdit.Content = "Edit Forecast";
                    btnEdit.Name = "btnEdit" + forecast.CityID + "_" + forecast.ForecastDate.ToShortDateString().Replace('/', '_');
                    btnEdit.Click += BtnEdit_Click;
                    btnEdit.Foreground = new SolidColorBrush(Colors.White);
                    btnEdit.Background = new SolidColorBrush(Color.FromArgb(51,0,0,0));



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
                    grid.Children.Add(btnEdit);
                    stackPanel.Children.Add(card);
                }

                foreach (City c in nm)
                {
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
                    lblCity.Text = c.name + ", " + c.country;

                    //No forecast message
                    TextBlock nvNoForecast = new TextBlock();
                    nvNoForecast.VerticalAlignment = VerticalAlignment.Top;
                    nvNoForecast.HorizontalAlignment = HorizontalAlignment.Left;
                    nvNoForecast.FontSize = 22;
                    nvNoForecast.Margin = new Thickness(98, 97, 0, 0);
                    nvNoForecast.Text = "No forecast for " + date.ToLongDateString();

                    grid.Children.Add(lblCity);
                    grid.Children.Add(nvNoForecast);
                    stackPanel.Children.Add(card);

                }

                dt = dt.AddDays(1);
                Console.WriteLine("");
            }

            tbctrlForecasts.SelectedIndex = 0;
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            Button editBtn = (Button)sender;
            MessageBox.Show(editBtn.Name);
        }

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

        private List<City> getSelectedCities()
        {
            List<City> cities = new List<City>();
            foreach (City c in lstCities.SelectedItems)
            {
                cities.Add(c);
            }
            return cities;
        }
    }
}
