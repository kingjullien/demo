using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Utility
{
    public class CreatePreferenceResponse
    {
        public string @ServiceVersionNumber { get; set; }
        public TransactionDetail TransactionDetail { get; set; }
        public TransactionResult TransactionResult { get; set; }
    }

    public class UserPreferenceReponse
    {
        public CreatePreferenceResponse CreatePreferenceResponse { get; set; }
    }
}