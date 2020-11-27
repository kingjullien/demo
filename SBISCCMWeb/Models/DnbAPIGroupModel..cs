using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Models
{
    public class DnbAPIGroupModel
    {
        internal List<DnBAPIGroupEntity> dnbAPIGroups = new List<DnBAPIGroupEntity>();
        internal List<DnbAPIEntity> dnbAPIs = new List<DnbAPIEntity>();
        internal DnBAPIGroupEntity objDnbGroupAPI = new DnBAPIGroupEntity();
    }
}