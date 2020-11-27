using System;
using System.ComponentModel;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class InvestigateViewEntity : ViewModelBase, IDataErrorInfo
    {
        public int UserId { get; set; }
        public string SubmittingOfficeID { get; set; }
        public string PostalCode { get; set; }
        public string DialingCode { get; set; }
        public string LangCode
        {
            get;
            set;
        }

        public string CharCode { get; set; }
        public string ProdCode { get; set; }
        public string OrderCode { get; set; }
        public string FirstName { get; set; }
        public string InvesDialingCode { get; set; }
        public string ProirityCode { get; set; }
        public string RemarkText { get; set; }
        public string CustomerReferenceText { get; set; }
        public string CustomerBillingEndorsementText { get; set; }
        public string LastName { get; set; }
        public bool IsMobileIndicator { get; set; }
        public bool ReqIsMobileIndicator { get; set; }

        public string requestorName;
        public string RequestorName
        {
            get { return requestorName; }
            set
            {
                requestorName = value;
                RaisePropertyChanged("RequestorName");
            }
        }
        public string ReqCountryCode { get; set; }
        public string ReqCity { get; set; }
        public string ReqState { get; set; }
        public string ReqPostalCode { get; set; }
        public string ReqDialingCode { get; set; }
        public string dunsNumber;
        public string DUNSNumber
        {
            get { return dunsNumber; }
            set
            {
                dunsNumber = value;
                RaisePropertyChanged("DUNSNumber");
            }
        }
        public string companyName;
        public string CompanyName
        {
            get { return companyName; }
            set
            {
                companyName = value;
                RaisePropertyChanged("CompanyName");
            }
        }
        public string address;
        public string Address
        {
            get { return address; }
            set
            {
                address = value;
                RaisePropertyChanged("Address");
            }
        }
        public string city;
        public string City
        {
            get { return city; }
            set
            {
                city = value;
                RaisePropertyChanged("City");
            }
        }
        public string countryCode;
        public string CountryCode
        {
            get { return countryCode; }
            set
            {
                countryCode = value;
                RaisePropertyChanged("CountryCode");
            }
        }

        public string state;
        public string State
        {
            get { return state; }
            set
            {
                state = value;
                RaisePropertyChanged("State");
            }
        }
        public string telecommunicationNumber;
        public string TelecommunicationNumber
        {
            get { return telecommunicationNumber; }
            set
            {
                telecommunicationNumber = value;
                RaisePropertyChanged("TelecommunicationNumber");
            }
        }

        public string organizationIdentificationNumber;
        public string OrganizationIdentificationNumber
        {
            get { return organizationIdentificationNumber; }
            set
            {
                organizationIdentificationNumber = value;
                RaisePropertyChanged("OrganizationIdentificationNumber");
            }
        }
        public string organizationIdentificationNumberTypeCode;
        public string OrganizationIdentificationNumberTypeCode
        {
            get { return organizationIdentificationNumberTypeCode; }
            set
            {
                organizationIdentificationNumberTypeCode = value;
                RaisePropertyChanged("OrganizationIdentificationNumberTypeCode");
            }
        }

        public string organizationIdentificationNumber2;
        public string OrganizationIdentificationNumber2
        {
            get { return organizationIdentificationNumber2; }
            set
            {
                organizationIdentificationNumber2 = value;
                RaisePropertyChanged("OrganizationIdentificationNumber2");
            }
        }
        public string organizationIdentificationNumberTypeCode2;
        public string OrganizationIdentificationNumberTypeCode2
        {
            get { return organizationIdentificationNumberTypeCode2; }
            set
            {
                organizationIdentificationNumberTypeCode2 = value;
                RaisePropertyChanged("OrganizationIdentificationNumberTypeCode2");
            }
        }

        public string organizationIdentificationNumber3;
        public string OrganizationIdentificationNumber3
        {
            get { return organizationIdentificationNumber3; }
            set
            {
                organizationIdentificationNumber3 = value;
                RaisePropertyChanged("OrganizationIdentificationNumber3");
            }
        }
        public string organizationIdentificationNumberTypeCode3;
        public string OrganizationIdentificationNumberTypeCode3
        {
            get { return organizationIdentificationNumberTypeCode3; }
            set
            {
                organizationIdentificationNumberTypeCode3 = value;
                RaisePropertyChanged("OrganizationIdentificationNumberTypeCode3");
            }
        }


        public string investTelecommunicationNumber;
        public string InvsetTelecommunicationNumber
        {
            get { return investTelecommunicationNumber; }
            set
            {
                investTelecommunicationNumber = value;
                RaisePropertyChanged("InvsetTelecommunicationNumber");
            }
        }
        public string reqAddress;
        public string ReqAddress
        {
            get { return reqAddress; }
            set
            {
                reqAddress = value;
                RaisePropertyChanged("ReqAddress");
            }
        }
        public string reqTelecommunicationNumber;
        public string ReqTelecommunicationNumber
        {
            get { return reqTelecommunicationNumber; }
            set
            {
                reqTelecommunicationNumber = value;
                RaisePropertyChanged("ReqTelecommunicationNumber");
            }
        }
        public string reqEmailAddress;
        public string ReqEmailAddress
        {
            get { return reqEmailAddress; }
            set
            {
                reqEmailAddress = value;
                RaisePropertyChanged("ReqEmailAddress");
            }
        }
        public string deliveryMethod;
        public string DeliveryMethod
        {
            get { return deliveryMethod; }
            set
            {
                deliveryMethod = value;
                RaisePropertyChanged("DeliveryMethod");
            }
        }
        public string delEmailAddress;
        public string DelEmailAddress
        {
            get { return delEmailAddress; }
            set
            {
                delEmailAddress = value;
                RaisePropertyChanged("DelEmailAddress");
            }
        }
        public string notificationMethod;
        public string NotificationMethod
        {
            get { return notificationMethod; }
            set
            {
                notificationMethod = value;
                RaisePropertyChanged("NotificationMethod");
            }
        }
        public string notificationEmailAddress;
        public string NotificationEmailAddress
        {
            get { return notificationEmailAddress; }
            set
            {
                notificationEmailAddress = value;
                RaisePropertyChanged("NotificationEmailAddress");
            }
        }
        public DateTime TransactionTimestamp { get; set; }
        public long PortfolioAssetContainerID { get; set; }
        public string InvestigationTrackingID { get; set; }
        public string SeverityText { get; set; }
        public string ResultID { get; set; }
        public string ResultText { get; set; }
        public string ResultDescription { get; set; }
        public long ApplicationTransactionID { get; set; }
        public string ResponseDateTime { get; set; }
        public string EncryptedApplicationTransactionID { get; set; }

        public int Id { get; set; }
        public string SrcRecordId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string CountryISOAlpha2Code { get; set; }
        public string PhoneNbr { get; set; }
        public string OrbNum { get; set; }
        public string EIN { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public string Subdomain { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string Message { get; set; }
        public DateTime RequestedDateTime { get; set; }
        public DateTime CompletedDateTime { get; set; }
        public string MatchOrbNumber { get; set; }
        public int InputId { get; set; }
        public string Tags { get; set; }
        public string TicketNumber { get; set; }


        private bool IsValidDUNS
        {
            get
            {
                return !string.IsNullOrEmpty(DUNSNumber);
            }
        }
        private bool IsValidCompanyName
        {
            get
            {
                return !string.IsNullOrEmpty(CompanyName);
            }
        }
        private bool IsValidAddress
        {
            get
            {
                return !string.IsNullOrEmpty(Address);
            }
        }
        private bool IsValidCity
        {
            get
            {
                return !string.IsNullOrEmpty(City);
            }
        }
        private bool IsValidCountry
        {
            get
            {
                return !string.IsNullOrEmpty(CountryCode);
            }
        }
        private bool IsValidState
        {
            get
            {
                return !string.IsNullOrEmpty(State);
            }
        }
        private bool IsValidTelecommunicationNo
        {
            get
            {
                return !string.IsNullOrEmpty(TelecommunicationNumber);
            }
        }
        private bool IsValidOrganizationIdentificationNumber
        {
            get
            {
                return !string.IsNullOrEmpty(OrganizationIdentificationNumber);
            }
        }
        private bool IsValidOrganizationIdentificationNumberTypeCode
        {
            get
            {
                return !string.IsNullOrEmpty(OrganizationIdentificationNumberTypeCode);
            }
        }
        private bool IsValidInvsetTelecommunicationNumber
        {
            get
            {
                return !string.IsNullOrEmpty(InvsetTelecommunicationNumber);
            }
        }
        private bool IsValidReqAddress
        {
            get
            {
                return !string.IsNullOrEmpty(ReqAddress);
            }
        }
        private bool IsValidReqTelecommunicationNumber
        {
            get
            {
                return !string.IsNullOrEmpty(ReqTelecommunicationNumber);
            }
        }
        private bool IsValidReqEmailAddress
        {
            get
            {
                return !string.IsNullOrEmpty(ReqEmailAddress);
            }
        }
        private bool IsValidDeliveryMethod
        {
            get
            {
                return !string.IsNullOrEmpty(DeliveryMethod);
            }
        }
        private bool IsValidDeliveryEmail
        {
            get
            {
                return !string.IsNullOrEmpty(DelEmailAddress);
            }
        }
        private bool IsValidRequestorName
        {
            get
            {
                return !string.IsNullOrEmpty(RequestorName);
            }
        }

        private bool IsValidNotificationMethod
        {
            get
            {
                return !string.IsNullOrEmpty(NotificationMethod);
            }
        }
        private bool IsValidNotificationEmail
        {
            get
            {
                return !string.IsNullOrEmpty(NotificationEmailAddress);
            }
        }
        public bool IsValidSave
        {
            get
            {
                return IsValidDUNS && IsValidCompanyName && IsValidCountry && IsValidRequestorName &&
                     IsValidReqEmailAddress && IsValidDeliveryMethod && IsValidDeliveryEmail && IsValidNotificationMethod && IsValidNotificationEmail;
            }
        }
        public string InvestigationStatus { get; set; }
        public DateTime ReceivedDate { get; set; }
        public string ReceivedNote { get; set; }
        public int ReceivedBy { get; set; }
        public string ReceivedFile { get; set; }
        public int ReferenceId;

        #region IDataErrorInfo
        private string error = string.Empty;
        public string Error
        {
            get { return error; }
        }

        public string this[string columnName]
        {
            get
            {
                error = string.Empty;
                if (columnName == "DUNSNumber" && !IsValidDUNS)
                {
                    error = "Please enter DUNSNumber.";
                }
                else if (columnName == "CompanyName" && !IsValidCompanyName)
                {
                    error = "Please enter CompanyName";
                }
                else if (columnName == "Address" && !IsValidAddress)
                {
                    error = "Please enter Address.";
                }
                else if (columnName == "City" && !IsValidCity)
                {
                    error = "Please enter City.";
                }
                else if (columnName == "CountryCode" && !IsValidCountry)
                {
                    error = "Please enter Country.";
                }
                else if (columnName == "State" && !IsValidState)
                {
                    error = "Please enter State.";
                }
                else if (columnName == "TelecommunicationNumber" && !IsValidTelecommunicationNo)
                {
                    error = "Please enter TelecommunicationNumber.";
                }
                else if (columnName == "OrganizationIdentificationNumber" && !IsValidOrganizationIdentificationNumber)
                {
                    error = "Please enter OrganizationIdentificationNumber.";
                }
                else if (columnName == "OrganizationIdentificationNumberTypeCode" && !IsValidOrganizationIdentificationNumberTypeCode)
                {
                    error = "Please enter OrganizationIdentificationNumberTypeCode.";
                }
                else if (columnName == "InvsetTelecommunicationNumber" && !IsValidInvsetTelecommunicationNumber)
                {
                    error = "Please enter TelecommunicationNumber.";
                }
                else if (columnName == "ReqAddress" && !IsValidReqAddress)
                {
                    error = "Please enter Address.";
                }
                else if (columnName == "ReqTelecommunicationNumber" && !IsValidReqTelecommunicationNumber)
                {
                    error = "Please enter TelecommunicationNumber.";
                }
                else if (columnName == "ReqEmailAddress" && !IsValidReqEmailAddress)
                {
                    error = "Please enter Email.";
                }
                else if (columnName == "DeliveryMethod" && !IsValidDeliveryMethod)
                {
                    error = "Please select delivery method.";
                }
                else if (columnName == "DelEmailAddress" && !IsValidDeliveryEmail)
                {
                    error = "Please enter Email.";
                }
                else if (columnName == "RequestorName" && !IsValidRequestorName)
                {
                    error = "Please enter Requestor Name.";
                }
                else if (columnName == "NotificationMethod" && !IsValidNotificationMethod)
                {
                    error = "Please select notification method.";
                }
                else if (columnName == "NotificationEmailAddress" && !IsValidNotificationEmail)
                {
                    error = "Please enter Email.";
                }
                RaisePropertyChanged("IsValidSave");
                return error;

            }
        }
        #endregion
    }
}
