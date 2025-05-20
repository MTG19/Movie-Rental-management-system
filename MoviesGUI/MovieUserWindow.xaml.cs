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
using System.Data;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

namespace MoviesGUI
{
    /// <summary>
    /// Interaction logic for MovieUserWindow.xaml
    /// </summary>
    public partial class MovieUserWindow : Window
    {
        private int _movieId;
        string connectionString = @"Server=localhost;Database=MovieRental;Trusted_Connection=True;TrustServerCertificate=True;";
        private int current_user_id;

        public MovieUserWindow(int movieId, int user_id)
        {
            InitializeComponent();
            _movieId = movieId;
            LoadMovieData(_movieId);
            this.current_user_id = user_id;
        }

        private void LoadMovieData(int movieId)
        {
            NavbarUser nav = new NavbarUser();
            NavbarContainer.Content = nav;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
                SELECT 
                    m.Title, m.RentalPrice, 
                    g.GenreName, 
                    a.Name AS LeadActorName,
                    s.SupplierName, s.Email, s.Phone,
                    ad.Street, ad.City, ad.PostalCode, ad.Country
                FROM Movie m
                JOIN Genre g ON m.GenreID = g.GenreID
                JOIN Actor a ON m.LeadActorID = a.ActorID
                JOIN Supplier s ON m.SupplierID = s.SupplierID
                JOIN Address ad ON s.AddressID = ad.AddressID
                WHERE m.MovieID = @MovieID";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@MovieID", movieId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        txtMovieTitle.Text = reader["Title"].ToString();
                        txtRentalPrice.Text = reader["RentalPrice"].ToString() + " $";
                        txtGenre.Text = reader["GenreName"].ToString();
                        txtLeadActor.Text = reader["LeadActorName"].ToString();
                        txtSupplier.Text = reader["SupplierName"].ToString();
                        txtSupplierEmail.Text = reader["Email"].ToString();
                        txtSupplierPhone.Text = reader["Phone"].ToString();
                        txtSupplierAddress.Text = $"{reader["Street"]}, {reader["City"]}, {reader["PostalCode"]}, {reader["Country"]}";
                        reader.Close();
                    }
                }

                // Load actors who acted in this movie
                string actorsQuery = @"
                SELECT a.Name 
                FROM Acted_in ai
                JOIN Actor a ON ai.ActorID = a.ActorID
                WHERE ai.MovieID = @MovieID";
                SqlCommand actorsCommand = new SqlCommand(actorsQuery, connection);
                actorsCommand.Parameters.AddWithValue("@MovieID", movieId);
                using (SqlDataReader actorsReader = actorsCommand.ExecuteReader())
                {
                    lstActedIn.Items.Clear();
                    while (actorsReader.Read())
                    {
                        lstActedIn.Items.Add(actorsReader["Name"].ToString());
                    }
                }
            }
        }

        private void btnRent_Click(object sender, RoutedEventArgs e)
        {
            RentingDetails Renting_Details = new RentingDetails(_movieId, current_user_id);  
            Renting_Details.Show();
            this.Close();
        }
    }

}
