using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchBusiness.Objects.Repositories;
using System.Collections.Generic;

namespace SBISCompanyCleanseMatchBusiness.Objects.Business
{
    public class ColumnMappingBusiness : BusinessParent
    {
        ColumnMappingRepository rep;
        public ColumnMappingBusiness(string connectionString) : base(connectionString) { rep = new ColumnMappingRepository(Connection); }
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
    }
}
