using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.Data.SqlClient;

namespace MoviesGUI
{
    public partial class profile : Window
    {
        string connectionString = @"Server=localhost;Database=MovieRental;Trusted_Connection=True;TrustServerCertificate=True;";
        private int userId;
        // Add this field to the profile class
        private System.Windows.Controls.ListView lstMovies;

        public profile(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            LoadUserProfile();
        }

        private void LoadUserProfile()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Load User Info + Subscription (use LEFT JOIN)
                    string userQuery = @"
                        SELECT u.Name, u.Email, u.Phone, 
                               s.SubscribingDate, s.PrepaidMonths, s.EndDate
                        FROM Users u
                        LEFT JOIN Subscription s ON u.UserID = s.UserID
                        WHERE u.UserID = @UserID";

                    SqlCommand userCmd = new SqlCommand(userQuery, connection);
                    userCmd.Parameters.AddWithValue("@UserID", userId);

                    using (SqlDataReader reader = userCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtName.Text = reader["Name"].ToString();
                            txtEmail.Text = reader["Email"].ToString();
                            txtPhone.Text = reader["Phone"].ToString();

                            txtSubDate.Text = reader["SubscribingDate"] != DBNull.Value
                                ? Convert.ToDateTime(reader["SubscribingDate"]).ToShortDateString()
                                : "N/A";

                            txtEndDate.Text = reader["EndDate"] != DBNull.Value
                                ? Convert.ToDateTime(reader["EndDate"]).ToShortDateString()
                                : "N/A";

                            txtMonths.Text = reader["PrepaidMonths"] != DBNull.Value
                                ? reader["PrepaidMonths"].ToString()
                                : "N/A";
                        }
                    }

                    // Load rented movies
                    string moviesQuery = @"
                        SELECT m.Title, r.rentingDate, r.returnDate
                        FROM rentingOrder r
                        JOIN rentingDetail d ON r.RentalID = d.RentalID
                        JOIN Library l ON d.TapeID = l.TapeID
                        JOIN Movie m ON l.MovieID = m.MovieID
                        WHERE r.UserID = @UserID";

                    SqlCommand movieCmd = new SqlCommand(moviesQuery, connection);
                    movieCmd.Parameters.AddWithValue("@UserID", userId);

                    using (SqlDataReader movieReader = movieCmd.ExecuteReader())
                    {
                        var movies = new List<RentedMovie>();
                        while (movieReader.Read())
                        {
                            movies.Add(new RentedMovie
                            {
                                Title = movieReader["Title"].ToString(),
                                RentedDate = Convert.ToDateTime(movieReader["rentingDate"]),
                                ReturnDate = Convert.ToDateTime(movieReader["returnDate"])
                            });
                        }
                        lstMovies.ItemsSource = movies;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading profile: " + ex.Message);
            }
        }

        public class RentedMovie
        {
            public string Title { get; set; }
            public DateTime RentedDate { get; set; }
            public DateTime ReturnDate { get; set; }

        }

        private void ViewMovies_Click(object sender, RoutedEventArgs e)
        {
            // Open the moviesadmin window
            MoviesAdmin adminWindow = new MoviesAdmin();
            adminWindow.Show();
            this.Close(); // Optional: Close profile window if you want

        }
    }
}
