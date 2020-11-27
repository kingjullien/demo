using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class ImportFileConfigurationEntity
    {
        public int Id { get; set; }
        public string ConfigurationName { get; set; }
        public int ExternalDataStoreId { get; set; }
        public string SourceFolderPath { get; set; }
        public int TemplateId { get; set; }
        public string FileNamePattern { get; set; }
        public string PostLoadAction { get; set; }
        public string PostLoadActionParameters { get; set; }
        public bool AppendUTCTimestamp { get; set; }
        public int UserId { get; set; }
        public string ArchivePath { get; set; }
    }
    public class ArchiveJSON
    {
        public string PostLoadAction { get; set; }
        public string ArchivePath { get; set; }
        public int AppendUTCTimestamp { get; set; }
    }
    public class RenameJSON
    {
        public string PostLoadAction { get; set; }
        public string NewFileExtension { get; set; }
        public int AppendUTCTimestamp { get; set; }
    }
}
