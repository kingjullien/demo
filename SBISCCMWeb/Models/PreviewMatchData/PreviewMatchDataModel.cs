using SBISCCMWeb.Models.PreviewMatchData.DCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Models.PreviewMatchData
{
    public class PreviewMatchDataModel
    {
        public List<MatchOutPutModel> lstMatchOutPutData { get; set; }
        public List<APIModel> lstApiModel { get; set; }
        public DCP_MainModel DCP { get; set; }
    }
}