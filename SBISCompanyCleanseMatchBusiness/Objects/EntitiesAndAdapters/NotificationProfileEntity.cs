using System;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class NotificationProfileEntity
    {
        public string NotificationProfileName { get; set; }
        public string NotificationProfileDescription { get; set; }
        public string DeliveryMode { get; set; }
        public string DeliveryChannelUserPreferenceName { get; set; }
        public string DeliveryFrequency { get; set; }
        public string DeliveryFormat { get; set; }
        public bool StopDeliveryIndicator { get; set; }
        public bool CompressedProductIndicator { get; set; }
        public string InquiryReferenceText { get; set; }
        public string ResultID { get; set; }
        public string SeverityText { get; set; }
        public string ResultText { get; set; }
        public int NotificationProfileID { get; set; }
        public DateTime RequestDateTime { get; set; }
        public DateTime ResponseDateTime { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime ModifiedDateTime { get; set; }
        public bool IsDeleted { get; set; }
        public int CredentialId { get; set; }

    }
}
