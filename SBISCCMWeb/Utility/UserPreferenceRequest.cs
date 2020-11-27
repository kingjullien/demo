using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBISCCMWeb.Utility
{
    public class UserPreferenceSpecification
    {
        public string PreferenceTypeUser { get; set; }
        public string ApplicationAreaNameUser { get; set; }
        public string PreferenceNameUser { get; set; }
        public string PreferenceDescriptionUser { get; set; }
        public string PreferenceValueTextUser { get; set; }
        public string DefaultPreferenceUser { get; set; }
    }

    public class UserPreferenceRequestDetail
    {
        public UserPreferenceSpecification PreferenceSpecificationUser { get; set; }
    }

    public class UserCreatePreferenceRequest
    {
        public string xmlnsuser { get; set; }
        public TransactionDetail TransactionDetailUser { get; set; }
        public UserPreferenceRequestDetail PreferenceRequestDetailUser { get; set; }
    }

    public class UserPreferenceRequest
    {
        public UserCreatePreferenceRequest CreatePreferenceRequestUser { get; set; }
    }
}