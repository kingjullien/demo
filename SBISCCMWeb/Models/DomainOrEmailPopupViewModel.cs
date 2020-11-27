using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Models
{
    public class DomainOrEmailPopupViewModel
    {
        public string type { get; set; }
        public string searchvalue { get; set; }
        public string SrcRecId { get; set; }
        public string InputId { get; set; }
        public bool IsCleanSearch { get; set; }
    }
}