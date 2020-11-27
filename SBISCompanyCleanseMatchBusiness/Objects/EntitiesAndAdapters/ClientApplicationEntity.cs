using System.Collections.Generic;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class ClientApplicationEntity
    {

        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public List<ClientApplicationListEntity> clientApplicationList { get; set; }
    }
}
