using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class DataBlocksAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<DataBlocksEntity> Adapt(DataTable dt)
        {
            List<DataBlocksEntity> results = new List<DataBlocksEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                DataBlocksEntity dataBlocks = new DataBlocksEntity();
                dataBlocks = AdaptItem(rw);
                results.Add(dataBlocks);
            }
            return results;
        }
        public DataBlocksEntity AdaptItem(DataRow rw)
        {
            DataBlocksEntity result = new DataBlocksEntity();

            if (rw.Table.Columns["DataBlockId"] != null)
            {
                result.DataBlockId = SafeHelper.GetSafeint(rw["DataBlockId"]);
            }

            if (rw.Table.Columns["DataBlockName"] != null)
            {
                result.DataBlockName = SafeHelper.GetSafestring(rw["DataBlockName"]);
            }

            if (rw.Table.Columns["Level"] != null)
            {
                result.Level = SafeHelper.GetSafeint(rw["Level"]);
            }

            if (rw.Table.Columns["Version"] != null)
            {
                result.Version = SafeHelper.GetSafeint(rw["Version"]);
            }

            if (rw.Table.Columns["ProductCode"] != null)
            {
                result.ProductCode = SafeHelper.GetSafestring(rw["ProductCode"]);
            }

            return result;
        }
    }
    public class DataBlockGroupsAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<DataBlockGroupsEntity> Adapt(DataTable dt)
        {
            List<DataBlockGroupsEntity> results = new List<DataBlockGroupsEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                DataBlockGroupsEntity dataBlockGroups = new DataBlockGroupsEntity();
                dataBlockGroups = AdaptItem(rw);
                results.Add(dataBlockGroups);
            }
            return results;
        }
        public DataBlockGroupsEntity AdaptItem(DataRow rw)
        {
            DataBlockGroupsEntity result = new DataBlockGroupsEntity();
            if (rw.Table.Columns["DataBlockGroupId"] != null)
            {
                result.DataBlockGroupId = SafeHelper.GetSafeint(rw["DataBlockGroupId"]);
            }

            if (rw.Table.Columns["DataBlockGroupName"] != null)
            {
                result.DataBlockGroupName = SafeHelper.GetSafestring(rw["DataBlockGroupName"]);
            }

            if (rw.Table.Columns["CustomerReference"] != null)
            {
                result.CustomerReference = SafeHelper.GetSafestring(rw["CustomerReference"]);
            }

            if (rw.Table.Columns["DataBlocksIds"] != null)
            {
                result.DataBlocksIds = SafeHelper.GetSafestring(rw["DataBlocksIds"]);
            }

            if (rw.Table.Columns["DataBlocks"] != null)
            {
                result.DataBlocks = SafeHelper.GetSafestring(rw["DataBlocks"]);
            }

            if (rw.Table.Columns["APIURL"] != null)
            {
                result.APIURL = SafeHelper.GetSafestring(rw["APIURL"]);
            }

            if (rw.Table.Columns["TradeUp"] != null)
            {
                result.TradeUp = SafeHelper.GetSafestring(rw["TradeUp"]);
            }

            return result;
        }
    }
}
