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

        public profile(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            Session.UserId = userId; 
            LoadUserProfile();
            NavbarUser navbar = new NavbarUser(); 
            NavbarContainer.Content = navbar;
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
                        FROM rentingOrder as r
                        JOIN rentingDetail as d ON r.RentalID = d.RentalID
                        JOIN movie as m ON d.MovieID = m.MovieID
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
            MoviesUser userWindow = new MoviesUser(userId);
            userWindow.Show();
            this.Close(); // Optional: Close profile window if you want

        }

        private void ExtendSubscription_Click(object sender, RoutedEventArgs e)
        {
            // Ask user how many months they want to extend
            var extendDialog = new ExtendSubscriptionDialog();
            if (extendDialog.ShowDialog() == true)
            {
                int monthsToExtend = extendDialog.MonthsToExtend;

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    using (SqlCommand command = new SqlCommand(
                        @"UPDATE Subscription 
                          SET EndDate = DATEADD(MONTH, @MonthsToExtend, EndDate),
                              PrepaidMonths = PrepaidMonths + @MonthsToExtend
                          WHERE UserID = @User_id", connection))
                    {
                        command.Parameters.AddWithValue("@MonthsToExtend", monthsToExtend);
                        command.Parameters.AddWithValue("@User_id", userId);

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show($"Subscription extended by {monthsToExtend} months!",
                                          "Success",
                                          MessageBoxButton.OK,
                                          MessageBoxImage.Information);
                            LoadUserProfile(); // Refresh the displayed dates
                        }
                        else
                        {
                            MessageBox.Show("Failed to extend subscription. User not found.",
                                          "Error",
                                          MessageBoxButton.OK,
                                          MessageBoxImage.Error);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show($"Database error: {ex.Message}",
                                  "Database Error",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Unexpected error: {ex.Message}",
                                  "Error",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Error);
                }
            }
        }
    }
}
