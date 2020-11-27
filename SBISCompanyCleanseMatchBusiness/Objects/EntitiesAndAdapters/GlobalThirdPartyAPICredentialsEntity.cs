using System;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    [Serializable]
    public class GlobalThirdPartyAPICredentialsEntity
    {
        public string ThirdPartyProvider { get; set; }
        public string Code { get; set; }
        public string APICredential { get; set; }
        public string AuthToken { get; set; }
        public string APIType { get; set; }
    }
}
