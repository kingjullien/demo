using SBISCompanyCleanseMatchBusiness.Objects.Business;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System.Collections.Generic;

namespace SBISCompanyCleanseMatchFacade.Objects
{
    public class DataBlockFacade : FacadeParent
    {
        DataBlockBusiness rep;
        public DataBlockFacade(string connectionString) : base(connectionString) { rep = new DataBlockBusiness(Connection); }
        public List<DataBlocksEntity> GetAllDataBlocks()
        {
            return rep.GetAllDataBlocks();
        }
        public DataBlockGroupsEntity GetDataBlockGroupsByGroupId(int DataBlockGroupId)
        {
            return rep.GetDataBlockGroupsByGroupId(DataBlockGroupId);
        }
        public List<DataBlockGroupsEntity> GetDataBlockGroups(int DataBlockGroupId)
        {
            return rep.GetDataBlockGroups(DataBlockGroupId);
        }
        public string DeleteDataBlockGroupsByGroupId(int DataBlockGroupId)
        {
            return rep.DeleteDataBlockGroupsByGroupId(DataBlockGroupId);
        }
        public string UpsertDataBlockGroups(DataBlockGroupsEntity obj)
        {
            return rep.UpsertDataBlockGroups(obj);
        }
    }
}
