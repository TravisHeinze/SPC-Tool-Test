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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class DataEntry : Window
    {
        public DataEntry(List<string> spcCharts_Import)
        {
            InitializeComponent();
            spcCharts = spcCharts_Import;
            textBoxPlan.TextChanged += textBoxPlanOnTextChanged;
        }

        private void ButtonSubmit_Click(object sender, RoutedEventArgs e)
        {
            OdbcConnection conn = new OdbcConnection("DSN=SPC Access Driver");
            OdbcCommand comd = conn.CreateCommand();
            conn.Open();
            comd.CommandText = "Insert into SPCDatabase(SPC_Plan, Data_Entry)Values('" + textBoxPlan.Text + "','" + textBoxData.Text + "')";
            comd.Connection = conn;
            comd.ExecuteNonQuery();
            conn.Close();
            MessageBox.Show("Data Submitted!");
            this.Close();
        }

        private void ButtonCacnel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private string _currentInput = "";
        private string _currentSuggestion = "";
        private string _currentText = "";

        private int _selectionStart;
        private int _selectionLength;
   
        private void textBoxPlanOnTextChanged(object sender, TextChangedEventArgs e)
        {
            var input = textBoxPlan.Text;
            if(input.Length > _currentInput.Length && input != _currentSuggestion)
            {
                _currentSuggestion = spcCharts.FirstOrDefault(x => x.StartsWith(input));
                if(_currentSuggestion != null)
                {
                    _currentText = _currentSuggestion;
                    _selectionStart = input.Length;
                    _selectionLength = _currentSuggestion.Length - input.Length;

                    textBoxPlan.Text = _currentText;
                    textBoxPlan.Select(_selectionStart, _selectionLength);
                }
            }
        }

        public List<string> spcCharts { get; set; }

    }
}
