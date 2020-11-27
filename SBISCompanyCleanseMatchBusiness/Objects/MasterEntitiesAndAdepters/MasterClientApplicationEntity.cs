using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SBISCompanyCleanseMatchBusiness.Objects.MasterEntitiesAndAdepters
{
    public class MasterClientApplicationEntity
    {


        public List<MasterClientApplicationEntity> ClientApplicationEntity { get; set; }
        public int ApplicationId
        {
            get;
            set;
        }
        [Display(Name = "Client")]

        public int ClientId
        {
            get;
            set;
        }
        [Required(ErrorMessage = "Please Enter Application Sub Domain")]
        [Display(Name = "Application Sub Domain")]
        public string AppicationSubDomain
        {
            get;
            set;
        }
        public string ApplicationDBConnectionStringHash
        {
            get;
            set;
        }
        [Required(ErrorMessage = "Please Enter Server Name")]
        [Display(Name = "Server Name")]
        public string DBServerName
        {
            get;
            set;
        }
        [Required(ErrorMessage = "Please Enter Database Name")]
        [Display(Name = "Database Name")]
        public string DBDatabaseName
        {
            get;
            set;
        }
        [Required(ErrorMessage = "Please Enter DB User Name")]
        [Display(Name = "DB User Name")]
        public string DBUserName
        {
            get;
            set;
        }
        [Required(ErrorMessage = "Please Enter DB Password")]
        [Display(Name = "DB Password")]
        public string DBPasswordHash
        {
            get;
            set;
        }
        public DateTime DateAdded
        {
            get;
            set;
        }
        public bool Active
        {
            get;
            set;
        }
        public int CreatedByUserId
        {
            get;
            set;
        }
        public string Notes
        {
            get;
            set;
        }
        public int UpdatedByUserId
        {
            get;
            set;
        }
        public DateTime DateUpdated
        {
            get;
            set;
        }
        [Required(ErrorMessage = "Please Enter Client User Name")]
        [Display(Name = "Client User Name")]
        public string ClientUserName
        {
            get;
            set;
        }


        //[Required(ErrorMessage = "Please Enter Client LoginId")]
        //[Display(Name = "Client LoginId")]
        //public string ClientLoginId
        //{
        //    get;
        //    set;
        //}
        [EmailAddress]
        [Required(ErrorMessage = "Please Enter Client Email")]
        [Display(Name = "Client Email Address")]
        public string Email
        {
            get;
            set;
        }
        [Required(ErrorMessage = "Please Enter License SKU")]
        [Display(Name = "License SKU")]
        public string LicenseSKU
        {
            get;
            set;
        }
        //[Required(ErrorMessage = "Please Enter License Number Of Users")]
        [Display(Name = "License Number Of Users")]
        public int? LicenseNumberOfUsers
        {
            get;
            set;
        }
        //[Required(ErrorMessage = "Please Enter License Number Of Transactions")]
        [Display(Name = "License Number Of Transactions")]
        public int? LicenseNumberOfTransactions
        {
            get;
            set;
        }
        //[Required(ErrorMessage = "Please Enter License Enable Monitoring")]
        [Display(Name = "License Enable Monitoring")]
        public bool LicenseEnableMonitoring
        {
            get;
            set;
        }
        //[Required(ErrorMessage = "Please Enter License Enable Tags")]
        [Display(Name = "License Enable Tags")]
        public bool LicenseEnableTags
        {
            get;
            set;
        }
        //[Required(ErrorMessage = "Please Enter License Enable Live API")]
        [Display(Name = "License Enable Live API")]
        public bool LicenseEnableLiveAPI
        {
            get;
            set;
        }

        [Display(Name = "License Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "mm/dd/yyyy")]
        public DateTime? LicenseStartDate { get; set; }

        [Display(Name = "License End Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "mm/dd/yyyy")]
        public DateTime? LicenseEndDate { get; set; }

        [Display(Name = "License Enable Investigations")]
        public bool LicenseEnableInvestigations { get; set; }

        [Display(Name = "License Enable Bing Search")]
        public bool LicenseEnableBingSearch { get; set; }
        [Display(Name = "API Key")]
        public string APIKey { get; set; }
        [Display(Name = "API Secret")]
        public string APISecret { get; set; }
        [Display(Name = "Build A List")]
        public bool LicenseBuildAList { get; set; }
        [Display(Name = "License Enable Google Map")]
        public bool LicenseEnableGoogleMap { get; set; }
        [Display(Name = "SAMLSSO")]
        public bool SAMLSSO { get; set; }
        [Display(Name = "Partner IdP")]
        public string PartnerIdP { get; set; }
        [Display(Name = "License Enable Advanced Match")]
        public bool LicenseEnableAdvancedMatch { get; set; }
        [Display(Name = "License Enable Command Line")]
        public bool LicenseEnableCommandLine { get; set; }

        [Display(Name = "License Enabled DNB")]
        public bool LicenseEnabledDNB { get; set; }

        [Display(Name = "License Enabled Orb")]
        public bool LicenseEnabledOrb { get; set; }
        [Display(Name = "License Enable DPM")]
        public bool LicenseEnableDPM { get; set; }
        [Display(Name = "License Enable FamilyTree")]
        public bool LicenseEnableFamilyTree { get; set; }
        [Display(Name = "License Enable Data Stewardship")]
        public bool LicenseEnableDataStewardship { get; set; }
        [Display(Name = "Branding")]
        public string Branding { get; set; }
        [Display(Name = "License Enable Stub Data")]
        public bool LicenseEnableStubData { get; set; }
        public string LicenseEntitlements { get; set; }

        #region "Report Code Not Use"
        //Power BI Report not used
        //[Display(Name = "Power BI Collection Name")]
        //public string PowerBICollectionName { get; set; }
        //[Display(Name = "Power BI Access Key")]
        //public string PowerBIAccessKey { get; set; }
        //[Display(Name = "Power BI Work space Id")]
        //public string PowerBIWorkspaceId { get; set; }
        #endregion
    }

    [Serializable]
    public class ClientApplicationDataEntity
    {
        public string ApplicationDBConnectionStringHash { get; set; }
        public string ClientName { get; set; }
        public string ClientLogo { get; set; }
        public int ApplicationId { get; set; }
        public bool SAMLSSO { get; set; }
        public string PartnerIdP { get; set; }



    }
}
