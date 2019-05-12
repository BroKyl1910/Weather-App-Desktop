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
            txtUsername.Focus();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if(CheckValid(txtUsername.Text, txtPassword.Password))
            {
                crdError.Visibility = Visibility.Hidden;
                this.Hide();
                new MainWindow(loggedInUser).Show();
            }
            else
            {
                crdError.Visibility = Visibility.Visible;
                lblError.Text = "Invalid Username or Password";
            }
        }

        private bool CheckValid(string username, string password)
        {
            //Convert input to MD5 hash
            string hash = Encryption.GetMD5Hash(password);

            bool valid = false;
            //Read all users from file
            List<User> users = FileUtilities.GetUsersFromFile();
            foreach(User user in users)
            {
                if (user.Username.ToLower().Equals(username.ToLower()) && user.Password.Equals(hash)){
                    valid = true;
                    loggedInUser = user;
                }
            }
            return valid;
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            new RegisterWindow().Show();
            this.Hide();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
