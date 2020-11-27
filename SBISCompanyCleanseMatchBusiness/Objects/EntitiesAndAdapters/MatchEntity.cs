using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class MatchEntity : INotifyPropertyChanged
    {
        public string _streetno { get; set; }
        public string SrcRecordId { get; set; }
        public string DnBDUNSNumber { get; set; }
        public string DnBOrganizationName { get; set; }
        public string DnBTradeStyleName { get; set; }
        public string DnBSeniorPrincipalName { get; set; }
        public DateTime TransactionTimestamp { get; set; }
        public string StewardshipNotes { get; set; }
        public bool IsMatchSelected { get; set; }

        public string DnBStreetAddressLine
        {
            get;
            set;

        }
        public string StreetNo
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(DnBStreetAddressLine) && DnBStreetAddressLine.IndexOf(" ") > 0)
                {
                    string streetNo = DnBStreetAddressLine.Substring(0, DnBStreetAddressLine.IndexOf(" "));
                    if (streetNo.Any(Char.IsDigit))
                    {
                        return streetNo;
                    }
                }
                return "";
            }

        }
        public string StreetName
        {
            get
            {
                if (!string.IsNullOrEmpty(StreetNo))
                {
                    return DnBStreetAddressLine.Replace(StreetNo, "").TrimStart(' ');
                }
                else
                {
                    return DnBStreetAddressLine;
                }
            }

        }
        public string DnBPrimaryTownName { get; set; }
        public string DnBCountryISOAlpha2Code { get; set; }
        public string DnBPostalCode { get; set; }
        public string DnBPostalCodeExtensionCode { get; set; }
        public string DnBTerritoryAbbreviatedName { get; set; }
        public string DnBAddressUndeliverable { get; set; }
        public string DnBTelephoneNumber { get; set; }
        public string DnBOperatingStatus { get; set; }
        public string DnBFamilyTreeMemberRole { get; set; }
        public string DnBStandaloneOrganization { get; set; }
        public int DnBConfidenceCode { get; set; }
        public string DnBMatchGradeText { get; set; }
        public string DnBMatchDataProfileText { get; set; }
        public int DnBMatchDataProfileComponentCount { get; set; }
        public string DnBDisplaySequence { get; set; }
        public string TTCompanyName { get; set; }
        public string TTAddress { get; set; }
        public string TTCity { get; set; }
        public string TTState { get; set; }
        public string TTPhoneNbr { get; set; }
        public string MGVCompanyName { get; set; }
        public string MGVStreetNo { get; set; }
        public string MGVStreetName { get; set; }
        public string MGVCity { get; set; }
        public string MGVState { get; set; }
        public string MGVMailingAddress { get; set; }
        public string MGVTelephone { get; set; }
        public string MGVZipCode { get; set; }
        public string MGVDensity { get; set; }
        public string MGVUniqueness { get; set; }
        public string MGVSIC { get; set; }
        public string MDPVCompanyName { get; set; }
        public string MDPVStreetNo { get; set; }
        public string MDPVStreetName { get; set; }
        public string MDPVCity { get; set; }
        public string MDPVState { get; set; }
        public string MDPVMailingAddress { get; set; }
        public string MDPVTelephone { get; set; }
        public string MDPVZipCode { get; set; }
        public string MDPVDensity { get; set; }
        public string MDPVUniqueness { get; set; }
        public string MDPVSIC { get; set; }
        public string MDPVDUNS { get; set; }
        public string MDPVNationalID { get; set; }
        public string MDPVURL { get; set; }
        public string MDPPhysicalAddress { get; set; }
        public string MDPPhone { get; set; }
        public string MDPCompanyName
        {
            get;
            set;
        }
        public string MDPMailingAddress { get; set; }
        public string MGCompanyName { get; set; }
        public string MGStreetNo { get; set; }
        public string MGStreetName { get; set; }
        public string MGCity { get; set; }
        public string MGState { get; set; }
        public string MGTelephone { get; set; }
        public string MGZipCode { get; set; }
        public string MGDensity { get; set; }
        public string MGUniqueness { get; set; }
        public string MGSIC { get; set; }
        public string MGMailingAddress { get; set; }
        public string MDPText { get; set; }
        public bool _IsSelected;
        public bool IsSelected
        {
            get
            {
                return _IsSelected;
            }
            set
            {
                _IsSelected = value;

                NotifyPropertyChanged("IsSelected");
            }
        }
        public string DnBMailingAddress { get; set; }
        public bool DnBMailingAddressUndeliverable { get; set; }
        public Nullable<bool> DnBMarketabilityIndicator { get; set; }
        public bool DnBTelephoneNumberUnreachableIndicator { get; set; }
        public string ScoreCompany { get; set; }
        public string MatchDataCriteriaText { get; set; }
        public string RegistrationNumbers { get; set; }
        public string WebsiteURL { get; set; }

        public int InputId { get; set; }
        #region Match Grade Read Only Properties
        //public string MGCompanyName
        //{
        //    get
        //    {
        //        return DnBMatchGradeText.Substring(0, 1);
        //    }
        //}

        //public string MGStreetNo
        //{
        //    get
        //    {
        //        return DnBMatchGradeText.Substring(1, 1);
        //    }
        //}

        //public string MGStreetName
        //{
        //    get
        //    {
        //        return DnBMatchGradeText.Substring(2, 1);
        //    }
        //}

        //public string MGCity
        //{
        //    get
        //    {
        //        return DnBMatchGradeText.Substring(3, 1);
        //    }
        //}

        //public string MGState
        //{
        //    get
        //    {
        //        return DnBMatchGradeText.Substring(4, 1);
        //    }
        //}

        //public string MGTelephone
        //{
        //    get
        //    {
        //        return DnBMatchGradeText.Substring(6, 1);
        //    }
        //}

        //public string MGZipCode
        //{
        //    get
        //    {
        //        try { return DnBMatchGradeText.Substring(7, 1); }
        //        catch { return "A"; }
        //    }
        //}
        //public string MGDensity
        //{
        //    get
        //    {
        //        try { return DnBMatchGradeText.Substring(8, 1); }
        //        catch { return "A"; }
        //    }
        //}
        //public string MGUniqueness
        //{
        //    get
        //    {
        //        try { return DnBMatchGradeText.Substring(9, 1); }
        //        catch { return "A"; }
        //    }
        //}
        //public string MGSIC
        //{
        //    get
        //    {
        //        try { return DnBMatchGradeText.Substring(10, 1); }
        //        catch { return "A"; }
        //    }
        //}
        #endregion

        #region Match Data Profile Read Only Properties
        /// <summary>
        /// Name Digits 1-2
        /// </summary>
        public string MDP_Name
        {
            get
            {
                return string.IsNullOrEmpty(DnBMatchDataProfileText) ? "" : DnBMatchDataProfileText.Substring(0, 2);
            }
        }
        /// <summary>
        /// PhysicalAddresDigits 3-10
        /// </summary>
        public string MDP_StreetNo
        {
            get
            {
                return string.IsNullOrEmpty(DnBMatchDataProfileText) ? "" : DnBMatchDataProfileText.Substring(2, 2);
            }
        }
        public string MDP_StreetName
        {
            get
            {
                return string.IsNullOrEmpty(DnBMatchDataProfileText) ? "" : DnBMatchDataProfileText.Substring(4, 2);
            }
        }
        public string MDP_City
        {
            get
            {
                return string.IsNullOrEmpty(DnBMatchDataProfileText) ? "" : DnBMatchDataProfileText.Substring(6, 2);
            }
        }
        public string MDP_State
        {
            get
            {
                return string.IsNullOrEmpty(DnBMatchDataProfileText) ? "" : DnBMatchDataProfileText.Substring(8, 2);
            }
        }
        public string MDP_PhysicalAddress
        {
            get
            {
                string result = "";
                if (DnBMatchDataProfileText != null && DnBMatchDataProfileText != "")
                {
                    result = DnBMatchDataProfileText.Substring(2, 2) + "-" + DnBMatchDataProfileText.Substring(4, 2) + "-" + DnBMatchDataProfileText.Substring(6, 2) + "-" + DnBMatchDataProfileText.Substring(8, 2);
                }

                return result;
            }
        }
        /// <summary>
        /// MailAddress Digits 11-12
        /// </summary>
        public string MDP_MailAddress
        {
            get
            {
                return string.IsNullOrEmpty(DnBMatchDataProfileText) ? "" : DnBMatchDataProfileText.Substring(10, 2);
            }
        }
        /// <summary>
        /// Phone Digits 13-14 
        /// </summary>
        public string MDP_Phone
        {
            get
            {
                return string.IsNullOrEmpty(DnBMatchDataProfileText) ? "" : DnBMatchDataProfileText.Substring(12, 2);
            }
        }


        #endregion


        public string ConfidenceCodeAndMatchGrade
        {
            get
            {
                return "(" + DnBConfidenceCode + ") " + DnBMatchGradeText;
            }
        }


        public string CompanyForeGround
        {
            get
            {
                return GetMatchColor(MGCompanyName);
            }
        }
        public string AddressForeGround
        {
            get
            {
                return GetMatchColor(MGMailingAddress);
            }
        }
        public string MGStreetNoForeGround
        {
            get
            {
                return GetMatchColor(MGStreetNo);
            }
        }
        public string MGStreetNameForeGround
        {
            get
            {
                return GetMatchColor(MGStreetName);
            }
        }
        public string MGCityForeGround
        {
            get
            {
                return GetMatchColor(MGCity);
            }
        }
        public string MGStateForeGround
        {
            get
            {
                return GetMatchColor(MGState);
            }
        }
        public string MGTelephoneForeGround
        {
            get
            {
                return GetMatchColor(MGTelephone);
            }
        }
        public string MGZipCodeForeGround
        {
            get
            {
                return GetMatchColor(MGZipCode);
            }
        }
        public string MGDensityForeGround
        {
            get
            {
                return GetMatchColor(MGDensity);
            }
        }
        public string MGUniquenessForeGround
        {
            get
            {
                return GetMatchColor(MGUniqueness);
            }
        }
        public string MGSICForeGround
        {
            get
            {
                return GetMatchColor(MGSIC);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private string GetMatchColor(string MG)
        {
            string color;
            switch (MG)
            {
                case "A":
                    color = "Green";
                    break;
                case "B":
                    color = "#CCCC00";
                    break;
                case "F":
                    color = "DarkRed";
                    break;
                default:
                    color = "Black";
                    break;
            }
            return color;
        }
        public string OriginalSrcRecordId { get; set; }
        public string Tags { get; set; }
        public string Address2 { get; set; }

    }
    public class MainMatchEntity
    {
        public string ResponseErroeMessage { get; set; }
        public List<MatchEntity> lstMatches { get; set; }
    }
}
