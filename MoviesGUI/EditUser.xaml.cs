using System.Windows;

namespace MoviesGUI
{
    public partial class EditUserWindow : Window
    {
        private UsersData.User user;

        public EditUserWindow(UsersData.User user)
        {
            InitializeComponent();
            this.user = user;

            NameBox.Text = user.Name;
            EmailBox.Text = user.Email;
            PhoneBox.Text = user.Phone;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            user.Name = NameBox.Text;
            user.Email = EmailBox.Text;
            user.Phone = PhoneBox.Text;
            this.DialogResult = true;
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
