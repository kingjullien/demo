using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Models.PreviewMatchData.DCP
{
    public class DCP_FamilyTreeMembers
    {
        public string DnbDUNSNumber { get; set; }
        public string APIType { get; set; }
        public string TransactionTimestamp { get; set; }
        public string FamilyTreeMemberRole { get; set; }
        public string FamilyTreeMemberRoleCode { get; set; }
    }
}