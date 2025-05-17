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


namespace MoviesGUI
{
    public partial class AddMovieAdminWindow : Window
    {
        public AddMovieAdminWindow()
        {
            InitializeComponent();
        }

        string connectionString = @"Server=localhost;Database=MovieRental;Trusted_Connection=True;TrustServerCertificate=True;";



        // TextBox
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null && tb.Foreground == Brushes.Gray)
            {
                tb.Text = "";
                tb.Foreground = Brushes.Black;
            }
        }

        //  TextBox
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null && string.IsNullOrWhiteSpace(tb.Text))
            {
                tb.Foreground = Brushes.Gray;
                if (tb == txtAddressStreet)
                    tb.Text = "Street";
                else if (tb == txtAddressCity)
                    tb.Text = "City";
                else if (tb == txtAddressPostal)
                    tb.Text = "Postal Code";
                else if (tb == txtAddressCountry)
                    tb.Text = "Country";
            }
        }

        private void txtAddressStreet_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnAddMovie_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    // 1. Insert Address
                    string insertAddress = @"
                INSERT INTO Address (Street, City, PostalCode, Country)
                OUTPUT INSERTED.AddressID
                VALUES (@Street, @City, @PostalCode, @Country)";
                    SqlCommand cmdAddress = new SqlCommand(insertAddress, connection, transaction);
                    cmdAddress.Parameters.AddWithValue("@Street", txtAddressStreet.Text);
                    cmdAddress.Parameters.AddWithValue("@City", txtAddressCity.Text);
                    cmdAddress.Parameters.AddWithValue("@PostalCode", txtAddressPostal.Text);
                    cmdAddress.Parameters.AddWithValue("@Country", txtAddressCountry.Text);
                    int addressId = (int)cmdAddress.ExecuteScalar();

                    // 2. Insert Supplier
                    string insertSupplier = @"
                INSERT INTO Supplier (SupplierName, Email, Phone, AddressID)
                OUTPUT INSERTED.SupplierID
                VALUES (@Name, @Email, @Phone, @AddressID)";
                    SqlCommand cmdSupplier = new SqlCommand(insertSupplier, connection, transaction);
                    cmdSupplier.Parameters.AddWithValue("@Name", txtSupplierName.Text);
                    cmdSupplier.Parameters.AddWithValue("@Email", txtSupplierEmail.Text);
                    cmdSupplier.Parameters.AddWithValue("@Phone", txtSupplierPhone.Text);
                    cmdSupplier.Parameters.AddWithValue("@AddressID", addressId);
                    int supplierId = (int)cmdSupplier.ExecuteScalar();

                    // 3. Insert Genre
                    string insertGenre = @"
                INSERT INTO Genre (GenreName)
                OUTPUT INSERTED.GenreID
                VALUES (@GenreName)";
                    SqlCommand cmdGenre = new SqlCommand(insertGenre, connection, transaction);
                    cmdGenre.Parameters.AddWithValue("@GenreName", txtGenre.Text);
                    int genreId = (int)cmdGenre.ExecuteScalar();

                    // 4. Insert Lead Actor
                    string insertActor = @"
                INSERT INTO Actor (Name, BirthDate)
                OUTPUT INSERTED.ActorID
                VALUES (@Name, NULL)"; // You can collect birthdate later if needed
                    SqlCommand cmdActor = new SqlCommand(insertActor, connection, transaction);
                    cmdActor.Parameters.AddWithValue("@Name", txtLeadActor.Text);
                    int leadActorId = (int)cmdActor.ExecuteScalar();

                    // 5. Insert Movie
                    string insertMovie = @"
                INSERT INTO Movie (Title, SupplierID, GenreID, LeadActorID, RentalPrice)
                OUTPUT INSERTED.MovieID
                VALUES (@Title, @SupplierID, @GenreID, @LeadActorID, @RentalPrice)";
                    SqlCommand cmdMovie = new SqlCommand(insertMovie, connection, transaction);
                    cmdMovie.Parameters.AddWithValue("@Title", txtMovieTitleInput.Text);
                    cmdMovie.Parameters.AddWithValue("@SupplierID", supplierId);
                    cmdMovie.Parameters.AddWithValue("@GenreID", genreId);
                    cmdMovie.Parameters.AddWithValue("@LeadActorID", leadActorId);
                    cmdMovie.Parameters.AddWithValue("@RentalPrice", decimal.Parse(txtRentalPriceInput.Text));
                    int movieId = (int)cmdMovie.ExecuteScalar();

                    // 6. Insert also-acted-in relation (optional)
                    if (!string.IsNullOrWhiteSpace(txtAlsoActedIn.Text))
                    {
                        string insertAlsoActor = @"
                    INSERT INTO Actor (Name, BirthDate)
                    OUTPUT INSERTED.ActorID
                    VALUES (@Name, NULL)";
                        SqlCommand cmdAlsoActor = new SqlCommand(insertAlsoActor, connection, transaction);
                        cmdAlsoActor.Parameters.AddWithValue("@Name", txtAlsoActedIn.Text);
                        int alsoActorId = (int)cmdAlsoActor.ExecuteScalar();

                        // Add Acted_in relation
                        string insertActedIn = @"
                    INSERT INTO Acted_in (ActorID, MovieID)
                    VALUES (@ActorID, @MovieID)";
                        SqlCommand cmdActedIn = new SqlCommand(insertActedIn, connection, transaction);
                        cmdActedIn.Parameters.AddWithValue("@ActorID", alsoActorId);
                        cmdActedIn.Parameters.AddWithValue("@MovieID", movieId);
                        cmdActedIn.ExecuteNonQuery();
                    }

                    // Add lead actor to Acted_in too
                    string insertLeadActedIn = @"
                INSERT INTO Acted_in (ActorID, MovieID)
                VALUES (@ActorID, @MovieID)";
                    SqlCommand cmdLeadActedIn = new SqlCommand(insertLeadActedIn, connection, transaction);
                    cmdLeadActedIn.Parameters.AddWithValue("@ActorID", leadActorId);
                    cmdLeadActedIn.Parameters.AddWithValue("@MovieID", movieId);
                    cmdLeadActedIn.ExecuteNonQuery();

                    // Commit
                    transaction.Commit();
                    MessageBox.Show("Movie added successfully!");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

    }
}
