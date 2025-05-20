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
using Microsoft.VisualBasic;

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
                    //MainWindow SigninWindow = new MainWindow();
                    //SigninWindow.Show();
                    //this.Close();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }

            using (SqlConnection connection2 = new SqlConnection(connectionString)) {
                connection2.Open();

                // get the inserted user's ID 
                string query2 = @"SELECT UserID FROM Users 
                                WHERE Email = @Email AND Password = @Password";


                int user_id;
                // when a user signs up give him a 1 month subscription from the time of him signing up
                string query3 = @"INSERT INTO Subscription (UserID, subscribingDate, PrepaidMonths, EndDate)
                                values (@user_id, GETDATE(), 1, DATEADD( MONTH, 1, GETDATE() ) )";


                // run query 2 
                SqlCommand command2 = new SqlCommand(query2, connection2);
                command2.Parameters.AddWithValue("@Email", txtemail.Text);
                command2.Parameters.AddWithValue("@Password", txtPassword.Password);

                using (SqlDataReader reader = command2.ExecuteReader()) {
                    reader.Read();
                    user_id = reader.GetInt32(0);
                }
                
                // run query 3
                SqlCommand command3 = new SqlCommand(query3, connection2);
                command3.Parameters.AddWithValue("@user_id", user_id);

                command3.ExecuteNonQuery();

                MainWindow SigninWindow = new MainWindow();
                SigninWindow.Show();
                this.Close();
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
