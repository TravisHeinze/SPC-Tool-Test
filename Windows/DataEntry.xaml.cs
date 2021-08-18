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
        public DataTable entrySource = new DataTable();
        //List<string> source = new List<string>();

        public DataEntry(OdbcConnection myConnection, string userName)
        {
            this.myConnection = myConnection;
            userFullName = userName;
            //LimitQuery();
            FillTable();
            comboBoxPopulate();
            InitializeComponent();
            //entry_grid.CanUserAddRows = false;
            //entry_grid.CanUserDeleteRows = false;
            //entry_grid.CanUserReorderColumns = false;
            //entry_grid.CanUserSortColumns = false;
            //LimitQuery();
        }

        public void FillTable()
        {
            string query = "SELECT * FROM SPCLimits";
            OdbcCommand cmd = new OdbcCommand(query, myConnection);
            OdbcDataAdapter adapter = new OdbcDataAdapter(cmd);
            adapter.Fill(spcLimits);
        }

        public void FillDatagrid()
        {
            //List<string> source = new List<string>();

            var num_entries = (from row in spcLimits.AsEnumerable()
                              where row.Field<string>("SPC_Plan") == comboBoxSPC.SelectedItem.ToString()
                              select row.Field<int>("Number of Entries")).FirstOrDefault();

            //MessageBox.Show(num_entries.ToString());

            for(int i = 0; i < num_entries; i++)
            {

                entrySource.Columns.Add("Entry " + (i + 1).ToString());


                /*entry_grid.Columns.Add("Entry " + i.ToString());
                //entry_grid.Rows.Add("");
                entry_grid.Items.Add("");*/


                //DataGridTextColumn col = new DataGridTextColumn();
                //col.Header = "Entry " + (i + 1).ToString();
                //col.Binding = new Binding(".");
                //source.Add("");
                //col.Binding = new Binding(source[i]);
                //entry_grid.Columns.Add(col);
                //source.Add("");
                //entrySource.Columns.Add("");
            }

            //entrySource.Rows.Add("");


           /*  foreach(DataColumn col in entrySource.Columns)
             {
                DataGridBoundColumn _xtemp = new DataGridTextColumn();
                _xtemp.Header = "Entry 1";
                _xtemp.Binding = new Binding("Entry 1");
                entry_grid.Columns.Add(_xtemp);
             }*/

            entry_grid.ItemsSource = entrySource.DefaultView;
/*
            for(int i = 0; i < entry_grid.Columns.Count; i++)
            {
                if(entry_grid.Columns[i].Header.ToString().Contains("Entry"))
                {
                    entry_grid.Columns[i].Visibility = Visibility.Hidden;
                }
            }*/

            //source.Add("");
            //entry_grid.ItemsSource = source;
            //entry_grid.Items.Add("");
            //entry_grid.IsReadOnly = false;

        }

        private void ButtonSubmit_Click(object sender, RoutedEventArgs e)
        {
            double dbl;
            double avg = 0;
            double sum = 0;
            //if (double.TryParse(textBoxData.Text, out dbl))
           // {

                //LimitQuery();

                var SPC_Data = (from x in spcLimits.AsEnumerable()
                                select new SPC_List_Data
                                {
                                    UCL = x.Field<double>("UCL"),
                                    LCL = x.Field<double>("LCL"),
                                    USL = x.Field<double>("USL"),
                                    LSL = x.Field<double>("LSL"),
                                    CL = x.Field<double>("CL")
                                }).ToArray();


                double UCL = SPC_Data.Select(x => x.UCL).First();
                double LCL = SPC_Data.Select(x => x.LCL).First();
                double USL = SPC_Data.Select(x => x.USL).First();
                double LSL = SPC_Data.Select(x => x.LSL).First();
                double CL = SPC_Data.Select(x => x.CL).First();
                string entryDate = DateTime.Now.ToString();

            for (int i = 0; i < entry_grid.Columns.Count; i++)
            {
                //MessageBox.Show(entrySource.Rows[0][i].ToString());
                sum += double.Parse(entrySource.Rows[0][i].ToString());
            }
            avg = sum / entry_grid.Columns.Count;

                    OdbcCommand comd = myConnection.CreateCommand();
                    comd.CommandText = @"Insert into SPCDatabase(SPC_Plan, Data_Entry, UCL, LCL, USL, LSL, CL, Upload_Date, User)
                               Values('" + comboBoxSPC.Text + "'," + Convert.ToDouble(avg.ToString())/*Convert.ToDouble(entrySource.Rows[0][i].ToString())*/ /*Convert.ToDouble(textBoxData.Text)*/ + "," + UCL + "," + LCL + "," + USL + "," + LSL + "," + CL + ",'" + entryDate + "','" + userFullName + "')";
                    comd.Connection = myConnection;
                    comd.ExecuteNonQuery();

                

                MessageBox.Show("Data Submitted!");
                this.Close();
            //}

            //else
            //{
             //   MessageBox.Show("Invalid input. Please enter numerical value");
            //}
        }

        private void comboBoxPopulate()
        {

            string sqlString = "SELECT DISTINCT SPC_Plan FROM SPCLimits WHERE [Status] = 'Active'";
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

       /* private void LimitQuery()
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

        }*/

        private void ButtonCacnel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public List<string> spcCharts { get; set; }

        private void FitToContent()
        {
            foreach (DataGridColumn column in entry_grid.Columns)
            {
                column.Width = new DataGridLength(1.0, DataGridLengthUnitType.Star);
            }
        }

        private void comboBoxSPC_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //LimitQuery();
            /*entry_grid.Columns.Clear();
            entry_grid.Items.Clear();*/
            //source.Clear();
            //entrySource.Columns.Clear();
            //entrySource.Clear();
            entrySource.Columns.Clear();
            entry_grid.ItemsSource = null;
            FillDatagrid();
            FitToContent();
        }
    }

}
