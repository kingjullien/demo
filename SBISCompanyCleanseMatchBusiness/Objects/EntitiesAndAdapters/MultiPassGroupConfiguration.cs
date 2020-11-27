using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class MultiPassGroupConfiguration
    {
        public int ProviderCode { get; set; }
        public string Tag { get; set; }
        public string VerificationGroupName { get; set; }
        public string VerifiationLookup { get; set; }
        public int VerificationGroupId { get; set; }
        public string Lookups{ get; set; }
        public string Category { get; set; }
    }

    public class MPMPrecedenceSelection
    {
        public List<MPMPrecedenceConfig> AvailablePrecedence { get; set; }
        public List<string> SelectedPrecedence { get; set; }
    }

    public class MPMSummary
    {
        public string Tag { get; set; }
        public string VerificationGroups { get; set; }
        public string PrecedenceSteps { get; set; }
    }

    public class MultiPassPrecedence
    {
        public string Tag { get; set; }
        public int ProviderCode { get; set; }
        public string Steps { get; set; }
    }

    public class MPMPrecedenceConfig
    {
        public bool IsVerificationGroup { get; set; }
        public string Name { get; set; }
    }
}
