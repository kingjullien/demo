using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{

    public class AutoAdditionalAcceptanceCriteriaEntity : ViewModelBase, IDataErrorInfo
    {

        public int CriteriaGroupId { get; set; }
        private int _groupId { get; set; }
        public int GroupId
        {
            get { return _groupId; }
            set
            {
                _groupId = value;
                RaisePropertyChanged("GroupId");
            }
        }

        private string _confidenceCode;
        public string ConfidenceCode
        {
            get { return _confidenceCode; }
            set
            {
                _confidenceCode = value;
                RaisePropertyChanged("ConfidenceCode");
            }
        }
        private string _matchDataCriteria;
        public string MatchDataCriteria
        {
            get { return _matchDataCriteria; }
            set
            {
                _matchDataCriteria = value;
                RaisePropertyChanged("MatchDataCriteria");
            }
        }
        private string _operatingStatus;
        public string OperatingStatus
        {
            get { return _operatingStatus; }
            set
            {
                _operatingStatus = value;
                RaisePropertyChanged("OperatingStatus");
            }
        }
        private string _businessType;
        public string BusinessType
        {
            get { return _businessType; }
            set
            {
                _businessType = value;
                RaisePropertyChanged("BusinessType");
            }
        }
        private bool _singleCandidateMatchOnly;
        public bool SingleCandidateMatchOnly
        {
            get
            {
                return _singleCandidateMatchOnly;
            }
            set
            {
                _singleCandidateMatchOnly = value;
                RaisePropertyChanged("SingleCandidateMatchOnly");
            }
        }
        public string MatchGrade { get; set; }
        public string MDPCode { get; set; }
        public int SequenceNo { get; set; }
        public string GroupName { get; set; }

        private string _companyGrade;
        public string CompanyGrade
        {
            get { return _companyGrade; }
            set
            {
                _companyGrade = value;
                RaisePropertyChanged("CompanyGrade");
            }
        }

        private string _companyCode;
        public string CompanyCode
        {
            get { return _companyCode; }
            set
            {
                _companyCode = value;
                RaisePropertyChanged("CompanyCode");
            }
        }

        private string _streetGrade;
        public string StreetGrade
        {
            get { return _streetGrade; }
            set
            {
                _streetGrade = value;
                RaisePropertyChanged("StreetGrade");
            }
        }

        private string _streetCode;
        public string StreetCode
        {
            get { return _streetCode; }
            set
            {
                _streetCode = value;
                RaisePropertyChanged("streetCode");
            }
        }

        private string _streetNameGrade;
        public string StreetNameGrade
        {
            get { return _streetNameGrade; }
            set
            {
                _streetNameGrade = value;
                RaisePropertyChanged("StreetNameGrade");
            }
        }

        private string _streetNameCode;
        public string StreetNameCode
        {
            get { return _streetNameCode; }
            set
            {
                _streetNameCode = value;
                RaisePropertyChanged("StreetNameCode");
            }
        }

        private string _cityGrade;
        public string CityGrade
        {
            get { return _cityGrade; }
            set
            {
                _cityGrade = value;
                RaisePropertyChanged("CityGrade");
            }
        }

        private string _cityCode;
        public string CityCode
        {
            get { return _cityCode; }
            set
            {
                _cityCode = value;
                RaisePropertyChanged("CityCode");
            }
        }

        private string _stateGrade;
        public string StateGrade
        {
            get { return _stateGrade; }
            set
            {
                _stateGrade = value;
                RaisePropertyChanged("StateGrade");
            }
        }

        private string _stateCode;
        public string StateCode
        {
            get { return _stateCode; }
            set
            {
                _stateCode = value;
                RaisePropertyChanged("StateCode");
            }
        }

        private string _addressGrade;
        public string AddressGrade
        {
            get { return _addressGrade; }
            set
            {
                _addressGrade = value;
                RaisePropertyChanged("AddressGrade");
            }
        }

        private string _addressCode;
        public string AddressCode
        {
            get { return _addressCode; }
            set
            {
                _addressCode = value;
                RaisePropertyChanged("AddressCode");
            }
        }

        private string _phoneGrade;
        public string PhoneGrade
        {
            get { return _phoneGrade; }
            set
            {
                _phoneGrade = value;
                RaisePropertyChanged("PhoneGrade");
            }
        }

        private string _zipGrade;
        public string ZipGrade
        {
            get { return _zipGrade; }
            set
            {
                _zipGrade = value;
                RaisePropertyChanged("ZipGrade");
            }
        }
        private bool _excludeFromAutoAccept;
        public bool ExcludeFromAutoAccept
        {
            get
            {
                return _excludeFromAutoAccept;
            }
            set
            {
                _excludeFromAutoAccept = value;
                RaisePropertyChanged("ExcludeFromAutoAccept");
            }
        }

        private string _phoneCode;
        public string PhoneCode
        {
            get { return _phoneCode; }
            set
            {
                _phoneCode = value;
                RaisePropertyChanged("PhoneCode");
            }
        }
        private string _zipCode;
        public string ZipCode
        {
            get { return _zipCode; }
            set
            {
                _zipCode = value;
                RaisePropertyChanged("ZipCode");
            }
        }
        private string _densityCode;
        public string DensityCode
        {
            get { return _densityCode; }
            set
            {
                _densityCode = value;
                RaisePropertyChanged("DensityCode");
            }
        }

        private string _uniquenessCode;
        public string UniquenessCode
        {
            get { return _uniquenessCode; }
            set
            {
                _uniquenessCode = value;
                RaisePropertyChanged("UniquenessCode");
            }
        }

        private string _sicCode;
        public string SICCode
        {
            get { return _sicCode; }
            set
            {
                _sicCode = value;
                RaisePropertyChanged("SICCode");
            }
        }
        private string _dunsCode;
        public string DUNSCode
        {
            get { return _dunsCode; }
            set
            {
                _dunsCode = value;
                RaisePropertyChanged("DUNSCode");
            }
        }
        private string _nationalIDCode;
        public string NationalIDCode
        {
            get { return _nationalIDCode; }
            set
            {
                _nationalIDCode = value;
                RaisePropertyChanged("NationalIDCode");
            }
        }
        private string _urlCode;
        public string URLCode
        {
            get { return _urlCode; }
            set
            {
                _urlCode = value;
                RaisePropertyChanged("URLCode");
            }
        }
        private string _density;
        public string Density
        {
            get { return _density; }
            set
            {
                _density = value;
                RaisePropertyChanged("Density");
            }
        }

        private string _uniqueness;
        public string Uniqueness
        {
            get { return _uniqueness; }
            set
            {
                _uniqueness = value;
                RaisePropertyChanged("Uniqueness");
            }
        }

        private string _sic;
        public string SIC
        {
            get { return _sic; }
            set
            {
                _sic = value;
                RaisePropertyChanged("SIC");
            }
        }
        private string _mgCombined;
        public string MG_Combined
        {
            get { return _mgCombined; }
            set
            {
                _mgCombined = value;
                RaisePropertyChanged("MG_Combined");
            }
        }
        private string _mdpCombined;
        public string MDP_Combined
        {
            get { return _mdpCombined; }
            set
            {
                _mdpCombined = value;
                RaisePropertyChanged("ConfidenceCode");
            }
        }
        private bool IsValidCountryGroup
        {
            get
            {
                return GroupId != null && GroupId != 0;
            }
        }
        private bool IsValid
        {
            get
            {
                return !string.IsNullOrEmpty(ConfidenceCode);
                //return ConfidenceCode != null && ConfidenceCode != 0;
            }
        }

        private bool IsValidCompanyGrade
        {
            get
            {
                return !string.IsNullOrEmpty(CompanyGrade);
            }
        }

        private bool IsValidCompanyCode
        {
            get
            {
                return !string.IsNullOrEmpty(CompanyCode);
            }
        }

        private bool IsValidStreetGrade
        {
            get
            {
                return !string.IsNullOrEmpty(StreetGrade);
            }
        }

        private bool IsValidStreetCode
        {
            get
            {
                return !string.IsNullOrEmpty(StreetCode);
            }
        }

        private bool IsValidStreetNameGrade
        {
            get
            {
                return !string.IsNullOrEmpty(StreetNameGrade);
            }
        }

        private bool IsValidStreetNameCode
        {
            get
            {
                return !string.IsNullOrEmpty(StreetNameCode);
            }
        }

        private bool IsValidCityGrade
        {
            get
            {
                return !string.IsNullOrEmpty(CityGrade);
            }
        }

        private bool IsValidCityCode
        {
            get
            {
                return !string.IsNullOrEmpty(CityCode);
            }
        }

        private bool IsValidStateGrade
        {
            get
            {
                return !string.IsNullOrEmpty(StateGrade);
            }
        }

        private bool IsValidStateCode
        {
            get
            {
                return !string.IsNullOrEmpty(StateCode);
            }
        }

        private bool IsValidAddressGrade
        {
            get
            {
                return !string.IsNullOrEmpty(AddressGrade);
            }
        }

        private bool IsValidAddressCode
        {
            get
            {
                return !string.IsNullOrEmpty(AddressCode);
            }
        }
        private bool IsValidPhoneGrade
        {
            get
            {
                return !string.IsNullOrEmpty(PhoneGrade);
            }
        }

        private bool IsValidPhoneCode
        {
            get
            {
                return !string.IsNullOrEmpty(PhoneCode);
            }
        }

        private bool IsValidZipGrade
        {
            get
            {
                return !string.IsNullOrEmpty(ZipGrade);
            }
        }
        private bool IsValidZipCode
        {
            get
            {
                return !string.IsNullOrEmpty(ZipCode);
            }
        }

        private bool IsValidDensity
        {
            get
            {
                return !string.IsNullOrEmpty(Density);
            }
        }
        private bool IsValidDensityCode
        {
            get
            {
                return !string.IsNullOrEmpty(DensityCode);
            }
        }
        private bool IsValidSIC
        {
            get
            {
                return !string.IsNullOrEmpty(SIC);
            }
        }
        private bool IsValidSICCode
        {
            get
            {
                return !string.IsNullOrEmpty(SICCode);
            }
        }
        private bool IsValidUniqueness
        {
            get
            {
                return !string.IsNullOrEmpty(Uniqueness);
            }
        }
        private bool IsValidUniquenessCode
        {
            get
            {
                return !string.IsNullOrEmpty(UniquenessCode);
            }
        }
        private bool IsValidDUNSCode
        {
            get
            {
                return !string.IsNullOrEmpty(DUNSCode);
            }
        }
        private bool IsValidNationalIDCode
        {
            get
            {
                return !string.IsNullOrEmpty(NationalIDCode);
            }
        }
        private bool IsValidURLCode
        {
            get
            {
                return !string.IsNullOrEmpty(URLCode);
            }
        }


        public string Tags { get; set; }
        public bool IsValidSave
        {
            get
            {
                return IsValid && IsValidCountryGroup && IsValidCompanyGrade && IsValidCompanyCode
                    && IsValidStateGrade && IsValidStreetCode
                    && IsValidStreetNameGrade && IsValidStreetNameCode
                    && IsValidCityGrade && IsValidCityCode
                    && IsValidStateGrade && IsValidStateCode
                    && IsValidAddressGrade && IsValidAddressCode
                    && IsValidPhoneGrade && IsValidPhoneCode && IsValidZipGrade && IsValidDensity && IsValidUniqueness && IsValidSIC
                    && IsValidZipCode && IsValidDensityCode && IsValidUniquenessCode && IsValidSICCode;

            }
        }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int MatchGradeComponentCount { get; set; }
        public int CompanyScore { get; set; }

        public List<AutoAcceptanceCriteriaDetail> lstAutoAcceptanceCriteriaDetail { get; set; }
        #region IDataErrorInfo
        private string _error = string.Empty;
        public string Error
        {
            get { return _error; }
        }

        public string this[string columnName]
        {
            get
            {
                _error = string.Empty;

                if (columnName == "ConfidenceCode" && !IsValid)
                {
                    _error = "Please select Confidence Code.";
                }
                else if (columnName == "GroupId" && !IsValidCountryGroup)
                {
                    _error = "Please select Country Group.";
                }
                else if (columnName == "CompanyGrade" && !IsValidCompanyGrade)
                {
                    _error = "Please select Company Grade.";
                }
                else if (columnName == "CompanyCode" && !IsValidCompanyCode)
                {
                    _error = "Please select Company Code.";
                }
                else if (columnName == "StreetGrade" && !IsValidStreetGrade)
                {
                    _error = "Please select Street Grade.";
                }
                else if (columnName == "StreetCode" && !IsValidStreetCode)
                {
                    _error = "Please select Street Code.";
                }
                else if (columnName == "StreetNameGrade" && !IsValidStreetNameGrade)
                {
                    _error = "Please select Street Name Grade.";
                }
                else if (columnName == "StreetNameCode" && !IsValidStreetNameCode)
                {
                    _error = "Please select Street Name Code.";
                }
                else if (columnName == "CityGrade" && !IsValidCityGrade)
                {
                    _error = "Please select City Grade.";
                }
                else if (columnName == "CityCode" && !IsValidCityCode)
                {
                    _error = "Please select City Code.";
                }
                else if (columnName == "StateGrade" && !IsValidStateGrade)
                {
                    _error = "Please select State Grade.";
                }
                else if (columnName == "StateCode" && !IsValidStateCode)
                {
                    _error = "Please select State Code.";
                }
                else if (columnName == "AddressGrade" && !IsValidAddressGrade)
                {
                    _error = "Please select Address Grade.";
                }
                else if (columnName == "AddressCode" && !IsValidAddressCode)
                {
                    _error = "Please select Address Code.";
                }
                else if (columnName == "PhoneGrade" && !IsValidPhoneGrade)
                {
                    _error = "Please select Phone Grade.";
                }
                else if (columnName == "PhoneCode" && !IsValidPhoneCode)
                {
                    _error = "Please select Phone Code.";
                }
                else if (columnName == "ZipGrade" && !IsValidZipGrade)
                {
                    _error = "Please select Zip Code.";
                }
                else if (columnName == "ZipCode" && !IsValidZipCode)
                {
                    _error = "Please select Zip Code.";
                }
                else if (columnName == "Density" && !IsValidDensity)
                {
                    _error = "Please select Density.";
                }
                else if (columnName == "DensityCode" && !IsValidDensityCode)
                {
                    _error = "Please select Density Code.";
                }
                else if (columnName == "SIC" && !IsValidSIC)
                {
                    _error = "Please select SIC.";
                }
                else if (columnName == "SICCode" && !IsValidSICCode)
                {
                    _error = "Please select SIC Code.";
                }
                else if (columnName == "Uniqueness" && !IsValidUniqueness)
                {
                    _error = "Please select Uniqueness.";
                }
                else if (columnName == "UniquenessCode" && !IsValidUniquenessCode)
                {
                    _error = "Please select Uniquenessip Code.";
                }
                RaisePropertyChanged("IsValidSave");
                return _error;

            }
        }
        #endregion

    }

    [Serializable]
    public class ViewModelBase : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            if (null != PropertyChanged)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }

    public class ConfidenceCodes
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class AutoAcceptanceCriteriaDetail
    {
        public int CriteriaId { get; set; }
        public int ConfidenceCode { get; set; }
        public string MatchGrade { get; set; }
        public string MDPCode { get; set; }
        public string Tag { get; set; }
        public int CriteriaGroupId { get; set; }
    }
}
