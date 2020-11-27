using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Utility.Monitoring
{
    public class TransactionDetail
    {
        public string transactionID { get; set; }
        public DateTime transactionTimestamp { get; set; }
        public string inLanguage { get; set; }
    }
    public class ListMonitoringRegistrationResponse
    {
        public TransactionDetail transactionDetail { get; set; }
        public Messages messages { get; set; }
    }
    public class InquiryDetail
    {
        public string reference { get; set; }
    }

    public class Destination
    {
        public string type { get; set; }
    }

    public class Registration
    {
        public string reference { get; set; }
        public Destination destination { get; set; }
        public string productId { get; set; }
        public string versionId { get; set; }
        public string email { get; set; }
        public bool seedData { get; set; }
        public string fileTransferProfile { get; set; }
        public string description { get; set; }
        public string deliveryTrigger { get; set; }
        public string deliveryFrequency { get; set; }
        public string notificationType { get; set; }
        public string Tags { get; set; }
        public int CredentialId { get; set; }
        public string CredentialName { get; set; }
        public string AuthToken { get; set; }
        public string blockIds { get; set; }
    }

    public class Messages
    {
        public Registration registration { get; set; }
        public bool notificationsSuppressed { get; set; }
        public int dunsCount { get; set; }
        public List<string> references { get; set; }
    }

    public class MonitoringRegistrationDetailResponse
    {
        public TransactionDetail transactionDetail { get; set; }
        public InquiryDetail inquiryDetail { get; set; }
        public Messages messages { get; set; }
    }
    public class Error
    {
        public string errorCode { get; set; }
        public string errorMessage { get; set; }
    }
    public class Information
    {
        public string code { get; set; }
        public string message { get; set; }
    }
    public class AddRemoveDUNSToMonitoringResponse
    {
        public TransactionDetail transactionDetail { get; set; }
        public Error error { get; set; }
        public Information information { get; set; }
    }

    public class EditRegistrationRequest
    {
        public string email { get; set; }
        public string destinationType { get; set; }
        public string destinationHost { get; set; }
        public string destinationPort { get; set; }
        public string destinationUrn { get; set; }
        public string fileTransferProfile { get; set; }
        public string maxDistributionConnections { get; set; }
        public string Description { get; set; }
    }

    public class MonitoringRegistrationResponse
    {
        public TransactionDetail transactionDetail { get; set; }
        public Error error { get; set; }
        public Information information { get; set; }
        public InquiryDetail InquiryDetail { get; set; }
    }
}