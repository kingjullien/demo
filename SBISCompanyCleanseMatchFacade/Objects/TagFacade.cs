using SBISCompanyCleanseMatchBusiness.Objects.Business;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchFacade.Objects
{
    public class TagFacade : FacadeParent
    {
        TagBusiness rep;
        public TagFacade(string connectionString) : base(connectionString) { rep = new TagBusiness(Connection); }

        public List<TagsEntity> GetAllTags(string LOBTag)
        {
            return rep.GetAllTags(LOBTag);
        }
        public List<TagsEntity> GetAutoAcceptanceFilterTags(string LOBTag)
        {
            return rep.GetAutoAcceptanceFilterTags(LOBTag);
        }
        public void InsertTags(TagsEntity objTags, int UserId)
        {
            rep.InsertTags(objTags, UserId);
        }

        public void UpdateTags(TagsEntity objTags)
        {
            rep.UpdateTags(objTags);
        }
        public DataTable GetTagTypeCode()
        {
            return rep.GetTagTypeCode();
        }
        public string DeleteTag(int TagId, int UserId)
        {
            return rep.DeleteTag(TagId, UserId);
        }
        public List<TagsEntity> GetAllTagsListPaging()
        {
            return rep.GetAllTagsListPaging();
        }
        public List<TagsEntity> GetTagByTypeCode(string TagTypeCode)
        {
            return rep.GetTagByTypeCode(TagTypeCode);
        }
        public DataTable GetTagsByTypeCode(string TagTypeCode)
        {
            return rep.GetTagsByTypeCode(TagTypeCode);
        }
        public List<TagsEntity> GetExportDataTags(string LOBTag, string SecurityTags, int UserId)
        {
            return rep.GetExportDataTags(LOBTag, SecurityTags, UserId);
        }
        public DataTable GetExportDataTag(string LOBTag, string SecurityTags, int UserId)
        {
            return rep.GetExportDataTag(LOBTag, SecurityTags, UserId);
        }

        public TagsEntity GetTagByTagId(int TagId)
        {
            return rep.GetTagByTagId(TagId);
        }
        public List<TagsEntity> GetExportedDataTags(string LOBTag, string SecurityTags, int UserId)
        {
            return rep.GetExportedDataTags(LOBTag, SecurityTags, UserId);
        }
        public List<TagsEntity> GetAllTagsForUser(string LOBTag, int UserId, bool FilterNoTag)
        {
            return rep.GetAllTagsForUser(LOBTag, UserId, FilterNoTag);
        }
        public DataTable GetAllTagsForUserInFilter(string LOBTag, int UserId, bool FilterNoTag)
        {
            return rep.GetAllTagsForUserInFilter(LOBTag, UserId, FilterNoTag);
        }
    }
}
