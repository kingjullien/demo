using SBISCompanyCleanseMatchBusiness.Objects.Business;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System.Collections.Generic;

namespace SBISCompanyCleanseMatchFacade.Objects
{
    public class ImportJobDataFacade : FacadeParent
    {
        ImportJobDataBusiness rep;
        public ImportJobDataFacade(string connectionString) : base(connectionString) { rep = new ImportJobDataBusiness(Connection); }

        public List<ImportJobDataEntity> GetNewFileImportRequestByUserID(int? UserId, string Section)
        {
            return rep.GetNewFileImportRequestByUserID(UserId, Section);
        }

        public List<ImportFileTemplates> GetAllImportFileTemplates()
        {
            return rep.GetAllImportFileTemplates();
        }

        public bool InsertNewFileImportRequest(ImportJobDataEntity importJobData)
        {
            return rep.InsertNewFileImportRequest(importJobData);
        }

        public string UpsertImportFileTemplates(ImportFileTemplates fileTemplates)
        {
            return rep.UpsertImportFileTemplates(fileTemplates);
        }
        public List<DashboardBackgroundProcessStatsEntity> DashboardV2GetBackgroundProcessStats()
        {
            return rep.DashboardV2GetBackgroundProcessStats();
        }

        public void DeleteImportFileTemplateByTemplateId(int TemplateId, string TemplateName, int UserId)
        {
            rep.DeleteImportFileTemplateByTemplateId(TemplateId, TemplateName, UserId);
        }


        public ImportFileTemplates GetImportFileTemplateByTemplateId(int templateId)
        {
            return rep.GetImportFileTemplateByTemplateId(templateId);
        }
    }
}
