using SBISCompanyCleanseMatchBusiness.Objects;
using SBISCompanyCleanseMatchBusiness.Objects.Business;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System.Collections.Generic;

namespace SBISCompanyCleanseMatchFacade.Objects
{
    public class DnbApiSettingFacade : FacadeParent
    {
        DnbApiSettingBusiness rep;
        public DnbApiSettingFacade(string connectionString) : base(connectionString)
        {
            try
            {
                rep = new DnbApiSettingBusiness(StringCipher.Decrypt(Connection, General.passPhrase));
            }
            catch
            {
                rep = new DnbApiSettingBusiness(Connection);
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
