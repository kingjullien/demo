using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchBusiness.Objects.Repositories;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.Business
{
    public class ThirdPartyAPICredentialsBusiness : BusinessParent
    {
        ThirdPartyAPICredentialsRepository rep;
        public ThirdPartyAPICredentialsBusiness(string connectionString) : base(connectionString) { rep = new ThirdPartyAPICredentialsRepository(Connection); }

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
