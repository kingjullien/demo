using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchBusiness.Objects.Repositories;
using System.Collections.Generic;

namespace SBISCompanyCleanseMatchBusiness.Objects.Business
{
    public class UserPreferenceBusiness : BusinessParent
    {
        UserPreferenceRepository rep;
        public UserPreferenceBusiness(string connectionString) : base(connectionString) { rep = new UserPreferenceRepository(Connection); }

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
