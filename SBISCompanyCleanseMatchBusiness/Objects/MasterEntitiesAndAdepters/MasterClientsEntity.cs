using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SBISCompanyCleanseMatchBusiness.Objects.MasterEntitiesAndAdepters
{
    public class MasterClientsEntity
    {
        //private int _clientId;
        //public int ClientId
        //{
        //    get { return _clientId; }
        //    set { _clientId = value; }
        //}

        //private string _clientGUID;
        //public string ClientGUID
        //{
        //    get { return _clientGUID; }
        //    set { _clientGUID = value; }
        //}

        //private string _clientName;
        //public string ClientName
        //{
        //    get { return _clientName; }
        //    set { _clientName = value; }
        //}

        //private string _primaryClientDUNSNumber;
        //public string PrimaryClientDUNSNumber
        //{
        //    get { return _primaryClientDUNSNumber; }
        //    set { _primaryClientDUNSNumber = value; }
        //}

        //private string _primaryContactName;
        //public string PrimaryContactName
        //{
        //    get { return _primaryContactName; }
        //    set { _primaryContactName = value; }
        //}

        //private string _primaryEamilAddress;
        //public string PrimaryEamilAddress
        //{
        //    get { return _primaryEamilAddress; }
        //    set { _primaryEamilAddress = value; }
        //}

        //private string _primaryContactPhone;
        //public string PrimaryContactPhone
        //{
        //    get { return _primaryContactPhone; }
        //    set { _primaryContactPhone = value; }
        //}

        //private string _secondaryContactName;
        //public string SecondaryContactName
        //{
        //    get { return _secondaryContactName; }
        //    set { _secondaryContactName = value; }
        //}

        //private string _secondaryEamilAddress;
        //public string SecondaryEamilAddress
        //{
        //    get { return _secondaryEamilAddress; }
        //    set { _secondaryEamilAddress = value; }
        //}

        //private string _secondaryContactPhone;
        //public string SecondaryContactPhone
        //{
        //    get { return _secondaryContactPhone; }
        //    set { _secondaryContactPhone = value; }
        //}

        //private DateTime _dateAdded;
        //public DateTime DateAdded
        //{
        //    get { return _dateAdded; }
        //    set { _dateAdded = value; }
        //}

        //private bool _active;
        //public bool Active
        //{
        //    get { return _active; }
        //    set { _active = value; }
        //}

        //private int _createdByUserId;
        //public int CreatedByUserId
        //{
        //    get { return _createdByUserId; }
        //    set { _createdByUserId = value; }
        //}

        //private string _notes;
        //public string Notes
        //{
        //    get { return _notes; }
        //    set { _notes = value; }
        //}

        //private int? _updatedByUserId;
        //public int? UpdatedByUserId
        //{
        //    get { return _updatedByUserId; }
        //    set { _updatedByUserId = value; }
        //}

        //private DateTime? _dateUpdated;
        //public DateTime? DateUpdated
        //{
        //    get { return _dateUpdated; }
        //    set { _dateUpdated = value; }
        //}


        public string _ClientGUID, _PrimaryClientDUNSNumber;

        public int ClientId { get; set; }
        public int ApplicationCount { get; set; }
        public string SrcRecordId { get; set; }
        public List<MatchEntity> Matches { get; set; }
        public string ClientGUID
        {
            get;
            set;
        }
        [Display(Name = "Client DUNS Number")]
        public string PrimaryClientDUNSNumber
        {
            get { return _PrimaryClientDUNSNumber; }
            set { _PrimaryClientDUNSNumber = value; }
        }

        [Required(ErrorMessage = "Please enter Primary Contact Name")]
        [Display(Name = "Primary Contact Name")]
        public string PrimaryContactName
        {
            get;
            set;
        }
        [EmailAddress]
        [Required(ErrorMessage = "Please enter Primary Email Address")]
        [Display(Name = "Primary Email Address")]
        public string PrimaryEamilAddress
        {
            get;
            set;
        }
        [Required(ErrorMessage = "Please enter Primary Contact Number")]
        [Display(Name = "Primary Contact Phone")]
        public string PrimaryContactPhone
        {
            get;
            set;
        }
        [Display(Name = "Secondary Contact Name")]
        public string SecondaryContactName
        {
            get;
            set;
        }
        [EmailAddress]
        [Display(Name = "Secondary Email Address")]
        public string SecondaryEamilAddress
        {
            get;
            set;
        }
        [Display(Name = "Secondary Contact Phone")]
        public string SecondaryContactPhone
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
        public string Notes
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
        public string ClientLogo
        {
            get;
            set;
        }

        public string _ClientName;
        [Required(ErrorMessage = "Please enter ClientName")]
        [Display(Name = "Client Name")]
        public string ClientName
        {
            get { return _ClientName; }
            set { _ClientName = value; }
        }

    }
}
