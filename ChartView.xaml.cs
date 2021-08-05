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
using LiveCharts;
using LiveCharts.Wpf;
using System.Data.Odbc;

namespace SPC_Tool
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class ChartView : Window
    {
        /// <ChartVariables>
        /// List of varibles needed for contorl chart. Made public to
        /// access all all functions.
        /// </ChartVariables>

        //Create Odnc public connection. 
        public OdbcConnection myConnection;

        //Instantiate the datatable variables
        DataTable spcNames = new DataTable();
        DataTable SPCData = new DataTable();

        //Line Series format for charts
        public LineSeries spcUCL = new LineSeries
        {
            Title = "UCL",
            Fill = Brushes.Transparent,
            PointGeometrySize = 0,
            Stroke = Brushes.Orange
        };
        public LineSeries spcLCL = new LineSeries
        {
            Title = "LCL",
            Fill = Brushes.Transparent,
            PointGeometrySize = 0,
            Stroke = Brushes.Orange
        };
        public LineSeries spcUSL = new LineSeries
        {
            Title = "USL",
            Fill = Brushes.Transparent,
            PointGeometrySize = 0,
            Stroke = Brushes.Red
        };
        public LineSeries spcLSL = new LineSeries
        {
            Title = "LSL",
            Fill = Brushes.Transparent,
            PointGeometrySize = 0,
            Stroke = Brushes.Red
        };
        public LineSeries spcCenterLine = new LineSeries
        {
            Title = "Center Line",
            Fill = Brushes.Transparent,
            PointGeometrySize = 0,
            Stroke = Brushes.Blue,
            StrokeDashArray = new DoubleCollection { 2 }
        };
        public LineSeries spcDataSet = new LineSeries
        {
            Fill = Brushes.Transparent,
            PointGeometrySize = 15,
            Stroke = Brushes.Black,
            StrokeThickness = 4
        };

        public ChartView(OdbcConnection myConnection)
        {
            this.myConnection = myConnection;
            CreateDataset();
            spcPlans();
            InitializeComponent();
        }

        public void CreateDataset()
        {
            spcUCL.Values = new ChartValues<double> { 4, 4, 4, 4, 4 };
            spcLCL.Values = new ChartValues<double> { 2, 2, 2, 2, 2, };
            spcLSL.Values = new ChartValues<double> { 1, 1, 1, 1, 1, };
            spcUSL.Values = new ChartValues<double> { 5, 5, 5, 5, 5, };
            spcCenterLine.Values = new ChartValues<double> { 3, 3, 3, 3, 3, };

            spcDataSet.Title = "Data Set";
            spcDataSet.Values = new ChartValues<double> { 1.5, 2.1, 4.9, 3.3, 2.7, };

            DataCollection = new SeriesCollection
            {
                spcUCL, spcUSL, spcLCL, spcLSL, spcCenterLine, spcDataSet
            };

            DataContext = this;
        }

        public void spcPlans()
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

        }

        public void GetTablesODBC()
        {
            SPCData.Clear();

            string sqlstring2 = "SELECT TOP 50 * FROM SPCDatabase WHERE SPC_Plan = '" + comboChartNames.SelectedItem.ToString() + "' ORDER BY Upload_Date DESC";

            try
            {
                OdbcCommand cmd2 = new OdbcCommand(sqlstring2, myConnection);
                OdbcDataAdapter adapter2 = new OdbcDataAdapter(cmd2);
                adapter2.Fill(SPCData);
            }
            catch (OdbcException oex)
            {
                MessageBox.Show(oex.ToString());
            }

        }

        public void UpdateGraph()
        {
            GetTablesODBC();

            var SPC_Data = (from x in SPCData.AsEnumerable()
                          where x.Field<string>("SPC_Plan") == comboChartNames.SelectedItem.ToString()
                          select new SPC_List_Data
                          {
                              UCL = x.Field<double>("UCL"),
                              LCL = x.Field<double>("LCL"),
                              USL = x.Field<double>("USL"),
                              LSL = x.Field<double>("LSL"),
                              CL = x.Field<double>("CL"),
                              Data = x.Field<double>("Data_Entry")
                          }).ToList();

            spcUCL.Values = new ChartValues<double>(SPC_Data.Select(x => x.UCL).ToList());
            spcLCL.Values = new ChartValues<double>(SPC_Data.Select(x => x.LCL).ToList());
            spcUSL.Values = new ChartValues<double>(SPC_Data.Select(x => x.USL).ToList());
            spcLSL.Values = new ChartValues<double>(SPC_Data.Select(x => x.LSL).ToList());
            spcCenterLine.Values = new ChartValues<double>(SPC_Data.Select(x => x.CL).ToList());
            spcDataSet.Values = new ChartValues<double>(SPC_Data.Select(x => x.Data).ToList());
        }

        private void ComboDataSets_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateGraph();
        }

        public List<string> spcCharts { get; set; }
        public SeriesCollection DataCollection { get; set; }
    }

    public class SPC_List_Data
    {
        //List to collect all contol, spec, and data 
        public double UCL { get; set; }
        public double LCL { get; set; }
        public double USL { get; set; }
        public double LSL { get; set; }
        public double CL { get; set; }
        public double Data { get; set; }

    }

}
