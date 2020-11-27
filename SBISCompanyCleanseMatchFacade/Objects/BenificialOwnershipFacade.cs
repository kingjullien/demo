using SBISCompanyCleanseMatchBusiness.Objects.Business;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBISCompanyCleanseMatchFacade.Objects
{
    public class BenificialOwnershipFacade : FacadeParent
    {
        BenificialOwnershipBusiness rep;
        public BenificialOwnershipFacade(string connectionString) : base(connectionString) { rep = new BenificialOwnershipBusiness(Connection); }

        public UXBeneficialOwnershipURLEntity UXGetBeneficialOwnershipURL(string duns,string country)
        {
            return rep.UXGetBeneficialOwnershipURL(duns,country);
        }
        public DataSet PreviewBenificialOwnershipData(string duns)
        {
            return rep.PreviewBenificialOwnershipData(duns);
        }

        public string InsertScreenQueueAndResponseJSON(string source,int userId,int credId,string requestUrl,string searchJSON,string resultsJSON)
        {
            return rep.InsertScreenQueueAndResponseJSON(source,userId,credId,requestUrl,searchJSON,resultsJSON);
        }

        public List<ScreenResponseEntity> GetScreenResponse(string alternateId,string beneficiaryType)
        {
            return rep.GetScreenResponse(alternateId, beneficiaryType);
        }
    }
}
