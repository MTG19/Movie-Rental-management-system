using System;
using System.Windows;
using Microsoft.Data.SqlClient;

namespace MoviesGUI
{
    public partial class EditProfileWindow : Window
    {
        private int userId;
        private string connectionString = @"Server=localhost;Database=MovieRental;Trusted_Connection=True;TrustServerCertificate=True;";

        public EditProfileWindow(int userId, string name, string email, string phone)
        {
            InitializeComponent();
            this.userId = userId;

            NameBox.Text = name;
            EmailBox.Text = email;
            PhoneBox.Text = phone;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE Users SET Name = @Name, Email = @Email, Phone = @Phone WHERE UserID = @UserID";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Name", NameBox.Text);
                    cmd.Parameters.AddWithValue("@Email", EmailBox.Text);
                    cmd.Parameters.AddWithValue("@Phone", PhoneBox.Text);
                    cmd.Parameters.AddWithValue("@UserID", userId);

                    cmd.ExecuteNonQuery();
                }

                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to update profile: " + ex.Message);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
