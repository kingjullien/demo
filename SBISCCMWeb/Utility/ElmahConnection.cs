using Elmah;
using SBISCompanyCleanseMatchBusiness.Objects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Windows.Forms;
using System.Xml;

namespace SBISCCMWeb.Utility
{
    public class ElmahConnection : SqlErrorLog
    {
        protected string connectionStringName;
        public ElmahConnection(IDictionary config) : base(config)
        {
            connectionStringName = (string)config["connectionStringName"];

        }
        public override string ConnectionString
        {
            get
            {
                return StringCipher.Decrypt(Helper.GetMasterConnctionstring(), General.passPhrase);
            }
        }

        public override string Log(Error error)
        {
            if (error == null)
            {
                throw new ArgumentNullException("error");
            }
            string xml = ErrorXml.EncodeString(error);

            Guid id = Guid.NewGuid();

            if (!string.IsNullOrEmpty(xml))
            {
                string ErrorMessage = xml;
                if (ErrorMessage.Contains("Connection Data Source") || ErrorMessage.Contains("Connection Server="))
                {
                    int firstIndex = ErrorMessage.IndexOf("Connection Data Source");
                    if(firstIndex < 1)
                        firstIndex = ErrorMessage.IndexOf("Connection Server");
                    string firstPart = ErrorMessage.Substring(0, firstIndex);
                    string temp = ErrorMessage.Substring(firstIndex);
                    int secondIndex = temp.IndexOf("at Sql");
                    string secondPart = secondIndex > 0 ? temp.Substring(secondIndex) : "";
                    xml = firstPart + secondPart;
                }
            }

            if (!string.IsNullOrEmpty(error.Message))
            {
                string ErrorMessage = error.Message;
                if (ErrorMessage.Contains("Connection Data Source") || ErrorMessage.Contains("Connection Server="))
                {
                    int firstIndex = ErrorMessage.IndexOf("Connection Data Source");
                    if (firstIndex < 1)
                        firstIndex = ErrorMessage.IndexOf("Connection Server");
                    string firstPart = ErrorMessage.Substring(0, firstIndex);
                    string temp = ErrorMessage.Substring(firstIndex);
                    int secondIndex = temp.IndexOf("at Sql");
                    string secondPart = secondIndex > 0 ? temp.Substring(secondIndex + 1) : "";
                    error.Message = firstPart + secondPart;
                }
            }

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand("ELMAH_LogError");
                sqlCommand.CommandType = CommandType.StoredProcedure;
                SqlParameterCollection parameters = sqlCommand.Parameters;
                parameters.Add("@ErrorId", SqlDbType.UniqueIdentifier).Value = id;
                parameters.Add("@Application", SqlDbType.NVarChar, 60).Value = base.ApplicationName;
                parameters.Add("@Host", SqlDbType.NVarChar, 30).Value = string.IsNullOrEmpty(Helper.hostName) ? "" : Helper.hostName;
                parameters.Add("@Type", SqlDbType.NVarChar, 100).Value = error.Type;
                parameters.Add("@Source", SqlDbType.NVarChar, 60).Value = error.Source;
                parameters.Add("@Message", SqlDbType.NVarChar, 500).Value = error.Message;
                parameters.Add("@User", SqlDbType.NVarChar, 50).Value = error.User;
                parameters.Add("@StatusCode", SqlDbType.Int).Value = error.StatusCode;
                parameters.Add("@TimeUtc", SqlDbType.DateTime).Value = error.Time.ToUniversalTime();
                parameters.Add("@AllXml", SqlDbType.NText).Value = xml;
                sqlCommand.Connection = sqlConnection;
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                return id.ToString();
            }
        }

        public override ErrorLogEntry GetError(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }
            if (id.Length == 0)
            {
                throw new ArgumentException(null, "id");
            }
            Guid id2;
            try
            {
                id2 = new Guid(id);
            }
            catch (FormatException ex)
            {
                throw new ArgumentException(ex.Message, "id", ex);
            }
            string text;
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand("ELMAH_GetErrorXml");
                sqlCommand.CommandType = CommandType.StoredProcedure;
                SqlParameterCollection parameters = sqlCommand.Parameters;
                parameters.Add("@Application", SqlDbType.NVarChar, 60).Value = base.ApplicationName;
                parameters.Add("@ErrorId", SqlDbType.UniqueIdentifier).Value = id2;
                sqlCommand.Connection = sqlConnection;
                sqlConnection.Open();
                text = (string)sqlCommand.ExecuteScalar();
            }
            if (text == null)
            {
                return null;
            }
            Error error = ErrorXml.DecodeString(text);
            return new ErrorLogEntry(this, id, error);
        }

        public override int GetErrors(int pageIndex, int pageSize, IList errorEntryList)
        {
            if (pageIndex < 0)
            {
                throw new ArgumentOutOfRangeException("pageIndex", pageIndex, null);
            }
            if (pageSize < 0)
            {
                throw new ArgumentOutOfRangeException("pageSize", pageSize, null);
            }
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand("ELMAH_GetErrorsXml");
                sqlCommand.CommandType = CommandType.StoredProcedure;
                SqlParameterCollection parameters = sqlCommand.Parameters;
                parameters.Add("@Application", SqlDbType.NVarChar, 60).Value = base.ApplicationName;
                parameters.Add("@PageIndex", SqlDbType.Int).Value = pageIndex;
                parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;
                parameters.Add("@TotalCount", SqlDbType.Int).Direction = ParameterDirection.Output;
                sqlCommand.Connection = sqlConnection;
                sqlConnection.Open();
                string xml = ReadSingleXmlStringResult(sqlCommand.ExecuteReader());
                ErrorsXmlToList(xml, errorEntryList);
                GetErrorsXmlOutputs(sqlCommand, out int totalCount);
                return totalCount;
            }
        }

        private static string ReadSingleXmlStringResult(SqlDataReader reader)
        {
            using (reader)
            {
                if (!reader.Read())
                {
                    return null;
                }
                StringBuilder stringBuilder = new StringBuilder(2033);
                do
                {
                    stringBuilder.Append(reader.GetString(0));
                }
                while (reader.Read());
                return stringBuilder.ToString();
            }
        }

        public static void GetErrorsXmlOutputs(SqlCommand command, out int totalCount)
        {
            totalCount = (int)command.Parameters["@TotalCount"].Value;
        }

        private void ErrorsXmlToList(string xml, IList errorEntryList)
        {
            if (xml != null && xml.Length != 0)
            {
                XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
                xmlReaderSettings.CheckCharacters = false;
                xmlReaderSettings.ConformanceLevel = ConformanceLevel.Fragment;
                using (XmlReader reader = XmlReader.Create(new StringReader(xml), xmlReaderSettings))
                {
                    ErrorsXmlToList(reader, errorEntryList);
                }
            }
        }

        private void ErrorsXmlToList(XmlReader reader, IList errorEntryList)
        {
            if (errorEntryList != null)
            {
                while (reader.IsStartElement("error"))
                {
                    string attribute = reader.GetAttribute("errorId");
                    Error error = ErrorXml.Decode(reader);
                    errorEntryList.Add(new ErrorLogEntry(this, attribute, error));
                }
            }
        }
    }

    public class ElmahMail : ErrorMailModule
    {
        protected override void OnError(object sender, EventArgs e)
        {
            HttpContext context = ((HttpApplication)sender).Context;
            OnError(context.Server.GetLastError(), context);
        }

        protected override void OnErrorSignaled(object sender, ErrorSignalEventArgs args)
        {
            OnError(args.Exception, args.Context);
        }
        protected override void OnError(Exception e, HttpContext context)
        {
            if (e == null)
            {
                throw new ArgumentNullException("e");
            }
            ExceptionFilterEventArgs exceptionFilterEventArgs = new ExceptionFilterEventArgs(e, context);
            OnFiltering(exceptionFilterEventArgs);
            if (!exceptionFilterEventArgs.Dismissed)
            {
                Error error = new Error(e, context);
                if (!string.IsNullOrEmpty(error.Message))
                {
                    string ErrorMessage = error.Message;
                    if (ErrorMessage.Contains("Connection Data Source"))
                    {
                        int firstIndex = ErrorMessage.IndexOf("Connection Data Source");
                        string firstPart = ErrorMessage.Substring(0, firstIndex);
                        string temp = ErrorMessage.Substring(firstIndex);
                        int secondIndex = temp.IndexOf("at Sql");
                        string secondPart = secondIndex > 0 ? temp.Substring(secondIndex) : "";
                        error.Message = firstPart + secondPart;
                    }
                }
                ReportError(error);
            }
        }

        protected override void SendMail(MailMessage mail)
        {
            string emailhost = Helper.GetAppSettingAsString("emailhost");
            int emailport = Convert.ToInt16(Helper.GetAppSettingAsString("emailport"));
            string emailFrom = Helper.GetAppSettingAsString("emailFrom");
            string emailuserName = Helper.GetAppSettingAsString("emailuserName");
            string emailpassword = Helper.GetAppSettingAsString("emailpassword");
            bool emailenableSsl = Convert.ToBoolean(Helper.GetAppSettingAsString("emailenableSsl"));

            if (mail == null)
            {
                throw new ArgumentNullException("mail");
            }
            if (!string.IsNullOrEmpty(mail.Body))
            {
                string ErrorMessage = mail.Body;
                if (ErrorMessage.Contains("Connection Data Source"))
                {
                    int firstIndex = ErrorMessage.IndexOf("Connection Data Source");
                    string firstPart = ErrorMessage.Substring(0, firstIndex);
                    string temp = ErrorMessage.Substring(firstIndex);
                    int secondIndex = temp.IndexOf("at Sql");
                    string secondPart = secondIndex > 0 ? temp.Substring(secondIndex) : "";
                    mail.Body = firstPart + secondPart;
                }
            }
            SmtpClient smtpClient = new SmtpClient();
            string text = emailhost;
            if (text.Length > 0)
            {
                smtpClient.Host = emailhost;
            }
            int smtpPort = emailport;
            if (smtpPort > 0)
            {
                smtpClient.Port = emailport;
            }
            string text2 = emailFrom;
            string text3 = emailuserName;
            if (text2.Length > 0 && text3.Length > 0)
            {
                smtpClient.Credentials = new NetworkCredential(emailuserName, emailpassword);
            }
            smtpClient.EnableSsl = emailenableSsl;
            smtpClient.Send(mail);
        }
    }
}


