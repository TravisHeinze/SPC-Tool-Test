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
using System.Data.Odbc;

namespace SPC_Tool
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Loader : Window
    {
        public Loader(DataTable SPCLimits, DataTable SPCData)
        {
            InitializeComponent();
            this.Show();
            GetTablesODBC(SPCLimits, SPCData);
            this.Close();
        }

        public void GetTablesODBC(DataTable SPCLimits, DataTable SPCData)
        {
            TestDataBase();

            string connString = (@"Driver={Microsoft Access Driver (*.mdb, *.accdb)};" +
                @"Dbq=\\tekfs6.central.tektronix.net\wce\\Maxtek\mxt-dept\MfgCommon\SPC Tool\Database\SPCDatabase.accdb; Uid=Admin; Pwd=;");

            string sqlString = "SELECT * FROM SPCLimits";
            string sqlstring2 = "SELECT * FROM SPCDatabase";

            try
            {
                using (OdbcConnection myConnection = new OdbcConnection(connString))
                {
                    OdbcCommand cmd = new OdbcCommand(sqlString, myConnection);
                    OdbcCommand cmd2 = new OdbcCommand(sqlstring2, myConnection);

                    myConnection.Open();

                    OdbcDataAdapter adapter = new OdbcDataAdapter(cmd);
                    OdbcDataAdapter adapter2 = new OdbcDataAdapter(cmd2);
                    adapter.Fill(SPCLimits);
                    adapter2.Fill(SPCData);
                    myConnection.Close();
                }
            }
            catch(OdbcException oex)
            {
                MessageBox.Show(oex.ToString());
            }

        }

        public void TestDataBase()
        {
            string connString = (@"Driver={Microsoft Access Driver (*.mdb, *.accdb)};" +
                @"Dbq=\\tekfs6.central.tektronix.net\wce\Maxtek\mxt-dept\MfgCommon\SPC Tool\Database\SPCDatabase.accdb; Uid=Admin; Pwd=;");

            OdbcConnection testConn = new OdbcConnection(connString);
            try
            {
                testConn.Open();
                testConn.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Failed to connect to database. Error " + ex.ToString());
                this.Close();
            }

        }
    }
}
