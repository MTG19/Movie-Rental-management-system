using System;
using System.Windows;
using System.ComponentModel;
using System.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Data.SqlClient;


namespace MoviesGUI
{
    public partial class Edit_subscription : Window, INotifyPropertyChanged
    {
        private Subscription _currentSubscription;
        public Subscription CurrentSubscription
        {
            get => _currentSubscription;
            set
            {
                _currentSubscription = value;
                OnPropertyChanged(nameof(CurrentSubscription));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Edit_subscription(Subscription s)
        {
            CurrentSubscription = new Subscription()
            {
                SubscriptionID = s.SubscriptionID,
                UserName = s.UserName,
                UserID = s.UserID,
                StartDate = s.StartDate,
                PrepaidMonths = s.PrepaidMonths
            };

            InitializeComponent();
            DataContext = this;
            
            // Initialize controls
            Subscription_ID.Text = CurrentSubscription.SubscriptionID.ToString();
            Start_Date.SelectedDate = CurrentSubscription.StartDate;
            PrepaidMonthsBox.Text = CurrentSubscription.PrepaidMonths.ToString();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void PrepaidMonthsBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(PrepaidMonthsBox.Text, out int months))
            {
                CurrentSubscription.PrepaidMonths = months;
            }
        }

    private bool UpdateSubscriptionInDatabase(Subscription subscription)
    {
        string connectionString = @"Server=localhost;Database=MovieRental;Trusted_Connection=True;TrustServerCertificate=True;";
        
        try{
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(
                @"UPDATE Subscription 
                SET SubscribingDate = @SubscribingDate, 
                    PrepaidMonths = @PrepaidMonths, 
                    EndDate = @EndDate 
                WHERE SubscriptionID = @SubscriptionID", connection))
            {
                // Add parameters to prevent SQL injection
                command.Parameters.AddWithValue("@SubscribingDate", subscription.StartDate);
                command.Parameters.AddWithValue("@PrepaidMonths", subscription.PrepaidMonths);
                command.Parameters.AddWithValue("@EndDate", subscription.EndDate);
                command.Parameters.AddWithValue("@SubscriptionID", subscription.SubscriptionID);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                
                // Return true if exactly one row was updated
                return rowsAffected == 1;
            }
        }
        
        catch (SqlException ex){
            MessageBox.Show($"Database error: {ex.Message}\n\nPlease check your connection and try again.",
                            "Database Error",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
            return false;
        }
        
        catch (Exception ex){
            MessageBox.Show($"Unexpected error: {ex.Message}",
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
            return false;
        }
    }


        private void edit_subcription_Click(object sender, RoutedEventArgs e)
        {
            // Validate input
            if (Start_Date.SelectedDate == null)
            {
                MessageBox.Show("Please select a valid start date");
                return;
            }

            if (!int.TryParse(PrepaidMonthsBox.Text, out int months) || months <= 0)
            {
                MessageBox.Show("Please enter a valid number of months");
                return;
            }

            // Update the subscription
            CurrentSubscription.EndDate = CurrentSubscription.StartDate.AddMonths(CurrentSubscription.PrepaidMonths);
            UpdateSubscriptionInDatabase(CurrentSubscription);

            //DialogResult = true;
            Close();
        }

        private void Cancel_btn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}