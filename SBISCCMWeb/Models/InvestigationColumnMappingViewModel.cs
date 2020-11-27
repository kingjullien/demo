using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SBISCCMWeb.Models
{
    public class InvestigationColumnMappingViewModel
    {
        public List<string> columns { get; set; }
        public List<SelectListItem> fileColumns { get; set; }
    }
}