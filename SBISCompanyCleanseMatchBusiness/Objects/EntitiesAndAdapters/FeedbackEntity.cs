using System;
using System.ComponentModel.DataAnnotations;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class FeedbackEntity
    {
        public int Id { get; set; }

        public string ClientInformation { get; set; }
        public string EnteredBy { get; set; }
        public string EmailAddress { get; set; }
        public string FeedbackType { get; set; }
        [Required]
        public string Feedback { get; set; }
        public DateTime AddDateTime { get; set; }
        public string FeedbackPath { get; set; }
    }
}
