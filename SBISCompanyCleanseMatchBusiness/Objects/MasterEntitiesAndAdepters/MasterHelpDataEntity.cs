using System;
using System.ComponentModel.DataAnnotations;


namespace SBISCompanyCleanseMatchBusiness.Objects.MasterEntitiesAndAdepters
{
    public class MasterHelpDataEntity
    {
        public int HelpDataId { get; set; }

        [Required(ErrorMessage = "Please enter Help Data")]
        [Display(Name = "Help Data")]
        public string Helpdata
        {
            get;
            set;
        }
        [Display(Name = "Notes")]
        public string Notes
        {
            get;
            set;
        }
        public DateTime DateAdded
        {
            get;
            set;
        }
        public bool Active
        {
            get;
            set;
        }
        public int CreatedByUserId
        {
            get;
            set;
        }
        public int UpdatedByUserId
        {
            get;
            set;
        }
        public DateTime DateUpdated
        {
            get;
            set;
        }
        public string getEnumData
        {
            get;
            set;
        }

        #region Section Master Property

        [Required(ErrorMessage = "Please select Section Name")]
        public int SectionMasterId { get; set; }
        [Display(Name = "Page Name")]
        public string PageName { get; set; }
        [Display(Name = "Section Name")]
        public string SectionName { get; set; }

        #endregion
    }

}
