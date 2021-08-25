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
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Configurations;

namespace SPC_Tool
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Window
    {
        public OdbcConnection myConnection;
        public DataTable SPCLimits = new DataTable();
        public DataTable SPCData = new DataTable();
        public DataTable SPCRules1 = new DataTable();
        public DataTable SPCRules2 = new DataTable();
        public DataTable SPCRules3 = new DataTable();
        public DataTable SPCRules4 = new DataTable();

        #region Line Series
        /// <summary>
        /// Set up line series for graph
        /// </summary>
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


        public LineSeries spcUCL2 = new LineSeries
        {
            Title = "UCL",
            Fill = Brushes.Transparent,
            PointGeometrySize = 0,
            Stroke = Brushes.Orange
        };
        public LineSeries spcLCL2 = new LineSeries
        {
            Title = "LCL",
            Fill = Brushes.Transparent,
            PointGeometrySize = 0,
            Stroke = Brushes.Orange
        };
        public LineSeries spcUSL2 = new LineSeries
        {
            Title = "USL",
            Fill = Brushes.Transparent,
            PointGeometrySize = 0,
            Stroke = Brushes.Red
        };
        public LineSeries spcLSL2 = new LineSeries
        {
            Title = "LSL",
            Fill = Brushes.Transparent,
            PointGeometrySize = 0,
            Stroke = Brushes.Red
        };
        public LineSeries spcCenterLine2 = new LineSeries
        {
            Title = "Center Line",
            Fill = Brushes.Transparent,
            PointGeometrySize = 0,
            Stroke = Brushes.Blue,
            StrokeDashArray = new DoubleCollection { 2 }
        };
        public LineSeries spcDataSet2 = new LineSeries
        {
            Fill = Brushes.Transparent,
            PointGeometrySize = 15,
            Stroke = Brushes.Black,
            StrokeThickness = 4
        };

        public LineSeries spcUCL3 = new LineSeries
        {
            Title = "UCL",
            Fill = Brushes.Transparent,
            PointGeometrySize = 0,
            Stroke = Brushes.Orange
        };
        public LineSeries spcLCL3 = new LineSeries
        {
            Title = "LCL",
            Fill = Brushes.Transparent,
            PointGeometrySize = 0,
            Stroke = Brushes.Orange
        };
        public LineSeries spcUSL3 = new LineSeries
        {
            Title = "USL",
            Fill = Brushes.Transparent,
            PointGeometrySize = 0,
            Stroke = Brushes.Red
        };
        public LineSeries spcLSL3 = new LineSeries
        {
            Title = "LSL",
            Fill = Brushes.Transparent,
            PointGeometrySize = 0,
            Stroke = Brushes.Red
        };
        public LineSeries spcCenterLine3 = new LineSeries
        {
            Title = "Center Line",
            Fill = Brushes.Transparent,
            PointGeometrySize = 0,
            Stroke = Brushes.Blue,
            StrokeDashArray = new DoubleCollection { 2 }
        };
        public LineSeries spcDataSet3 = new LineSeries
        {
            Fill = Brushes.Transparent,
            PointGeometrySize = 15,
            Stroke = Brushes.Black,
            StrokeThickness = 4
        };

        public LineSeries spcUCL4 = new LineSeries
        {
            Title = "UCL",
            Fill = Brushes.Transparent,
            PointGeometrySize = 0,
            Stroke = Brushes.Orange
        };
        public LineSeries spcLCL4 = new LineSeries
        {
            Title = "LCL",
            Fill = Brushes.Transparent,
            PointGeometrySize = 0,
            Stroke = Brushes.Orange
        };
        public LineSeries spcUSL4 = new LineSeries
        {
            Title = "USL",
            Fill = Brushes.Transparent,
            PointGeometrySize = 0,
            Stroke = Brushes.Red
        };
        public LineSeries spcLSL4 = new LineSeries
        {
            Title = "LSL",
            Fill = Brushes.Transparent,
            PointGeometrySize = 0,
            Stroke = Brushes.Red
        };
        public LineSeries spcCenterLine4 = new LineSeries
        {
            Title = "Center Line",
            Fill = Brushes.Transparent,
            PointGeometrySize = 0,
            Stroke = Brushes.Blue,
            StrokeDashArray = new DoubleCollection { 2 }
        };
        public LineSeries spcDataSet4 = new LineSeries
        {
            Fill = Brushes.Transparent,
            PointGeometrySize = 15,
            Stroke = Brushes.Black,
            StrokeThickness = 4
        };


        #endregion


        public Dashboard(OdbcConnection myConnection)
        {
            this.myConnection = myConnection;
            CreateDataset();
            //FillRulesTable();
            InitializeComponent();
            HideLabels();
            FillRulesTables();
            GetTopCharts();
            //FillRulesTables();
        }

        public void HideLabels()
        {
            Rule1_Label_1.Visibility = Visibility.Hidden;
            Rule2_Label_Asc_1.Visibility = Visibility.Hidden;
            Rule2_Label_Desc_1.Visibility = Visibility.Hidden;
            Rule3_Label_1.Visibility = Visibility.Hidden;

            Rule1_Label_2.Visibility = Visibility.Hidden;
            Rule2_Label_Asc_2.Visibility = Visibility.Hidden;
            Rule2_Label_Desc_2.Visibility = Visibility.Hidden;
            Rule3_Label_2.Visibility = Visibility.Hidden;

            Rule1_Label_3.Visibility = Visibility.Hidden;
            Rule2_Label_Asc_3.Visibility = Visibility.Hidden;
            Rule2_Label_Desc_3.Visibility = Visibility.Hidden;
            Rule3_Label_3.Visibility = Visibility.Hidden;

            Rule1_Label_4.Visibility = Visibility.Hidden;
            Rule2_Label_Asc_4.Visibility = Visibility.Hidden;
            Rule2_Label_Desc_4.Visibility = Visibility.Hidden;
            Rule3_Label_4.Visibility = Visibility.Hidden;
        }

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



            spcUCL2.Values = new ChartValues<double> { 4, 4, 4, 4, 4 };
            spcLCL2.Values = new ChartValues<double> { 2, 2, 2, 2, 2, };
            spcLSL2.Values = new ChartValues<double> { 1, 1, 1, 1, 1, };
            spcUSL2.Values = new ChartValues<double> { 5, 5, 5, 5, 5, };
            spcCenterLine2.Values = new ChartValues<double> { 3, 3, 3, 3, 3, };

            //Give dataset a title. This is variable which is why it is set here
            spcDataSet2.Title = "Data Set 2";
            spcDataSet2.Values = new ChartValues<double> { 1.5, 2.1, 4.9, 3.3, 2.7, };

            spcUCL3.Values = new ChartValues<double> { 4, 4, 4, 4, 4 };
            spcLCL3.Values = new ChartValues<double> { 2, 2, 2, 2, 2, };
            spcLSL3.Values = new ChartValues<double> { 1, 1, 1, 1, 1, };
            spcUSL3.Values = new ChartValues<double> { 5, 5, 5, 5, 5, };
            spcCenterLine3.Values = new ChartValues<double> { 3, 3, 3, 3, 3, };

            //Give dataset a title. This is variable which is why it is set here
            spcDataSet3.Title = "Data Set 3";
            spcDataSet3.Values = new ChartValues<double> { 1.5, 2.1, 4.9, 3.3, 2.7, };

            spcUCL4.Values = new ChartValues<double> { 4, 4, 4, 4, 4 };
            spcLCL4.Values = new ChartValues<double> { 2, 2, 2, 2, 2, };
            spcLSL4.Values = new ChartValues<double> { 1, 1, 1, 1, 1, };
            spcUSL4.Values = new ChartValues<double> { 5, 5, 5, 5, 5, };
            spcCenterLine4.Values = new ChartValues<double> { 3, 3, 3, 3, 3, };

            //Give dataset a title. This is variable which is why it is set here
            spcDataSet4.Title = "Data Set 4";
            spcDataSet4.Values = new ChartValues<double> { 1.5, 2.1, 4.9, 3.3, 2.7, };



            //Set brush colors for points OOC or OOS
            WarningBrush = new SolidColorBrush(Color.FromRgb(255, 160, 0));
            DangerBrush = new SolidColorBrush(Color.FromRgb(238, 83, 80));

            //DataCollection is bound to graph. Here we set the new line seres to dataseris
            DataCollection1 = new SeriesCollection
            {
                spcUCL, spcUSL, spcLCL, spcLSL, spcCenterLine, spcDataSet
            };
            DataCollection2 = new SeriesCollection
            {
                spcUCL2, spcUSL2, spcLCL2, spcLSL2, spcCenterLine2, spcDataSet2
            }; 
            DataCollection3 = new SeriesCollection
            {
                spcUCL3, spcUSL3, spcLCL3, spcLSL3, spcCenterLine3, spcDataSet3
            };
            DataCollection4 = new SeriesCollection
            {
                spcUCL4, spcUSL4, spcLCL4, spcLSL4, spcCenterLine4, spcDataSet4
            };
            //Set contect to this form
            DataContext = this;

        }

        public void GetTopCharts()
        {
           /* string query = "SELECT TOP 4 * FROM SPCLimits WHERE [Status] = 'Active' ORDER BY [Date_Updated] DESC;";
            OdbcCommand cmd = new OdbcCommand(query, myConnection);
            OdbcDataAdapter adpt = new OdbcDataAdapter(cmd);
            adpt.Fill(SPCLimits);*/

            var names = (from row in SPCLimits.AsEnumerable()
                         select row.Field<string>("SPC_Plan")).ToList();

            string chart1 = "SELECT TOP 50 * FROM SPCDatabase WHERE SPC_Plan = '" + names.ElementAt(0).ToString() + "' ORDER BY Upload_date DESC";
            OdbcCommand cmd2 = new OdbcCommand(chart1, myConnection);
            OdbcDataAdapter adapter2 = new OdbcDataAdapter(cmd2);
            adapter2.Fill(SPCData);


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
                .Fill(item => item > sUSL || item < sLSL ? DangerBrush : item > sUCL || item < sLCL ? WarningBrush : null)
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

            SPCData.Clear();

            string chart2 = "SELECT TOP 50 * FROM SPCDatabase WHERE SPC_Plan = '" + names.ElementAt(1).ToString() + "' ORDER BY Upload_date DESC";
            OdbcCommand cmd3 = new OdbcCommand(chart2, myConnection);
            OdbcDataAdapter adapter3 = new OdbcDataAdapter(cmd3);
            adapter3.Fill(SPCData);


            var SPC_Data2 = (from x in SPCData.AsEnumerable()
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
            double sUCL2 = SPC_Data2.Select(x => x.UCL).First();
            double sLCL2 = SPC_Data2.Select(x => x.LCL).First();
            double sUSL2 = SPC_Data2.Select(x => x.USL).First();
            double sLSL2 = SPC_Data2.Select(x => x.LSL).First();
            double sCL2 = SPC_Data2.Select(x => x.CL).First();

            //Set properties to mapper and set limits
            Mapper = Mappers.Xy<double>()
                .X((item, index) => index)
                .Y(item => item)
                .Fill(item => item > sUSL2 || item < sLSL2 ? DangerBrush : item > sUCL2 || item < sLCL2 ? WarningBrush : null)
                .Stroke(item => item > sUSL2 || item < sLSL2 ? DangerBrush : item > sUCL2 || item < sLCL2 ? WarningBrush : null);

            //set mapper to the data set
            spcDataSet2.Configuration = Mapper;

            //Set the values of the chart to the new query return.
            spcUCL2.Values = new ChartValues<double>(SPC_Data2.Select(x => x.UCL).ToList());
            spcLCL2.Values = new ChartValues<double>(SPC_Data2.Select(x => x.LCL).ToList());
            spcUSL2.Values = new ChartValues<double>(SPC_Data2.Select(x => x.USL).ToList());
            spcLSL2.Values = new ChartValues<double>(SPC_Data2.Select(x => x.LSL).ToList());
            spcCenterLine2.Values = new ChartValues<double>(SPC_Data2.Select(x => x.CL).ToList());
            spcDataSet2.Values = new ChartValues<double>(SPC_Data2.Select(x => x.Data).ToList());

            SPCData.Clear();

            string chart3 = "SELECT TOP 50 * FROM SPCDatabase WHERE SPC_Plan = '" + names.ElementAt(2).ToString() + "' ORDER BY Upload_date DESC";
            OdbcCommand cmd4 = new OdbcCommand(chart3, myConnection);
            OdbcDataAdapter adapter4 = new OdbcDataAdapter(cmd4);
            adapter4.Fill(SPCData);


            var SPC_Data3 = (from x in SPCData.AsEnumerable()
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
            double sUCL3 = SPC_Data3.Select(x => x.UCL).First();
            double sLCL3 = SPC_Data3.Select(x => x.LCL).First();
            double sUSL3 = SPC_Data3.Select(x => x.USL).First();
            double sLSL3 = SPC_Data3.Select(x => x.LSL).First();
            double sCL3 = SPC_Data3.Select(x => x.CL).First();

            //Set properties to mapper and set limits
            Mapper = Mappers.Xy<double>()
                .X((item, index) => index)
                .Y(item => item)
                .Fill(item => item > sUSL3 || item < sLSL3 ? DangerBrush : item > sUCL3 || item < sLCL3 ? WarningBrush : null)
                .Stroke(item => item > sUSL3 || item < sLSL3 ? DangerBrush : item > sUCL3 || item < sLCL3 ? WarningBrush : null);

            //set mapper to the data set
            spcDataSet3.Configuration = Mapper;

            //Set the values of the chart to the new query return.
            spcUCL3.Values = new ChartValues<double>(SPC_Data3.Select(x => x.UCL).ToList());
            spcLCL3.Values = new ChartValues<double>(SPC_Data3.Select(x => x.LCL).ToList());
            spcUSL3.Values = new ChartValues<double>(SPC_Data3.Select(x => x.USL).ToList());
            spcLSL3.Values = new ChartValues<double>(SPC_Data3.Select(x => x.LSL).ToList());
            spcCenterLine3.Values = new ChartValues<double>(SPC_Data3.Select(x => x.CL).ToList());
            spcDataSet3.Values = new ChartValues<double>(SPC_Data3.Select(x => x.Data).ToList());

            SPCData.Clear();

            string chart4 = "SELECT TOP 50 * FROM SPCDatabase WHERE SPC_Plan = '" + names.ElementAt(3).ToString() + "' ORDER BY Upload_date DESC";
            OdbcCommand cmd5 = new OdbcCommand(chart4, myConnection);
            OdbcDataAdapter adapter5 = new OdbcDataAdapter(cmd5);
            adapter5.Fill(SPCData);


            var SPC_Data4 = (from x in SPCData.AsEnumerable()
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
            double sUCL4 = SPC_Data4.Select(x => x.UCL).First();
            double sLCL4 = SPC_Data4.Select(x => x.LCL).First();
            double sUSL4 = SPC_Data4.Select(x => x.USL).First();
            double sLSL4 = SPC_Data4.Select(x => x.LSL).First();
            double sCL4 = SPC_Data4.Select(x => x.CL).First();

            //Set properties to mapper and set limits
            Mapper = Mappers.Xy<double>()
                .X((item, index) => index)
                .Y(item => item)
                .Fill(item => item > sUSL4 || item < sLSL4 ? DangerBrush : item > sUCL4 || item < sLCL4 ? WarningBrush : null)
                .Stroke(item => item > sUSL4 || item < sLSL4 ? DangerBrush : item > sUCL4 || item < sLCL4 ? WarningBrush : null);

            //set mapper to the data set
            spcDataSet4.Configuration = Mapper;

            //Set the values of the chart to the new query return.
            spcUCL4.Values = new ChartValues<double>(SPC_Data4.Select(x => x.UCL).ToList());
            spcLCL4.Values = new ChartValues<double>(SPC_Data4.Select(x => x.LCL).ToList());
            spcUSL4.Values = new ChartValues<double>(SPC_Data4.Select(x => x.USL).ToList());
            spcLSL4.Values = new ChartValues<double>(SPC_Data4.Select(x => x.LSL).ToList());
            spcCenterLine4.Values = new ChartValues<double>(SPC_Data4.Select(x => x.CL).ToList());
            spcDataSet4.Values = new ChartValues<double>(SPC_Data4.Select(x => x.Data).ToList());

            //Call Rules
            Rule1(spcDataSet, spcUCL, spcLCL, SPCRules1, Rule1_Label_1);
            Rule2(spcDataSet, SPCRules1, Rule2_Label_Asc_1, Rule2_Label_Desc_1);
            Rule3(spcDataSet, SPCRules1, Rule3_Label_1);

            Rule1(spcDataSet2, spcUCL2, spcLCL2, SPCRules2, Rule1_Label_2);
            Rule2(spcDataSet2, SPCRules2, Rule2_Label_Asc_2, Rule2_Label_Desc_2);
            Rule3(spcDataSet2, SPCRules2, Rule3_Label_2);

            Rule1(spcDataSet3, spcUCL3, spcLCL3, SPCRules3, Rule1_Label_3);
            Rule2(spcDataSet3, SPCRules3, Rule2_Label_Asc_3, Rule2_Label_Desc_3);
            Rule3(spcDataSet3, SPCRules3, Rule3_Label_3);

            Rule1(spcDataSet4, spcUCL4, spcLCL4, SPCRules4, Rule1_Label_4);
            Rule2(spcDataSet4, SPCRules4, Rule2_Label_Asc_4, Rule2_Label_Desc_4);
            Rule3(spcDataSet4, SPCRules4, Rule3_Label_4);

            //Mean and SD
            GetStats(mean_label_1, sd_label_1, 0);
            GetStats(mean_label_2, sd_label_2, 1);
            GetStats(mean_label_3, sd_label_3, 2);
            GetStats(mean_label_4, sd_label_4, 3);

        }

        #region LiveChart Bindings

        public List<string> spcCharts { get; set; }
        public SeriesCollection DataCollection1 { get; set; }
        public SeriesCollection DataCollection2 { get; set; }
        public SeriesCollection DataCollection3 { get; set; }
        public SeriesCollection DataCollection4 { get; set; }
        public CartesianMapper<double> Mapper { get; set; }
        public Brush WarningBrush { get; set; }
        public Brush DangerBrush { get; set; }

        #endregion

        #region Rules

        /// <summary>
        /// Ruleset implementations
        /// </summary>
        /// 

        public void FillRulesTables()
        {
            string query = "SELECT TOP 4 * FROM SPCLimits WHERE [Status] = 'Active' ORDER BY [Date_Updated] DESC;";
            OdbcCommand cmd = new OdbcCommand(query, myConnection);
            OdbcDataAdapter adpt = new OdbcDataAdapter(cmd);
            adpt.Fill(SPCLimits);

            var names = (from row in SPCLimits.AsEnumerable()
                         select row.Field<string>("SPC_Plan")).ToList();

            /*for (int i = 0; i < 4; i++)
            {
                MessageBox.Show(names.ElementAt(i));
            }*/

            string rules_query1 = $"SELECT [Rule 1], [Rule 2], [Rule 3], [CL] FROM SPCLimits WHERE SPC_Plan = '{names.ElementAt(0).ToString()}'";
            OdbcCommand cmd1 = new OdbcCommand(rules_query1, myConnection);
            OdbcDataAdapter adpt1 = new OdbcDataAdapter(cmd1);
            adpt1.Fill(SPCRules1);
            Title1.Content = names.ElementAt(0);

            string rules_query2 = $"SELECT [Rule 1], [Rule 2], [Rule 3], [CL] FROM SPCLimits WHERE SPC_Plan = '{names.ElementAt(1).ToString()}'";
            OdbcCommand cmd2 = new OdbcCommand(rules_query2, myConnection);
            OdbcDataAdapter adpt2 = new OdbcDataAdapter(cmd2);
            adpt2.Fill(SPCRules2);
            Title2.Content = names.ElementAt(1);

            string rules_query3 = $"SELECT [Rule 1], [Rule 2], [Rule 3], [CL] FROM SPCLimits WHERE SPC_Plan = '{names.ElementAt(2).ToString()}'";
            OdbcCommand cmd3 = new OdbcCommand(rules_query3, myConnection);
            OdbcDataAdapter adpt3 = new OdbcDataAdapter(cmd3);
            adpt3.Fill(SPCRules3);
            Title3.Content = names.ElementAt(2);

            string rules_query4 = $"SELECT [Rule 1], [Rule 2], [Rule 3], [CL] FROM SPCLimits WHERE SPC_Plan = '{names.ElementAt(3).ToString()}'";
            OdbcCommand cmd4 = new OdbcCommand(rules_query4, myConnection);
            OdbcDataAdapter adpt4 = new OdbcDataAdapter(cmd4);
            adpt4.Fill(SPCRules4);
            Title4.Content = names.ElementAt(3);


        }

        public void Rule1(LineSeries dataset, LineSeries UCL, LineSeries LCL, DataTable SPCRules, Label label)
        {
            //MessageBox.Show(UCL.ActualValues[0].ToString(), LCL.ActualValues[0].ToString());
            int y;
            int x = SPCRules.AsEnumerable().Select(z => z.Field<int>("Rule 1")).FirstOrDefault();

            //MessageBox.Show(x.ToString());

            /*for(int i = 0; i < SPCRules.Rows.Count; i++)
            {
                for (int j = 0; j < SPCRules.Columns.Count; j++)
                {
                    MessageBox.Show(SPCRules.Rows[i][j].ToString());
                }
            }*/

            if (x > 1)
            {
                y = (2 * x) - 1;
            }
            else
            {
                y = 1;
            }

            int count = 1;
            int inner_idx = y;

            for (int i = 0; i < dataset.ActualValues.Count; i++)
            {
                for (int j = i; j < inner_idx; j++)
                {
                    if (j < dataset.ActualValues.Count)
                    {
                        if ((double.Parse(dataset.ActualValues[j].ToString()) > double.Parse(UCL.ActualValues[j].ToString())) || (double.Parse(dataset.ActualValues[j].ToString()) < double.Parse(LCL.ActualValues[j].ToString())))
                        {
                            count++;
                        }
                    }
                }

                if (count >= x)
                {
                    label.Visibility = Visibility.Visible;
                    //MessageBox.Show("Rule 1: Out of control");
                    break;
                }

                count = 1;
            }

        }

        public void Rule2(LineSeries dataset, DataTable SPCRules, Label label_a, Label label_b)
        {
            int x = SPCRules.AsEnumerable().Select(z => z.Field<int>("Rule 2")).FirstOrDefault();
            int count = 0;

            for (int i = 0; i < dataset.ActualValues.Count - 1; i++)
            {
                if (double.Parse(dataset.ActualValues[i].ToString()) < double.Parse(dataset.ActualValues[i + 1].ToString()))
                {
                    count++;
                    if (count >= x)
                    {
                        label_a.Visibility = Visibility.Visible;
                        //MessageBox.Show("Rule 2 - Ascending: Out of Control");
                        break;
                    }
                }
                else
                {
                    count = 0;
                }
            }

            count = 0;

            for (int i = 0; i < dataset.ActualValues.Count - 1; i++)
            {
                if (double.Parse(dataset.ActualValues[i].ToString()) > double.Parse(dataset.ActualValues[i + 1].ToString()))
                {
                    count++;
                    if (count >= x)
                    {
                        label_b.Visibility = Visibility.Visible;
                        //MessageBox.Show("Rule 2 - Descending: Out of Control");
                        break;
                    }
                }
                else
                {
                    count = 0;
                }
            }

        }

        public void Rule3(LineSeries dataset, DataTable SPCRules, Label label)
        {
            int x = SPCRules.AsEnumerable().Select(z => z.Field<int>("Rule 3")).FirstOrDefault();
            double CL = SPCRules.AsEnumerable().Select(z => z.Field<double>("CL")).FirstOrDefault();

            int abv_count = 0;
            int bel_count = 0;

            for (int i = 0; i < dataset.ActualValues.Count; i++)
            {
                for (int j = i; j < x; j++)
                {
                    if (double.Parse(dataset.ActualValues[j].ToString()) > CL)
                    {
                        abv_count++;
                    }
                    else
                    {
                        abv_count = 0;
                    }
                }

                if (abv_count >= x)
                {
                    label.Visibility = Visibility.Visible;
                    //MessageBox.Show("Rule 3: Above center line");
                    break;
                }

                for (int j = i; j < x; j++)
                {
                    if (double.Parse(dataset.ActualValues[j].ToString()) < CL)
                    {
                        bel_count++;
                    }
                    else
                    {
                        bel_count = 0;
                    }
                }

                if (bel_count >= x)
                {
                    label.Visibility = Visibility.Visible;
                    //MessageBox.Show("Rule 3: Below center line");
                    break;
                }

            }

        }

        #endregion


        public void GetStats(Label mean_label, Label sd_label, int plan_num)
        {
            SPCData.Clear();
            string query = "SELECT * FROM SPCDatabase";
            OdbcCommand cmd = new OdbcCommand(query, myConnection);
            OdbcDataAdapter adpt = new OdbcDataAdapter(cmd);
            adpt.Fill(SPCData);

            var names = (from row in SPCLimits.AsEnumerable()
                         select row.Field<string>("SPC_Plan")).ToList();

            mean_label.Content = "Mean: ";
            sd_label.Content = "Standard Deviation: ";

            DateTime three_months = DateTime.Now.AddMonths(-3);

            var recent_data = (from row in SPCData.AsEnumerable()
                               where row.Field<string>("SPC_Plan") == names.ElementAt(plan_num).ToString()
                               && DateTime.Compare(row.Field<DateTime>("Upload_Date"), three_months) > 0
                               select row.Field<double>("Data_Entry")).ToList();

            double avg = recent_data.Average();
            double sum_of_squares = recent_data.Select(val => (val - avg) * (val - avg)).Sum();
            double SD = Math.Sqrt(sum_of_squares / recent_data.Count);

            mean_label.Content += avg.ToString("F");
            sd_label.Content += SD.ToString("F");

        }


    }
}

