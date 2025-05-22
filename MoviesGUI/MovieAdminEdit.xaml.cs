using System.Data;
using System.Windows;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace MoviesGUI
{
    public partial class MovieAdminEdit : Window
    {
        private int _movieId;
        private string connectionString = @"Server=localhost;Database=MovieRental;Trusted_Connection=True;TrustServerCertificate=True;";

        public MovieAdminEdit(int movieId)
        {
            InitializeComponent();
            _movieId = movieId;
            LoadMovieDetails();
        }

        private void LoadMovieDetails()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                SELECT 
                    m.Title, m.RentalPrice, g.GenreName, a.Name AS ActorName, 
                    s.SupplierName, s.Email, s.Phone,
                    ad.Street, ad.City, ad.PostalCode, ad.Country
                FROM Movie m
                JOIN Genre g ON m.GenreID = g.GenreID
                JOIN Actor a ON m.LeadActorID = a.ActorID
                JOIN Supplier s ON m.SupplierID = s.SupplierID
                JOIN Address ad ON s.AddressID = ad.AddressID
                WHERE m.MovieID = @MovieID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MovieID", _movieId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtMovieTitleInput.Text = reader["Title"].ToString();
                            txtRentalPriceInput.Text = reader["RentalPrice"].ToString();
                            txtGenreInput.Text = reader["GenreName"].ToString();
                            txtLeadActorInput.Text = reader["ActorName"].ToString();
                            txtSupplierNameInput.Text = reader["SupplierName"].ToString();
                            txtSupplierEmailInput.Text = reader["Email"].ToString();
                            txtSupplierPhoneInput.Text = reader["Phone"].ToString();
                            txtSupplierStreetInput.Text = reader["Street"].ToString();
                            txtSupplierCityInput.Text = reader["City"].ToString();
                            txtSupplierPostalInput.Text = reader["PostalCode"].ToString();
                            txtSupplierCountryInput.Text = reader["Country"].ToString();
                        }
                        else
                        {
                            MessageBox.Show("Movie not found.");
                        }
                    }
                }
            }
        }

        private void btnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // 1. Get IDs for Genre, Actor, Supplier
                int genreId = GetIdByName(conn, "Genre", "GenreID", "GenreName", txtGenreInput.Text.Trim());
                int actorId = GetIdByName(conn, "Actor", "ActorID", "Name", txtLeadActorInput.Text.Trim());
                int supplierId = GetIdByName(conn, "Supplier", "SupplierID", "SupplierName", txtSupplierNameInput.Text.Trim());

                if (genreId == -1 || actorId == -1 || supplierId == -1)
                {
                    MessageBox.Show("Invalid genre, actor, or supplier name.");
                    return;
                }

                // 2. Update Movie
                string updateMovieQuery = @"
                UPDATE Movie
                SET Title = @Title,
                    RentalPrice = @RentalPrice,
                    GenreID = @GenreID,
                    LeadActorID = @ActorID,
                    SupplierID = @SupplierID
                WHERE MovieID = @MovieID";

                using (SqlCommand cmd = new SqlCommand(updateMovieQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@Title", txtMovieTitleInput.Text.Trim());
                    cmd.Parameters.AddWithValue("@RentalPrice", decimal.Parse(txtRentalPriceInput.Text.Trim()));
                    cmd.Parameters.AddWithValue("@GenreID", genreId);
                    cmd.Parameters.AddWithValue("@ActorID", actorId);
                    cmd.Parameters.AddWithValue("@SupplierID", supplierId);
                    cmd.Parameters.AddWithValue("@MovieID", _movieId);

                    cmd.ExecuteNonQuery();
                }

                // 3. Update Supplier Email & Phone
                string updateSupplierQuery = @"
                UPDATE Supplier
                SET Email = @Email,
                    Phone = @Phone
                WHERE SupplierID = @SupplierID";

                using (SqlCommand cmd = new SqlCommand(updateSupplierQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", txtSupplierEmailInput.Text.Trim());
                    cmd.Parameters.AddWithValue("@Phone", txtSupplierPhoneInput.Text.Trim());
                    cmd.Parameters.AddWithValue("@SupplierID", supplierId);
                    cmd.ExecuteNonQuery();
                }

                // 4. Update Address
                string updateAddressQuery = @"
                UPDATE Address
                SET Street = @Street,
                    City = @City,
                    PostalCode = @PostalCode,
                    Country = @Country
                WHERE AddressID = (
                    SELECT AddressID FROM Supplier WHERE SupplierID = @SupplierID
                )";

                using (SqlCommand cmd = new SqlCommand(updateAddressQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@Street", txtSupplierStreetInput.Text.Trim());
                    cmd.Parameters.AddWithValue("@City", txtSupplierCityInput.Text.Trim());
                    cmd.Parameters.AddWithValue("@PostalCode", txtSupplierPostalInput.Text.Trim());
                    cmd.Parameters.AddWithValue("@Country", txtSupplierCountryInput.Text.Trim());
                    cmd.Parameters.AddWithValue("@SupplierID", supplierId);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Movie updated successfully.");
            }
        }

        private int GetIdByName(SqlConnection conn, string table, string idColumn, string nameColumn, string name)
        {
            string query = $"SELECT {idColumn} FROM {table} WHERE {nameColumn} = @Name";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Name", name);
                var result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : -1;
            }
        }
    }
}