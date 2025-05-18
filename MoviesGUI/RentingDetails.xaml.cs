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

            // Payment Method example
            string method = CashRadio.IsChecked == true ? "Cash"
                            : CardRadio.IsChecked == true ? "Credit Card"
                            : "Online Payment";

            OrderSummaryWindow userWindow = new OrderSummaryWindow(RentOrders, method);
            userWindow.Show();
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
