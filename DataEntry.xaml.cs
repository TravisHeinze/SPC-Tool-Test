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
using System.Data.Odbc;

namespace SPC_Tool
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class DataEntry : Window
    {
        public OdbcConnection myConnection;

        public DataEntry(OdbcConnection myConnection)
        {
            this.myConnection = myConnection;
            InitializeComponent();
        }

        private void ButtonSubmit_Click(object sender, RoutedEventArgs e)
        {
            OdbcCommand comd = myConnection.CreateCommand();
            comd.CommandText = "Insert into SPCDatabase(SPC_Plan, Data_Entry)Values('" + textBoxPlan.Text + "','" + textBoxData.Text + "')";
            comd.Connection = myConnection;
            comd.ExecuteNonQuery();
            MessageBox.Show("Data Submitted!");
            this.Close();
        }

        private void ButtonCacnel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
