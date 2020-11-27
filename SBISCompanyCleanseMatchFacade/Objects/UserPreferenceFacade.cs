using SBISCompanyCleanseMatchBusiness.Objects.Business;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System.Collections.Generic;

namespace SBISCompanyCleanseMatchFacade.Objects
{
    public class UserPreferenceFacade : FacadeParent
    {
        UserPreferenceBusiness rep;
        public UserPreferenceFacade(string connectionString) : base(connectionString) { rep = new UserPreferenceBusiness(Connection); }

        public int InsertUpdateUserPreference(UserPreferenceEntity obj)
        {
            return rep.InsertUpdateUserPreference(obj);
        }
        public List<UserPreferenceEntity> GetUserPreference(int CredentialId)
        {
            return rep.GetUserPreference(CredentialId);
        }
        public string DeleteUserPreference(int id)
        {
            return rep.DeleteUserPreference(id);
        }
        public UserPreferenceEntity GetUserPreferenceById(int id)
        {
            return rep.GetUserPreferenceById(id);
        }
        public List<UserPreferenceEntity> GetAllUserPreference()
        {
            return rep.GetAllUserPreference();
        }
        public List<UserPreferenceEntity> GetActiveUserPreference()
        {
            return rep.GetActiveUserPreference();
        }
        public bool CheckUserPreferenceUsed(string UserPreference)
        {
            return rep.CheckUserPreferenceUsed(UserPreference);
        }
        public bool CheckUserPreferenceName(int PreferenceID, string UserPreference)
        {
            return rep.CheckUserPreferenceName(PreferenceID, UserPreference);
        }
        public void UpdateUserPreference(UserPreferenceEntity obj)
        {
            rep.UpdateUserPreference(obj);
        }

    }
}
