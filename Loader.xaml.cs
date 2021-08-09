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
using System.Windows.Media.Animation;
using Microsoft.Win32;
using System.Threading;
using System.ComponentModel;
using OracleDriverLib;

namespace SPC_Tool
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    /// 

    public partial class Loader : Window
    {
        public MainWindow mainWindow;
        BackgroundWorker main_thread;
        public string full_name;
        string permissionLevel = null;
        OdbcConnection myConnection = new OdbcConnection("DSN=SPC Access Driver");

        public Loader()
        {
            main_thread = new BackgroundWorker();
            main_thread.DoWork += MainThread;
            main_thread.RunWorkerCompleted += ThreadComplete;
            main_thread.RunWorkerAsync();
        }

        public bool VerifyUser()
        {
            OracleDriver O = new OracleDriver();
            DataTable dt = new DataTable();
            string windows_id = Environment.UserName.ToString().ToUpper();
            string id_query = "select full_name from APPS.HR_EMPLOYEES where employee_id = (select employee_id from APPS.FND_USER where user_name = '" + windows_id + "')";
            full_name = null;

            O.run_query(id_query, ref dt);

            foreach (DataRow row in dt.Rows)
            {
                foreach (var item in row.ItemArray)
                {
                    full_name = item.ToString();
                }
            }

            if (full_name != null)
            {
                return true;
            }

            return false;
        }

        public void AssignPermissions()
        {
            DataTable sqlPermission = new DataTable();
            string user_query = "SELECT Permissions FROM SPCUsers WHERE User = '" + full_name + "'";

            try
            {
                OdbcCommand cmd = new OdbcCommand(user_query, myConnection);
                OdbcDataAdapter adapter = new OdbcDataAdapter(cmd);
                adapter.Fill(sqlPermission);
            }

            catch (OdbcException oex)
            {
                MessageBox.Show(oex.ToString());
            }

            if(sqlPermission.Rows.Count == 0)
            {
                permissionLevel = "Basic";
            }
            else
            {
                permissionLevel = sqlPermission.Rows[0][0].ToString();
            }

        }

        private void MainThread(object sender, DoWorkEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                if (VerifyUser())
                {
                    myConnection.Open();
                    AssignPermissions();
                    VerifyDriver();
                }
                else
                {
                    MessageBox.Show("Unauthorized user");
                    Application.Current.Shutdown();
                }
            });
        }

        private void UpdateProgress(object sender, ProgressChangedEventArgs e)
        {
            loaderProgress.Value += e.ProgressPercentage;
        }

        private void ThreadComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(permissionLevel, myConnection, full_name);
            this.Close();
            mainWindow.Show();
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

