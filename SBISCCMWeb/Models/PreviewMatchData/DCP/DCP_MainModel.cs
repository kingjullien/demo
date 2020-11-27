using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Models.PreviewMatchData.DCP
{
    public class DCP_MainModel
    {
        public List<DCP_Base> lstBase { get; set; }
        public List<DCP_Competitors> lstCompetitors { get; set; }
        public List<DCP_ConsolidatedEmployees> lstConsolidatedEmployees { get; set; }
        public List<DCP_CurrentPrincipals> lstCurrentPrincipals { get; set; }
        public List<DCP_FamilyTreeMembers> lstFamilyTreeMembers { get; set; }
        public List<DCP_FinancialStatements> lstFinancialStatements { get; set; }
        public List<DCP_IndustryCodes> lstIndustryCodes { get; set; }
        public List<DCP_KeyFinancialFigures> lstKeyFinancialFigures { get; set; }
        public List<DCP_NonMarketableReasons> lstNonMarketableReasons { get; set; }
        public List<DCP_OrganizationIDNumbers> lstOrganizationIDNumbers { get; set; }
        public List<DCP_SocialMedia> lstSocialMedia { get; set; }
        public List<DCP_StockExchange> lstStockExchange { get; set; }
    }
}