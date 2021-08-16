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
    /// Interaction logic for EditCharts.xaml
    /// </summary>
    public partial class EditCharts : Window
    {
        public OdbcConnection myConnection;
        public DataTable dt = new DataTable();
        string full_name;

        public EditCharts(OdbcConnection myConnection, string full_name)
        {
            this.full_name = full_name;
            this.myConnection = myConnection;
            FillTable();
            InitializeComponent();
            FillComboBox();
        }

        public void FillTable()
        {
            string query = "SELECT * FROM SPCLimits";
            OdbcCommand cmd = new OdbcCommand(query, myConnection);
            OdbcDataAdapter adpt = new OdbcDataAdapter(cmd);
            adpt.Fill(dt);
        }

        public void FillComboBox()
        {
            var plan_names = dt.AsEnumerable().Select(z => z.Field<string>("SPC_Plan")).ToList();

            foreach(var item in plan_names)
            {
                comboBox1.Items.Add(item);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string update_query = $"UPDATE [SPCLimits] SET [UCL] = ?, [LCL] = ?, [USL] = ?, [LSL] = ?, [CL] = ?, [Rule 1] = ?, [Rule 2] = ?, [Rule 3] = ?, [User] = '{full_name}', [Date_Updated] = '{DateTime.Now}' WHERE [SPC_Plan] = '{comboBox1.SelectedItem}'";
            OdbcCommand cmd = new OdbcCommand(update_query, myConnection);

            cmd.Parameters.AddWithValue("@UCL", OdbcType.Double).Value = double.Parse(UCL_Text.Text);
            cmd.Parameters.AddWithValue("@LCL", OdbcType.Double).Value = double.Parse(LCL_Text.Text);
            cmd.Parameters.AddWithValue("@USL", OdbcType.Double).Value = double.Parse(USL_Text.Text);
            cmd.Parameters.AddWithValue("@LSL", OdbcType.Double).Value = double.Parse(LSL_Text.Text);
            cmd.Parameters.AddWithValue("@CL", OdbcType.Double).Value = double.Parse(CL_Text.Text);
            cmd.Parameters.AddWithValue("@Rule 1", OdbcType.Int).Value = int.Parse(Rule1_Text.Text);
            cmd.Parameters.AddWithValue("@Rule 2", OdbcType.Int).Value = int.Parse(Rule2_Text.Text);
            cmd.Parameters.AddWithValue("@Rule 3", OdbcType.Int).Value = int.Parse(Rule3_Text.Text);
            cmd.Parameters.AddWithValue("@Rule 3", OdbcType.Int).Value = int.Parse(Rule3_Text.Text);
            cmd.Parameters.AddWithValue("@User", OdbcType.VarChar).Value = full_name; 

            cmd.ExecuteNonQuery();

            MessageBox.Show("Plan updated.");
            this.Close();

        }

        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var UCL = (from row in dt.AsEnumerable()
                      where row.Field<string>("SPC_Plan") == comboBox1.SelectedItem.ToString()
                      select row.Field<double>("UCL")).FirstOrDefault();

            var LCL = (from row in dt.AsEnumerable()
                       where row.Field<string>("SPC_Plan") == comboBox1.SelectedItem.ToString()
                       select row.Field<double>("LCL")).FirstOrDefault();

            var USL = (from row in dt.AsEnumerable()
                       where row.Field<string>("SPC_Plan") == comboBox1.SelectedItem.ToString()
                       select row.Field<double>("USL")).FirstOrDefault();

            var LSL = (from row in dt.AsEnumerable()
                       where row.Field<string>("SPC_Plan") == comboBox1.SelectedItem.ToString()
                       select row.Field<double>("LSL")).FirstOrDefault();

            var CL = (from row in dt.AsEnumerable()
                       where row.Field<string>("SPC_Plan") == comboBox1.SelectedItem.ToString()
                       select row.Field<double>("CL")).FirstOrDefault();

            var Rule1 = (from row in dt.AsEnumerable()
                       where row.Field<string>("SPC_Plan") == comboBox1.SelectedItem.ToString()
                       select row.Field<int>("Rule 1")).FirstOrDefault();

            var Rule2 = (from row in dt.AsEnumerable()
                       where row.Field<string>("SPC_Plan") == comboBox1.SelectedItem.ToString()
                       select row.Field<int>("Rule 2")).FirstOrDefault();

            var Rule3 = (from row in dt.AsEnumerable()
                       where row.Field<string>("SPC_Plan") == comboBox1.SelectedItem.ToString()
                       select row.Field<int>("Rule 3")).FirstOrDefault();

            UCL_Text.Text = UCL.ToString();
            LCL_Text.Text = LCL.ToString();
            USL_Text.Text = USL.ToString();
            LSL_Text.Text = LSL.ToString();
            CL_Text.Text = CL.ToString();
            Rule1_Text.Text = Rule1.ToString();
            Rule2_Text.Text = Rule2.ToString();
            Rule3_Text.Text = Rule3.ToString();

        }

    }
}
