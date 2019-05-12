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
        User user;
        List<string> favCityIds;


        public AddFavouritesWindow(User user, List<string> favCityIds)
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            this.user = user;
            this.favCityIds = favCityIds;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            grdMain.Background = FileUtilities.ChooseBackground(); //Randomly select background

            NameCityDict = CityUtilities.getNameCityDict(); // Get dictionary of City Names to City objects

            txtCity.Focus();
        }

        //Search cities when text is changed
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

        //When user clicks on listbox item, the selected city is added to the favourites file
        private void LstCities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Cannot add same city twice
            if (!alreadyFavourite(((City)lstCities.SelectedItem).id))
            {
                //Add new city to list
                favCityIds.Add(((City)lstCities.SelectedItem).id + "");

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
                    foreach (string line in lines)
                    {
                        file.WriteLine(line);
                    }
                }
            }


            this.Close();
        }

        //Check IDs in file and see if the specified ID is already in the file
        private bool alreadyFavourite(int id)
        {
            return favCityIds.Contains(id + "");
        }
    }
}
