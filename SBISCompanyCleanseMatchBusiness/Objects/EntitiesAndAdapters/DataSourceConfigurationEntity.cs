using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Web;
using Newtonsoft.Json;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class DataSourceConfigurationEntity
    {
        public int? Id { get; set; }
        public string DataSourceCode { get; set; }
        public AWS amazon { get; set; }
        public Azure azure { get; set; }
        public FTP ftp { get; set; }
        public SFTP sftp { get; set; }
        public string ExternalDataStoreName { get; set; }
        public int? ExternalDataStoreTypeId { get; set; }
        public string DataStoreConfiguration { get; set; }
        public int? UserId { get; set; }
        public List<DataSourceConfigurationEntity> lstExternalSourceConfiguration { get; set; }
        public ExternalDataStoreType externalDataStoreType { get; set; }
    }

    public class ExternalDataStoreType
    {
        public int? Id { get; set; }
        public string ExternalDataStoreTypeName { get; set; }
    }
    // Amazon Connection
    public class AWS
    {
        [Required]
        public string AccessKey { get; set; }
        public string SecurityKey { get; set; }
        public string ServiceURL { get; set; }
        [JsonIgnore]
        public string AWSConfiguration { get; set; }
        [JsonIgnore]
        public string AWSExternalDataStoreName { get; set; }
    }
    // Azure Connection
    public class Azure
    {
        public string AccountName { get; set; }
        public string AccountKey { get; set; }
        public string EndpointSuffix { get; set; }
        [JsonIgnore]
        public string AzureConfiguration { get; set; }
        [JsonIgnore]
        public string AzureExternalDataStoreName { get; set; }
    }
    // FTP Connection
    public class FTP
    {
        public string Host { get; set; }
        public int? Port { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        [JsonIgnore]
        public string FTPConfiguration { get; set; }
        [JsonIgnore]
        public string FTPExternalDataStoreName { get; set; }
    }
    // SFTP Connection
    public class SFTP
    {
        public string SFTPHost { get; set; }
        public int? SFTPPort { get; set; }
        public string SFTPUserName { get; set; }
        [JsonIgnore]
        public HttpPostedFileBase SSHFile { get; set; }
        public string SSHFilePath { get; set; }
        [JsonIgnore]
        public HttpPostedFileBase SSHFileForUpdate { get; set; }
        [JsonIgnore]
        public string SFTPConfiguration { get; set; }
        [JsonIgnore]
        public string SFTPExternalDataStoreName { get; set; }
    }
}

