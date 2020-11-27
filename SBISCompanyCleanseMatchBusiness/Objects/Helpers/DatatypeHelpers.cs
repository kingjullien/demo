using System;

namespace SBISCompanyCleanseMatchBusiness.Objects.Helpers
{
    class DatatypeHelpers
    {
        public int GetSafeint(object ValueIn)
        {
            int result = 0;
            if (ValueIn != DBNull.Value)
            {
                result = Convert.ToInt32(ValueIn);
            }
            return result;
        }

        public double GetSafedouble(object ValueIn)
        {
            double result = 0;
            if (ValueIn != DBNull.Value)
            {
                result = Convert.ToDouble(ValueIn);
            }
            return result;
        }

        public string GetSafestring(object ValueIn)
        {
            string result = "";
            if (ValueIn != DBNull.Value)
            {
                result = Convert.ToString(ValueIn);
            }
            return result;
        }

        public DateTime GetSafeDateTime(object ValueIn)
        {
            DateTime result = DateTime.Now;
            if (ValueIn != DBNull.Value)
            {
                result = Convert.ToDateTime(ValueIn);
            }
            return result;
        }

        public bool GetSafebool(object ValueIn)
        {
            bool result = false;
            if (ValueIn != DBNull.Value)
            {
                result = Convert.ToBoolean(ValueIn);
            }
            return result;
        }

        public bool? GetSafeboolIfNull(object ValueIn)
        {
            bool? result = false;
            if (ValueIn != DBNull.Value)
            {
                result = Convert.ToBoolean(ValueIn);
            }
            else
            {
                result = null;
            }
            return result;
        }

        public DateTime? GetSafeDateTimeIfNull(object ValueIn)
        {
            DateTime? result = DateTime.Now;
            if (ValueIn != DBNull.Value)
            {
                result = Convert.ToDateTime(ValueIn);
            }
            else
            {
                result = null;
            }
            return result;
        }

    }
}
