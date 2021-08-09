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
    /// Interaction logic for CreateChart.xaml
    /// </summary>
    public partial class CreateChart : Window
    {
        private OdbcConnection myConnection;
        string full_name;

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

            if (double.TryParse(ucl.Text, out dbl) && double.TryParse(lcl.Text, out dbl) && double.TryParse(usl.Text, out dbl) && double.TryParse(lsl.Text, out dbl) && double.TryParse(cl.Text, out dbl) && double.TryParse(data_entry.Text, out dbl))
            {
                return true;
            }

            return false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Verify_Input())
            {
                string date = DateTime.Now.ToString();
                OdbcCommand comd = myConnection.CreateCommand();
                comd.CommandText = @"Insert into SPCDatabase(SPC_Plan, Data_Entry, UCL, LCL, USL, LSL, CL, Upload_Date, User)
                               Values('" + plan_name.Text + "'," + Convert.ToDouble(data_entry.Text) + "," + ucl.Text + "," + lcl.Text + "," + usl.Text + "," + lsl.Text + "," + cl.Text + ",'" + date + "','" + full_name + "')";
                comd.Connection = myConnection;
                comd.ExecuteNonQuery();
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid input detected. Please enter appropriate values for all fields. Make sure all fields are filled.");
            }
        }
    }
}
