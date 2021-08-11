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
    /// Interaction logic for CreateChart.xaml
    /// </summary>
    public partial class CreateChart : Window
    {
        private OdbcConnection myConnection;
        string full_name;
        DataTable spcNames = new DataTable();

        #region constructor
        /// <summary>
        /// Constructor for creating new chart window
        /// </summary>
        /// <param name="myConnection">Connection object</param>
        public CreateChart(OdbcConnection myConnection, string full_name)
        {
            this.myConnection = myConnection;
            this.full_name = full_name;
            InitializeComponent();
        }


        #endregion

        private bool Verify_Input()
        {
            double dbl;
            string plan_query = "SELECT DISTINCT SPC_Plan FROM SPCLimits";
            bool unique_planName = true;

            try
            {
                OdbcCommand cmd = new OdbcCommand(plan_query, myConnection);
                OdbcDataAdapter adapter = new OdbcDataAdapter(cmd);
                adapter.Fill(spcNames);
            }
            catch (OdbcException oex)
            {
                MessageBox.Show(oex.ToString());
                this.Close();
            }

            for (int i = 0; i < spcNames.Rows.Count; i++)
            {
                if (spcNames.Rows[i][0].ToString() == plan_name.Text)
                {
                    unique_planName = false;
                }
            }

            if (double.TryParse(ucl.Text, out dbl) && double.TryParse(lcl.Text, out dbl) && double.TryParse(usl.Text, out dbl) && double.TryParse(lsl.Text, out dbl) && double.TryParse(cl.Text, out dbl) && double.TryParse(rule1.Text, out dbl) && double.TryParse(rule2.Text, out dbl) && double.TryParse(rule3.Text, out dbl) && unique_planName)
            {
                return true;
            }

            return false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Verify_Input())
            {
                string updateDate = DateTime.Now.ToString();
                OdbcCommand comd = myConnection.CreateCommand();
                comd.CommandText = "INSERT INTO SPCLimits([SPC_Plan], [UCL], [LCL], [USL], [LSL], [CL], [Rule 1], [Rule 2], [Rule 3], [Date_Updated], [User])";
                comd.CommandText += $"VALUES('{plan_name.Text}', {ucl.Text}, {lcl.Text}, {usl.Text}, {lsl.Text}, {cl.Text}, {rule1.Text}, {rule2.Text}, {rule3.Text}, '{updateDate}', '{full_name}')";
                comd.Connection = myConnection;
                comd.ExecuteNonQuery();
                MessageBox.Show("Plan Submitted!");
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid input detected. Please enter appropriate values for all fields. Make sure all fields are filled and the plan name is unique.");
            }
        }
    }
}
