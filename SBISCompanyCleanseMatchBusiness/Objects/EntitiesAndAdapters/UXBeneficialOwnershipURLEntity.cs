using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class UXBeneficialOwnershipURLEntity
    {
        public int DnBAPIId { get; set; }
        public string APIType { get; set; }
        public string DnBDUNSNumber { get; set; }
        public string EnrichmentURL { get; set; }
        public string APIFamily { get; set; }
        public string AuthToken { get; set; }
        public int CredentialId { get; set; }
        public bool EnrichmentExists { get; set; }
    }
}
