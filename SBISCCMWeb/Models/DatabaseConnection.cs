using MySql.Data.MySqlClient;
using SBISCCMWeb.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

//using Oracle.DataAccess.Client;

namespace SBISCCMWeb.Models
{
    public class DatabaseConnection
    {
        #region Property
        [Required]
        [Display(Name = "ServerName")]
        public string ServerName { get; set; }

        [Required]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }
        #endregion
        #region Check Connection

        public static string GenerateConnectionstring(string ServerName, string UserName, string Password, bool IsIntegratedSecurity, string DatabaseType, string HostName, string strConnectionDatabaseName = null)
        {
            //Generate Connection string for SQL / MYSQL / Oracle
            string cnnStringLocal = null;
            try
            {
                if (DatabaseType == "SQL")
                {
                    string ans = string.Empty;
                    strConnectionDatabaseName = strConnectionDatabaseName == null ? "" : strConnectionDatabaseName;
                    if (!IsIntegratedSecurity)
                        cnnStringLocal = "Data Source=" + ServerName + ";" + "Initial Catalog=" + strConnectionDatabaseName + ";" + "Persist Security Info=True;" + "User ID=" + UserName + ";" + "Password=" + Password + ";";
                    else
                        cnnStringLocal = "Data Source=" + ServerName + ";" + "Initial Catalog=" + strConnectionDatabaseName + ";" + "Integrated Security=SSPI;";
                    SqlConnection sqlCon = new SqlConnection(cnnStringLocal);
                    sqlCon.Open();
                    sqlCon.Close();
                    return Convert.ToString(cnnStringLocal);
                }
                else if (DatabaseType == "MYSQL")
                {
                    cnnStringLocal = "SERVER=" + ServerName + ";" + "DATABASE=" + strConnectionDatabaseName + ";" + "UID=" + UserName + ";" + "PASSWORD=" + Password + ";Convert Zero Datetime=True;";
                    MySqlConnection mysqlCon = new MySqlConnection(cnnStringLocal);
                    mysqlCon.Open();
                    mysqlCon.Close();
                    return Convert.ToString(cnnStringLocal);
                }
                else if (DatabaseType == "ORACLE")
                {
                    //use for future use :-dll issue is genrated 

                    //cnnStringLocal = "Data Source=(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = "+ HostName + ")(PORT = 1521))(CONNECT_DATA =(SERVER = DEDICATED))); User Id=" + UserName + ";Password=" + Password + ";";
                    //OracleConnection oraclecon = new OracleConnection(cnnStringLocal);
                    //oraclecon.Open();
                    //oraclecon.Close();
                    //return Convert.ToString(cnnStringLocal);
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return null;
        }
        #endregion
        #region Get All Database Name or Table Name or Data of Table 
        public static DataTable GetAllDatabases(string SqlInstance, string UserName, string Password, bool IsIntegratedSecurity, string databaseType, string databasename, string tableName, string Hostname)
        {
            //check the connection valid or invalid.if valid than retrieve all the database name.
            try
            {
                DataTable dt = new DataTable();
                if (databaseType == "SQL")
                {
                    // Generate Connection string for the SQL
                    SqlConnection sqlCon = new SqlConnection(GenerateConnectionstring(SqlInstance, UserName, Password, IsIntegratedSecurity, databaseType, Hostname, databasename));
                    if (sqlCon != null)
                    {
                        using (sqlCon)
                        {
                            
                            sqlCon.Open();
                            if (!string.IsNullOrEmpty(databasename) && !string.IsNullOrEmpty(tableName))
                            {
                                string query = "SELECT * FROM @tableName";
                                SqlDataAdapter da = new SqlDataAdapter();
                                using (SqlConnection conn = new SqlConnection(sqlCon.ConnectionString)) {
                                    SqlCommand com = new SqlCommand();
                                    SqlDataReader sqlReader;

                                    com.CommandText = query;
                                    com.CommandType = CommandType.Text;
                                    com.Parameters.Add(new SqlParameter("@tableName", tableName));

                                    com.Connection = sqlCon;
                                    sqlCon.Open();
                                    sqlReader = com.ExecuteReader();
                                    dt.Load(sqlReader);
                                }
                            }
                            else if (!string.IsNullOrEmpty(databasename) && string.IsNullOrEmpty(tableName))
                            {
                                string query = "SELECT TABLE_SCHEMA+'.'+TABLE_NAME as TABLE_NAME FROM INFORMATION_SCHEMA.TABLES  WHERE TABLE_CATALOG = @dataBaseName";

                                SqlDataAdapter da = new SqlDataAdapter();
                                SqlCommand com = new SqlCommand(query, sqlCon);
                                com.Parameters.AddWithValue("@dataBaseName", databasename);
                                da.SelectCommand = com;
                                da.Fill(dt);
                            }
                            else
                            {
                                SqlDataAdapter da = new SqlDataAdapter("SELECT name as [Database] FROM sys.sysdatabases where name != 'master'", sqlCon);
                                da.Fill(dt);
                            }
                            sqlCon.Close();
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                else if (databaseType == "MYSQL")
                {
                    // Generate Connection string for the MySQL 
                    string Connection = DatabaseConnection.GenerateConnectionstring(SqlInstance, UserName, Password, IsIntegratedSecurity, databaseType, Hostname, databasename);
                    MySqlConnection mysqlCon = new MySqlConnection(Connection);
                    if (mysqlCon != null)
                    {
                        using (mysqlCon)
                        {
                            mysqlCon.Open();
                            if (!string.IsNullOrEmpty(databasename) && !string.IsNullOrEmpty(tableName))
                            {
                                string query = "SELECT * FROM @tableName";
                                MySqlDataAdapter adapter = new MySqlDataAdapter();
                                MySqlCommand cmd = new MySqlCommand(query, mysqlCon);
                                cmd.Parameters.AddWithValue("@tableName", tableName);
                                adapter.SelectCommand = cmd;
                                adapter.Fill(dt);
                            }
                            else if (!string.IsNullOrEmpty(databasename) && string.IsNullOrEmpty(tableName))
                            {
                                MySqlDataAdapter da = new MySqlDataAdapter("SHOW FULL TABLES FROM " + databasename + ";", mysqlCon);
                                da.Fill(dt);
                            }
                            else
                            {
                                MySqlDataAdapter da = new MySqlDataAdapter("SHOW databases", mysqlCon);
                                da.Fill(dt);
                            }
                            mysqlCon.Close();
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                else if (databaseType == "ORACLE")
                {
                    //use for future use :-dll issue is genrated 
                    // Generate Connection string for the Oracle 

                    //string Connection = DatabaseConnection.GenerateConnectionstring(SqlInstance, UserName, Password, IsIntegratedSecurity, databaseType, Hostname, databasename);
                    //OracleConnection oraclecon = new OracleConnection(Connection);
                    //if (oraclecon != null)
                    //{
                    //    using (oraclecon)
                    //    {
                    //        oraclecon.Open();
                    //        if (!string.IsNullOrEmpty(databasename) && !string.IsNullOrEmpty(tableName))
                    //        {
                    //            OracleDataAdapter da = new OracleDataAdapter("SELECT * FROM "+databasename+"."+tableName, oraclecon);
                    //            da.Fill(dt);
                    //        }
                    //        else if (!string.IsNullOrEmpty(databasename) && string.IsNullOrEmpty(tableName))
                    //        {
                    //            OracleDataAdapter da = new OracleDataAdapter("SELECT TABLE_NAME as TABLE_NAME FROM all_tables where OWNER='" + databasename + "'", oraclecon);
                    //            da.Fill(dt);
                    //        }
                    //        else
                    //        {
                    //            OracleDataAdapter da = new OracleDataAdapter("SELECT USERNAME as Database from all_users", oraclecon);
                    //            da.Fill(dt);
                    //        }
                    //        oraclecon.Close();
                    //    }
                    //}
                    //else
                    //{
                    //    return null;
                    //}
                }
                return dt;
            }
            catch (Exception ex)
            {
                Helper.ErrorMessage = Convert.ToString(ex.Message);
                return null;
            }
        }
        #endregion
    }
}
