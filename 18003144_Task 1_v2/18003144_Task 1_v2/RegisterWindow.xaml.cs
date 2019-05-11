using System;
using System.Collections.Generic;
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
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            grdMain.Background = FileUtilities.ChooseBackground(); //Randomly select background
            cmbUserType.Items.Add("General User");
            cmbUserType.Items.Add("Forecaster");
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new LoginWindow().Show();
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateForm())
            {
                MessageBox.Show("Valid");
            }
        }

        private bool ValidateForm()
        {
            //Check all fields are filled
            if (!AllFieldsFilled())
            {
                crdError.Visibility = Visibility.Visible;
                lblError.Text = "Please complete all fields";
                return false;
            }

            //Check username isn't already used
            if(FileUtilities.GetUsersFromFile().Any(u => u.Username.ToLower().Equals(txtUsername.Text.ToLower())))
            {
                crdError.Visibility = Visibility.Visible;
                lblError.Text = "Username already taken";
                return false;
            }

            //Password must be at least 8 characters and have 1 uppercase and 1 lowercase and 1 digit
            char[] passwordChars = txtPassword.Password.ToCharArray();
            if(!(passwordChars.Length >=8 && passwordChars.Where(c=> Char.IsUpper(c)).ToList().Count >= 1 && passwordChars.Where(c => Char.IsLower(c)).ToList().Count >= 1 && passwordChars.Where(c => Char.IsDigit(c)).ToList().Count >= 1))
            {
                crdError.Visibility = Visibility.Visible;
                lblError.Text = "Password must be at least 8 characters and contain 1 uppercase character, 1 lowercase character and 1 digit";
                return false;
            }

            //Check passwords match
            if (!txtPassword.Password.Equals(txtConfirmPassword.Password))
            {
                crdError.Visibility = Visibility.Visible;
                lblError.Text = "Passwords do not match";
                return false;
            }


            return true;
        }

        private bool AllFieldsFilled()
        {
            return !(txtUsername.Text.Equals("") || txtPassword.Password.Equals("") || txtConfirmPassword.Password.Equals("") || cmbUserType.SelectedIndex == -1);
        }
    }
}
