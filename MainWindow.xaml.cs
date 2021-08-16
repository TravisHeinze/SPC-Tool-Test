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
        public OdbcConnection myConnection;
        public string userFullName;

        public MainWindow(string permissionLevel, OdbcConnection myConnection, string userName)
        {
            this.Closing += new System.ComponentModel.CancelEventHandler(Close_Window);
            this.myConnection = myConnection;
            InitializeComponent();
            if (permissionLevel == "Admin")
            {

                perm.Content = "Administrator";
            }
            else if (permissionLevel == "Engineer")
            {
                buttonPermissions.Visibility = Visibility.Hidden;
                perm.Content = "Engineer";
            }
            else
            {
                buttonPermissions.Visibility = Visibility.Hidden;
                buttonNewPlan.Visibility = Visibility.Hidden;
                perm.Content = "Basic";
            }
            userFullName = userName;
        }

        private void NewChart_Click(object sender, RoutedEventArgs e)
        {
            CreateChart CreateWindow = new CreateChart(myConnection, userFullName);
            CreateWindow.Show();
        }

        private void ButtonCharts_Click(object sender, RoutedEventArgs e)
        {
            ChartView ChartWindow = new ChartView(myConnection);
            ChartWindow.Show();
        }

        private void ButtonData_Click(object sender, RoutedEventArgs e)
        {
            DataEntry DataWindow = new DataEntry(myConnection, userFullName);
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

        private void buttonEdit_Charts_Click(object sender, RoutedEventArgs e)
        {
            EditCharts editCharts = new EditCharts(myConnection, userFullName);
            editCharts.Show();
        }
    }
}
