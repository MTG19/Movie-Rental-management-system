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
    public partial class AddMovieAdminWindow : Window
    {
        public AddMovieAdminWindow()
        {
            InitializeComponent();
        }

        // ADD button
        private void btnAddMovie_Click(object sender, RoutedEventArgs e)
        {
            
            MessageBox.Show("Movie added successfully! (This is just a placeholder)", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

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
    }
}
