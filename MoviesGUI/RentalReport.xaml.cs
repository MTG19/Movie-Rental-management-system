using System;
using System.Collections.Generic;
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
    public partial class RentalReport : Window
    {
        string connectionString = @"Server=localhost;Database=MovieRental;Trusted_Connection=True;TrustServerCertificate=True;";

        public RentalReport()
        {
            InitializeComponent();
            LoadRentalReport();
        }

        private void LoadRentalReport()
        {
            string query = @"
                SELECT 
                    u.Name AS UserName,
                    m.Title AS MovieTitle,
                    ro.rentingDate,
                    ro.returnDate,
                    rd.rentingPrice,
                    p.Method AS PaymentMethod,
                    p.PaymentDate
                FROM Users u
                JOIN rentingOrder ro ON u.UserID = ro.UserID
                JOIN rentingDetail rd ON ro.RentalID = rd.RentalID
                JOIN Library l ON rd.TapeID = l.TapeID
                JOIN Movie m ON l.MovieID = m.MovieID
                LEFT JOIN payment p ON ro.RentalID = p.RentalId
                ORDER BY u.Name, ro.rentingDate;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgRentalReport.ItemsSource = dt.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading report: " + ex.Message);
            }
        }
    }
}

