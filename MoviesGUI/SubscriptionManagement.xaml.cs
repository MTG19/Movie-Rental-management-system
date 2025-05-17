using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
using Microsoft.Data.SqlClient;

namespace MoviesGUI
{
    /// <summary>
    /// Interaction logic for SubsriptionManagement.xaml
    /// </summary>
    public class Subscription
    {
        public int SubscriptionID { get; set; }
        public string UserName { get; set; }
        public int UserID { get; set; }
        public DateTime StartDate { get; set; }
        public int PrepaidMonths { get; set; }
        public DateTime EndDate { get; set; }
    }
    public partial class SubscriptionManagement : Window
    {
        public ObservableCollection<Subscription> Subscriptions { get; } = new ObservableCollection<Subscription>();
        // Property to store the selected item
        public Subscription SelectedSubscription { get; set; }

        public SubscriptionManagement()
        {
            DataContext = this;  // Enable data binding
            InitializeComponent();
            get_all_subscriptions();
        }

        private void get_all_subscriptions()
        {
            Subscriptions.Clear();
            string constr = @"Server=localhost;Database=MovieRental;Trusted_Connection=True;TrustServerCertificate=True;";
            SqlConnection sqlConnection = new SqlConnection(constr);
            sqlConnection.Open();

            string cmd = @"select 
                        s.SubscriptionID, u.name, s.UserID, s.SubscribingDate, s.PrepaidMonths, s.EndDate 
                        from Subscription as s inner join Users as u 
                        on s.UserID = u.UserID";

            SqlCommand sql_cmd = new SqlCommand(cmd, sqlConnection);

            using (var reader = sql_cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Subscriptions.Add(new Subscription
                    {
                        SubscriptionID = reader.GetInt32(0),
                        UserName = reader.GetString(1),
                        UserID = reader.GetInt32(2),
                        StartDate = reader.GetDateTime(3),
                        PrepaidMonths = reader.GetInt32(4),
                        EndDate = reader.GetDateTime(5)
                    });
                }
            }
            sqlConnection.Close();
        }

        // Enable/disable button based on selection
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            edit_subscription_btn.IsEnabled = Subscription_data.SelectedItem != null;
        }

        // Button click handler - opens edit window
        private void edit_subscription_btn_Click(object sender, RoutedEventArgs e)
        {
            if (Subscription_data.SelectedItem is Subscription selected)
            {
                var editWindow = new Edit_subscription(selected)
                {
                    Owner = this, // Makes management window the parent
                    WindowStartupLocation = WindowStartupLocation.CenterOwner // Centers over parent
                };

                editWindow.Show();

                // Disable main window while editing
                this.IsEnabled = false;
                editWindow.Closed += (s, args) => this.IsEnabled = true;
            }
        }
    }
}
