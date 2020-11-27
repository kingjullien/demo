using System;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class MonitoringProductEntity
    {
        public string ProductCode { get; set; }
        public int ProductID { get; set; }
        public string ProductDescription { get; set; }
        public bool IsActive { get; set; }
    }
    public class MonitoringProductElementEntity
    {
        public int ProductID { get; set; }
        public string ElementName { get; set; }
        public string ElementPCMXPath { get; set; }
        public string ElementType { get; set; }
        public int ProductElementID { get; set; }
        public bool IsActive { get; set; }
        public string ElementTypeWithId { get; set; }
        public bool IsAggregate { get; set; }
        public string MontoringType { get; set; }
    }
    public class MonitoringProfileEntity
    {
        public string ProfileName { get; set; }
        public string ProfileDescription { get; set; }
        public string ProductCode { get; set; }
        public int ProductID { get; set; }
        public string MonitoringLevel { get; set; }
        public long ApplicationTransactionID { get; set; }
        public DateTime TransactionTimestamp { get; set; }
        public string CustomerReferenceText { get; set; }
        public string ResultID { get; set; }
        public string SeverityText { get; set; }
        public string ResultText { get; set; }
        public int MonitoringProfileID { get; set; }
        public DateTime RequestDateTime { get; set; }
        public DateTime ResponseDateTime { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime ModifiedDateTime { get; set; }
        public string Condition { get; set; }
        public bool SingleCondition { get; set; }
        public string ElementName { get; set; }
        public bool IsDeleted { get; set; }
        public int CredentialId { get; set; }
    }
    public class MonitoringElementConditionsEntity
    {
        public int MonitoringConditionID { get; set; }
        public int ProfileID { get; set; }
        public int ProductElementID { get; set; }
        public string ElementName { get; set; }
        public string ChangeCondition { get; set; }
        public string Condition { get; set; }
        public string JsonCondition { get; set; }
        public string ElementPCMXPath { get; set; }
        public int TempConditionId { get; set; }
    }


}
