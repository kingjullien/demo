using SBISCompanyCleanseMatchBusiness.Objects.Business;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBISCompanyCleanseMatchFacade.Objects
{
    public class MonitoringFacade : FacadeParent
    {
        public List<MonitoringProductEntity> GetProductData()
        {
            MonitoringBusiness rep = new MonitoringBusiness();
            return rep.GetProductData();
        }
        public List<MonitoringProductElementEntity> GetProductElementData(int productID)
        {
            MonitoringBusiness rep = new MonitoringBusiness();
            return rep.GetProductElementData(productID);
        }
    }
}
