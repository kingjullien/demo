using System.ComponentModel.DataAnnotations;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class CommandDownloadMappingEntity
    {
        public int Id { get; set; }
        [Required]
        public string ConfigurationName { get; set; }
        public string Tag { get; set; }
        public string LOBTag { get; set; }
        public bool DownloadMatchOutput { get; set; }
        public bool DownloadEnrichmentOutput { get; set; }
        public bool DownloadMonitoringUpdates { get; set; }
        public bool DownloadActiveDataQueue { get; set; }
        public bool DownloadTransferDUNS { get; set; }
        public string DownloadFormat { get; set; }
        public string Formatvalue { get; set; }
        public bool MarkAsExported { get; set; }
        public bool IsDefault { get; set; }
        public string APILayer { get; set; }
        public bool IsAppendDateTime { get; set; }
        public bool IsFilePrefix { get; set; }
        public string DateTimeFileFormat { get; set; }
        public string FilePrefix { get; set; }
        public string ProviderType { get; set; }
        public bool DownloadCompanyTree { get; set; }
        public bool DownloadNoMatchQueue { get; set; }
        public bool DownloadLCMQueue { get; set; }
        public bool ApplyTextQualifierToAll { get; set; }
        public int UserId { get; set; }

    }
}
