using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using static MoviesGUI.UsersData;

namespace MoviesGUI
{
    public partial class OrderSummaryWindow : Window
    {
        int current_user_id;
        public OrderSummaryWindow(ObservableCollection<RentingOrderItem> rentOrders, string paymentMethod, int user_id)
        {
            InitializeComponent();
            current_user_id = user_id;
            NavbarUser nav = new NavbarUser();
            NavbarContainer.Content = nav;

            SummaryGrid.ItemsSource = rentOrders;
            TotalMoviesText.Text = rentOrders.Count.ToString();
            TotalPriceText.Text = $"${rentOrders.Sum(item => item.Price):0.00}";
            PaymentMethodText.Text = paymentMethod;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            profile p = new profile(current_user_id);
            p.Show();
            this.Close();
        }

        

    }
}
