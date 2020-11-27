using System;
using System.ComponentModel.DataAnnotations;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class CommandUploadMappingEntity
    {
        public int Id { get; set; }
        [Required]
        public string ConfigurationName { get; set; }
        public string ImportType { get; set; }
        public string FileFormat { get; set; }
        public string Formatvalue { get; set; }
        public string ColumnMapping { get; set; }
        public bool IsDefault { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Tags { get; set; }
        public string InLanguage { get; set; }
        public string OriginalColumns { get; set; }
        public string FileFormatCommandLine { get; set; }
    }
}
