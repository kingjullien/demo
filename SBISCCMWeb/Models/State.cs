using System;
using System.Collections.Generic;
using System.Text;


namespace SBISCCMWeb.Models
{
    public static class State
    {
        // Make List of the State of the Specific USA
        public static List<TemplateState> GetState(string countryCode)
        {
            List<TemplateState> lstState = new List<TemplateState>();
            if (countryCode.ToUpper().Equals("US"))
            {
                lstState.Add(new TemplateState() { Text = "Alabama", Value = "AL" });
                lstState.Add(new TemplateState() { Text = "Alaska", Value = "Ak" });
                lstState.Add(new TemplateState() { Text = "Arizona", Value = "AZ" });
                lstState.Add(new TemplateState() { Text = "Arkansas", Value = "AR" });
                lstState.Add(new TemplateState() { Text = "California", Value = "CA" });
                lstState.Add(new TemplateState() { Text = "Canal Zone", Value = "CZ" });
                lstState.Add(new TemplateState() { Text = "Colorado", Value = "CO" });
                lstState.Add(new TemplateState() { Text = "Connecticut", Value = "CT" });
                lstState.Add(new TemplateState() { Text = "Delaware", Value = "DE" });
                lstState.Add(new TemplateState() { Text = "District of Columbia", Value = "DC" });
                lstState.Add(new TemplateState() { Text = "Florida", Value = "FL" });
                lstState.Add(new TemplateState() { Text = "Georgia", Value = "GA" });
                lstState.Add(new TemplateState() { Text = "Guam", Value = "GU" });
                lstState.Add(new TemplateState() { Text = "Hawaii", Value = "HI" });
                lstState.Add(new TemplateState() { Text = "Idaho", Value = "ID" });
                lstState.Add(new TemplateState() { Text = "Illinois", Value = "IL" });
                lstState.Add(new TemplateState() { Text = "Indiana", Value = "IN" });
                lstState.Add(new TemplateState() { Text = "Iowa", Value = "IA" });
                lstState.Add(new TemplateState() { Text = "Kansas", Value = "KS" });
                lstState.Add(new TemplateState() { Text = "Kentucky", Value = "KY" });
                lstState.Add(new TemplateState() { Text = "Louisiana", Value = "LA" });
                lstState.Add(new TemplateState() { Text = "Maine", Value = "ME" });
                lstState.Add(new TemplateState() { Text = "Maryland", Value = "MD" });
                lstState.Add(new TemplateState() { Text = "Massachusetts", Value = "MA" });
                lstState.Add(new TemplateState() { Text = "Michigan", Value = "MI" });
                lstState.Add(new TemplateState() { Text = "Minnesota", Value = "MN" });
                lstState.Add(new TemplateState() { Text = "Mississippi", Value = "MS" });
                lstState.Add(new TemplateState() { Text = "Missouri", Value = "MO" });
                lstState.Add(new TemplateState() { Text = "Montana", Value = "MT" });
                lstState.Add(new TemplateState() { Text = "Nebraska", Value = "NE" });
                lstState.Add(new TemplateState() { Text = "Nevada", Value = "NV" });
                lstState.Add(new TemplateState() { Text = "New Hampshire", Value = "NH" });
                lstState.Add(new TemplateState() { Text = "New Jersey", Value = "NJ" });
                lstState.Add(new TemplateState() { Text = "New Mexico", Value = "NM" });
                lstState.Add(new TemplateState() { Text = "New York", Value = "NY" });
                lstState.Add(new TemplateState() { Text = "North Carolina", Value = "NC" });
                lstState.Add(new TemplateState() { Text = "North Dakota", Value = "ND" });
                lstState.Add(new TemplateState() { Text = "Ohio", Value = "OH" });
                lstState.Add(new TemplateState() { Text = "Oklahoma", Value = "OK" });
                lstState.Add(new TemplateState() { Text = "Oregon", Value = "OR" });
                lstState.Add(new TemplateState() { Text = "Pennsylvania", Value = "PA" });
                lstState.Add(new TemplateState() { Text = "Puerto Rico", Value = "PR" });
                lstState.Add(new TemplateState() { Text = "Rhode Island", Value = "RI" });
                lstState.Add(new TemplateState() { Text = "South Carolina", Value = "SC" });
                lstState.Add(new TemplateState() { Text = "South Dakota", Value = "SD" });
                lstState.Add(new TemplateState() { Text = "Tennessee", Value = "TN" });
                lstState.Add(new TemplateState() { Text = "Texas", Value = "TX" });
                lstState.Add(new TemplateState() { Text = "Utah", Value = "UT" });
                lstState.Add(new TemplateState() { Text = "Vermont", Value = "VT" });
                lstState.Add(new TemplateState() { Text = "Virgin Islands", Value = "VI" });
                lstState.Add(new TemplateState() { Text = "Virginia", Value = "VA" });
                lstState.Add(new TemplateState() { Text = "Washington", Value = "WA" });
                lstState.Add(new TemplateState() { Text = "West Virginia", Value = "WV" });
                lstState.Add(new TemplateState() { Text = "Wisconsin", Value = "WI" });
                lstState.Add(new TemplateState() { Text = "Wyoming", Value = "WY" });

            }
            else if (countryCode.ToUpper().Equals("CA"))
            {
                lstState.Add(new TemplateState() { Text = "Alberta", Value = "AB" });
                lstState.Add(new TemplateState() { Text = "Labrador", Value = "LB" });
                lstState.Add(new TemplateState() { Text = "New Brunswick", Value = "NB" });
                lstState.Add(new TemplateState() { Text = "Nova Scotia", Value = "NS" });
                lstState.Add(new TemplateState() { Text = "North West Terr.", Value = "NW" });
                lstState.Add(new TemplateState() { Text = "Prince Edward Is.", Value = "PE" });
                lstState.Add(new TemplateState() { Text = "Saskatchewen", Value = "SK" });
                lstState.Add(new TemplateState() { Text = "British Columbia", Value = "BC" });
                lstState.Add(new TemplateState() { Text = "Manitoba", Value = "MB" });
                lstState.Add(new TemplateState() { Text = "Newfoundland", Value = "NF" });
                lstState.Add(new TemplateState() { Text = "Nunavut", Value = "NU" });
                lstState.Add(new TemplateState() { Text = "Ontario", Value = "ON" });
                lstState.Add(new TemplateState() { Text = "Quebec", Value = "QC" });
                lstState.Add(new TemplateState() { Text = "Yukon", Value = "YU" });
            }
            return lstState;
        }
    }
    public class TemplateState
    {
        public string Text { get; set; }

        public string Value { get; set; }
    }
    public class ViewBagValue
    {
        public string Text { get; set; }
    }
}
