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
        public DataEntry()
        {
            InitializeComponent();
        }

        private void ButtonSubmit_Click(object sender, RoutedEventArgs e)
        {
            string connString = (@"Driver={Microsoft Access Driver (*.mdb, *.accdb)};" +
                         @"Dbq=\\tekfs6.central.tektronix.net\wce\\Maxtek\mxt-dept\MfgCommon\SPC Tool\Database\SPCDatabase.accdb; Uid=Admin; Pwd=;");

            OdbcConnection conn = new OdbcConnection(connString);
            OdbcCommand comd = conn.CreateCommand();
            conn.Open();
            comd.CommandText = "Insert into SPCDatabase(SPC_Plan, Data_Entry)Values('" + textBoxPlan.Text + "','" + textBoxData.Text + "')";
            comd.Connection = conn;
            comd.ExecuteNonQuery();
            conn.Close();
            MessageBox.Show("Data Submitted!");
            this.Close();
        }

        private void ButtonCacnel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
