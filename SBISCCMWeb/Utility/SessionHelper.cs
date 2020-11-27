using SBISCCMWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Utility
{
    public class SessionHelper
    {
        public static string GetConnctionstring
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["SBISCCMConnection"])
                    return (string)HttpContext.Current.Session["SBISCCMConnection"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["SBISCCMConnection"] = value;
            }
        }
        public static string ClientName
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["SBISCCMClientName"])
                    return (string)HttpContext.Current.Session["SBISCCMClientName"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["SBISCCMClientName"] = value;
            }
        }

        public static string BackgroundProcessStats
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["BackgroundProcessStats"])
                    return (string)HttpContext.Current.Session["BackgroundProcessStats"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["BackgroundProcessStats"] = value;
            }
        }

        public static bool NotRemember
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["NotRemember"])
                    return (bool)HttpContext.Current.Session["NotRemember"];
                else
                    return false;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["NotRemember"] = value;
            }
        }

        public static string RedirectUrl
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["RedirectUrl"])
                    return (string)HttpContext.Current.Session["RedirectUrl"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["RedirectUrl"] = value;
            }
        }

        public static bool PredefineUser
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["PredefineUser"])
                    return (bool)HttpContext.Current.Session["PredefineUser"];
                else
                    return false;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["PredefineUser"] = value;
            }
        }
        public static string ModelStateError
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["ModelStateError"])
                    return (string)HttpContext.Current.Session["ModelStateError"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["ModelStateError"] = value;
            }
        }

        public static string IDPSingleLogoutServiceUrl
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["IDPSingleLogoutServiceUrl"])
                    return (string)HttpContext.Current.Session["IDPSingleLogoutServiceUrl"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["IDPSingleLogoutServiceUrl"] = value;
            }
        }
        public static bool RememberMeSAML
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["RememberMeSAML"])
                    return (bool)HttpContext.Current.Session["RememberMeSAML"];
                else
                    return false;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["RememberMeSAML"] = value;
            }
        }

        public static string SamlLogoutLink
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["SamlLogoutLink"])
                    return (string)HttpContext.Current.Session["SamlLogoutLink"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["SamlLogoutLink"] = value;
            }
        }

        public static string ImportJobTableList
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["ImportJobTableList"])
                    return (string)HttpContext.Current.Session["ImportJobTableList"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["ImportJobTableList"] = value;
            }
        }

        public static string lstImportFileTemplates
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["lstImportFileTemplates"])
                    return (string)HttpContext.Current.Session["lstImportFileTemplates"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["lstImportFileTemplates"] = value;
            }
        }

        public static string objimportJobData
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["objimportJobData"])
                    return (string)HttpContext.Current.Session["objimportJobData"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["objimportJobData"] = value;
            }
        }

        public static string ImportFilePath
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["ImportFilePath"])
                    return (string)HttpContext.Current.Session["ImportFilePath"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["ImportFilePath"] = value;
            }
        }

        public static string ImportData_Data
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["ImportData_Data"])
                    return (string)HttpContext.Current.Session["ImportData_Data"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["ImportData_Data"] = value;
            }
        }

        public static bool ImportData_IsTag
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["ImportData_IsTag"])
                    return (bool)HttpContext.Current.Session["ImportData_IsTag"];
                else
                    return false;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["ImportData_IsTag"] = value;
            }
        }

        public static bool ImportData_IsInLanguage
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["ImportData_IsInLanguage"])
                    return (bool)HttpContext.Current.Session["ImportData_IsInLanguage"];
                else
                    return false;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["ImportData_IsInLanguage"] = value;
            }
        }

        public static bool ImportData_IsHeader
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["ImportData_IsHeader"])
                    return (bool)HttpContext.Current.Session["ImportData_IsHeader"];
                else
                    return false;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["ImportData_IsHeader"] = value;
            }
        }

        public static string ImportData_LineData
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["ImportData_LineData"])
                    return (string)HttpContext.Current.Session["ImportData_LineData"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["ImportData_LineData"] = value;
            }
        }
        public static string ImportData_HeaderLine
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["ImportData_HeaderLine"])
                    return (string)HttpContext.Current.Session["ImportData_HeaderLine"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["ImportData_HeaderLine"] = value;
            }
        }

        public static string InvestigationRecordList
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["InvestigationRecordList"])
                    return (string)HttpContext.Current.Session["InvestigationRecordList"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["InvestigationRecordList"] = value;
            }
        }
        public static string InvestigationStats
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["InvestigationStats"])
                    return (string)HttpContext.Current.Session["InvestigationStats"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["InvestigationStats"] = value;
            }
        }

        public static string InvestigationFileData
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["InvestigationFileData"])
                    return (string)HttpContext.Current.Session["InvestigationFileData"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["InvestigationFileData"] = value;
            }
        }

        public static string CleanMatchPageNo
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["CleanMatchPageNo"])
                    return (string)HttpContext.Current.Session["CleanMatchPageNo"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["CleanMatchPageNo"] = value;
            }
        }

        public static string CleanQueueMessage
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["CleanQueueMessage"])
                    return (string)HttpContext.Current.Session["CleanQueueMessage"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["CleanQueueMessage"] = value;
            }
        }

        public static string CleanMatchPage
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["CleanMatchPage"])
                    return (string)HttpContext.Current.Session["CleanMatchPage"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["CleanMatchPage"] = value;
            }
        }
        public static string CleanTotalCount
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["CleanTotalCount"])
                    return (string)HttpContext.Current.Session["CleanTotalCount"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["CleanTotalCount"] = value;
            }
        }

        public static string CleanMatchedCompany
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["CleanMatchedCompany"])
                    return (string)HttpContext.Current.Session["CleanMatchedCompany"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["CleanMatchedCompany"] = value;
            }
        }

        public static bool BadInputData_IsFirstTimeFilter
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["BadInputData_IsFirstTimeFilter"])
                    return (bool)HttpContext.Current.Session["BadInputData_IsFirstTimeFilter"];
                else
                    return false;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["BadInputData_IsFirstTimeFilter"] = value;
            }
        }
        public static string SearchCompany
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["SearchCompany"])
                    return (string)HttpContext.Current.Session["SearchCompany"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["SearchCompany"] = value;
            }
        }
        public static string SearchCompanySrcId
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["SearchCompanySrcId"])
                    return (string)HttpContext.Current.Session["SearchCompanySrcId"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["SearchCompanySrcId"] = value;
            }
        }

        public static string SearchCompanyInputId
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["SearchCompanyInputId"])
                    return (string)HttpContext.Current.Session["SearchCompanyInputId"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["SearchCompanyInputId"] = value;
            }
        }

        public static string SearchMatches
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["SearchMatches"])
                    return (string)HttpContext.Current.Session["SearchMatches"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["SearchMatches"] = value;
            }
        }

        public static string SearchMatch
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["SearchMatch"])
                    return (string)HttpContext.Current.Session["SearchMatch"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["SearchMatch"] = value;
            }
        }

        public static string SearchModel
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["SearchModel"])
                    return (string)HttpContext.Current.Session["SearchModel"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["SearchModel"] = value;
            }
        }

        public static string MatchRecord
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["MatchRecord"])
                    return (string)HttpContext.Current.Session["MatchRecord"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["MatchRecord"] = value;
            }
        }

        public static string DUNSNumber
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["DUNSNumber"])
                    return (string)HttpContext.Current.Session["DUNSNumber"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["DUNSNumber"] = value;
            }
        }
        public static string pagevalueStewData
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["pagevalueStewData"])
                    return (string)HttpContext.Current.Session["pagevalueStewData"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["pagevalueStewData"] = value;
            }
        }

        public static string pageNumberStewData
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["pageNumberStewData"])
                    return (string)HttpContext.Current.Session["pageNumberStewData"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["pageNumberStewData"] = value;
            }
        }

        public static string QueueMessage
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["QueueMessage"])
                    return (string)HttpContext.Current.Session["QueueMessage"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["QueueMessage"] = value;
            }
        }

        public static string TotalCountStew
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["TotalCountStew"])
                    return (string)HttpContext.Current.Session["TotalCountStew"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["TotalCountStew"] = value;
            }
        }

        public static string TempCompanies
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["TempCompanies"])
                    return (string)HttpContext.Current.Session["TempCompanies"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["TempCompanies"] = value;
            }
        }
        public static int pagevalueReviewData
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["pagevalueReviewData"])
                    return (Convert.ToInt32(HttpContext.Current.Session["pagevalueReviewData"]));
                else
                    return 0;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["pagevalueReviewData"] = value;
            }
        }
        public static string OrderByColumnReviewData
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["OrderByColumnReviewData"])
                    return (string)HttpContext.Current.Session["OrderByColumnReviewData"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["OrderByColumnReviewData"] = value;
            }

        }
        public static int CountryGroupId
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["CountryGroupId"])
                    return (Convert.ToInt32(HttpContext.Current.Session["CountryGroupId"]));
                else
                    return 0;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["CountryGroupId"] = value;
            }

        }
        public static string ConfidenceCode
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["ConfidenceCode"])
                    return (string)HttpContext.Current.Session["ConfidenceCode"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["ConfidenceCode"] = value;
            }

        }
        public static int CurrentPageIndex
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["CurrentPageIndex"])
                    return (Convert.ToInt32(HttpContext.Current.Session["CurrentPageIndex"]));
                else
                    return 0;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["CurrentPageIndex"] = value;
            }

        }
        public static bool TopMatchReviewData
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["TopMatchReviewData"])
                    return (bool)HttpContext.Current.Session["TopMatchReviewData"];
                else
                    return false;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["TopMatchReviewData"] = value;
            }

        }
        public static string LOBTag
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["LOBTag"])
                    return (string)HttpContext.Current.Session["LOBTag"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["LOBTag"] = value;
            }

        }
        public static string Tag
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["Tag"])
                    return (string)HttpContext.Current.Session["Tag"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["Tag"] = value;
            }

        }
        public static string pageNumberReviewData
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["pageNumberReviewData"])
                    return (string)HttpContext.Current.Session["pageNumberReviewData"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["pageNumberReviewData"] = value;
            }
        }
        public static string TotalCountReview
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["TotalCountReview"])
                    return (string)HttpContext.Current.Session["TotalCountReview"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["TotalCountReview"] = value;
            }
        }

        public static string AcceptedBy
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["AcceptedBy"])
                    return (string)HttpContext.Current.Session["AcceptedBy"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["AcceptedBy"] = value;
            }
        }
        public static bool Stew_IsFirstTimeFilter
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["Stew_IsFirstTimeFilter"])
                    return (bool)HttpContext.Current.Session["Stew_IsFirstTimeFilter"];
                else
                    return false;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["Stew_IsFirstTimeFilter"] = value;
            }
        }
        public static bool Prev_IsFirstTimeFilter
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["Prev_IsFirstTimeFilter"])
                    return (bool)HttpContext.Current.Session["Prev_IsFirstTimeFilter"];
                else
                    return false;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["Prev_IsFirstTimeFilter"] = value;
            }
        }
        public static bool Rev_IsFirstTimeFilter
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["Prev_IsFirstTimeFilter"])
                    return (bool)HttpContext.Current.Session["Prev_IsFirstTimeFilter"];
                else
                    return false;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["Prev_IsFirstTimeFilter"] = value;
            }
        }
        public static string Stew_Data
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["Stew_Data"])
                    return (string)HttpContext.Current.Session["Stew_Data"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["Stew_Data"] = value;
            }
        }

        public static string dtAcceptData
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["dtAcceptData"])
                    return (string)HttpContext.Current.Session["dtAcceptData"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["dtAcceptData"] = value;
            }
        }

        public static string BuildList_pageno
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["BuildList_pageno"])
                    return (string)HttpContext.Current.Session["BuildList_pageno"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["BuildList_pageno"] = value;
            }
        }

        public static string Preview_NodataMessage
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["Preview_NodataMessage"])
                    return (string)HttpContext.Current.Session["Preview_NodataMessage"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["Preview_NodataMessage"] = value;
            }
        }
        public static bool CommandMapping_IsInLanguage
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["CommandMapping_IsInLanguage"])
                    return (bool)HttpContext.Current.Session["CommandMapping_IsInLanguage"];
                else
                    return false;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["CommandMapping_IsInLanguage"] = value;
            }
        }
        public static string ExportView_Message
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["ExportView_Message"])
                    return (string)HttpContext.Current.Session["ExportView_Message"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["ExportView_Message"] = value;
            }
        }

        public static string Portal_Data
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["Portal_Data"])
                    return (string)HttpContext.Current.Session["Portal_Data"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["Portal_Data"] = value;
            }
        }

        public static string EntityMessage
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["EntityMessage"])
                    return (string)HttpContext.Current.Session["EntityMessage"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["EntityMessage"] = value;
            }
        }

        public static string Portal_APIKEY
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["Portal_APIKEY"])
                    return (string)HttpContext.Current.Session["Portal_APIKEY"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["Portal_APIKEY"] = value;
            }
        }

        public static string Portal_APISecret
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["Portal_APISecret"])
                    return (string)HttpContext.Current.Session["Portal_APISecret"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["Portal_APISecret"] = value;
            }
        }

        public static string Portal_SubDomain
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["Portal_SubDomain"])
                    return (string)HttpContext.Current.Session["Portal_SubDomain"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["Portal_SubDomain"] = value;
            }
        }
        public static string lstDataQueue
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["lstDataQueue"])
                    return (string)HttpContext.Current.Session["lstDataQueue"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["lstDataQueue"] = value;
            }
        }

        public static string dtDataStewardStatistics
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["dtDataStewardStatistics"])
                    return (string)HttpContext.Current.Session["dtDataStewardStatistics"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["dtDataStewardStatistics"] = value;
            }
        }
        public static string dsDataAPIUsage
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["dsDataAPIUsage"])
                    return (string)HttpContext.Current.Session["dsDataAPIUsage"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["dsDataAPIUsage"] = value;
            }
        }

        public static string Review_pageno
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["Review_pageno"])
                    return (string)HttpContext.Current.Session["Review_pageno"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["Review_pageno"] = value;
            }
        }
        public static bool Review_IsLoad
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["Review_IsLoad"])
                    return (bool)HttpContext.Current.Session["Review_IsLoad"];
                else
                    return false;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["Review_IsLoad"] = value;
            }
        }

        public static string ListMatchEntity
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["ListMatchEntity"])
                    return (string)HttpContext.Current.Session["ListMatchEntity"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["ListMatchEntity"] = value;
            }
        }

        public static string EmptyDataMessage
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["EmptyDataMessage"])
                    return (string)HttpContext.Current.Session["EmptyDataMessage"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["EmptyDataMessage"] = value;
            }
        }

        public static string ImageName
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["ImageName"])
                    return (string)HttpContext.Current.Session["ImageName"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["ImageName"] = value;
            }
        }

        public static string TicketMessage
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["TicketMessage"])
                    return (string)HttpContext.Current.Session["TicketMessage"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["TicketMessage"] = value;
            }
        }
        public static string TicketHistory
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["TicketHistory"])
                    return (string)HttpContext.Current.Session["TicketHistory"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["TicketHistory"] = value;
            }
        }

        public static string lstTempMonirtoring
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["lstTempMonirtoring"])
                    return (string)HttpContext.Current.Session["lstTempMonirtoring"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["lstTempMonirtoring"] = value;
            }
        }

        public static string lstMonirtoringTemp
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["lstMonirtoringTemp"])
                    return (string)HttpContext.Current.Session["lstMonirtoringTemp"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["lstMonirtoringTemp"] = value;
            }
        }

        public static string lstMonitoringElemetsEntity
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["lstMonitoringElemetsEntity"])
                    return (string)HttpContext.Current.Session["lstMonitoringElemetsEntity"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["lstMonitoringElemetsEntity"] = value;
            }
        }

        public static string lstBusinessElementssConditon
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["lstBusinessElementssConditon"])
                    return (string)HttpContext.Current.Session["lstBusinessElementssConditon"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["lstBusinessElementssConditon"] = value;
            }
        }

        public static string DandB_strCondition
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["DandB_strCondition"])
                    return (string)HttpContext.Current.Session["DandB_strCondition"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["DandB_strCondition"] = value;
            }
        }

        public static string DandB_ResponseErroeMessage
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["DandB_ResponseErroeMessage"])
                    return (string)HttpContext.Current.Session["DandB_ResponseErroeMessage"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["DandB_ResponseErroeMessage"] = value;
            }
        }

        public static string ConditionCount
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["ConditionCount"])
                    return (string)HttpContext.Current.Session["ConditionCount"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["ConditionCount"] = value;
            }
        }

        public static string BusinessCondionUpdateId
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["BusinessCondionUpdateId"])
                    return (string)HttpContext.Current.Session["BusinessCondionUpdateId"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["BusinessCondionUpdateId"] = value;
            }
        }

        public static string ConditionListUpdatesId
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["ConditionListUpdatesId"])
                    return (string)HttpContext.Current.Session["ConditionListUpdatesId"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["ConditionListUpdatesId"] = value;
            }
        }

        public static string objMTM
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["objMTM"])
                    return (string)HttpContext.Current.Session["objMTM"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["objMTM"] = value;
            }
        }

        public static string lstPreference
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["lstPreference"])
                    return (string)HttpContext.Current.Session["lstPreference"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["lstPreference"] = value;
            }
        }
        public static string DandB_Data
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["DandB_Data"])
                    return (string)HttpContext.Current.Session["DandB_Data"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["DandB_Data"] = value;
            }
        }

        public static bool DandB_IsTag
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["DandB_IsTag"])
                    return (bool)HttpContext.Current.Session["DandB_IsTag"];
                else
                    return false;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["DandB_IsTag"] = value;
            }
        }

        public static bool IsCompanyScore
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["IsCompanyScore"])
                    return (bool)HttpContext.Current.Session["IsCompanyScore"];
                else
                    return false;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["IsCompanyScore"] = value;
            }
        }

        public static string objAutoSetting
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["objAutoSetting"])
                    return (string)HttpContext.Current.Session["objAutoSetting"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["objAutoSetting"] = value;
            }
        }

        public static string DUNSDetailsPagevalue
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["DUNSDetailsPagevalue"])
                    return (string)HttpContext.Current.Session["DUNSDetailsPagevalue"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["DUNSDetailsPagevalue"] = value;
            }
        }

        public static string ThirdPartyAPI
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["ThirdPartyAPI"])
                    return (string)HttpContext.Current.Session["ThirdPartyAPI"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["ThirdPartyAPI"] = value;
            }
        }

        public static string BuildList_Data
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["BuildList_Data"])
                    return (string)HttpContext.Current.Session["BuildList_Data"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["BuildList_Data"] = value;
            }
        }



        public static string lstOIMatchCompanies
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["lstOIMatchCompanies"])
                    return Convert.ToString(HttpContext.Current.Session["lstOIMatchCompanies"]);
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["lstOIMatchCompanies"] = value;
            }
        }
        public static int lstOIMatchCompaniesTotalcount
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["lstOIMatchCompaniesTotalcount"])
                    return (Convert.ToInt32(HttpContext.Current.Session["lstOIMatchCompaniesTotalcount"]));
                else
                    return 0;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["lstOIMatchCompaniesTotalcount"] = value;
            }
        }


        public static string oIlstMatchDetails
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["oIlstMatchDetails"])
                    return Convert.ToString(HttpContext.Current.Session["oIlstMatchDetails"]);
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["oIlstMatchDetails"] = value;
            }
        }
        public static string OIMatchdata_dtAcceptData
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["OIMatchdata_dtAcceptData"])
                    return Convert.ToString(HttpContext.Current.Session["OIMatchdata_dtAcceptData"]);
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["OIMatchdata_dtAcceptData"] = value;
            }
        }
        public static string OIMatchdata_data
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["OIMatchdata_data"])
                    return Convert.ToString(HttpContext.Current.Session["OIMatchdata_data"]);
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["OIMatchdata_data"] = value;
            }
        }
        public static string OIMatchdata_DeleteMappingData
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["OIMatchdata_DeleteMappingData"])
                    return Convert.ToString(HttpContext.Current.Session["OIMatchdata_DeleteMappingData"]);
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["OIMatchdata_DeleteMappingData"] = value;
            }
        }
        public static string oReviewDataFilter
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["oReviewDataFilter"])
                    return Convert.ToString(HttpContext.Current.Session["oReviewDataFilter"]);
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["oReviewDataFilter"] = value;
            }
        }
        public static string pagevaluePreviewData
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["pagevaluePreviewData"])
                    return (string)HttpContext.Current.Session["pagevaluePreviewData"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["pagevaluePreviewData"] = value;
            }
        }
        public static string pageNumberPreviewData
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["pageNumberPreviewData"])
                    return (string)HttpContext.Current.Session["pageNumberPreviewData"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["pageNumberPreviewData"] = value;
            }
        }
        public static string TotalCountPreview
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["TotalCountPreview"])
                    return (string)HttpContext.Current.Session["TotalCountPreview"];
                else
                    return string.Empty;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["TotalCountPreview"] = value;
            }
        }

        public static string BeneficialOwnershipDataSet
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["BeneficialOwnershipDataSet"])
                    return Convert.ToString(HttpContext.Current.Session["BeneficialOwnershipDataSet"]);
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["BeneficialOwnershipDataSet"] = value;
            }
        }

        public static string BeneficialOwnershipData
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["BeneficialOwnershipData"])
                    return Convert.ToString(HttpContext.Current.Session["BeneficialOwnershipData"]);
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["BeneficialOwnershipData"] = value;
            }
        }

        public static string ListMultiPassGroupConfigurationForCreation
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["ListMultiPassGroupConfigurationForCreation"])
                    return Convert.ToString(HttpContext.Current.Session["ListMultiPassGroupConfigurationForCreation"]);
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["ListMultiPassGroupConfigurationForCreation"] = value;
            }
        }

        public static string BeneficialOwnershipFilteredData
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["BeneficialOwnershipFilteredData"])
                    return Convert.ToString(HttpContext.Current.Session["BeneficialOwnershipFilteredData"]);
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["BeneficialOwnershipFilteredData"] = value;
            }
        }

        public static string ScreenResultData
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["ScreenResultData"])
                    return Convert.ToString(HttpContext.Current.Session["ScreenResultData"]);
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["ScreenResultData"] = value;
            }
        }

        public static string GraphImage
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["GraphImage"])
                    return Convert.ToString(HttpContext.Current.Session["GraphImage"]);
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["GraphImage"] = value;
            }
        }

    }
}