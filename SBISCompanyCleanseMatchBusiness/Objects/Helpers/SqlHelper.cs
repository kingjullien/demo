using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using static SBISCompanyCleanseMatchBusiness.Objects.Helpers.EnumHelper;

public class SqlHelper : IDisposable
{
    public System.Data.SqlClient.SqlTransaction InsertTransaction;
    private SqlConnection InsertConnection;
    private string ApplicationRead = ";ApplicationIntent=ReadOnly";
    private string ApplicationReadWrite = ";ApplicationIntent=ReadWrite";
    private bool disposed = false;

    private string _ConnectionString;
    public SqlHelper(string ConnectionString)
    {
        _ConnectionString = ConnectionString;
    }

    public void OpenInsertConnectionWithTransaction()
    {
        InsertConnection = new SqlConnection(_ConnectionString);
        InsertConnection.Open();
        InsertTransaction = InsertConnection.BeginTransaction();
    }

    public void CloseInsertConnectionWithTransaction(bool TrnsOk)
    {
        if (TrnsOk)
        {
            InsertTransaction.Commit();
        }
        else
        {
            InsertTransaction.Rollback();
        }
        InsertConnection.Close();
    }

    public DataTable ExecuteDataTable(System.Data.CommandType CommandType, StoredProcedureEntity Sproc, string ConnectionString = "", string Intent = "")
    {
        DataSet ds = new DataSet();
        List<SqlParameter> SQLParams = new List<SqlParameter>();
        try
        {
            SqlConnection selectConnection = default(SqlConnection);
            SQLParams = GetParameters(Sproc);
            using (selectConnection = new SqlConnection(_ConnectionString))
            {
                if (string.IsNullOrEmpty(ConnectionString))
                {
                    if (!string.IsNullOrEmpty(Intent) && Intent == DBIntent.Read.ToString())
                    {
                        selectConnection = new SqlConnection(_ConnectionString + ApplicationRead);
                    }
                    else
                    {
                        selectConnection = new SqlConnection(_ConnectionString + ApplicationReadWrite);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(Intent) && Intent == DBIntent.Read.ToString())
                    {
                        selectConnection = new SqlConnection(ConnectionString + ApplicationRead);
                    }
                    else
                    {
                        selectConnection = new SqlConnection(ConnectionString + ApplicationReadWrite);
                    }
                }

                selectConnection.Open();

                SqlCommand selectCMD = new SqlCommand(Sproc.StoredProcedureName, selectConnection);
                selectCMD.CommandTimeout = 0;
                if (SQLParams.Count > 0)
                {
                    selectCMD.Parameters.AddRange(SQLParams.ToArray());
                }
                selectCMD.CommandType = CommandType;
                SqlDataAdapter ThisDA = new SqlDataAdapter();
                ThisDA.SelectCommand = selectCMD;

                ThisDA.Fill(ds, "Table1");
                selectConnection.Close();
            }
        }
        catch (Exception exc)
        {
            string Message = exc.Message + "  Connection " + _ConnectionString;
            throw new Exception(Message);
        }

        return ds.Tables[0];
    }

    public DataTable ExecuteDataTableWithOutputParam(System.Data.CommandType CommandType, StoredProcedureEntity Sproc, out string OutParam, string ConnectionString = "", string Intent = "")
    {
        DataSet ds = new DataSet();
        List<SqlParameter> SQLParams = new List<SqlParameter>();
        try
        {
            SqlConnection selectConnection = default(SqlConnection);
            SQLParams = GetParameters(Sproc);
            using (selectConnection = new SqlConnection(_ConnectionString))
            {
                if (string.IsNullOrEmpty(ConnectionString))
                {
                    if (!string.IsNullOrEmpty(Intent) && Intent == DBIntent.Read.ToString())
                    {
                        selectConnection = new SqlConnection(_ConnectionString + ApplicationRead);
                    }
                    else
                    {
                        selectConnection = new SqlConnection(_ConnectionString + ApplicationReadWrite);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(Intent) && Intent == DBIntent.Read.ToString())
                    {
                        selectConnection = new SqlConnection(ConnectionString + ApplicationRead);
                    }
                    else
                    {
                        selectConnection = new SqlConnection(ConnectionString + ApplicationReadWrite);
                    }
                }
                selectConnection.Open();

                SqlCommand selectCMD = new SqlCommand(Sproc.StoredProcedureName, selectConnection);
                selectCMD.CommandTimeout = 180;
                if (SQLParams.Count > 0)
                {
                    selectCMD.Parameters.AddRange(SQLParams.ToArray());
                    //selectCMD.Parameters[3].Direction = ParameterDirection.Output;
                }
                selectCMD.CommandType = CommandType;
                SqlDataAdapter ThisDA = new SqlDataAdapter();
                ThisDA.SelectCommand = selectCMD;

                ThisDA.Fill(ds, "Table1");
                selectConnection.Close();

                OutParam = string.Empty;
                if (SQLParams.Count > 0)
                {
                    OutParam = Convert.ToString(selectCMD.Parameters["@TotalRecords"].Value);
                }

            }
        }
        catch (Exception exc)
        {
            string Message = exc.Message + "  Connection " + _ConnectionString;
            throw new Exception(Message);
        }

        return ds.Tables[0];
    }

    public DataTable ExecuteDataTableWithSQL(string SQL, string ConnectionString = "", string Intent = "")
    {
        DataSet ds = new DataSet();
        List<SqlParameter> SQLParams = new List<SqlParameter>();
        try
        {
            SqlConnection selectConnection = default(SqlConnection);
            using (selectConnection = new SqlConnection(_ConnectionString))
            {
                if (string.IsNullOrEmpty(ConnectionString))
                {
                    if (!string.IsNullOrEmpty(Intent) && Intent == DBIntent.Read.ToString())
                    {
                        selectConnection = new SqlConnection(_ConnectionString + ApplicationRead);
                    }
                    else
                    {
                        selectConnection = new SqlConnection(_ConnectionString + ApplicationReadWrite);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(Intent) && Intent == DBIntent.Read.ToString())
                    {
                        selectConnection = new SqlConnection(ConnectionString + ApplicationRead);
                    }
                    else
                    {
                        selectConnection = new SqlConnection(ConnectionString + ApplicationReadWrite);
                    }
                }
                selectConnection.Open();

                SqlCommand selectCMD = new SqlCommand(SQL, selectConnection);
                selectCMD.CommandTimeout = 180;
                if (SQLParams.Count > 0)
                {
                    selectCMD.Parameters.AddRange(SQLParams.ToArray());
                }
                selectCMD.CommandType = CommandType.Text;
                SqlDataAdapter ThisDA = new SqlDataAdapter();
                ThisDA.SelectCommand = selectCMD;

                ThisDA.Fill(ds, "Table1");

                selectConnection.Close();
            }
        }
        catch (Exception exc)
        {
            throw new Exception(exc.Message);
        }

        return ds.Tables[0];
    }
    public void ExecuteNoReturn(System.Data.CommandType CommandType, StoredProcedureEntity Sproc, string ConnectionString = "", string Intent = "")
    {
        try
        {
            SqlConnection selectConnection = default(SqlConnection);
            using (selectConnection = new SqlConnection(_ConnectionString))
            {

                List<SqlParameter> SQLParams = new List<SqlParameter>();
                if (Sproc.StoredProceduresParameter.Count > 0)
                {
                    SQLParams = GetParameters(Sproc);
                }
                if (string.IsNullOrEmpty(ConnectionString))
                {
                    if (!string.IsNullOrEmpty(Intent) && Intent == DBIntent.Read.ToString())
                    {
                        selectConnection = new SqlConnection(_ConnectionString + ApplicationRead);
                    }
                    else
                    {
                        selectConnection = new SqlConnection(_ConnectionString + ApplicationReadWrite);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(Intent) && Intent == DBIntent.Read.ToString())
                    {
                        selectConnection = new SqlConnection(ConnectionString + ApplicationRead);
                    }
                    else
                    {
                        selectConnection = new SqlConnection(ConnectionString + ApplicationReadWrite);
                    }
                }
                selectConnection.Open();

                SqlCommand selectCMD = new SqlCommand(Sproc.StoredProcedureName, selectConnection);
                selectCMD.CommandType = CommandType;
                selectCMD.CommandTimeout = 180;
                if (SQLParams.Count > 0)
                {
                    selectCMD.Parameters.AddRange(SQLParams.ToArray());
                }
                selectCMD.ExecuteNonQuery();

                selectConnection.Close();

            }
        }
        catch (Exception exc)
        {
            throw new Exception(exc.Message);
        }
    }

    public void ExecuteNoReturnInTransaction(System.Data.CommandType CommandType, StoredProcedureEntity Sproc)
    {
        try
        {
            List<SqlParameter> SQLParams = new List<SqlParameter>();
            SQLParams = GetParameters(Sproc);

            SqlCommand selectCMD = new SqlCommand(Sproc.StoredProcedureName, InsertConnection);
            selectCMD.Transaction = InsertTransaction;
            selectCMD.CommandType = CommandType;
            selectCMD.CommandTimeout = 180;
            if (SQLParams.Count > 0)
            {
                selectCMD.Parameters.AddRange(SQLParams.ToArray());
            }
            selectCMD.ExecuteNonQuery();

        }
        catch (Exception exc)
        {
            throw new Exception(exc.Message);
        }
    }

    public DataTable ExecuteDataTableInTransaction(System.Data.CommandType CommandType, StoredProcedureEntity Sproc)
    {
        DataSet ds = new DataSet();
        List<SqlParameter> SQLParams = new List<SqlParameter>();
        try
        {
            SQLParams = GetParameters(Sproc);

            SqlCommand selectCMD = new SqlCommand(Sproc.StoredProcedureName, InsertConnection);
            selectCMD.CommandTimeout = 180;
            selectCMD.Transaction = InsertTransaction;
            selectCMD.CommandType = CommandType;

            SqlDataAdapter ThisDA = new SqlDataAdapter();
            ThisDA.SelectCommand = selectCMD;
            if (SQLParams.Count > 0)
            {
                ThisDA.SelectCommand.Parameters.AddRange(SQLParams.ToArray());
            }
            ThisDA.Fill(ds, "Table1");

        }
        catch (Exception exc)
        {
            throw new Exception(exc.Message);
        }

        return ds.Tables[0];
    }

    public object ExecuteScalar(System.Data.CommandType CommandType, StoredProcedureEntity Sproc, string Intent = "")
    {
        object result = null;
        List<SqlParameter> SQLParams = new List<SqlParameter>();
        try
        {
            SqlConnection selectConnection = default(SqlConnection);
            SQLParams = GetParameters(Sproc);

            using (selectConnection = new SqlConnection(_ConnectionString))
            {
                if (!string.IsNullOrEmpty(Intent) && Intent == DBIntent.Read.ToString())
                {
                    selectConnection = new SqlConnection(_ConnectionString + ApplicationRead);
                }
                else
                {
                    selectConnection = new SqlConnection(_ConnectionString + ApplicationReadWrite);
                }
                selectConnection.Open();
                SqlCommand selectCMD = new SqlCommand(Sproc.StoredProcedureName, selectConnection);
                selectCMD.CommandType = CommandType;
                selectCMD.CommandTimeout = 180;
                if (SQLParams.Count > 0)
                {
                    selectCMD.Parameters.AddRange(SQLParams.ToArray());
                }
                result = selectCMD.ExecuteScalar();
                selectConnection.Close();
            }
        }
        catch (Exception exc)
        {
            throw new Exception(exc.Message);
        }
        return result;
    }

    public object ExecuteScalarInTransaction(System.Data.CommandType CommandType, StoredProcedureEntity Sproc)
    {
        object result = null;
        List<SqlParameter> SQLParams = new List<SqlParameter>();
        try
        {
            SQLParams = GetParameters(Sproc);

            SqlCommand selectCMD = new SqlCommand(Sproc.StoredProcedureName, InsertConnection);
            selectCMD.Transaction = InsertTransaction;
            selectCMD.CommandType = CommandType;
            selectCMD.CommandTimeout = 180;
            if (SQLParams.Count > 0)
            {
                selectCMD.Parameters.AddRange(SQLParams.ToArray());
            }
            result = selectCMD.ExecuteScalar();

        }
        catch (Exception exc)
        {
            throw new Exception(exc.Message);
        }
        return result;
    }

    public int ExecuteReader(System.Data.CommandType CommandType, StoredProcedureEntity Sproc)
    {
        System.Data.SqlClient.SqlDataReader result = default(System.Data.SqlClient.SqlDataReader);
        List<SqlParameter> SQLParams = new List<SqlParameter>();
        int i = 0;
        try
        {
            SqlConnection selectConnection = default(SqlConnection);
            SQLParams = GetParameters(Sproc);

            using (selectConnection = new SqlConnection(_ConnectionString))
            {
                selectConnection.Open();
                SqlCommand selectCMD = new SqlCommand(Sproc.StoredProcedureName, selectConnection);
                selectCMD.CommandType = CommandType;
                selectCMD.CommandTimeout = 180;
                if (SQLParams.Count > 0)
                {
                    selectCMD.Parameters.AddRange(SQLParams.ToArray());
                }
                result = selectCMD.ExecuteReader();

                while (result.Read())
                {
                    i += 1;
                }

                result.Close();
                result.Dispose();
                selectConnection.Close();
            }
        }
        catch (Exception exc)
        {
            throw new Exception(exc.Message);
        }
        return i;
    }


    private List<SqlParameter> GetParameters(StoredProcedureEntity Sproc)
    {
        List<SqlParameter> result = new List<SqlParameter>();

        foreach (StoredProceduresParameterEntity parm in Sproc.StoredProceduresParameter)
        {

            SqlParameter SQLParm = new SqlParameter();
            SQLParm.ParameterName = parm.ParameterName;
            SQLParm.Value = parm.ParameterValue;
            SQLParm.Direction = parm.Direction;
            switch (parm.Datatype)
            {
                case SQLServerDatatype.IntDataType:
                    SQLParm.DbType = DbType.Int32;
                    break;
                case SQLServerDatatype.BigintDataType:
                    SQLParm.DbType = DbType.Int64;
                    break;
                case SQLServerDatatype.BinaryDataType:
                    SQLParm.DbType = DbType.Binary;
                    break;
                case SQLServerDatatype.BitDataType:
                    SQLParm.DbType = DbType.Boolean;
                    break;
                case SQLServerDatatype.charDataType:
                    SQLParm.DbType = DbType.String;
                    break;
                case SQLServerDatatype.DateTimeDataType:
                    SQLParm.DbType = DbType.DateTime;
                    break;
                case SQLServerDatatype.DecimalDataType:
                    SQLParm.DbType = DbType.Decimal;
                    break;
                case SQLServerDatatype.FloatDataType:
                    SQLParm.DbType = DbType.Decimal;
                    break;
                case SQLServerDatatype.ImageDataType:
                    SQLParm.DbType = DbType.Binary;
                    break;
                case SQLServerDatatype.MoneyDataType:
                    SQLParm.DbType = DbType.Decimal;
                    break;
                case SQLServerDatatype.NcharDataType:
                    SQLParm.DbType = DbType.String;
                    break;
                case SQLServerDatatype.NTextDataType:
                    SQLParm.DbType = DbType.Binary;
                    break;
                case SQLServerDatatype.numericDataType:
                    SQLParm.DbType = DbType.VarNumeric;
                    break;
                case SQLServerDatatype.NvarcharDataType:
                    SQLParm.DbType = DbType.String;
                    break;
                case SQLServerDatatype.RealDataType:
                    SQLParm.DbType = DbType.String;
                    break;
                case SQLServerDatatype.SmalldatetimeDataType:
                    SQLParm.DbType = DbType.String;
                    break;
                case SQLServerDatatype.SmallintDataType:
                    SQLParm.DbType = DbType.String;
                    break;
                case SQLServerDatatype.SmallMoneyDataType:
                    SQLParm.DbType = DbType.Decimal;
                    break;
                case SQLServerDatatype.TextDataType:
                    SQLParm.DbType = DbType.Binary;
                    break;
                case SQLServerDatatype.TinyintDataType:
                    SQLParm.DbType = DbType.Int16;
                    break;
                case SQLServerDatatype.UniqueidentifierDataType:
                    SQLParm.DbType = DbType.String;
                    break;
                case SQLServerDatatype.VarbinaryDataType:
                    SQLParm.DbType = DbType.Binary;
                    break;
                case SQLServerDatatype.VarcharDataType:
                    SQLParm.DbType = DbType.String;
                    break;
                default:
                    SQLParm.DbType = DbType.String;
                    break;
            }
            result.Add(SQLParm);
        }
        return result;
    }
    public DataSet ExecuteDataSet(System.Data.CommandType CommandType, StoredProcedureEntity Sproc, string ConnectionString = "", string Intent = "")
    {
        DataSet ds = new DataSet();
        List<SqlParameter> SQLParams = new List<SqlParameter>();
        try
        {
            SqlConnection selectConnection = default(SqlConnection);
            SQLParams = GetParameters(Sproc);
            using (selectConnection = new SqlConnection(_ConnectionString))
            {
                if (string.IsNullOrEmpty(ConnectionString))
                {
                    if (!string.IsNullOrEmpty(Intent) && Intent == DBIntent.Read.ToString())
                    {
                        selectConnection = new SqlConnection(_ConnectionString + ApplicationRead);
                    }
                    else
                    {
                        selectConnection = new SqlConnection(_ConnectionString + ApplicationReadWrite);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(Intent) && Intent == DBIntent.Read.ToString())
                    {
                        selectConnection = new SqlConnection(ConnectionString + ApplicationRead);
                    }
                    else
                    {
                        selectConnection = new SqlConnection(ConnectionString + ApplicationReadWrite);
                    }
                }
                selectConnection.Open();

                SqlCommand selectCMD = new SqlCommand(Sproc.StoredProcedureName, selectConnection);
                selectCMD.CommandTimeout = 180;
                if (SQLParams.Count > 0)
                {
                    selectCMD.Parameters.AddRange(SQLParams.ToArray());
                }
                selectCMD.CommandType = CommandType;
                SqlDataAdapter ThisDA = new SqlDataAdapter();
                ThisDA.SelectCommand = selectCMD;

                ThisDA.Fill(ds, "Table1");
                selectConnection.Close();



            }
        }
        catch (Exception exc)
        {
            string Message = exc.Message + "  Connection " + _ConnectionString;
            throw new Exception(Message);
        }

        return ds;
    }

    public DataSet ExecuteDataSetWithOutputParam(System.Data.CommandType CommandType, StoredProcedureEntity Sproc, out string OutParam, string ConnectionString = "", string Intent = "")
    {
        DataSet ds = new DataSet();
        List<SqlParameter> SQLParams = new List<SqlParameter>();
        try
        {
            SqlConnection selectConnection = default(SqlConnection);
            SQLParams = GetParameters(Sproc);
            using (selectConnection = new SqlConnection(_ConnectionString))
            {
                if (string.IsNullOrEmpty(ConnectionString))
                {
                    if (!string.IsNullOrEmpty(Intent) && Intent == DBIntent.Read.ToString())
                    {
                        selectConnection = new SqlConnection(_ConnectionString + ApplicationRead);
                    }
                    else
                    {
                        selectConnection = new SqlConnection(_ConnectionString + ApplicationReadWrite);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(Intent) && Intent == DBIntent.Read.ToString())
                    {
                        selectConnection = new SqlConnection(ConnectionString + ApplicationRead);
                    }
                    else
                    {
                        selectConnection = new SqlConnection(ConnectionString + ApplicationReadWrite);
                    }
                }
                selectConnection.Open();

                SqlCommand selectCMD = new SqlCommand(Sproc.StoredProcedureName, selectConnection);
                selectCMD.CommandTimeout = 180;
                if (SQLParams.Count > 0)
                {
                    selectCMD.Parameters.AddRange(SQLParams.ToArray());
                    //selectCMD.Parameters[3].Direction = ParameterDirection.Output;
                }
                selectCMD.CommandType = CommandType;
                SqlDataAdapter ThisDA = new SqlDataAdapter();
                ThisDA.SelectCommand = selectCMD;

                ThisDA.Fill(ds);
                selectConnection.Close();

                OutParam = string.Empty;
                if (SQLParams.Count > 0)
                {
                    OutParam = Convert.ToString(selectCMD.Parameters["@TotalRecords"].Value);
                }

            }
        }
        catch (Exception exc)
        {
            string Message = exc.Message + "  Connection " + _ConnectionString;
            throw new Exception(Message);
        }

        return ds;
    }

    public string ExecuteText(string query, string ConnectionString = "", string Intent = "")
    {
        SqlConnection selectConnection = default(SqlConnection);
        using (selectConnection = new SqlConnection(_ConnectionString))
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                if (!string.IsNullOrEmpty(Intent) && Intent == DBIntent.Read.ToString())
                {
                    selectConnection = new SqlConnection(_ConnectionString + ApplicationRead);
                }
                else
                {
                    selectConnection = new SqlConnection(_ConnectionString + ApplicationReadWrite);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(Intent) && Intent == DBIntent.Read.ToString())
                {
                    selectConnection = new SqlConnection(ConnectionString + ApplicationRead);
                }
                else
                {
                    selectConnection = new SqlConnection(ConnectionString + ApplicationReadWrite);
                }
            }

            using (SqlCommand cmd = new SqlCommand(query, selectConnection))
            {
                cmd.CommandType = CommandType.Text;
                selectConnection.Open();
                return (string)cmd.ExecuteScalar();
            }

        }
    }



    public SqlDataReader ExecuteDataReader(System.Data.CommandType CommandType, StoredProcedureEntity Sproc, string Intent = "")
    {
        System.Data.SqlClient.SqlDataReader result = default(System.Data.SqlClient.SqlDataReader);
        List<SqlParameter> SQLParams = new List<SqlParameter>();
        try
        {
            SqlConnection selectConnection = default(SqlConnection);
            SQLParams = GetParameters(Sproc);

            using (selectConnection = new SqlConnection(_ConnectionString))
            {
                if (!string.IsNullOrEmpty(Intent) && Intent == DBIntent.Read.ToString())
                {
                    selectConnection = new SqlConnection(_ConnectionString + ApplicationRead);
                }
                else
                {
                    selectConnection = new SqlConnection(_ConnectionString + ApplicationReadWrite);
                }
                selectConnection.Open();
                SqlCommand selectCMD = new SqlCommand(Sproc.StoredProcedureName, selectConnection);
                selectCMD.CommandType = CommandType;
                selectCMD.CommandTimeout = 180;
                if (SQLParams.Count > 0)
                {
                    selectCMD.Parameters.AddRange(SQLParams.ToArray());
                }
                result = selectCMD.ExecuteReader();

                //while (result.Read())
                //{
                //    i += 1;
                //}

                //result.Close();
                //result.Dispose();
                //selectConnection.Close();
            }
        }
        catch (Exception exc)
        {
            throw new Exception(exc.Message);
        }
        return result;
    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    protected void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
            {
                InsertConnection.Dispose();
            }
        }
        this.disposed = true;
    }
}

