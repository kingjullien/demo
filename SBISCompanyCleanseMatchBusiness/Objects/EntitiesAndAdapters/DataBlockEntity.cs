namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class DataBlocksEntity
    {
        public int DataBlockId { get; set; }
        public string DataBlockName { get; set; }
        public int Level { get; set; }
        public int Version { get; set; }
        public string ProductCode { get; set; }
    }
    public class DataBlockGroupsEntity
    {
        public int DataBlockGroupId { get; set; }
        public string DataBlockGroupName { get; set; }
        public string CustomerReference { get; set; }
        public string DataBlocksIds { get; set; }
        public string DataBlocks { get; set; }
        public string APIURL { get; set; }
        public string TradeUp { get; set; }
    }
}
