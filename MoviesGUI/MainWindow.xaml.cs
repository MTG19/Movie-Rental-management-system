using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using System.Text.RegularExpressions;

namespace MoviesGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        string connectionString = @"Server=localhost;Database=MovieRental;Trusted_Connection=True;TrustServerCertificate=True;";


        private void btnSignIn_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Users WHERE Email = @Email AND Password = @Password";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                command.Parameters.AddWithValue("@Password", txtPassword.Password.Trim());

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    string role = reader["Role"].ToString();
                    if (role == "admin")
                    {
                        AddMovieAdminWindow adminWindow = new AddMovieAdminWindow();
                        adminWindow.Show();
                        this.Close();
                    }
                    else
                    {
                        profile userWindow = new profile();
                        userWindow.Show();
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Invalid email or password.");

                }

            }
        }

        private void SignUpIN_Click(object sender, RoutedEventArgs e)
        {
            SignUpWindow signUpWindow = new SignUpWindow();
            signUpWindow.Show();
            this.Close();
        }

    }
}