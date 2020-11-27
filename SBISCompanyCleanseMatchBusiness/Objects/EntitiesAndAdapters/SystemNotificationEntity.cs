using System;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class SystemNotificationEntity
    {
        public int MessageId { get; set; }
        public string Message { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string FontColor { get; set; }
        public bool isRead { get; set; }
        public bool isDismiss { get; set; }
    }
}
