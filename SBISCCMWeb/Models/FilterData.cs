using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Models
{
    public class FilterData
    {
        public string FieldName { get; set; }
        public string Operator { get; set; }
        public string FilterValue { get; set; }
    }

    public class DropDownReturn
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }
}