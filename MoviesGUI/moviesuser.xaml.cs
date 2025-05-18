using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class MoviesUser : Window
    {
        private string connectionString = @"Server=localhost;Database=MovieRental;Trusted_Connection=True;TrustServerCertificate=True;";

        public MoviesUser()
        {
            InitializeComponent();
            LoadMoviesFromDatabase();
            NavbarUser nav = new NavbarUser();
            NavbarContainer.Content = nav;
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
                var detailsWindow = new MovieUserWindow(movie.MovieID);
                detailsWindow.ShowDialog();
            }
        }
    }

    public class Movie
    {
        public int MovieID { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
    }
}