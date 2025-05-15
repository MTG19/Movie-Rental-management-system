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

namespace MoviesGUI
{
    /// <summary>
    /// Interaction logic for MovieUserWindow.xaml
    /// </summary>
    public partial class MovieUserWindow : Window
    {
        public MovieUserWindow()
        {
            InitializeComponent();
        }

        private void btnRent_Click(object sender, RoutedEventArgs e)
        {
            RentingDetails Renting_Details = new RentingDetails();
            Renting_Details.Show();
            this.Close();

        }
    }
}
