namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class MDPCodeEntity
    {
        private bool _FullMatchUnResolved = false;
        private string _MatchCode = "";
        public string MatchField { get; set; }
        public string MatchCode
        {
            get
            {
                return _MatchCode;
            }

            set
            {
                _MatchCode = value;
                if (_MatchCode == "-96-96-96-96-96-")
                {
                    _FullMatchUnResolved = true;
                }
            }
        }
        public string MatchType { get; set; }
        public string MatchDescription { get; set; }

        public string MDPCode { get; set; }
        public string MDPValue { get; set; }

        /// <summary>
        /// Name Digits 1-2
        /// </summary>
        public string MDP_Name
        {
            get
            {
                string result = "";
                if (_FullMatchUnResolved)
                {
                    result = "96";
                }
                else
                {
                    result = MatchCode.Substring(0, 2);
                }

                return result;
            }
        }
        /// <summary>
        /// PhysicalAddresDigits 3-10
        /// </summary>
        public string MDP_PhysicalAddress
        {
            get
            {
                string result = "";
                if (_FullMatchUnResolved)
                {
                    result = "96";
                }
                else
                {
                    result = MatchCode.Substring(3, 11);
                }

                return result;

            }
        }
        /// <summary>
        /// MailAddress Digits 11-12
        /// </summary>
        public string MDP_MailAddress
        {
            get
            {
                string result = "";
                if (_FullMatchUnResolved)
                {
                    result = "96";
                }
                else
                {
                    result = MatchCode.Substring(12, 2);
                }

                return result;

            }
        }
        /// <summary>
        /// Phone Digits 13-14 
        /// </summary>
        public string MDP_Phone
        {
            get
            {
                string result = "";
                if (_FullMatchUnResolved)
                {
                    result = "96";
                }
                else
                {
                    result = MatchCode.Substring(16, 2);
                }

                return result;

            }
        }
    }
}
