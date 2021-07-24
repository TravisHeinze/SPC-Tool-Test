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
using System.Windows.Shapes;
using System.Data.OleDb;

namespace SPC_Tool
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Loader : Window
    {
        string connString;
        DataTable SPCLimits = new DataTable();

        public Loader()
        {
            InitializeComponent();
            connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\theinze\\source\\repos\\SPC-Tool-Test\\SPCDatabase.accdb";
            GetTables();
        }

        public void GetTables()
        {
            using (OleDbConnection conn = new OleDbConnection(connString))
            {
                OleDbCommand cmd = new OleDbCommand("SELECT * FROM SPCLimits", conn);

                conn.Open();

                OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                adapter.Fill(SPCLimits);
            }
        }
    }
}
