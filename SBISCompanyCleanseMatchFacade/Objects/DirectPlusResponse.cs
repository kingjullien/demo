namespace SBISCompanyCleanseMatchFacade.Objects
{
    public class DirectPlusResponse
    {
        public string access_token { get; set; }
        public int expiresIn { get; set; }
        public ErrorDirectPlus error { get; set; }

    }
    public class ErrorDirectPlus
    {
        public string errorMessage { get; set; }
        public string errorCode { get; set; }
    }
}
