﻿using System;
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
        DataTable SPCLimits = new DataTable(); 
        DataTable SPCData = new DataTable();
        public string full_name;
        bool edit_permissions = false;

        public Loader()
        {
            if (VerifyUser())
            {
                AssignPermissions();
                VerifyDriver();
                InitializeComponent();
                this.Show();
                GetTablesODBC(SPCLimits, SPCData);
                MainWindow mainWindow = new MainWindow(edit_permissions);
                this.Close();
                mainWindow.Show();
            }
            else
            {
                MessageBox.Show("Unauthorized user");
                Application.Current.Shutdown();
            }
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
            DataTable SPCUsers = new DataTable();

            string connString = (@"Driver={Microsoft Access Driver (*.mdb, *.accdb)};" +
                @"Dbq=\\tekfs6.central.tektronix.net\wce\Maxtek\mxt-dept\MfgCommon\SPC Tool\Database\SPCDatabase.accdb; Uid=Admin; Pwd=;");

            string user_query = "SELECT * FROM SPCUsers";

            try
            {
                using (OdbcConnection myConnection = new OdbcConnection(connString))
                {
                    OdbcCommand cmd = new OdbcCommand(user_query, myConnection);
                    myConnection.Open();
                    OdbcDataAdapter adapter = new OdbcDataAdapter(cmd);
                    adapter.Fill(SPCUsers);
                    myConnection.Close();
                }
            }

            catch (OdbcException oex)
            {
                MessageBox.Show(oex.ToString());
            }

            var admins = from row in SPCUsers.AsEnumerable()
                         where row.Field<string>("Permissions") == "Admin"
                         select row.Field<string>("User");

            var engineers = from row in SPCUsers.AsEnumerable()
                            where row.Field<string>("Permissions") == "Engineer"
                            select row.Field<string>("User");

            SetPermissions(admins, engineers);
        }

        public void SetPermissions(EnumerableRowCollection<string> admins, EnumerableRowCollection<string> engineers)
        {
            if(admins.Contains(full_name))
            {
                edit_permissions = true;
            }
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

