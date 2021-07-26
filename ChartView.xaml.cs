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

        DataTable SPCLimits = new DataTable();
        DataTable SPCData = new DataTable();

        public ChartView(DataTable SPCLimits_Import, DataTable SPCData_Import)
        {
            InitializeComponent();
            SPCLimits = SPCLimits_Import;
            SPCData = SPCData_Import;
            CreateDataset();
        }

        public void CreateDataset()
        {
            spcCharts = SPCData.AsEnumerable()
                .Select(x => x.Field<string>("SPC_Plan"))
                .Distinct()
                .ToList();

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

            Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May" };

            DataContext = this;
        }

        public void UpdateGraph()
        {
            List<double> holder = new List<double>();
            ChartValues<double> convert = new ChartValues<double>();           

        }

        private void ComboDataSets_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateGraph();
        }

        public List<string> spcCharts { get; set; }
        public SeriesCollection DataCollection { get; set; }
        public string[] Labels { get; set; }
    }
}
