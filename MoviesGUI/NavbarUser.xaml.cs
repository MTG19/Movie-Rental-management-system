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
    /// Interaction logic for NavbarUser.xaml
    /// </summary>
    public partial class NavbarUser : UserControl
    {
        private int userId;

        public NavbarUser(int id)
        {
            InitializeComponent();
            userId = id;
        }

        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            new profile(userId).Show();
        }

        private void Movies_Click(object sender, RoutedEventArgs e)
        {
            new MoviesUser().Show();
        }
    }
}
