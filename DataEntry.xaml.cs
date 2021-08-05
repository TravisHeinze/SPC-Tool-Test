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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class DataEntry : Window
    {
        public OdbcConnection myConnection;
        public string userFullName;

        private DataTable spcNames = new DataTable();
        private DataTable spcLimits = new DataTable();

        public DataEntry(OdbcConnection myConnection, string userName)
        {
            this.myConnection = myConnection;
            userFullName = userName;
            comboBoxPopulate();
            InitializeComponent();
        }

        private void ButtonSubmit_Click(object sender, RoutedEventArgs e)
        {
            LimitQuery();

            var SPC_Data = (from x in spcLimits.AsEnumerable()
                            select new SPC_List_Data
                            {
                                UCL = Convert.ToDouble(x.Field<string>("UCL")),
                                LCL = Convert.ToDouble(x.Field<string>("LCL")),
                                USL = Convert.ToDouble(x.Field<string>("USL")),
                                LSL = Convert.ToDouble(x.Field<string>("LSL")),
                                CL = Convert.ToDouble(x.Field<string>("CL"))
                            }).ToArray();


            double UCL = SPC_Data.Select(x => x.UCL).First();
            double LCL = SPC_Data.Select(x => x.LCL).First();
            double USL = SPC_Data.Select(x => x.USL).First();
            double LSL = SPC_Data.Select(x => x.LSL).First();
            double CL = SPC_Data.Select(x => x.CL).First();
            string entryDate = DateTime.Now.ToString();

            OdbcCommand comd = myConnection.CreateCommand();
            comd.CommandText = @"Insert into SPCDatabase(SPC_Plan, Data_Entry, UCL, LCL, USL, LSL, CL, Upload_Date, User)
                               Values('" + comboBoxSPC.Text + "'," + Convert.ToDouble(textBoxData.Text) + "," +  UCL + "," + LCL + "," + USL + "," + LSL + "," + CL + ",'" + entryDate + "','" + userFullName + "')";
            comd.Connection = myConnection;
            comd.ExecuteNonQuery();
            MessageBox.Show("Data Submitted!");
            this.Close();
        }

        private void comboBoxPopulate()
        {

            string sqlString = "SELECT DISTINCT SPC_Plan FROM SPCDatabase";
            try
            {
                OdbcCommand cmd = new OdbcCommand(sqlString, myConnection);
                OdbcDataAdapter adapter = new OdbcDataAdapter(cmd);
                adapter.Fill(spcNames);
            }
            catch (OdbcException oex)
            {
                MessageBox.Show(oex.ToString());
                this.Close();
            }

            spcCharts = spcNames.AsEnumerable()
                            .Select(x => x.Field<string>("SPC_Plan"))
                            .ToList();

            DataContext = this;

        }

        private void LimitQuery()
        {

            string sqlString = "SELECT * FROM SPCLimits WHERE SPC_Plan = '" + comboBoxSPC.Text + "'";

            try
            {
                OdbcCommand cmd = new OdbcCommand(sqlString, myConnection);
                OdbcDataAdapter adapter = new OdbcDataAdapter(cmd);
                adapter.Fill(spcLimits);
            }
            catch (OdbcException oex)
            {
                MessageBox.Show(oex.ToString());
                this.Close();
            }



        }

        private void ButtonCacnel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public List<string> spcCharts { get; set; }
    }

}
