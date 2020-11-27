using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class CompanyEntity : INotifyPropertyChanged
    {
        public CompanyEntity()
        {
            Matches = new List<MatchEntity>();
        }

        public string _CompanyName;
        public string _Address;
        public string _City;
        public string _State;
        public string _PostalCode;
        public string _PhoneNbr;
        public string SrcRecordId { get; set; }
        public string StewardshipNotes { get; set; }
        public bool RejectAllMatches { get; set; }
        public int OriginalMatchCount { get; set; }
        public string EncryptedSrcRecordId { get; set; }
        public string CompanyName
        {
            get
            {
                return _CompanyName;
            }
            set
            {
                _CompanyName = value;
                NotifyPropertyChanged("CompanyName");
            }
        }
        public string Address
        {
            get
            {
                return _Address;
            }
            set
            {
                _Address = value;
                NotifyPropertyChanged("Address");
            }
        }
        public string StreetNo
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_Address) && _Address.IndexOf(" ") > 0)
                {
                    string streetNo = _Address.Substring(0, _Address.IndexOf(" "));
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
                    return !string.IsNullOrWhiteSpace(_Address) ? _Address.Replace(StreetNo, "").TrimStart(' ') : "";
                }
                else
                {
                    return _Address;
                }
            }

        }

        public string City
        {
            get
            {
                return _City;
            }
            set
            {
                _City = value;
                NotifyPropertyChanged("City");
            }
        }
        public string State
        {
            get
            {
                return _State;
            }
            set
            {
                _State = value;
                NotifyPropertyChanged("State");
            }
        }
        public string PostalCode
        {
            get
            {
                return _PostalCode;
            }
            set
            {
                _PostalCode = value;
                NotifyPropertyChanged("PostalCode");
            }
        }
        public string CountryISOAlpha2Code { get; set; }
        public string PhoneNbr
        {
            get
            {
                return _PhoneNbr;
            }
            set
            {
                _PhoneNbr = value;
                NotifyPropertyChanged("PhoneNbr");
            }
        }
        public string ErrorCode { get; set; }
        public string ErrorDescription { get; set; }
        public bool RejectCompany { get; set; }

        public bool IsEdited { get; set; }

        public List<MatchEntity> Matches { get; set; }
        public List<MatchEntity> MatchesFiltered { get; set; }


        public int MatchCount
        {
            get
            {
                return Matches.Count;
            }
        }
        public int FilteredMatchCount
        {
            get
            {
                return MatchesFiltered != null ? MatchesFiltered.Count : 0;
            }
        }

        public int _SelectedMatchCount;
        public int SelectedMatchCount
        {
            get
            {
                return _SelectedMatchCount;
            }
            set
            {
                _SelectedMatchCount = value;
                //_SelectedMatchCount = (from m in Matches where m.IsSelected == true select m).Count();
                NotifyPropertyChanged("SelectedMatchCount");
            }
        }

        public string MatchDisplay
        {
            get
            {
                return "Matches: " + MatchCount.ToString() + " (" + FilteredMatchCount.ToString() + ")";
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

        public string DUNSNumber { get; set; }
        public string CEOName { get; set; }
        public string Website { get; set; }
        public string AltCompanyName { get; set; }
        public string AltAddress { get; set; }
        public string AltCity { get; set; }
        public string AltState { get; set; }
        public string AltPostalCode { get; set; }
        public string AltCountry { get; set; }
        public string Email { get; set; }
        public string RegistrationNbr { get; set; }
        public string RegistrationType { get; set; }
        public string Tags { get; set; }
        public int InputId { get; set; }
        public string inLanguage { get; set; }
        public string Address1 { get; set; }
        public string AltAddress1 { get; set; }
        public string FullAddress { get; set; }
    }
}
public class LSTMatchCompany
{
    public List<CompanyEntity> lstcompany { get; set; }
    public string Message { get; set; }
}
