using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
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
    public partial class RentingDetails : Window
    {
        public ObservableCollection<RentingOrderItem> RentOrders { get; set; }
        private string connectionString = @"Server=localhost;Database=MovieRental;Trusted_Connection=True;TrustServerCertificate=True;";
        private int initialMovieId;

        public RentingDetails(int movieId)
        {
            InitializeComponent();

            NavbarUser nav = new NavbarUser();
            NavbarContainer.Content = nav;

            initialMovieId = movieId;

            RentOrders = new ObservableCollection<RentingOrderItem>();
            RentingOrderGrid.ItemsSource = RentOrders;

            LoadMovieDetails(initialMovieId);
        }

        private void LoadMovieDetails(int movieId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT Title, RentalPrice FROM Movie WHERE MovieID = @MovieID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MovieID", movieId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        RentOrders.Add(new RentingOrderItem
                        {
                            MovieTitle = reader["Title"].ToString(),
                            Amount = 1,
                            RentDate = DateTime.Today,
                            ReturnDate = DateTime.Today.AddDays(3),
                            Price = Convert.ToDouble(reader["RentalPrice"])
                        });

                        UpdateSummary();
                    }
                    else
                    {
                        MessageBox.Show("Movie not found.");
                    }
                }
            }
        }

        private void UpdateSummary()
        {
            TotalMoviesText.Text = RentOrders.Count.ToString();

            double totalPrice = 0;
            foreach (var item in RentOrders)
                totalPrice += item.Total;

            TotalPriceText.Text = $"${totalPrice:F2}";
        }

        private void AddMovie_Click(object sender, RoutedEventArgs e)
        {
            MoviesUser userWindow = new MoviesUser();
            userWindow.Show();
            this.Close();
        }

        private void CancelAll_Click(object sender, RoutedEventArgs e)
        {
            RentOrders.Clear();
            UpdateSummary();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.DataContext is RentingOrderItem item)
            {
                RentOrders.Remove(item);
                UpdateSummary();
            }
        }

        private void PlaceOrder_Click(object sender, RoutedEventArgs e)
        {
            if (RentOrders.Count == 0)
            {
                MessageBox.Show("No movies selected for rent.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Payment Method
            string method = CashRadio.IsChecked == true ? "Cash"
                            : CardRadio.IsChecked == true ? "Credit Card"
                            : "Online Payment";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Get the next available RentalID
                SqlCommand getIdCmd = new SqlCommand("SELECT ISNULL(MAX(RentalID), 0) + 1 FROM rentingOrder", conn);
                int nextRentalID = (int)getIdCmd.ExecuteScalar();

                foreach (var item in RentOrders)
                {
                    string insertQuery = @"
                        INSERT INTO rentingOrder (RentalID, rentingDate, returnDate, UserID)
                        VALUES (@RentalID, @RentDate, @ReturnDate, @UserID)";

                    SqlCommand cmd = new SqlCommand(insertQuery, conn);
                    cmd.Parameters.AddWithValue("@RentalID", nextRentalID++);
                    cmd.Parameters.AddWithValue("@RentDate", item.RentDate);
                    cmd.Parameters.AddWithValue("@ReturnDate", item.ReturnDate);
                    cmd.Parameters.AddWithValue("@UserID", 1); // Replace with actual user ID if needed

                    cmd.ExecuteNonQuery();
                }
            }

            OrderSummaryWindow summaryWindow = new OrderSummaryWindow(RentOrders, method);
            summaryWindow.Show();
            this.Close();
        }
    }

    public class RentingOrderItem
    {
        public string MovieTitle { get; set; }
        public int Amount { get; set; }
        public DateTime RentDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public double Price { get; set; }
        public double Total => Amount * Price;
    }
}
