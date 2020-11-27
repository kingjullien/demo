using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Models.PreviewMatchData.DCP
{
    public class DCP_CurrentPrincipals
    {
        public string DnbDUNSNumber { get; set; }
        public string APIType { get; set; }
        public string TransactionTimestamp { get; set; }
        public string JobTitle { get; set; }
        public string CurrentManagementResponsibilityCode0 { get; set; }
        public string CurrentManagementResponsibility0 { get; set; }
        public string CurrentManagementResponsibilityCode1 { get; set; }
        public string CurrentManagementResponsibility1 { get; set; }
        public string CurrentManagementResponsibilityCode2 { get; set; }
        public string CurrentManagementResponsibility2 { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string PrincipalNameType { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string NamePrefix { get; set; }
        public string NameSuffix { get; set; }
        public string PrincipalIdentificationNumber { get; set; }
        public string PrincipalIdentificationNumberType { get; set; }
        public string PrincipalIdentificationNumberTypeCode { get; set; }
        public string CurrentCompensationDate { get; set; }
        public string CompensationAmountCurrencyISOAlpha3Code0 { get; set; }
        public string CompensationAmount0 { get; set; }
        public string CompensationType0 { get; set; }
        public string CompensationTypeCode0 { get; set; }
        public string CompensationAmountCurrencyISOAlpha3Code1 { get; set; }
        public string CompensationAmount1 { get; set; }
        public string CompensationType1 { get; set; }
        public string CompensationTypeCode1 { get; set; }
        public string CompensationAmountCurrencyISOAlpha3Code2 { get; set; }
        public string CompensationAmount2 { get; set; }
        public string CompensationType2 { get; set; }
        public string CompensationTypeCode2 { get; set; }
        public string PrincipalAge { get; set; }
    }
}