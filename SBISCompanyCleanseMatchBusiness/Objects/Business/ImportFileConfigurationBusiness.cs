using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchBusiness.Objects.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBISCompanyCleanseMatchBusiness.Objects.Business
{
    public class ImportFileConfigurationBusiness : BusinessParent
    {
        ImportFileConfigurationRepository rep;
        public ImportFileConfigurationBusiness(string connectionString) : base(connectionString) { rep = new ImportFileConfigurationRepository(Connection); }
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
