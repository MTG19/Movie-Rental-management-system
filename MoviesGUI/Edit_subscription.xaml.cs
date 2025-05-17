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
using Microsoft.Data.SqlClient;


namespace MoviesGUI
{
    /// <summary>
    /// Interaction logic for Edit_subscription.xaml
    /// </summary>
    public partial class Edit_subscription : Window
    {
        Subscription new_curr_subscription { get; set; }
        public Edit_subscription(Subscription s)
        {
            Subscription_ID.Text = s.SubscriptionID.ToString();

            new_curr_subscription = s;

            InitializeComponent();
            DataContext = this;
        }



        public void edit_current_subscription(Subscription s) {
            
        }
    }
}
