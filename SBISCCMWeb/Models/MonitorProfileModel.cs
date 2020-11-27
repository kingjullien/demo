using SBISCCMWeb.Utility;
using SBISCompanyCleanseMatchBusiness.Objects;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchFacade.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SBISCCMWeb.Models
{
    public class MonitorProfileModel
    {
        #region Property

        #endregion

        #region Operators & Elements List for bind dropdown list
        public static List<SelectListItem> ElementsList()
        {
            List<SelectListItem> lstAllElements = new List<SelectListItem>();
            //SettingFacade fac = new SettingFacade();
            lstAllElements.Add(new SelectListItem { Value = "element", Text = "Element" });
            lstAllElements.Add(new SelectListItem { Value = "elementPrevious", Text = "Element Previous" });
            return lstAllElements;
        }


        public static List<SelectListItem> BussnessLevel()
        {
            List<SelectListItem> lstAllBussnessLevel = new List<SelectListItem>();
            lstAllBussnessLevel.Add(new SelectListItem { Value = "", Text = "Select" });
            lstAllBussnessLevel.Add(new SelectListItem { Value = "Level1", Text = "Level1" });
            return lstAllBussnessLevel;
        }



        #endregion


    }
    public class BusinessElementssConditonList
    {
        public int ElementsConditonListId { get; set; }
        public List<MonitoingProfileElementsConditon> ElementsConditonList { get; set; }
        public string strCondition { get; set; }
        public List<TempMonitoring> lstBusinessElements { get; set; }
    }

    [Serializable]
    public class MonitoingProfileElementsConditon
    {
        public MonitoingProfileElementsConditon()
        {
            lstCondition = new List<TempMonitoring>();
        }
        public string TempConditionId { get; set; }
        public int Level { get; set; }
        public string Element { get; set; }
        public string ConditonOpetator { get; set; }
        public string Conditon { get; set; } // And ,or Group
        public string ConditonValue { get; set; }

        public List<TempMonitoring> lstCondition { get; set; }
    }
    [Serializable]
    public class TempMonitoring
    {
        public string ParentElement { get; set; }
        public string ParentConditon { get; set; }
        public MonitoingProfileElementsConditon objM { get; set; }
    }
    public enum MonitoringType
    {
        And, OR, Group, AndGroup, ORGroup
    }
    public class MonitoringElements
    {
        public int MonitoringConditionID { get; set; }
        public int ProfileID { get; set; }
        public int ProductElementID { get; set; }
        public string ElementName { get; set; }
        public string ElementPCMXPath { get; set; }
        public List<MonitoringChanges> lstMonitoringChanges { get; set; }
    }
}