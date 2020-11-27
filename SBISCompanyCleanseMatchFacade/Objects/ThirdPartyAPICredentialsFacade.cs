using SBISCompanyCleanseMatchBusiness.Objects.Business;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchFacade.Objects
{
    public class ThirdPartyAPICredentialsFacade : FacadeParent
    {
        public const string Direct20UAuthURL = "https://maxcvservices.dnb.com/Authentication/V2.0/";
        public const string DirectPlusAuthURL = "https://plus.dnb.com/v2/token";

        public enum serviceProvider
        {
            DNB,
            ORB
        }
        ThirdPartyAPICredentialsBusiness rep;
        public ThirdPartyAPICredentialsFacade(string connectionString) : base(connectionString) { rep = new ThirdPartyAPICredentialsBusiness(Connection); }

        public List<ThirdPartyAPICredentialsEntity> GetThirdPartyAPICredentials(string ThirdPartyProvider)
        {
            return rep.GetThirdPartyAPICredentials(ThirdPartyProvider);
        }
        public List<ThirdPartyAPICredentialsEntity> GetThirdPartyAPICredentialsForRefresh(string ThirdPartyProvider)
        {
            return rep.GetThirdPartyAPICredentialsForRefresh(ThirdPartyProvider);
        }
        public ThirdPartyAPICredentialsEntity GetThirdPartyAPICredentialsById(int CredentialId)
        {
            return rep.GetThirdPartyAPICredentialsById(CredentialId);
        }
        public string DeleteThirdPartyAPICredentials(int CredentialId, int UserId)
        {
            return rep.DeleteThirdPartyAPICredentials(CredentialId, UserId);
        }
        public string InsertUpdateThirdPartyAPICredentials(ThirdPartyAPICredentialsEntity obj, int UserId)
        {
            return rep.InsertUpdateThirdPartyAPICredentials(obj, UserId);
        }
        public string UpdateThirdPartyAPIAuthToken(int CredentialId, string AuthToken, int Duration, string ErrorCode, string ErrorDescription)
        {
            return rep.UpdateThirdPartyAPIAuthToken(CredentialId, AuthToken, Duration, ErrorCode, ErrorDescription);
        }

        public bool RefreshThirdPartyAPICredentials(string ThirdPartyProvider)
        {
            List<ThirdPartyAPICredentialsEntity> lstThirdPartyAPICredential = GetThirdPartyAPICredentialsForRefresh(ThirdPartyProvider);
            foreach (var item in lstThirdPartyAPICredential)
            {
                if (item.APIType == "Direct20")
                {
                    var client = new RestfulClientFacade<Direct20Response>(item.APICredential, item.APIPassword, Direct20UAuthURL);
                    Direct20Response responseData = client.GetResponse();
                    if (responseData != null)
                    {
                        if (!string.IsNullOrEmpty(responseData?.AuthenticationDetail?.Token))
                        {
                            responseData.AuthenticationDetail.Token = responseData.AuthenticationDetail.Token;
                        }
                        UpdateThirdPartyAPIAuthToken(item.CredentialId, responseData?.AuthenticationDetail?.Token, 86000, responseData?.TransactionResult?.ResultID, responseData?.TransactionResult?.ResultMessage?.ResultDescription);
                    }
                }
                if (item.APIType == "DirectPlus")
                {
                    var client = new RestfulClientFacade<DirectPlusResponse>(item.APICredential, item.APIPassword, DirectPlusAuthURL);
                    DirectPlusResponse responseData = client.GetDirectPlusResponse();
                    if (responseData != null)
                    {
                        if (!string.IsNullOrEmpty(responseData?.access_token))
                        {
                            responseData.access_token = "Bearer " + responseData.access_token;
                        }
                        UpdateThirdPartyAPIAuthToken(item.CredentialId, responseData?.access_token, responseData != null ? responseData.expiresIn : 0, responseData?.error?.errorCode, responseData?.error?.errorMessage);
                    }
                }
            }
            return true;

        }

        public DataTable GetMinConfidenceSettingsListPaging(bool IsGlobal, string LOBTag)
        {
            return rep.GetMinConfidenceSettingsListPaging(IsGlobal, LOBTag);
        }
        public List<ThirdPartyAPICredentialsEntity> GetMinConfidenceSettingsListPage(bool IsGlobal, string LOBTag)
        {
            return rep.GetMinConfidenceSettingsListPage(IsGlobal, LOBTag);
        }
        public void InsertUpdateMinConfidenceSettings(int Id, int MinConfidenceCode, int MaxCandidateQty, string Tag, int CredentialId, bool IsGlobal)
        {
            rep.InsertUpdateMinConfidenceSettings(Id, MinConfidenceCode, MaxCandidateQty, Tag, CredentialId, IsGlobal);
        }
        public ThirdPartyAPICredentialsEntity GetMinConfidenceSettingsById(int MinCCId)
        {
            return rep.GetMinConfidenceSettingsById(MinCCId);
        }
        public void DeleteMinConfidenceSettings(int Id)
        {
            rep.DeleteMinConfidenceSettings(Id);
        }

        public List<UXDefaultCredentialsEntity> GetUXDefaultCredentials()
        {
            return rep.GetUXDefaultCredentials();
        }
        public string UpdateUXDefaultCredentials(string Code, string CredentialId, int UserId)
        {
            return rep.UpdateUXDefaultCredentials(Code, CredentialId, UserId);
        }
        public List<GlobalThirdPartyAPICredentialsEntity> GetThirdPartyAPICredentialsForUser(int UserId)
        {
            return rep.GetThirdPartyAPICredentialsForUser(UserId);
        }
        public List<ThirdPartyAPIForEnrichmentEntity> GetThirdPartyAPICredentialsForEhrichment(string ThirdPartyProvider)
        {
            return rep.GetThirdPartyAPICredentialsForEhrichment(ThirdPartyProvider);
        }
        public List<ThirdPartyAPIForEnrichmentEntity> GetAPITypeForUXDefaultUXEnrichment()
        {
            return rep.GetAPITypeForUXDefaultUXEnrichment();
        }
        public List<ThirdPartyAPIForEnrichmentEntity> GetUXDefaultUXEnrichments()
        {
            return rep.GetUXDefaultUXEnrichments();
        }
        public string UpdateUXDefaultCredentialsForEnrichment(string EnrichmentType, int DnBAPIId, int CredentialId)
        {
            return rep.UpdateUXDefaultCredentialsForEnrichment(EnrichmentType, DnBAPIId, CredentialId);
        }

        public void UpsertDnBAPIEntitlements(int credId, string dnBAPIId)
        {
            rep.UpsertDnBAPIEntitlements(credId, dnBAPIId);
        }
    }
}
