//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//namespace SBISCCMWeb.Models
//{
//    public class iResearchInvestigationRequest
//    {
//        public string customerTransactionID { get; set; }
//        public string customerReference { get; set; }
//        public List<RequestorEmail> requestorEmails { get; set; }
//        public string researchRequestType { get; set; }
//        public Organization organization { get; set; }
//        public List<ResearchComment> researchComments { get; set; }
//    }

//        public class RequestorEmail
//        {
//            public string email { get; set; }
//            public int roleType { get; set; }
//        }

//        public class AddressCountry
//        {
//            public string isoAlpha2Code { get; set; }
//        }

//        public class AddressRegion
//        {
//            public string name { get; set; }
//        }

//        public class AddressLocality
//        {
//            public string name { get; set; }
//        }

//        public class StreetAddress
//        {
//            public string line1 { get; set; }
//        }

//        public class PrimaryAddress
//        {
//            public AddressCountry addressCountry { get; set; }
//            public AddressRegion addressRegion { get; set; }
//            public AddressLocality addressLocality { get; set; }
//            public string postalCode { get; set; }
//            public StreetAddress streetAddress { get; set; }
//        }

//        public class Organization
//        {
//            public string primaryName { get; set; }
//            public string countryISOAlpha2Code { get; set; }
//            public PrimaryAddress primaryAddress { get; set; }
//        }

//        public class ResearchComment
//        {
//            public int typeDnBCode { get; set; }
//            public string comment { get; set; }
//        }
//}