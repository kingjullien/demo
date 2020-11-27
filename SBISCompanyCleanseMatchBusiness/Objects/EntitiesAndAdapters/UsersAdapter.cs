using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    class UsersAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<UsersEntity> Adapt(DataTable dt)
        {
            List<UsersEntity> results = new List<UsersEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                UsersEntity matchCode = new UsersEntity();
                matchCode = AdaptItem(rw, dt);
                results.Add(matchCode);
            }
            return results;
        }

        public UsersEntity AdaptItem(DataRow rw, DataTable dt)
        {
            UsersEntity result = new UsersEntity();
            result.UserId = SafeHelper.GetSafeint(rw["UserId"]);
            result.UserName = SafeHelper.GetSafestring(rw["UserName"]);
            result.UserFullName = SafeHelper.GetSafestring(rw["UserName"]);
            //if (dt.Columns.Contains("LoginId"))
            //    result.LoginId = SafeHelper.GetSafestring(rw["LoginId"]);
            if (dt.Columns.Contains("UserStatusCode"))
            {
                result.UserStatusCode = SafeHelper.GetSafestring(rw["UserStatusCode"]);
            }

            if (dt.Columns.Contains("UserTypeCode"))
            {
                result.UserTypeCode = SafeHelper.GetSafestring(rw["UserTypeCode"]);
            }

            if (dt.Columns.Contains("StatusDescription"))
            {
                result.StatusDescription = SafeHelper.GetSafestring(rw["StatusDescription"]);
            }

            if (dt.Columns.Contains("TypeDescription"))
            {
                result.TypeDescription = SafeHelper.GetSafestring(rw["TypeDescription"]);
            }

            if (dt.Columns.Contains("UserType"))
            {
                result.UserType = SafeHelper.GetSafestring(rw["UserType"]);
            }

            if (dt.Columns.Contains("ClientName"))
            {
                result.ClientName = SafeHelper.GetSafestring(rw["ClientName"]);
            }

            if (dt.Columns.Contains("UserLayoutPreference"))
            {
                result.UserLayoutPreference = SafeHelper.GetSafestring(rw["UserLayoutPreference"]);
            }

            if (dt.Columns.Contains("ClientGUID"))
            {
                result.ClientGUID = SafeHelper.GetSafestring(rw["ClientGUID"]);
            }

            if (dt.Columns.Contains("IsApprover"))
            {
                result.IsApprover = SafeHelper.GetSafebool(rw["IsApprover"]);
            }

            if (dt.Columns.Contains("Enable2StepUpdate"))
            {
                result.Enable2StepUpdate = SafeHelper.GetSafebool(rw["Enable2StepUpdate"]);
            }

            if (dt.Columns.Contains("SecurityStamp"))
            {
                result.SecurityStamp = SafeHelper.GetSafestring(rw["SecurityStamp"]);
            }

            if (dt.Columns.Contains("PasswordHash"))
            {
                result.PasswordHash = SafeHelper.GetSafestring(rw["PasswordHash"]);
            }

            if (dt.Columns.Contains("EmailAddress"))
            {
                result.EmailAddress = SafeHelper.GetSafestring(rw["EmailAddress"]);
            }

            if (dt.Columns.Contains("Image_path"))
            {
                result.Imagepath = SafeHelper.GetSafestring(rw["Image_path"]);
            }

            if (dt.Columns.Contains("IpAddress"))
            {
                result.IpAddress = SafeHelper.GetSafestring(rw["IpAddress"]);
            }

            if (dt.Columns.Contains("PasswordResetDate"))
            {
                result.PasswordResetDate = SafeHelper.GetSafeDateTime(rw["PasswordResetDate"]);
            }

            if (dt.Columns.Contains("LastActivityTime"))
            {
                result.LastActivityTime = SafeHelper.GetSafeDateTime(rw["LastActivityTime"]);
            }

            if (dt.Columns.Contains("SecurityQuestionId"))
            {
                result.SecurityQuestionId = SafeHelper.GetSafeint(rw["SecurityQuestionId"]);
            }

            if (dt.Columns.Contains("SecurityAnswer"))
            {
                result.SecurityAnswer = SafeHelper.GetSafestring(rw["SecurityAnswer"]);
            }

            if (dt.Columns.Contains("IsUserLoginFirstTime"))
            {
                result.IsUserLoginFirstTime = SafeHelper.GetSafebool(rw["IsUserLoginFirstTime"]);
            }

            if (dt.Columns.Contains("EmailVerificationCode"))
            {
                result.EmailVerificationCode = SafeHelper.GetSafestring(rw["EmailVerificationCode"]);
            }

            if (dt.Columns.Contains("AttemptCount"))
            {
                result.AttemptCount = SafeHelper.GetSafeint(rw["AttemptCount"]);
            }

            if (dt.Columns.Contains("Tags"))
            {
                result.Tags = SafeHelper.GetSafestring(rw["Tags"]);
            }

            if (dt.Columns.Contains("EnableInvestigations"))
            {
                result.EnableInvestigations = SafeHelper.GetSafebool(rw["EnableInvestigations"]);
            }

            if (dt.Columns.Contains("EnableSearchByDUNS"))
            {
                result.EnableSearchByDUNS = SafeHelper.GetSafebool(rw["EnableSearchByDUNS"]);
            }

            if (dt.Columns.Contains("EnableCreateAutoAcceptRules"))
            {
                result.EnableCreateAutoAcceptRules = SafeHelper.GetSafebool(rw["EnableCreateAutoAcceptRules"]);
            }

            if (dt.Columns.Contains("EULAAcceptedDateTime"))
            {
                if (!string.IsNullOrEmpty(Convert.ToString(rw["EULAAcceptedDateTime"])))
                {
                    result.EULAAcceptedDateTime = SafeHelper.GetSafeDateTime(rw["EULAAcceptedDateTime"]);
                }
            }
            if (dt.Columns.Contains("EULAAcceptedIPAddress"))
            {
                result.EULAAcceptedIPAddress = SafeHelper.GetSafestring(rw["EULAAcceptedIPAddress"]);
            }

            if (dt.Columns.Contains("EnablePurgeData"))
            {
                result.EnablePurgeData = SafeHelper.GetSafebool(rw["EnablePurgeData"]);
            }

            if (dt.Columns.Contains("LOBTag"))
            {
                result.LOBTag = SafeHelper.GetSafestring(rw["LOBTag"]);
            }

            if (dt.Columns.Contains("SSOUser"))
            {
                result.SSOUser = SafeHelper.GetSafestring(rw["SSOUser"]);
            }

            if (dt.Columns.Contains("UserRole"))
            {
                result.UserRole = SafeHelper.GetSafestring(rw["UserRole"]);
            }

            if (dt.Columns.Contains("EnablePreviewMatchRules"))
            {
                result.EnablePreviewMatchRules = SafeHelper.GetSafebool(rw["EnablePreviewMatchRules"]);
            }

            if (dt.Columns.Contains("TagsInclusive"))
            {
                result.TagsInclusive = SafeHelper.GetSafebool(rw["TagsInclusive"]);
            }

            if (dt.Columns.Contains("LicenseAllowEnrichment"))
            {
                result.LicenseAllowEnrichment = SafeHelper.GetSafebool(rw["LicenseAllowEnrichment"]);
            }
            if (dt.Columns.Contains("EnableImportData"))
            {
                result.EnableImportData = SafeHelper.GetSafebool(rw["EnableImportData"]);
            }
            if (dt.Columns.Contains("EnableExportData"))
            {
                result.EnableExportData = SafeHelper.GetSafebool(rw["EnableExportData"]);
            }
            if (dt.Columns.Contains("EnableCompliance"))
            {
                result.EnableCompliance = SafeHelper.GetSafebool(rw["EnableCompliance"]);
            }

            return result;
        }

        public UsersEntity AdaptItemforSecurityQue(DataRow rw, DataTable dt)
        {
            UsersEntity result = new UsersEntity();
            result.SecurityQuestionId = SafeHelper.GetSafeint(rw["ID"]);
            result.SecurityQuestion = SafeHelper.GetSafestring(rw["Questions"]);

            return result;
        }


        public List<UserStatus> AdaptUserUtil(DataTable dt)
        {
            List<UserStatus> results = new List<UserStatus>();
            foreach (DataRow item in dt.Rows)
            {
                UserStatus matchCode = new UserStatus()
                {
                    Code = SafeHelper.GetSafestring(item["Code"]),
                    Value = SafeHelper.GetSafestring(item["Description"]),
                };
                results.Add(matchCode);
            }
            return results;
        }

        public List<UsersEntity> AdaptforSecurity(DataTable dt)
        {
            List<UsersEntity> results = new List<UsersEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                UsersEntity matchCode = new UsersEntity();
                matchCode = AdaptItemforSecurityQue(rw, dt);
                results.Add(matchCode);
            }
            return results;
        }


    }


}
