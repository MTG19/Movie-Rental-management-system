using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace MoviesGUI
{
    public partial class OrderSummaryWindow : Window
    {
        public OrderSummaryWindow(ObservableCollection<RentingOrderItem> rentOrders, string paymentMethod)
        {
            InitializeComponent();

            SummaryGrid.ItemsSource = rentOrders;
            TotalMoviesText.Text = rentOrders.Count.ToString();
            TotalPriceText.Text = $"${rentOrders.Sum(item => item.Price):0.00}";
            PaymentMethodText.Text = paymentMethod;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
