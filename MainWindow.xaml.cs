using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Wpf;
using System.Data.Odbc;

namespace SPC_Tool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        DataTable SPCLimits = new DataTable();
        DataTable SPCData = new DataTable();
        public OdbcConnection myConnection;

        public MainWindow(bool edit_permissions, OdbcConnection myConnection)
        {
            this.Closing += new System.ComponentModel.CancelEventHandler(Close_Window);
            this.myConnection = myConnection;
            InitializeComponent();
            if (edit_permissions)
            {
                buttonPermissions.Visibility = Visibility.Visible;
                perm.Content = "Administrator";
            }
            else
            {
                perm.Content = "Engineer";
            }
        }

        private void ButtonCharts_Click(object sender, RoutedEventArgs e)
        {
            ChartView ChartWindow = new ChartView(SPCLimits, SPCData, myConnection);
            ChartWindow.Show();
        }

        private void ButtonData_Click(object sender, RoutedEventArgs e)
        {
            DataEntry DataWindow = new DataEntry(myConnection);
            DataWindow.Show();
        }

        private void ButtonPermissions_Click(object sender, RoutedEventArgs e)
        {
            AccessOperations AccessWindow = new AccessOperations(myConnection);
            AccessWindow.Show();
        }

        private void Close_Window(object sender, EventArgs e)
        {
            myConnection.Close();
            Application.Current.Shutdown();
        }

    }
}
