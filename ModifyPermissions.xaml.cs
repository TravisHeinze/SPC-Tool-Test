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

namespace SPC_Tool
{
    /// <summary>
    /// Interaction logic for ModifyPermissions.xaml
    /// </summary>
    public partial class ModifyPermissions : Window
    {
        public OdbcConnection myConnection;

        public ModifyPermissions(OdbcConnection myConnection)
        {
            this.myConnection = myConnection;
            InitializeComponent();
            fill_combo_boxes();
        }

        public void fill_combo_boxes()
        {
            DataTable SPCUsers = new DataTable();
            string user_query = "SELECT * FROM SPCUsers";

            try
            {
                OdbcCommand cmd = new OdbcCommand(user_query, myConnection);
                OdbcDataAdapter adapter = new OdbcDataAdapter(cmd);
                adapter.Fill(SPCUsers);
            }

            catch (OdbcException oex)
            {
                MessageBox.Show(oex.ToString());
            }

            for (int i = 0; i < SPCUsers.Rows.Count; i++)
            {
                comboBox1.Items.Add(SPCUsers.Rows[i]["User"].ToString());
            }

            comboBox2.Items.Add("Admin");
            comboBox2.Items.Add("Engineer");

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

                int user_idx = 0;

                for(int i = 0; i < dataSet.Tables["SPCUsers"].Rows.Count; i++)
                {
                    if(dataSet.Tables["SPCUsers"].Rows[i]["User"].ToString() == comboBox1.Text)
                    {
                        user_idx = i;
                    }
                }

                dataSet.Tables["SPCUsers"].Rows[user_idx]["Permissions"] = comboBox2.Text;

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
