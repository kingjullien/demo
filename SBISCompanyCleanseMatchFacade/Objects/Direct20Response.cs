using System;
namespace SBISCompanyCleanseMatchFacade.Objects
{
    public class Direct20Response
    {
        public TransactionDetail TransactionDetail { get; set; }
        public TransactionResult TransactionResult { get; set; }
        public AuthenticationDetail AuthenticationDetail { get; set; }
    }
    public class TransactionDetail
    {
        public string ServiceTransactionID { get; set; }
        public DateTime TransactionTimestamp { get; set; }
        public string ApplicationTransactionID { get; set; }
    }
    public class TransactionResult
    {
        public string SeverityText { get; set; }
        public string ResultID { get; set; }
        public string ResultText { get; set; }
        public ResultMessage ResultMessage { get; set; }
    }
    public class AuthenticationDetail
    {
        public string Token { get; set; }
    }
    public class ResultMessage
    {
        public string ResultDescription { get; set; }
    }
}