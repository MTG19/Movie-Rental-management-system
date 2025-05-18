using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.SqlClient;

namespace MoviesGUI
{
    public partial class MoviesUser : Window
    {
        public ObservableCollection<Movie> Movies { get; set; } = new ObservableCollection<Movie>();

        private readonly string connectionString = @"Server=localhost;Database=MovieRental;Trusted_Connection=True;TrustServerCertificate=True;";

        public MoviesUser()
        {
            DataContext = this;
            LoadMovies();
        }

        private void LoadMovies()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                        SELECT m.MovieID, m.Title, g.GenreName
                        FROM Movie m
                        INNER JOIN Genre g ON m.GenreID = g.GenreID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Movies.Clear();
                        while (reader.Read())
                        {
                            Movies.Add(new Movie
                            {
                                MovieID = reader.GetInt32(0),
                                Title = reader.GetString(1),
                                Genre = reader.GetString(2)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading movies: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ViewDetails_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Movie movie)
            {
                var detailsWindow = new MovieUserWindow(movie.MovieID);
                detailsWindow.ShowDialog();
            }
        }
    }

    public class Movie
    {
        public int MovieID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
    }
}
