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
        }

        private void Movies_Click(object sender, RoutedEventArgs e)
        {
            new MoviesAdmin().Show();
        }

        private void AddMovie_Click(object sender, RoutedEventArgs e)
        {
            new AddMovieAdminWindow().Show();
        }

        private void Library_Click(object sender, RoutedEventArgs e)
        {
            new Library_management().Show();
        }

        private void Users_Click(object sender, RoutedEventArgs e)
        {
            new UsersData().Show();
        }

        private void RentalReport_Click(object sender, RoutedEventArgs e)
        {
            new RentalReport().Show();
        }
    }
}
