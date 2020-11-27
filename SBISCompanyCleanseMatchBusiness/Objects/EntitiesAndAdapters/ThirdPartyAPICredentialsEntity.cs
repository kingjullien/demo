using System;
using System.ComponentModel.DataAnnotations;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class ThirdPartyAPICredentialsEntity
    {
        public int CredentialId { get; set; }
        [Required]
        public string CredentialName { get; set; }
        public string ThirdPartyProviderCode { get; set; }
        public string ThirdPartyProvider { get; set; }
        [Required]
        public string APICredential { get; set; }
        [Required]
        public string APIPassword { get; set; }
        public string GetAuthTokenURL { get; set; }
        public string AuthToken { get; set; }
        public DateTime AuthTokenExpirationDate { get; set; }
        public string APIType { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorDescription { get; set; }
        public DateTime ErrorDateTime { get; set; }

        public int MinConfidenceCode { get; set; }
        public int MaxCandidateQty { get; set; }
        public int Id { get; set; }
        [Required]
        public string Tag { get; set; }
    }
}
