using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchBusiness.Objects.Repositories;
using System.Collections.Generic;

namespace SBISCompanyCleanseMatchBusiness.Objects.Business
{
    public class DnbApiSettingBusiness : BusinessParent
    {
        DnbApiSettingRepository rep;
        public DnbApiSettingBusiness(string connectionString) : base(connectionString)
        {
            try
            {
                rep = new DnbApiSettingRepository(StringCipher.Decrypt(Connection, General.passPhrase));
            }
            catch
            {
                rep = new DnbApiSettingRepository(Connection);
            }
        }
        public List<DnbAPIEntity> GetDnBAPIDetailList()
        {
            List<DnbAPIEntity> results = new List<DnbAPIEntity>();
            results = rep.GetDnBAPIDetailList();
            return results;
        }
        public int UpdateDnBAPIFamily(int DnBAPIId, string APIFamily)
        {
            return rep.UpdateDnBAPIFamily(DnBAPIId, APIFamily);
        }
    }
}
