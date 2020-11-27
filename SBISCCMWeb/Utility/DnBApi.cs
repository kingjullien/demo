using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Utility
{
    public class DnBApi
    {
        public static string MonitoringProfileUrl = "https://direct.dnb.com/V7.0/monitoring/monitoringprofiles";
        public static string SyncMonitoringProfileList = "https://direct.dnb.com/V7.0/monitoring/monitoringprofiles?CandidatePerPageMaximumQuantity={0}&CandidateDisplayStartSequenceNumber={1}";   //"https://direct.dnb.com/V7.0/monitoring/monitoringprofiles?CandidateDisplayStartSequenceNumber=0";
        public static string MonitoingService = "http://services.dnb.com/MonitoringServiceV2.0";
        public static string UserPreferenceService = "http://services.dnb.com/UserServiceV2.0";
        public static string UserpreferenceUrl = "https://direct.dnb.com/V2.2/userpreference";
        public static string NotificationUrl = "https://direct.dnb.com/V7.0/monitoring/notificationprofiles";
        public static string UserPrefrenceLis = "https://direct.dnb.com/V2.2/userpreference?SortDirectionText=Ascending&ApplicationAreaName=Monitoring&SortBasisText=UserPreferenceName&CandidatePerPageMaximumQuantity={0}&CandidateDisplayStartSequenceNumber={1}";
        public static string NotificationList = "https://direct.dnb.com/V7.0/monitoring/notificationprofiles?DeliveryFormat=XML&DeliveryMode=Email&CandidatePerPageMaximumQuantity={0}&CandidateDisplayStartSequenceNumber={1}";
        public static string CleanseMatchURL = "https://direct.dnb.com/V6.0/organizations?cleansematch=true";
        public static string MatchURL = "https://direct.dnb.com/V6.0/organizations?&cleansematch=true";
        public static string DnbBuildAListURL = "https://plus.dnb.com/v1/search/criteria";
    }
    public class DirectPlus
    {
        public static string DirectPLUSAuth = "https://plus.dnb.com/v2/token";
        public static string CleanseMatchURL = "https://plus.dnb.com/v1/match/cleanseMatch?";
        public static string SearchByDomainOrEmail = "https://plus.dnb.com/v1/duns-search/{0}/{1}?view=standard";
        public static string ListMonitoringRegistrations = "https://plus.dnb.com/v1/monitoring/registrations/";
        public static string DetailMonitoringRegistration = "https://plus.dnb.com/v1/monitoring/registrations/{0}";
        public static string AddDunsToMonitoringRegistration = "https://plus.dnb.com/v1/monitoring/registrations/{0}/duns/add";
        public static string RemoveDunsToMonitoringRegistration = "https://plus.dnb.com/v1/monitoring/registrations/{0}/duns/remove";
        public static string EditMonitoringRegistration = "https://plus.dnb.com/v1/monitoring/registrations/{0}";
        public static string ExportMonitoringRegistration = "https://plus.dnb.com/v1/monitoring/registrations/export/{0}/duns";
        public static string SuppressUnSuppressMonitoringRegistration = "https://plus.dnb.com/v1/monitoring/registrations/{0}/suppress";

    }
    public class DPMInvestigation
    {
        public static string DPMInvestigationURL = "https://plus.dnb.com/v1/investigate/submit";
        public static string DPMInvestigationstatusURL = "https://plus.dnb.com/v1/investigate/status/";
    }
    public class iResearchInvestigation
    {
        public static string iResearchInvestigationURL = "https://plus.dnb.com/v1/researches";
    }
    public class TypeAhead
    {
        public static string SearchDataTypeAheadWithCountry = "https://plus.dnb.com/v1/search/typeahead?searchTerm=";
        public static string SearchDataTypeAheadWithoutCountry = "https://plus.dnb.com/v1/search/typeahead?searchTerm=";
    }
    public enum ApiLayerType
    {
        [Description("MixMode")]
        MixMode,
        [Description("Direct20")]
        Direct20,
        [Description("Directplus")]
        Directplus
    }
}