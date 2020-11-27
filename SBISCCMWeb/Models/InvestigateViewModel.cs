using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Models
{
    public class InvestigateViewModel
    {
        internal InvestigateViewEntity InvestigateViewEntity = new InvestigateViewEntity();
    }
    public class CompanyIdentificationModel : ViewModelBase
    {
        private string _number;
        public string Number
        {
            get
            {
                return _number;
            }
            set
            {
                _number = value;
                RaisePropertyChanged("Number");
            }
        }
        private string _type;
        public string Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
                RaisePropertyChanged("Type");
            }
        }
        private string _value;
        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                RaisePropertyChanged("Value");
            }
        }
    }
}