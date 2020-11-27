using SBISCompanyCleanseMatchBusiness.Objects.Business;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System.Collections.Generic;

namespace SBISCompanyCleanseMatchFacade.Objects
{
    public class ColumnMappingFacade : FacadeParent
    {
        ColumnMappingBusiness rep;
        public ColumnMappingFacade(string connectionString) : base(connectionString) { rep = new ColumnMappingBusiness(Connection); }
        #region Command Upload
        public string InsertUpdateCommandUploadMapping(CommandUploadMappingEntity objCommandMapping)
        {
            return rep.InsertUpdateCommandUploadMapping(objCommandMapping);
        }
        public List<CommandUploadMappingEntity> GetCommandMapping()
        {
            return rep.GetCommandMapping();
        }
        public void DeleteCommandMapping(int Id)
        {
            rep.DeleteCommandMapping(Id);
        }
        public CommandUploadMappingEntity GetCommandMappingById(int Id)
        {
            return rep.GetCommandMappingById(Id);
        }

        #endregion
        #region Command Download
        public string InsertUpdateCommandDownloadMapping(CommandDownloadMappingEntity objCommandDownloadMapping)
        {
            return rep.InsertUpdateCommandDownloadMapping(objCommandDownloadMapping);
        }
        public List<CommandDownloadMappingEntity> GetCommandDownloadMapping()
        {
            return rep.GetCommandDownloadMapping();
        }
        public void DeleteCommandDownloadMapping(int Id)
        {
            rep.DeleteCommandDownloadMapping(Id);
        }
        public CommandDownloadMappingEntity GetCommandDownloadMappingById(int Id)
        {
            return rep.GetCommandDownloadMappingById(Id);
        }
        #endregion





        #region "OI ExportBaseTableMetadata"

        #endregion
    }
}
