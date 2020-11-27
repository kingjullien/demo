using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Models
{
    public class EntitlementsViewModel
    {
        public string lstAPIIds { get; set; }
        public int CredentialId { get; set; }
        public string DnBAPIId { get; set; }
        public string APIType { get; set; }
    }
}