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
using System.Data.OleDb;

namespace SPC_Tool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        DataTable SPCLimits = new DataTable();
        DataTable SPCData = new DataTable();

        public MainWindow(bool edit_permissions)
        {
            InitializeComponent();
            if (edit_permissions)
            {
                buttonPermissions.Visibility = Visibility.Visible;
            }
        }

        private void ButtonCharts_Click(object sender, RoutedEventArgs e)
        {
            ChartView ChartWindow = new ChartView(SPCLimits, SPCData);
            ChartWindow.Show();
        }

        private void ButtonData_Click(object sender, RoutedEventArgs e)
        {
            DataEntry DataWindow = new DataEntry();
            DataWindow.Show();
        }
    }
}
