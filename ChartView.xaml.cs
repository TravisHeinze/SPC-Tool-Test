using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Data;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Wpf;
using System.Data.Odbc;
using LiveCharts.Configurations;

namespace SPC_Tool
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class ChartView : Window
    {
        #region Public Variables

        /// <ChartVariables>
        /// List of varibles needed for contorl chart. Made public to
        /// access all all functions.
        /// </ChartVariables>
        
        //Create Odnc public connection. 
        public OdbcConnection myConnection;

        //Instantiate the datatable variables
        DataTable spcNames = new DataTable();
        DataTable SPCData = new DataTable();

        #endregion

        #region Line Series
        /// <summary>
        /// Set up line series for graph
        /// </summary>
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

        #endregion

        #region Construction
        /// <summary>
        /// Standard constuctor
        /// </summary>
        /// <param name="myConnection">Odbc connection passed from main </param>
        public ChartView(OdbcConnection myConnection)
        {
            //set this froms SQL connection to passed connection from main
            this.myConnection = myConnection;

            //Run Create dataset for dummy data
            CreateDataset();

            //Run spcPlans to populate combo box
            spcPlans();

            //Initialze form and show
            InitializeComponent();
        }

        #endregion

        #region Dummy Data
        /// <summary>
        /// Create dummy data to show on graph
        /// </summary>
        public void CreateDataset()
        {
            //Set values to line series 
            spcUCL.Values = new ChartValues<double> { 4, 4, 4, 4, 4 };
            spcLCL.Values = new ChartValues<double> { 2, 2, 2, 2, 2, };
            spcLSL.Values = new ChartValues<double> { 1, 1, 1, 1, 1, };
            spcUSL.Values = new ChartValues<double> { 5, 5, 5, 5, 5, };
            spcCenterLine.Values = new ChartValues<double> { 3, 3, 3, 3, 3, };

            //Give dataset a title. This is variable which is why it is set here
            spcDataSet.Title = "Data Set";
            spcDataSet.Values = new ChartValues<double> { 1.5, 2.1, 4.9, 3.3, 2.7, };

            //Set brush colors for points OOC or OOS
            WarningBrush = new SolidColorBrush(Color.FromRgb(255, 160, 0));
            DangerBrush = new SolidColorBrush(Color.FromRgb(238, 83, 80));

            //DataCollection is bound to graph. Here we set the new line seres to dataseris
            DataCollection = new SeriesCollection
            {
                spcUCL, spcUSL, spcLCL, spcLSL, spcCenterLine, spcDataSet
            };

            //Set contect to this form
            DataContext = this;
        }

        #endregion

        #region SQL Queries

        /// <summary>
        /// Query to populate the comboBox
        /// </summary>
        public void spcPlans()
        {
            //Query string to select unique SPC plans
            string sqlString = "SELECT DISTINCT SPC_Plan FROM SPCDatabase";

            //Try catch to run the query
            try
            {
                //Set query string and connection to new odbc command
                OdbcCommand cmd = new OdbcCommand(sqlString, myConnection);

                //Create new adapter and populate it with the command.
                OdbcDataAdapter adapter = new OdbcDataAdapter(cmd);

                //Fill the datatable with the query
                adapter.Fill(spcNames);
            }
            //Catch errors
            catch (OdbcException oex)
            {
                //report errors out as text box
                MessageBox.Show(oex.ToString());
                this.Close();
            }

            //Linq to populate databinding source from datatable. 
            spcCharts = spcNames.AsEnumerable()
                            .Select(x => x.Field<string>("SPC_Plan"))
                            .ToList();

        }

        /// <summary>
        /// Query to get the top 50 items from the selected SPC Chart
        /// </summary>
        public void GetTablesODBC()
        {
            //Clear the datatable
            SPCData.Clear();

            //Run if the combobox isnt null
            if(comboChartNames != null)
            {
                //set Query string to select top 50 items from the selected comboBox name. Order by newest dates. 
                string sqlstring2 = "SELECT TOP 50 * FROM SPCDatabase WHERE SPC_Plan = '" + comboChartNames.SelectedItem.ToString() + "' ORDER BY Upload_Date DESC";

                //Try catch for Odbc query
                try
                {
                    //Set query string and connection to new odbc command
                    OdbcCommand cmd2 = new OdbcCommand(sqlstring2, myConnection);

                    //Create new adapter and populate it with the command. 
                    OdbcDataAdapter adapter2 = new OdbcDataAdapter(cmd2);

                    //Fill the datatable with the query
                    adapter2.Fill(SPCData);
                }
                //Catch errors
                catch (OdbcException oex)
                {
                    //report errors out as text box
                    MessageBox.Show(oex.ToString());
                }

            }
        }

        #endregion

        #region Update Graph
        /// <summary>
        /// Function to update the graph with the newest values
        /// </summary>
        public void UpdateGraph()
        {
            //Call the get table function for the selected comboBox item
            GetTablesODBC();

            //Linq expression to filter data out of query
            var SPC_Data = (from x in SPCData.AsEnumerable()
                          select new SPC_List_Data
                          {
                              UCL = x.Field<double>("UCL"),
                              LCL = x.Field<double>("LCL"),
                              USL = x.Field<double>("USL"),
                              LSL = x.Field<double>("LSL"),
                              CL = x.Field<double>("CL"),
                              Data = x.Field<double>("Data_Entry")
                          }).ToList();

            //Set values for mapper limits
            double sUCL = SPC_Data.Select(x => x.UCL).First();
            double sLCL = SPC_Data.Select(x => x.LCL).First();
            double sUSL = SPC_Data.Select(x => x.USL).First();
            double sLSL = SPC_Data.Select(x => x.LSL).First();
            double sCL = SPC_Data.Select(x => x.CL).First();

            //Set properties to mapper and set limits
            Mapper = Mappers.Xy<double>()
                .X((item, index) => index)
                .Y(item => item)
                .Fill(item => item > sUSL ||  item < sLSL ? DangerBrush : item > sUCL || item < sLCL ? WarningBrush : null)
                .Stroke(item => item > sUSL || item < sLSL ? DangerBrush : item > sUCL || item < sLCL ? WarningBrush : null);

            //set mapper to the data set
            spcDataSet.Configuration = Mapper;

            //Set the values of the chart to the new query return.
            spcUCL.Values = new ChartValues<double>(SPC_Data.Select(x => x.UCL).ToList());
            spcLCL.Values = new ChartValues<double>(SPC_Data.Select(x => x.LCL).ToList());
            spcUSL.Values = new ChartValues<double>(SPC_Data.Select(x => x.USL).ToList());
            spcLSL.Values = new ChartValues<double>(SPC_Data.Select(x => x.LSL).ToList());
            spcCenterLine.Values = new ChartValues<double>(SPC_Data.Select(x => x.CL).ToList());
            spcDataSet.Values = new ChartValues<double>(SPC_Data.Select(x => x.Data).ToList());
        }

        #endregion

        #region Helper Functions

        /// <summary>
        /// Helper function to run when comboBox selection changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboDataSets_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateGraph();
        }

        #endregion

        #region LiveChart Bindings

        public List<string> spcCharts { get; set; }
        public SeriesCollection DataCollection { get; set; }
        public CartesianMapper<double> Mapper { get; set; }
        public Brush WarningBrush { get; set; }
        public Brush DangerBrush { get; set; }

        #endregion
    }

    #region Helper Class
    /// <summary>
    /// Create a class to return data for charts
    /// </summary>
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

    #endregion
}
