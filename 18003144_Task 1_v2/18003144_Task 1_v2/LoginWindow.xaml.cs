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
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        User loggedInUser;

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            grdMain.Background = FileUtilities.ChooseBackground(); //Randomly select background
            loggedInUser = new User();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if(CheckValid(txtUsername.Text, txtPassword.Password))
            {
                MessageBox.Show("Valid - UserType = "+loggedInUser.UserType);
            } else
            {
                MessageBox.Show("Invalid");
            }
        }

        private bool CheckValid(string username, string password)
        {
            //Convert input to MD5 hash
            string hash = Encryption.GetMD5Hash(password);

            bool valid = false;
            //Read all users from file
            List<User> users = GetUsersFromFile();
            foreach(User user in users)
            {
                if (user.Username.ToLower().Equals(username.ToLower()) && user.Password.Equals(hash)){
                    valid = true;
                    loggedInUser = user;
                }
            }
            return valid;
        }

        private List<User> GetUsersFromFile()
        {
            List<User> users = new List<User>();
            using (StreamReader sr = new StreamReader("Users.txt"))
            {
                string line = sr.ReadLine();
                while (line != null)
                {
                    var lineParts = line.Split(',');
                    users.Add(new User(lineParts[0], lineParts[1], (UserType)Convert.ToInt16(lineParts[2])));
                    line = sr.ReadLine();
                }

            }
            return users;
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
