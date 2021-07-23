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
using System.Data.OleDb;

namespace SPC_Tool
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class DataEntry : Window
    {
        OleDbConnection con = new OleDbConnection();
        public DataEntry()
        {
            InitializeComponent();
            con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\theinze\\source\\repos\\SPC-Tool-Test\\SPCDatabase.accdb");
        }

        private void ButtonSubmit_Click(object sender, RoutedEventArgs e)
        {
            OleDbCommand cmd = con.CreateCommand();
            con.Open();
            cmd.CommandText = "Insert into SPCDatabase(SPC_Plan, Data_Entry)Values('" + textBoxPlan.Text + "','" + textBoxData.Text + "')";
            cmd.Connection = con;
            cmd.ExecuteNonQuery();
            MessageBox.Show("Record Submitted", "Congrats");
            con.Close();
            this.Close();
        }

        private void ButtonCacnel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
