using PagedList;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Models
{
    public class UnprocessedInputViewModel
    {
        public IPagedList<UnprocessedInputEntity> pglstUnprocesseds { get; set; }
        public int pageSize { get; set; }
        public int pageNumber { get; set; }
        public string importProcess { get; set; }
    }
}