using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MoviesGUI
{
    /// <summary>
    /// Interaction logic for NavbarAdmin.xaml
    /// </summary>
    public partial class NavbarAdmin : UserControl
    {
        public NavbarAdmin()
        {
            InitializeComponent();
        }

        private void Subscription_Click(object sender, RoutedEventArgs e)
        {
            new SubscriptionManagement().Show();
            Window.GetWindow(this)?.Close();
        }

        private void Movies_Click(object sender, RoutedEventArgs e)
        {
            new MoviesAdmin().Show();
            Window.GetWindow(this)?.Close();
        }

        private void AddMovie_Click(object sender, RoutedEventArgs e)
        {
            new AddMovieAdminWindow().Show();
            Window.GetWindow(this)?.Close();
        }

        private void Users_Click(object sender, RoutedEventArgs e)
        {
            new UsersData().Show();
            Window.GetWindow(this)?.Close();
        }

        private void RentalReport_Click(object sender, RoutedEventArgs e)
        {
            new RentalReport().Show();
            Window.GetWindow(this)?.Close();
        }
    }
}
