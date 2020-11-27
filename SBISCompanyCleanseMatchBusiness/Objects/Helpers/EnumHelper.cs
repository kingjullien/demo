using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace SBISCompanyCleanseMatchBusiness.Objects.Helpers
{
    public class EnumHelper
    {

        public enum HelpDataenum
        {
            [Display(Name = "Dash Board")]
            DashBoard = 1,
            [Display(Name = "Import Data")]
            ImportData,
            [Display(Name = "Review Matches")]
            ReviewMatches,
            [Display(Name = "Match Data")]
            MatchData,
            [Display(Name = "Approve Match Data")]
            ApproveMatchData,
            [Display(Name = "Clean Data")]
            CleanData,
            [Display(Name = "Search Data")]
            SearchData,
            [Display(Name = "Export Data")]
            ExportData,
            [Display(Name = "Cleanse Match")]
            CleanseMatch,
            [Display(Name = "System")]
            System,
            [Display(Name = "Configuration Data")]
            ConfigurationData,
            [Display(Name = "About Us")]
            AboutUs,
            [Display(Name = "API Usage")]
            APIUsage,
            [Display(Name = "Input and Output")]
            InputandOutput,
            [Display(Name = "Company Process Audit")]
            CompanyProcessAudit,
            [Display(Name = "Data Stewardship Statistics")]
            DataStewardshipStatistics,
            [Display(Name = "Top Match Grades")]
            TopMatchGrades,
            [Display(Name = "Help Desk")]
            HelpDesk,
        }

        public enum DBIntent
        {
            Read,
            ReadWrite
        }

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DisplayAttribute[] attributes =
                (DisplayAttribute[])fi.GetCustomAttributes(
                typeof(DisplayAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
            {
                return attributes[0].Name;
            }
            else
            {
                return value.ToString();
            }
        }
    }
}
