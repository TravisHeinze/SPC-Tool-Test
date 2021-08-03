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
using System.Data;
using System.Data.Odbc;
using Microsoft.Win32;

namespace SPC_Tool
{
    /// <summary>
    /// Interaction logic for NewUser.xaml
    /// </summary>
    public partial class NewUser : Window
    {
        public OdbcConnection myConnection;

        public NewUser(OdbcConnection myConnection)
        {
            this.myConnection = myConnection;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string user_query = "SELECT * FROM SPCUsers";

            try
            {
                OdbcDataAdapter adapter = new OdbcDataAdapter();
                adapter.SelectCommand = new OdbcCommand(user_query, myConnection);
                OdbcCommandBuilder builder = new OdbcCommandBuilder(adapter);

                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet, "SPCUsers");

                DataRow newRow = dataSet.Tables["SPCUsers"].NewRow();
                newRow["User"] = user_id.Text;
                newRow["Email"] = user_email.Text;
                newRow["Permissions"] = comboBox1.Text;

                dataSet.Tables["SPCUsers"].Rows.Add(newRow);

                builder.GetUpdateCommand();
                adapter.Update(dataSet, "SPCUsers");
            }

            catch (OdbcException oex)
            {
                MessageBox.Show(oex.ToString());
            }

            this.Close();
        }
    }
}
