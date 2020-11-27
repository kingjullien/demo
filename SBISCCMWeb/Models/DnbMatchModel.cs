using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Models
{
    public class APIResponse
    {
        public List<DnbMatchModel> DnbMatchModels { get; set; }
        public List<MatchEntity> MatchEntities { get; set; }
        public TransactionResponseDetail TransactionResponseDetail { get; set; }
        public string APIRequest { get; set; }
        public string ResponseJSON { get; set; }
    }
    public class TransactionResponseDetail
    {
        public string ServiceTransactionID { get; set; }
        public DateTime TransactionTimestamp { get; set; }
        public string SeverityText { get; set; }
        public string ResultID { get; set; }
        public string ResultText { get; set; }
        public string MatchDataCriteriaText { get; set; }
        public int MatchedQuantity { get; set; }
        public string DnBDUNSNumber { get; set; }
    }
    public class DnbMatchModel
    {
        public string DnsNumber { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string TelephoneNumber { get; set; }
        public string Country { get; set; }
        public string Operating { get; set; }
        public string Role { get; set; }
        public string StreetNo { get; set; }
        public string StreetName { get; set; }
        public string MatchGrade { get; set; }
        public string MDP { get; set; }
        public int ConfidenceCodeValue { get; set; }
        public int Sequance { get; set; }
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
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}