using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MoviesGUI
{
    public partial class RentingDetails : Window
    {
        public ObservableCollection<RentingOrderItem> RentOrders { get; set; }

        public RentingDetails(int movieId)
        {
            InitializeComponent();
            RentOrders = new ObservableCollection<RentingOrderItem>();
            RentingOrderGrid.ItemsSource = RentOrders;
            UpdateTotals();
        }

        private void AddMovie_Click(object sender, RoutedEventArgs e)
        {
            MoviesUser moviesuserWindow = new MoviesUser();
            moviesuserWindow.Show();
            this.Close(); 
        }


        private void CancelAll_Click(object sender, RoutedEventArgs e)
        {
            RentOrders.Clear();
            UpdateTotals();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var item = button?.DataContext as RentingOrderItem;

            if (item != null && RentOrders.Contains(item))
            {
                RentOrders.Remove(item);
                UpdateTotals();
            }
        }

        private void UpdateTotals()
        {
            TotalMoviesText.Text = RentOrders.Count.ToString();
            double totalPrice = RentOrders.Sum(item => item.Price);
            TotalPriceText.Text = $"${totalPrice:0.00}";
        }

        private void PlaceOrder_Click(object sender, RoutedEventArgs e)
        {
            if (RentOrders.Count == 0)
            {
                MessageBox.Show("No movies in the renting order.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string paymentMethod = CashRadio.IsChecked == true ? "Cash" :
                                   CardRadio.IsChecked == true ? "Credit Card" :
                                   "Online Payment";

            MessageBox.Show($"Order placed successfully!\nPayment Method: {paymentMethod}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            RentOrders.Clear();
            UpdateTotals();
        }
    }

    public class RentingOrderItem
    {
        public string MovieTitle { get; set; }
        public int Amount { get; set; }
        public DateTime RentDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public double Price { get; set; }
    }
}
