using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Models
{
    [Serializable]
    public class PreviewMatchDataModel
    {
        public string SrcRecordId { get; set; }
        public bool IsExactMatch { get; set; }
        public string LobTag { get; set; }
        public string Tag { get; set; }
        public string ImportProcess { get; set; }
        public string ConfidenceCode { get; set; }
        public string AcceptedBy { get; set; }
        public int UserId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
    }
}