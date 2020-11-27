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
    public class ImportFileConfigurationFacade : FacadeParent
    {
        ImportFileConfigurationBusiness rep;
        public ImportFileConfigurationFacade(string connectionString) : base(connectionString) { rep = new ImportFileConfigurationBusiness(Connection); }
        public List<ImportFileConfigurationEntity> GetImportFileConfiguration(int? Id)
        {
            return rep.GetImportFileConfiguration(Id);
        }
        public string InsertImportFileConfiguration(ImportFileConfigurationEntity objImportFileConfiguration)
        {
            return rep.InsertImportFileConfiguration(objImportFileConfiguration);
        }
        public void DeleteImportFileConfiguration(int id)
        {
            rep.DeleteImportFileConfiguration(id);
        }
    }
}
