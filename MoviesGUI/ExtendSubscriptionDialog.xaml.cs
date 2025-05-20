using System.Windows;

namespace MoviesGUI
{
    public partial class ExtendSubscriptionDialog : Window
    {
        public int MonthsToExtend { get; private set; }

        public ExtendSubscriptionDialog()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(((System.Windows.Controls.ComboBoxItem)MonthsComboBox.SelectedItem).Content.ToString(), out int months))
            {
                MonthsToExtend = months;
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Please select a valid number of months.",
                              "Invalid Input",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}