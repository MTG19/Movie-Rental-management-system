using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.Data.SqlClient;

namespace MoviesGUI
{
    public partial class UsersData : Window
    {
        private string connectionString = @"Server=localhost;Database=MovieRental;Trusted_Connection=True;TrustServerCertificate=True;";
        private List<User> allUsers = new List<User>();

        public UsersData()
        {
            InitializeComponent();
            LoadUsers();

            SearchBox.TextChanged += (s, e) => FilterUsers(SearchBox.Text);
        }

        private void LoadUsers()
        {
            try
            {
                allUsers.Clear();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT UserID, Name, Email, Phone FROM Users";
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        allUsers.Add(new User
                        {
                            Id = Convert.ToInt32(reader["UserID"]),
                            Name = reader["Name"].ToString(),
                            Email = reader["Email"].ToString(),
                            Phone = reader["Phone"].ToString(),
                            //Address = "N/A" // the user has no address 
                        });
                    }

                    reader.Close();
                }

                UsersGrid.ItemsSource = allUsers;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load users: " + ex.Message);
            }
        }

        private void FilterUsers(string searchText)
        {
            var filtered = allUsers.Where(u =>
                u.Name.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                u.Email.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0).ToList();

            UsersGrid.ItemsSource = filtered;
        }

        public class User
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public string Address { get; set; }
        }
    }
}
