using Microsoft.Data.SqlClient;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace MoviesGUI
{
    public partial class MoviesAdmin : Window
    {
        private string connectionString = @"Server=localhost;Database=MovieRental;Trusted_Connection=True;TrustServerCertificate=True;";

        public MoviesAdmin()
        {
            InitializeComponent();
            LoadMoviesFromDatabase();
        }

        private void LoadMoviesFromDatabase()
        {
            var movies = new ObservableCollection<Movie>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"
                    SELECT m.MovieID, m.Title, g.GenreName
                    FROM Movie m
                    JOIN Genre g ON m.GenreID = g.GenreID";

                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    movies.Add(new Movie
                    {
                        MovieID = Convert.ToInt32(reader["MovieID"]),
                        Title = reader["Title"].ToString(),
                        Genre = reader["GenreName"].ToString()
                    });
                }
            }

            MoviesItemsControl.ItemsSource = movies;
        }

        private void ViewDetails_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Movie movie)
            {
                MovieUserWindow detailsWindow = new MovieUserWindow(movie.MovieID);
                detailsWindow.ShowDialog();
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddMovieAdminWindow addMovieWindow = new AddMovieAdminWindow();
            addMovieWindow.ShowDialog();
            LoadMoviesFromDatabase();

        }
    }
}
