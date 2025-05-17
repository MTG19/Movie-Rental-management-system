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
using Microsoft.Data.SqlClient;

namespace MoviesGUI
{
    /// <summary>
    /// Interaction logic for MovieUserWindow.xaml
    /// </summary>
    public partial class MovieAdminViewWindow : Window
    {

        private int _movieId;
        string connectionString = @"Server=localhost;Database=MovieRental;Trusted_Connection=True;TrustServerCertificate=True;";

        public MovieAdminViewWindow(int movieId)
        {
            InitializeComponent();
            _movieId = movieId;
            LoadMovieData(_movieId);
        }

        private void LoadMovieData(int movieId)
        {
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

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            MovieAdminEdit editWindow = new MovieAdminEdit(_movieId);
            editWindow.Show();
            this.Close();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var confirm = MessageBox.Show("Are you sure you want to delete this movie?", "Confirm Delete", MessageBoxButton.YesNo);
            if (confirm == MessageBoxResult.Yes)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string deleteActors = "DELETE FROM Acted_in WHERE MovieID = @MovieID";
                    SqlCommand cmd1 = new SqlCommand(deleteActors, connection);
                    cmd1.Parameters.AddWithValue("@MovieID", _movieId);
                    cmd1.ExecuteNonQuery();

                    string deleteMovie = "DELETE FROM Movie WHERE MovieID = @MovieID";
                    SqlCommand cmd2 = new SqlCommand(deleteMovie, connection);
                    cmd2.Parameters.AddWithValue("@MovieID", _movieId);
                    int rowsAffected = cmd2.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Movie deleted successfully!");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Something went wrong. Movie not deleted.");
                    }
                }
            }
        }
    }

}


