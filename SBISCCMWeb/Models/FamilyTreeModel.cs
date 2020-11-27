using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SBISCCMWeb.Models
{
    public class FamilyTreeDetailModel

    {
        public string FamilyTreeId { get; set; }
        public string FamilyTreeDetailId { get; set; }
        public string DetailId { get; set; }
        public string NodeName { get; set; }
        public string NodeDisplayDetail { get; set; }
        public string NodeType { get; set; }
        public int cntChildren { get; set; }
        public string ParentFamilyTreeDetailId { get; set; }
        public string ParentNodeFamilyTreeDeailId { get; set; }

        public List<FamilyTreeDetailModel> Children { get; set; }
    }
    public class FamilyTreeModel
    {
        public string FamilyTreeId { get; set; }
        public string FamilyTreeName { get; set; }
        public string FamilyTreeType { get; set; }
        public bool Editable { get; set; }
        public bool LockForEdit { get; set; }
        public string AlternateId { get; set; }
        public int LastModifiedUserId { get; set; }
        public DateTime LastRefreshedDate { get; set; }
        public string LastModifiedUserName { get; set; }

    }

    public class FamilyTreeParentModel
    {
        public SelectList lstFamilyTreeType { get; set; }
        public SelectList lstFamilyTree { get; set; }
        public List<FamilyTreeDetailModel> lstMenu { get; set; }
        public string FamilyTreeType { get; set; }
        public int FamilyTreeId { get; set; }
        public FamilyTreeModel FamilyTreeDetails { get; set; }
    }

    public class SideBySideModel
    {
        public FamilyTreeParentModel LeftView { get; set; }
        public FamilyTreeParentModel RightView { get; set; }
    }

    public class DeleteFamilyTreeNode
    {
        public int SourceFamilyTreeId { get; set; }
        public int SourceFamilyTreeDetailId { get; set; }
        public int UserId { get; set; }
    }
}