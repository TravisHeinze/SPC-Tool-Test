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
using System.Data;

namespace SPC_Tool
{
    /// <summary>
    /// Interaction logic for RemoveUser.xaml
    /// </summary>
    public partial class RemoveUser : Window
    {
        public RemoveUser()
        {
            InitializeComponent();
            fill_comboBox();
        }

        public void fill_comboBox()
        {
            DataTable SPCUsers = new DataTable();

            string connString = (@"Driver={Microsoft Access Driver (*.mdb, *.accdb)};" +
                @"Dbq=\\tekfs6.central.tektronix.net\wce\Maxtek\mxt-dept\MfgCommon\SPC Tool\Database\SPCDatabase.accdb; Uid=Admin; Pwd=;");

            string user_query = "SELECT * FROM SPCUsers";

            try
            {
                using (OdbcConnection myConnection = new OdbcConnection(connString))
                {
                    OdbcCommand cmd = new OdbcCommand(user_query, myConnection);
                    myConnection.Open();
                    OdbcDataAdapter adapter = new OdbcDataAdapter(cmd);
                    adapter.Fill(SPCUsers);
                    myConnection.Close();
                }
            }

            catch (OdbcException oex)
            {
                MessageBox.Show(oex.ToString());
            }

            for(int i = 0; i < SPCUsers.Rows.Count; i++)
            {
                comboBox1.Items.Add(SPCUsers.Rows[i]["User"].ToString());
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string connString = (@"Driver={Microsoft Access Driver (*.mdb, *.accdb)};" +
                @"Dbq=\\tekfs6.central.tektronix.net\wce\Maxtek\mxt-dept\MfgCommon\SPC Tool\Database\SPCDatabase.accdb; Uid=Admin; Pwd=;");

            string user_query = "DELETE FROM SPCUsers WHERE [User] = '" + comboBox1.Text + "'";

            try
            {
                using (OdbcConnection myConnection = new OdbcConnection(connString))
                {

                    myConnection.Open();
                    OdbcCommand cmd = new OdbcCommand(user_query, myConnection);
                    cmd.ExecuteNonQuery();

                    myConnection.Close();
                }
            }

            catch (OdbcException oex)
            {
                MessageBox.Show(oex.ToString());
            }

            this.Close();
        }
    }
}
