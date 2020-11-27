using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchBusiness.Objects.Repositories;
using System.Collections.Generic;

namespace SBISCompanyCleanseMatchBusiness.Objects.Business
{
    public class DataBlockBusiness : BusinessParent
    {
        DataBlockRepository rep;
        public DataBlockBusiness(string connectionString) : base(connectionString) { rep = new DataBlockRepository(Connection); }

        public List<DataBlocksEntity> GetAllDataBlocks()
        {
            return rep.GetAllDataBlocks();
        }
        public List<DataBlockGroupsEntity> GetDataBlockGroups(int DataBlockGroupId)
        {
            return rep.GetDataBlockGroups(DataBlockGroupId);
        }
        public DataBlockGroupsEntity GetDataBlockGroupsByGroupId(int DataBlockGroupId)
        {
            return rep.GetDataBlockGroupsByGroupId(DataBlockGroupId);
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
