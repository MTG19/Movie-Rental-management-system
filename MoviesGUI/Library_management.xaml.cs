using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for Library_management.xaml
    /// </summary>
    
    public class Tape{
        public string Title { get; set; }
        public int TapeID { get; set; }
        public int MovieID { get; set; } 
        public int SupplierID { get; set; }
        public string SupplierName { get; set; }
        public string leadActor { get; set; }
        public float RentalPrice { get; set; }
    }

    public partial class Library_management : Window
    {
        public ObservableCollection<Tape> Tapes { get; } = new ObservableCollection<Tape>();

        public Library_management()
        {
            DataContext = this;  // Enable data binding
            InitializeComponent();
            get_all_tapes();
        }

        private void get_all_tapes()
        {
            Tapes.Clear();
            string constr = @"Server=localhost;Database=MovieRental;Trusted_Connection=True;TrustServerCertificate=True;";
            SqlConnection sqlConnection = new SqlConnection(constr);
            sqlConnection.Open();

            string cmd = @"select l.TapeID, m.Title, l.MovieID, RentalPrice, a.Name, s.SupplierID, s.SupplierName
                        from library as l inner join Movie as m 
                        on m.MovieID = l.movieID
                        inner join Actor as a
                        on a.ActorID = m.LeadActorID
                        inner join Supplier as s
                        on s.SupplierID = m.SupplierID";

            SqlCommand sql_cmd = new SqlCommand(cmd, sqlConnection);

            using (var reader = sql_cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Tapes.Add(new Tape
                    {
                        TapeID = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        MovieID = reader.GetInt32(2),
                        RentalPrice = reader.GetFloat(3),
                        leadActor = reader.GetString(4),
                        SupplierID = reader.GetInt32(5),
                        SupplierName = reader.GetString(6)
                    });
                }
            }
            sqlConnection.Close();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EditTape.IsEnabled = tape_data.SelectedItem != null;
        }

        private void EditTape_Click(object sender, RoutedEventArgs e)
        {
            if (tape_data.SelectedItem is Tape selected)
            {
                var editWindow = new MovieAdminEdit(/*selected.MovieID*/) // maybe I should also send the tape Id  
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

        private void AddTape_Click(object sender, RoutedEventArgs e)
        {
            var AddWindow = new AddMovieAdminWindow();
            AddWindow.Show();
            this.Close();
        }


    }
}
