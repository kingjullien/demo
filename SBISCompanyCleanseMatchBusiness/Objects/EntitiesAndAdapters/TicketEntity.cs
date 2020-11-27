using System;
using System.ComponentModel.DataAnnotations;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class TicketEntity
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please select Client Information."), MaxLength(200)]
        public string ClientInformation { get; set; }

        [Required(ErrorMessage = "Please select Application User."), MaxLength(200)]
        public string ApplicationUser { get; set; }
        [MaxLength(64)]
        public string EnteredBy { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Please enter Primary Email Address"), MaxLength(200)]
        public string PrimaryEmailAddress { get; set; }

        [Required(ErrorMessage = "Please fill Contact detail."), MaxLength(25)]
        public string PrimaryContactNumber { get; set; }
        public string IssueDescription { get; set; }
        public string Note { get; set; }
        public string Files { get; set; }
        public DateTime AddDateTime { get; set; }
        [Required(ErrorMessage = "Please select Assigned To.")]
        public int AssignedTo { get; set; }
        public string AssignedToName { get; set; }

        [Required(ErrorMessage = "Please select priority.")]
        public int Priority { get; set; }
        public string PriorityValue { get; set; }

        [Required(ErrorMessage = "Please select Current Status.")]
        public int CurrentStatus { get; set; }

        public string CurrentStatusValue { get; set; }
        public string ResolutionDescription { get; set; }
        public DateTime DateTimeCompleted { get; set; }
        [Required(ErrorMessage = "Please select Ticket Source")]
        public int TicketSource { get; set; }
        public string TicketSourceValue { get; set; }
        [Required(ErrorMessage = "The TicketType field is required.")]
        public int TicketType { get; set; }
        public string TicketTypeValue { get; set; }
        [Required(ErrorMessage = "Please fill Title."), MaxLength(200)]
        public string Title { get; set; }


    }

    public class TicketStatus
    {
        public string Code { get; set; }
        public string Value { get; set; }
        public string TypeCode { get; set; }
    }

}
