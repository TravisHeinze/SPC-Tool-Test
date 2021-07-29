using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Data;
using System.IO;
using System.Collections;
using System.Data.Odbc;
using Microsoft.Win32;

namespace SPC_Tool
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    /// 

    public partial class Loader : Window
    {
        public Loader(DataTable SPCLimits, DataTable SPCData)
        {
            VerifyDriver();
            InitializeComponent();
            this.Show();
            GetTablesODBC(SPCLimits, SPCData);
            this.Close();
        }

        public void VerifyDriver()
        {
            bool driverCheck = false;

            RegistryKey reg = Registry.CurrentUser.OpenSubKey("Software\\ODBC\\ODBC.INI\\ODBC Data Sources\\");

            if (reg != null)
            {
                foreach (string name in reg.GetValueNames())
                {
                    if (name == "SPC Access Driver")
                    {
                        driverCheck = true;
                    }
                }
                reg.Close();
            }
            
            if(driverCheck == false)
            {
                CreateDSN();
            }

        }

        public void GetTablesODBC(DataTable SPCLimits, DataTable SPCData)
        {
            string sqlString = "SELECT * FROM SPCLimits";
            string sqlstring2 = "SELECT * FROM SPCDatabase";

            try
            {
                using (OdbcConnection myConnection = new OdbcConnection("DSN=SPC Access Driver"))
                {
                    OdbcCommand cmd = new OdbcCommand(sqlString, myConnection);
                    OdbcCommand cmd2 = new OdbcCommand(sqlstring2, myConnection);

                    myConnection.Open();

                    OdbcDataAdapter adapter = new OdbcDataAdapter(cmd);
                    OdbcDataAdapter adapter2 = new OdbcDataAdapter(cmd2);
                    adapter.Fill(SPCLimits);
                    adapter2.Fill(SPCData);
                    myConnection.Close();
                }
            }
            catch(OdbcException oex)
            {
                MessageBox.Show(oex.ToString());
            }
        }

        public void CreateDSN()
        {
            string ODBC_PATH = "SOFTWARE\\ODBC\\ODBC.INI\\";
            string driverName = "Microsoft Access Driver (*.mdb, *.accdb)";
            string dsnName = "SPC Access Driver";
            string description = "Driver needed for SPC tool";
            string DBQ = @"R:\MfgCommon\SPC Tool\Database\SPCDatabase.accdb";


            string driverPath;
            string driverPathx64 = @"C:\Program Files\Microsoft Office\root\vfs\ProgramFilesCommonX64\Microsoft Shared\OFFICE16\ACEODBC.DLL";
            string driverPathx86 = @"C:\Program Files (x86)\Microsoft Office\root\vfs\ProgramFilesCommonX86\Microsoft Shared\OFFICE16\ACEODBC.DLL";

            if (File.Exists(driverPathx64))
            {
                driverPath = driverPathx64;
            }
            else
            {
                driverPath = driverPathx86;
            }

            // value to odbc data source         
            var datasourcesKey = Registry.CurrentUser.CreateSubKey(ODBC_PATH + "ODBC Data Sources");

            if (datasourcesKey == null)
            {
                throw new Exception("ODBC Registry key does not exist!");
            }

            datasourcesKey.SetValue(dsnName, driverName);
            

            //It will Create new key in odbc.ini         
            var dsnKey = Registry.CurrentUser.CreateSubKey(ODBC_PATH + dsnName);

            if (dsnKey == null)
            {
                throw new Exception("ODBC Registry key not created!");
            }

            dsnKey.SetValue("DBQ", DBQ);
            dsnKey.SetValue("Driver", driverPath);
            dsnKey.SetValue("Description", description);
            dsnKey.SetValue("FIL", "MS Access;");
            dsnKey.SetValue("LastUser", Environment.UserName);
            dsnKey.SetValue("DriverId", 25);
        }
    }
}

