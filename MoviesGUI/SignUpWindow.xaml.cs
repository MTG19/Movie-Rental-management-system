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
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using System.Text.RegularExpressions;

namespace MoviesGUI
{
    /// <summary>
    /// Interaction logic for SignUpWindow.xaml
    /// </summary>
 
    public partial class SignUpWindow : Window
    {
        public SignUpWindow()
        {
            InitializeComponent();
        }

        string connectionString = @"Server=localhost;Database=MovieRental;Trusted_Connection=True;TrustServerCertificate=True;";
        private void SIgnIn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow MainnWindow = new MainWindow();
            MainnWindow.Show();
            this.Close();
        }

        private void btnSignUp_Click(object sender, RoutedEventArgs e)
        {

            string email = txtemail.Text;
            string password = txtPassword.Password;
            string confirmPassword = txtconfpass.Password;

            if (!email.Contains("@") || !email.EndsWith(".com"))
{
                MessageBox.Show("Please enter a valid email address (must contain '@' and end with '.com').");
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }

            if (password.Length < 8 || !password.Any(char.IsDigit) || !password.Any(char.IsLetter))
{
                MessageBox.Show("Password must be at least 8 characters long and contain both letters and numbers.");
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Users (Name,Email,Phone,CreditCardNumber,Password,Role) values (@Name,@Email,@Phone,@CreditCardNumber,@Password,@Role)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Name", txtfirstN.Text);
                command.Parameters.AddWithValue("@Email", txtemail.Text);
                command.Parameters.AddWithValue("@Phone", txtphone.Text);
                command.Parameters.AddWithValue("@CreditCardNumber", txtcreditcard.Text);
                command.Parameters.AddWithValue("@Password", txtPassword.Password);
                command.Parameters.AddWithValue("@Role", "User");
                try
                {
                    command.ExecuteNonQuery();
                    MessageBox.Show("User Created Successfully");
                    MainWindow SigninWindow = new MainWindow();
                    SigninWindow.Show();
                    this.Close();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }


            }
        }
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
{
            // Hide image if width is less than 600px
            if (this.ActualWidth < 600)
            {
                ImagePanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                ImagePanel.Visibility = Visibility.Visible;
            }
        }
    }
}
