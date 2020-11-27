using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class ElmahEntity
    {

        public string ErrorId { get; set; }
        public string Host { get; set; }
        public string StatusCode { get; set; }
        public string Type { get; set; }
        public string Error { get; set; }
        public string User { get; set; }
        public DateTime TimeUtc { get; set; }

        public string Application { get; set; }
        public string Source { get; set; }
        public string AllXml { get; set; }
    }
}
