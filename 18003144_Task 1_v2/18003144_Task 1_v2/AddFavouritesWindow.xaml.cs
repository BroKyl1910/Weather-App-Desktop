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

        public AddFavouritesWindow(User user)
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            this.user = user;

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

                //If line is empty, just ID is added, otherwise a , and id is added
                //eg. Empty file - id
                //    Not empty - 1234,5678(,id)
                string newLine = (ids.Length > 0) ? ids + "," + ((City)lstCities.SelectedItem).id : ((City)lstCities.SelectedItem).id + "";
                using (StreamWriter file = new StreamWriter("Favourites.txt", false))
                {
                    file.Write(newLine);
                }
            }


            this.Close();
        }

        //Check IDs in file and see if the specified ID is already in the file
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
