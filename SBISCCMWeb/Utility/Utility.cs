using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PagedList;
using SBISCCMWeb.Models;
using SBISCCMWeb.Utility.BuildList;
using SBISCCMWeb.Utility.IdentityResolution;
using SBISCCMWeb.Utility.Monitoring;
using SBISCCMWeb.Utility.SearchByDomain;
using SBISCompanyCleanseMatchBusiness.Objects;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchFacade.Objects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace SBISCCMWeb.Utility
{
    public class Utility
    {
        #region Members
        string AuthenticationUrl = "https://direct.dnb.com/rest/Authentication";

        public static string urlseparator
        {
            get
            {
                return Convert.ToString(ConfigurationManager.AppSettings["urlseparator"]); ;
            }
        }
        public static char Colonseparator
        {
            get
            {
                return Convert.ToChar(ConfigurationManager.AppSettings["Colonseparator"]); ;
            }
        }
        public static char Equalseparator
        {
            get
            {
                return Convert.ToChar(ConfigurationManager.AppSettings["Equalseparator"]); ;
            }
        }




        List<DnbMatchModel> objMatches = new List<DnbMatchModel>();

        #endregion //Members

        #region SOAP Config Functions

        #region CompanySoapBinding

        //public static CustomBinding CompanySoapBinding()
        //{
        //    // define custom binding

        //    CustomBinding cb = new CustomBinding();

        //    cb.Name = "CompanySoapBinding";
        //    SecurityBindingElement sbe = SecurityBindingElement.CreateUserNameOverTransportBindingElement();
        //    sbe.DefaultAlgorithmSuite = System.ServiceModel.Security.SecurityAlgorithmSuite.Default;
        //    sbe.IncludeTimestamp = false;
        //    sbe.SetKeyDerivation(false);
        //    sbe.MessageSecurityVersion = System.ServiceModel.MessageSecurityVersion.WSSecurity10WSTrustFebruary2005WSSecureConversationFebruary2005WSSecurityPolicy11BasicSecurityProfile10;
        //    cb.Elements.Add(sbe);
        //    TextMessageEncodingBindingElement tmebe = new TextMessageEncodingBindingElement();
        //    tmebe.MessageVersion = System.ServiceModel.Channels.MessageVersion.Soap11;
        //    cb.Elements.Add(tmebe);
        //    // define httpsTransport elements NOTE: This should be last
        //    HttpsTransportBindingElement htbs = new HttpsTransportBindingElement();
        //    htbs.MaxReceivedMessageSize = 2000000000;
        //    cb.Elements.Add(htbs);
        //    return cb;

        //}
        #endregion //CompanySoapBinding

        #region CompanyEndPoint

        //public static EndpointAddress CompanyEndPoint()
        //{

        //    AddressHeader aHbindingConfiguration = AddressHeader.CreateAddressHeader("bindingConfiguration", "SolidQ", "CompanySoapBinding");
        //    AddressHeader aHbinding = AddressHeader.CreateAddressHeader("binding", "SolidQ", "customBinding");
        //    AddressHeader aHcontract = AddressHeader.CreateAddressHeader("contract", "SolidQ", "DnbAPI20Company.Company");
        //    AddressHeader aHname = AddressHeader.CreateAddressHeader("binding", "SolidQ", "CompanyServiceEndPoint");
        //    EndpointAddress endPoint = new EndpointAddress("https://maxcvservices.dnb.com/Company/V4.0");
        //    EndpointAddressBuilder builder = new EndpointAddressBuilder(endPoint);
        //    builder.Headers.Add(aHbindingConfiguration);
        //    builder.Headers.Add(aHbinding);
        //    builder.Headers.Add(aHcontract);
        //    builder.Headers.Add(aHname);
        //    return builder.ToEndpointAddress();

        //}

        #endregion //CompanyEndPoint

        #endregion //SOAP Config Functions

        #region Utility Functions
        #region GetCountryISOEnum

        public CountryGroupModel.CountryISOAlpha2Enum GetCountryISOEnum(string countrycode)
        {
            CountryGroupModel.CountryISOAlpha2Enum country;

            switch (countrycode.ToUpper())
            {

                case "US":
                case "UNITED STATES":
                case "USA":
                case "UNITED STATES OF AMERICA": country = CountryGroupModel.CountryISOAlpha2Enum.US; break;

                case "CA":
                case "CANADA": country = CountryGroupModel.CountryISOAlpha2Enum.CA; break;

                case "AD":
                case "ANDORRA": country = CountryGroupModel.CountryISOAlpha2Enum.AD; break;

                case "AE":
                case "UNITED ARAB EMIRATES": country = CountryGroupModel.CountryISOAlpha2Enum.AE; break;

                case "AF":
                case "AFGHANISTAN": country = CountryGroupModel.CountryISOAlpha2Enum.AF; break;

                case "AG":
                case "ANTIGUA AND BARBUDA": country = CountryGroupModel.CountryISOAlpha2Enum.AG; break;

                case "AI":
                case "ANGUILLA": country = CountryGroupModel.CountryISOAlpha2Enum.AI; break;

                case "AL":
                case "ALBANIA": country = CountryGroupModel.CountryISOAlpha2Enum.AL; break;

                case "AM":
                case "ARMENIA": country = CountryGroupModel.CountryISOAlpha2Enum.AM; break;

                case "AO":
                case "ANGOLA": country = CountryGroupModel.CountryISOAlpha2Enum.AO; break;

                //case "AQ": case "ANTARCTICA": country = CountryISOAlpha2Enum.AQ; break;

                case "AR":
                case "ARGENTINA": country = CountryGroupModel.CountryISOAlpha2Enum.AR; break;

                case "AS":
                case "AMERICAN SAMOA": country = CountryGroupModel.CountryISOAlpha2Enum.AS; break;

                case "AT":
                case "AUSTRIA": country = CountryGroupModel.CountryISOAlpha2Enum.AT; break;

                case "AU":
                case "AUSTRALIA": country = CountryGroupModel.CountryISOAlpha2Enum.AU; break;

                case "AW":
                case "ARUBA": country = CountryGroupModel.CountryISOAlpha2Enum.AW; break;

                //case "AX": case "ÅLAND ISLANDS": country = CountryGroupModel.CountryISOAlpha2Enum.AX; break;

                case "AZ":
                case "AZERBAIJAN": country = CountryGroupModel.CountryISOAlpha2Enum.AZ; break;

                case "BA":
                case "BOSNIA AND HERZEGOVINA": country = CountryGroupModel.CountryISOAlpha2Enum.BA; break;

                case "BB":
                case "BARBADOS": country = CountryGroupModel.CountryISOAlpha2Enum.BB; break;

                case "BD":
                case "BANGLADESH": country = CountryGroupModel.CountryISOAlpha2Enum.BD; break;

                case "BE":
                case "BELGIUM": country = CountryGroupModel.CountryISOAlpha2Enum.BE; break;

                case "BF":
                case "BURKINA FASO": country = CountryGroupModel.CountryISOAlpha2Enum.BF; break;

                case "BG":
                case "BULGARIA": country = CountryGroupModel.CountryISOAlpha2Enum.BG; break;

                case "BH":
                case "BAHRAIN": country = CountryGroupModel.CountryISOAlpha2Enum.BH; break;

                case "BI":
                case "BURUNDI": country = CountryGroupModel.CountryISOAlpha2Enum.BI; break;

                case "BJ":
                case "BENIN": country = CountryGroupModel.CountryISOAlpha2Enum.BJ; break;

                //case "BL": case "SAINT BARTHÉLEMY": country = CountryGroupModel.CountryISOAlpha2Enum.BL; break;

                case "BM":
                case "BERMUDA": country = CountryGroupModel.CountryISOAlpha2Enum.BM; break;

                case "BN":
                case "BRUNEI DARUSSALAM": country = CountryGroupModel.CountryISOAlpha2Enum.BN; break;

                case "BO":
                case "BOLIVIA, PLURINATIONAL STATE OF": country = CountryGroupModel.CountryISOAlpha2Enum.BO; break;

                //case "BQ": case "BONAIRE, SINT EUSTATIUS AND SABA": country = CountryGroupModel.CountryISOAlpha2Enum.BQ; break;

                case "BR":
                case "BRAZIL": country = CountryGroupModel.CountryISOAlpha2Enum.BR; break;

                case "BS":
                case "BAHAMAS": country = CountryGroupModel.CountryISOAlpha2Enum.BS; break;

                case "BT":
                case "BHUTAN": country = CountryGroupModel.CountryISOAlpha2Enum.BT; break;

                //case "BV": case "BOUVET ISLAND": country = CountryGroupModel.CountryISOAlpha2Enum.BV; break;

                case "BW":
                case "BOTSWANA": country = CountryGroupModel.CountryISOAlpha2Enum.BW; break;

                case "BY":
                case "BELARUS": country = CountryGroupModel.CountryISOAlpha2Enum.BY; break;

                case "BZ":
                case "BELIZE": country = CountryGroupModel.CountryISOAlpha2Enum.BZ; break;

                //case "CC": case "COCOS (KEELING) ISLANDS": country = CountryGroupModel.CountryISOAlpha2Enum.CC; break;

                case "CD":
                case "CONGO, THE DEMOCRATIC REPUBLIC OF THE": country = CountryGroupModel.CountryISOAlpha2Enum.CD; break;

                case "CF":
                case "CENTRAL AFRICAN REPUBLIC": country = CountryGroupModel.CountryISOAlpha2Enum.CF; break;

                case "CG":
                case "CONGO": country = CountryGroupModel.CountryISOAlpha2Enum.CG; break;

                case "CH":
                case "SWITZERLAND": country = CountryGroupModel.CountryISOAlpha2Enum.CH; break;

                case "CI":
                case "CÔTE D'IVOIRE": country = CountryGroupModel.CountryISOAlpha2Enum.CI; break;

                case "CK":
                case "COOK ISLANDS": country = CountryGroupModel.CountryISOAlpha2Enum.CK; break;

                case "CL":
                case "CHILE": country = CountryGroupModel.CountryISOAlpha2Enum.CL; break;

                case "CM":
                case "CAMEROON": country = CountryGroupModel.CountryISOAlpha2Enum.CM; break;

                case "CN":
                case "CHINA": country = CountryGroupModel.CountryISOAlpha2Enum.CN; break;

                case "CO":
                case "COLOMBIA": country = CountryGroupModel.CountryISOAlpha2Enum.CO; break;

                case "CR":
                case "COSTA RICA": country = CountryGroupModel.CountryISOAlpha2Enum.CR; break;

                case "CU":
                case "CUBA": country = CountryGroupModel.CountryISOAlpha2Enum.CU; break;

                case "CV":
                case "CAPE VERDE": country = CountryGroupModel.CountryISOAlpha2Enum.CV; break;

                //case "CW": case "CURAÇAO": country = CountryGroupModel.CountryISOAlpha2Enum.CW; break;

                case "CX":
                case "CHRISTMAS ISLAND": country = CountryGroupModel.CountryISOAlpha2Enum.CX; break;

                case "CY":
                case "CYPRUS": country = CountryGroupModel.CountryISOAlpha2Enum.CY; break;

                case "CZ":
                case "CZECH REPUBLIC": country = CountryGroupModel.CountryISOAlpha2Enum.CZ; break;

                case "DE":
                case "GERMANY": country = CountryGroupModel.CountryISOAlpha2Enum.DE; break;

                case "DJ":
                case "DJIBOUTI": country = CountryGroupModel.CountryISOAlpha2Enum.DJ; break;

                case "DK":
                case "DENMARK": country = CountryGroupModel.CountryISOAlpha2Enum.DK; break;

                case "DM":
                case "DOMINICA": country = CountryGroupModel.CountryISOAlpha2Enum.DM; break;

                case "DO":
                case "DOMINICAN REPUBLIC": country = CountryGroupModel.CountryISOAlpha2Enum.DO; break;

                case "DZ":
                case "ALGERIA": country = CountryGroupModel.CountryISOAlpha2Enum.DZ; break;

                case "EC":
                case "ECUADOR": country = CountryGroupModel.CountryISOAlpha2Enum.EC; break;

                case "EE":
                case "ESTONIA": country = CountryGroupModel.CountryISOAlpha2Enum.EE; break;

                case "EG":
                case "EGYPT": country = CountryGroupModel.CountryISOAlpha2Enum.EG; break;

                //case "EH": case "WESTERN SAHARA": country = CountryGroupModel.CountryISOAlpha2Enum.EH; break;

                case "ER":
                case "ERITREA": country = CountryGroupModel.CountryISOAlpha2Enum.ER; break;

                case "ES":
                case "SPAIN": country = CountryGroupModel.CountryISOAlpha2Enum.ES; break;

                case "ET":
                case "ETHIOPIA": country = CountryGroupModel.CountryISOAlpha2Enum.ET; break;

                case "FI":
                case "FINLAND": country = CountryGroupModel.CountryISOAlpha2Enum.FI; break;

                case "FJ":
                case "FIJI": country = CountryGroupModel.CountryISOAlpha2Enum.FJ; break;

                case "FK":
                case "FALKLAND ISLANDS (MALVINAS)": country = CountryGroupModel.CountryISOAlpha2Enum.FK; break;

                case "FM":
                case "MICRONESIA, FEDERATED STATES OF": country = CountryGroupModel.CountryISOAlpha2Enum.FM; break;

                case "FO":
                case "FAROE ISLANDS": country = CountryGroupModel.CountryISOAlpha2Enum.FO; break;

                case "FR":
                case "FRANCE": country = CountryGroupModel.CountryISOAlpha2Enum.FR; break;

                case "GA":
                case "GABON": country = CountryGroupModel.CountryISOAlpha2Enum.GA; break;

                case "GB":
                case "UNITED KINGDOM": country = CountryGroupModel.CountryISOAlpha2Enum.GB; break;

                case "GD":
                case "GRENADA": country = CountryGroupModel.CountryISOAlpha2Enum.GD; break;

                case "GE":
                case "GEORGIA": country = CountryGroupModel.CountryISOAlpha2Enum.GE; break;

                case "GF":
                case "FRENCH GUIANA": country = CountryGroupModel.CountryISOAlpha2Enum.GF; break;

                //case "GG": case "GUERNSEY": country = CountryGroupModel.CountryISOAlpha2Enum.GG; break;

                case "GH":
                case "GHANA": country = CountryGroupModel.CountryISOAlpha2Enum.GH; break;

                case "GI":
                case "GIBRALTAR": country = CountryGroupModel.CountryISOAlpha2Enum.GI; break;

                case "GL":
                case "GREENLAND": country = CountryGroupModel.CountryISOAlpha2Enum.GL; break;

                case "GM":
                case "GAMBIA": country = CountryGroupModel.CountryISOAlpha2Enum.GM; break;

                case "GN":
                case "GUINEA": country = CountryGroupModel.CountryISOAlpha2Enum.GN; break;

                case "GP":
                case "GUADELOUPE": country = CountryGroupModel.CountryISOAlpha2Enum.GP; break;

                case "GQ":
                case "EQUATORIAL GUINEA": country = CountryGroupModel.CountryISOAlpha2Enum.GQ; break;

                case "GR":
                case "GREECE": country = CountryGroupModel.CountryISOAlpha2Enum.GR; break;

                case "GS":
                case "SOUTH GEORGIA AND THE SOUTH SANDWICH ISLANDS": country = CountryGroupModel.CountryISOAlpha2Enum.GS; break;

                case "GT":
                case "GUATEMALA": country = CountryGroupModel.CountryISOAlpha2Enum.GT; break;

                case "GU":
                case "GUAM": country = CountryGroupModel.CountryISOAlpha2Enum.GU; break;

                case "GW":
                case "GUINEA-BISSAU": country = CountryGroupModel.CountryISOAlpha2Enum.GW; break;

                case "GY":
                case "GUYANA": country = CountryGroupModel.CountryISOAlpha2Enum.GY; break;

                case "HK":
                case "HONG KONG": country = CountryGroupModel.CountryISOAlpha2Enum.HK; break;

                //case "HM": case "HEARD ISLAND AND MCDONALD ISLANDS": country = CountryGroupModel.CountryISOAlpha2Enum.HM; break;

                case "HN":
                case "HONDURAS": country = CountryGroupModel.CountryISOAlpha2Enum.HN; break;

                case "HR":
                case "CROATIA": country = CountryGroupModel.CountryISOAlpha2Enum.HR; break;

                case "HT":
                case "HAITI": country = CountryGroupModel.CountryISOAlpha2Enum.HT; break;

                case "HU":
                case "HUNGARY": country = CountryGroupModel.CountryISOAlpha2Enum.HU; break;

                case "ID":
                case "INDONESIA": country = CountryGroupModel.CountryISOAlpha2Enum.ID; break;

                case "IE":
                case "IRELAND": country = CountryGroupModel.CountryISOAlpha2Enum.IE; break;

                case "IL":
                case "ISRAEL": country = CountryGroupModel.CountryISOAlpha2Enum.IL; break;

                //case "IM": case "ISLE OF MAN": country = CountryGroupModel.CountryISOAlpha2Enum.IM; break;

                case "IN":
                case "INDIA": country = CountryGroupModel.CountryISOAlpha2Enum.IN; break;

                //case "IO": case "BRITISH INDIAN OCEAN TERRITORY": country = CountryGroupModel.CountryISOAlpha2Enum.IO; break;

                case "IQ":
                case "IRAQ": country = CountryGroupModel.CountryISOAlpha2Enum.IQ; break;

                case "IR":
                case "IRAN, ISLAMIC REPUBLIC OF": country = CountryGroupModel.CountryISOAlpha2Enum.IR; break;

                case "IS":
                case "ICELAND": country = CountryGroupModel.CountryISOAlpha2Enum.IS; break;

                case "IT":
                case "ITALY": country = CountryGroupModel.CountryISOAlpha2Enum.IT; break;

                //case "JE": case "JERSEY": country = CountryGroupModel.CountryISOAlpha2Enum.JE; break;

                case "JM":
                case "JAMAICA": country = CountryGroupModel.CountryISOAlpha2Enum.JM; break;

                case "JO":
                case "JORDAN": country = CountryGroupModel.CountryISOAlpha2Enum.JO; break;

                case "JP":
                case "JAPAN": country = CountryGroupModel.CountryISOAlpha2Enum.JP; break;

                case "KE":
                case "KENYA": country = CountryGroupModel.CountryISOAlpha2Enum.KE; break;

                case "KG":
                case "KYRGYZSTAN": country = CountryGroupModel.CountryISOAlpha2Enum.KG; break;

                case "KH":
                case "CAMBODIA": country = CountryGroupModel.CountryISOAlpha2Enum.KH; break;

                case "KI":
                case "KIRIBATI": country = CountryGroupModel.CountryISOAlpha2Enum.KI; break;

                case "KM":
                case "COMOROS": country = CountryGroupModel.CountryISOAlpha2Enum.KM; break;

                case "KN":
                case "SAINT KITTS AND NEVIS": country = CountryGroupModel.CountryISOAlpha2Enum.KN; break;

                case "KP":
                case "KOREA, DEMOCRATIC PEOPLE'S REPUBLIC OF": country = CountryGroupModel.CountryISOAlpha2Enum.KP; break;

                case "KR":
                case "KOREA, REPUBLIC OF": country = CountryGroupModel.CountryISOAlpha2Enum.KR; break;

                case "KW":
                case "KUWAIT": country = CountryGroupModel.CountryISOAlpha2Enum.KW; break;

                case "KY":
                case "CAYMAN ISLANDS": country = CountryGroupModel.CountryISOAlpha2Enum.KY; break;

                case "KZ":
                case "KAZAKHSTAN": country = CountryGroupModel.CountryISOAlpha2Enum.KZ; break;

                case "LA":
                case "LAO PEOPLE'S DEMOCRATIC REPUBLIC": country = CountryGroupModel.CountryISOAlpha2Enum.LA; break;

                case "LB":
                case "LEBANON": country = CountryGroupModel.CountryISOAlpha2Enum.LB; break;

                case "LC":
                case "SAINT LUCIA": country = CountryGroupModel.CountryISOAlpha2Enum.LC; break;

                case "LI":
                case "LIECHTENSTEIN": country = CountryGroupModel.CountryISOAlpha2Enum.LI; break;

                case "LK":
                case "SRI LANKA": country = CountryGroupModel.CountryISOAlpha2Enum.LK; break;

                case "LR":
                case "LIBERIA": country = CountryGroupModel.CountryISOAlpha2Enum.LR; break;

                case "LS":
                case "LESOTHO": country = CountryGroupModel.CountryISOAlpha2Enum.LS; break;

                case "LT":
                case "LITHUANIA": country = CountryGroupModel.CountryISOAlpha2Enum.LT; break;

                case "LU":
                case "LUXEMBOURG": country = CountryGroupModel.CountryISOAlpha2Enum.LU; break;

                case "LV":
                case "LATVIA": country = CountryGroupModel.CountryISOAlpha2Enum.LV; break;

                case "LY":
                case "LIBYA": country = CountryGroupModel.CountryISOAlpha2Enum.LY; break;

                case "MA":
                case "MOROCCO": country = CountryGroupModel.CountryISOAlpha2Enum.MA; break;

                case "MC":
                case "MONACO": country = CountryGroupModel.CountryISOAlpha2Enum.MC; break;

                case "MD":
                case "MOLDOVA, REPUBLIC OF": country = CountryGroupModel.CountryISOAlpha2Enum.MD; break;

                case "ME":
                case "MONTENEGRO": country = CountryGroupModel.CountryISOAlpha2Enum.ME; break;

                //case "MF": case "SAINT MARTIN (FRENCH PART)": country = CountryGroupModel.CountryISOAlpha2Enum.MF; break;

                case "MG":
                case "MADAGASCAR": country = CountryGroupModel.CountryISOAlpha2Enum.MG; break;

                case "MH":
                case "MARSHALL ISLANDS": country = CountryGroupModel.CountryISOAlpha2Enum.MH; break;

                case "MK":
                case "MACEDONIA, THE FORMER YUGOSLAV REPUBLIC OF": country = CountryGroupModel.CountryISOAlpha2Enum.MK; break;

                case "ML":
                case "MALI": country = CountryGroupModel.CountryISOAlpha2Enum.ML; break;

                case "MM":
                case "MYANMAR": country = CountryGroupModel.CountryISOAlpha2Enum.MM; break;

                case "MN":
                case "MONGOLIA": country = CountryGroupModel.CountryISOAlpha2Enum.MN; break;

                case "MO":
                case "MACAO": country = CountryGroupModel.CountryISOAlpha2Enum.MO; break;

                case "MP":
                case "NORTHERN MARIANA ISLANDS": country = CountryGroupModel.CountryISOAlpha2Enum.MP; break;

                case "MQ":
                case "MARTINIQUE": country = CountryGroupModel.CountryISOAlpha2Enum.MQ; break;

                case "MR":
                case "MAURITANIA": country = CountryGroupModel.CountryISOAlpha2Enum.MR; break;

                case "MS":
                case "MONTSERRAT": country = CountryGroupModel.CountryISOAlpha2Enum.MS; break;

                case "MT":
                case "MALTA": country = CountryGroupModel.CountryISOAlpha2Enum.MT; break;

                case "MU":
                case "MAURITIUS": country = CountryGroupModel.CountryISOAlpha2Enum.MU; break;

                case "MV":
                case "MALDIVES": country = CountryGroupModel.CountryISOAlpha2Enum.MV; break;

                case "MW":
                case "MALAWI": country = CountryGroupModel.CountryISOAlpha2Enum.MW; break;

                case "MX":
                case "MEXICO": country = CountryGroupModel.CountryISOAlpha2Enum.MX; break;

                case "MY":
                case "MALAYSIA": country = CountryGroupModel.CountryISOAlpha2Enum.MY; break;

                case "MZ":
                case "MOZAMBIQUE": country = CountryGroupModel.CountryISOAlpha2Enum.MZ; break;

                case "NA":
                case "NAMIBIA": country = CountryGroupModel.CountryISOAlpha2Enum.NA; break;

                case "NC":
                case "NEW CALEDONIA": country = CountryGroupModel.CountryISOAlpha2Enum.NC; break;

                case "NE":
                case "NIGER": country = CountryGroupModel.CountryISOAlpha2Enum.NE; break;

                case "NF":
                case "NORFOLK ISLAND": country = CountryGroupModel.CountryISOAlpha2Enum.NF; break;

                case "NG":
                case "NIGERIA": country = CountryGroupModel.CountryISOAlpha2Enum.NG; break;

                case "NI":
                case "NICARAGUA": country = CountryGroupModel.CountryISOAlpha2Enum.NI; break;

                case "NL":
                case "NETHERLANDS": country = CountryGroupModel.CountryISOAlpha2Enum.NL; break;

                case "NO":
                case "NORWAY": country = CountryGroupModel.CountryISOAlpha2Enum.NO; break;

                case "NP":
                case "NEPAL": country = CountryGroupModel.CountryISOAlpha2Enum.NP; break;

                case "NR":
                case "NAURU": country = CountryGroupModel.CountryISOAlpha2Enum.NR; break;

                case "NU":
                case "NIUE": country = CountryGroupModel.CountryISOAlpha2Enum.NU; break;

                case "NZ":
                case "NEW ZEALAND": country = CountryGroupModel.CountryISOAlpha2Enum.NZ; break;

                case "OM":
                case "OMAN": country = CountryGroupModel.CountryISOAlpha2Enum.OM; break;

                case "PA":
                case "PANAMA": country = CountryGroupModel.CountryISOAlpha2Enum.PA; break;

                case "PE":
                case "PERU": country = CountryGroupModel.CountryISOAlpha2Enum.PE; break;

                case "PF":
                case "FRENCH POLYNESIA": country = CountryGroupModel.CountryISOAlpha2Enum.PF; break;

                case "PG":
                case "PAPUA NEW GUINEA": country = CountryGroupModel.CountryISOAlpha2Enum.PG; break;

                case "PH":
                case "PHILIPPINES": country = CountryGroupModel.CountryISOAlpha2Enum.PH; break;

                case "PK":
                case "PAKISTAN": country = CountryGroupModel.CountryISOAlpha2Enum.PK; break;

                case "PL":
                case "POLAND": country = CountryGroupModel.CountryISOAlpha2Enum.PL; break;

                case "PM":
                case "SAINT PIERRE AND MIQUELON": country = CountryGroupModel.CountryISOAlpha2Enum.PM; break;

                //case "PN": case "PITCAIRN": country = CountryGroupModel.CountryISOAlpha2Enum.PN; break;

                case "PR":
                case "PUERTO RICO": country = CountryGroupModel.CountryISOAlpha2Enum.PR; break;

                //case "PS": case "PALESTINE, STATE OF": country = CountryGroupModel.CountryISOAlpha2Enum.PS; break;

                case "PT":
                case "PORTUGAL": country = CountryGroupModel.CountryISOAlpha2Enum.PT; break;

                case "PW":
                case "PALAU": country = CountryGroupModel.CountryISOAlpha2Enum.PW; break;

                case "PY":
                case "PARAGUAY": country = CountryGroupModel.CountryISOAlpha2Enum.PY; break;

                case "QA":
                case "QATAR": country = CountryGroupModel.CountryISOAlpha2Enum.QA; break;

                case "RE":
                case "RÉUNION": country = CountryGroupModel.CountryISOAlpha2Enum.RE; break;

                case "RO":
                case "ROMANIA": country = CountryGroupModel.CountryISOAlpha2Enum.RO; break;

                case "RS":
                case "SERBIA": country = CountryGroupModel.CountryISOAlpha2Enum.RS; break;

                case "RU":
                case "RUSSIAN FEDERATION": country = CountryGroupModel.CountryISOAlpha2Enum.RU; break;

                case "RW":
                case "RWANDA": country = CountryGroupModel.CountryISOAlpha2Enum.RW; break;

                case "SA":
                case "SAUDI ARABIA": country = CountryGroupModel.CountryISOAlpha2Enum.SA; break;

                case "SB":
                case "SOLOMON ISLANDS": country = CountryGroupModel.CountryISOAlpha2Enum.SB; break;

                case "SC":
                case "SEYCHELLES": country = CountryGroupModel.CountryISOAlpha2Enum.SC; break;

                case "SD":
                case "SUDAN": country = CountryGroupModel.CountryISOAlpha2Enum.SD; break;

                case "SE":
                case "SWEDEN": country = CountryGroupModel.CountryISOAlpha2Enum.SE; break;

                case "SG":
                case "SINGAPORE": country = CountryGroupModel.CountryISOAlpha2Enum.SG; break;

                case "SH":
                case "SAINT HELENA, ASCENSION AND TRISTAN DA CUNHA": country = CountryGroupModel.CountryISOAlpha2Enum.SH; break;

                case "SI":
                case "SLOVENIA": country = CountryGroupModel.CountryISOAlpha2Enum.SI; break;

                //case "SJ": case "SVALBARD AND JAN MAYEN": country = CountryGroupModel.CountryISOAlpha2Enum.SJ; break;

                case "SK":
                case "SLOVAKIA": country = CountryGroupModel.CountryISOAlpha2Enum.SK; break;

                case "SL":
                case "SIERRA LEONE": country = CountryGroupModel.CountryISOAlpha2Enum.SL; break;

                case "SM":
                case "SAN MARINO": country = CountryGroupModel.CountryISOAlpha2Enum.SM; break;

                case "SN":
                case "SENEGAL": country = CountryGroupModel.CountryISOAlpha2Enum.SN; break;

                case "SO":
                case "SOMALIA": country = CountryGroupModel.CountryISOAlpha2Enum.SO; break;

                case "SR":
                case "SURINAME": country = CountryGroupModel.CountryISOAlpha2Enum.SR; break;

                //case "SS": case "SOUTH SUDAN": country = CountryGroupModel.CountryISOAlpha2Enum.SS; break;

                case "ST":
                case "SAO TOME AND PRINCIPE": country = CountryGroupModel.CountryISOAlpha2Enum.ST; break;

                case "SV":
                case "EL SALVADOR": country = CountryGroupModel.CountryISOAlpha2Enum.SV; break;

                //case "SX": case "SINT MAARTEN (DUTCH PART)": country = CountryGroupModel.CountryISOAlpha2Enum.SX; break;

                case "SY":
                case "SYRIAN ARAB REPUBLIC": country = CountryGroupModel.CountryISOAlpha2Enum.SY; break;

                case "SZ":
                case "SWAZILAND": country = CountryGroupModel.CountryISOAlpha2Enum.SZ; break;

                case "TC":
                case "TURKS AND CAICOS ISLANDS": country = CountryGroupModel.CountryISOAlpha2Enum.TC; break;

                case "TD":
                case "CHAD": country = CountryGroupModel.CountryISOAlpha2Enum.TD; break;

                //case "TF": case "FRENCH SOUTHERN TERRITORIES": country = CountryGroupModel.CountryISOAlpha2Enum.TF; break;

                case "TG":
                case "TOGO": country = CountryGroupModel.CountryISOAlpha2Enum.TG; break;

                case "TH":
                case "THAILAND": country = CountryGroupModel.CountryISOAlpha2Enum.TH; break;

                case "TJ":
                case "TAJIKISTAN": country = CountryGroupModel.CountryISOAlpha2Enum.TJ; break;

                case "TK":
                case "TOKELAU": country = CountryGroupModel.CountryISOAlpha2Enum.TK; break;

                case "TL":
                case "TIMOR-LESTE": country = CountryGroupModel.CountryISOAlpha2Enum.TL; break;

                case "TM":
                case "TURKMENISTAN": country = CountryGroupModel.CountryISOAlpha2Enum.TM; break;

                case "TN":
                case "TUNISIA": country = CountryGroupModel.CountryISOAlpha2Enum.TN; break;

                case "TO":
                case "TONGA": country = CountryGroupModel.CountryISOAlpha2Enum.TO; break;

                case "TR":
                case "TURKEY": country = CountryGroupModel.CountryISOAlpha2Enum.TR; break;

                case "TT":
                case "TRINIDAD AND TOBAGO": country = CountryGroupModel.CountryISOAlpha2Enum.TT; break;

                case "TV":
                case "TUVALU": country = CountryGroupModel.CountryISOAlpha2Enum.TV; break;

                case "TW":
                case "TAIWAN, PROVINCE OF CHINA": country = CountryGroupModel.CountryISOAlpha2Enum.TW; break;

                case "TZ":
                case "TANZANIA, UNITED REPUBLIC OF": country = CountryGroupModel.CountryISOAlpha2Enum.TZ; break;

                case "UA":
                case "UKRAINE": country = CountryGroupModel.CountryISOAlpha2Enum.UA; break;

                case "UG":
                case "UGANDA": country = CountryGroupModel.CountryISOAlpha2Enum.UG; break;

                case "UM":
                case "UNITED STATES MINOR OUTLYING ISLANDS": country = CountryGroupModel.CountryISOAlpha2Enum.UM; break;

                case "UY":
                case "URUGUAY": country = CountryGroupModel.CountryISOAlpha2Enum.UY; break;

                case "UZ":
                case "UZBEKISTAN": country = CountryGroupModel.CountryISOAlpha2Enum.UZ; break;

                case "VA":
                case "HOLY SEE (VATICAN CITY STATE)": country = CountryGroupModel.CountryISOAlpha2Enum.VA; break;

                case "VC":
                case "SAINT VINCENT AND THE GRENADINES": country = CountryGroupModel.CountryISOAlpha2Enum.VC; break;

                case "VE":
                case "VENEZUELA, BOLIVARIAN REPUBLIC OF": country = CountryGroupModel.CountryISOAlpha2Enum.VE; break;

                case "VG":
                case "VIRGIN ISLANDS, BRITISH": country = CountryGroupModel.CountryISOAlpha2Enum.VG; break;

                case "VI":
                case "VIRGIN ISLANDS, U.S.": country = CountryGroupModel.CountryISOAlpha2Enum.VI; break;

                case "VN":
                case "VIET NAM": country = CountryGroupModel.CountryISOAlpha2Enum.VN; break;

                case "VU":
                case "VANUATU": country = CountryGroupModel.CountryISOAlpha2Enum.VU; break;

                case "WF":
                case "WALLIS AND FUTUNA": country = CountryGroupModel.CountryISOAlpha2Enum.WF; break;

                case "WS":
                case "SAMOA": country = CountryGroupModel.CountryISOAlpha2Enum.WS; break;

                case "YE":
                case "YEMEN": country = CountryGroupModel.CountryISOAlpha2Enum.YE; break;

                case "YT":
                case "MAYOTTE": country = CountryGroupModel.CountryISOAlpha2Enum.YT; break;

                case "ZA":
                case "SOUTH AFRICA": country = CountryGroupModel.CountryISOAlpha2Enum.ZA; break;

                case "ZM":
                case "ZAMBIA": country = CountryGroupModel.CountryISOAlpha2Enum.ZM; break;

                case "ZW":
                case "ZIMBABWE": country = CountryGroupModel.CountryISOAlpha2Enum.ZW; break;



                default:

                    country = CountryGroupModel.CountryISOAlpha2Enum.US;

                    break;

            }



            return country;

        }

        #endregion //GetCountryISOEnum
        #endregion //Utility Functions





        #region GetCleanseMatchResult Function

        public APIResponse GetCleanseMatchResult(string companyName, string address, string Address2, string city, string state, CountryGroupModel.CountryISOAlpha2Enum country, string zipcode, string telephon, bool ExcludeNonHeadQuarters = false, bool ExcludeNonMarketable = false, bool ExcludeOutofBusiness = false, bool ExcludeUndeliverable = false, bool ExcludeUnreachable = false, int CandidateMaximumQuantity = 10, int ConfidenceLowerLevelThresholdValue = 4, string connectionString = "", string InputId = null)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            APIResponse response = new APIResponse();
            response.DnbMatchModels = new List<DnbMatchModel>();
            response.MatchEntities = new List<MatchEntity>();
            TransactionResponseDetail objResponse = new TransactionResponseDetail();
            GetCleanseMatchResponseMain objCleanseMatch = null;
            GetCleanseMatchResponse companyinfo_res = null;

            try
            {
                DataTable dt = new DataTable();
                SettingFacade sfac = new SettingFacade(connectionString);
                string endPoint = sfac.GetURLEncode("", companyName, address, Address2, city, state, zipcode, country.ToString(), telephon, "", "", "Direct20", ConfidenceLowerLevelThresholdValue, null, null, null, InputId);

                if (ExcludeNonHeadQuarters) { endPoint = endPoint + "&ExclusionCriteria-1=Exclude Non HeadQuarters"; }
                if (ExcludeNonMarketable) { endPoint = endPoint + "&ExclusionCriteria-2=Exclude Non Marketable"; }
                if (ExcludeOutofBusiness) { endPoint = endPoint + "&ExclusionCriteria-3=Exclude Out of Business"; }
                if (ExcludeUndeliverable) { endPoint = endPoint + "&ExclusionCriteria-4=Exclude Undeliverable"; }
                if (ExcludeUnreachable) { endPoint = endPoint + "&ExclusionCriteria-5=Exclude Unreachable"; }

                endPoint = endPoint.Replace("CandidateMaximumQuantity=50", "CandidateMaximumQuantity=" + CandidateMaximumQuantity);

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(endPoint);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "GET";
                httpWebRequest.Headers.Add("Authorization", CommonMethod.GetThirdPartyProperty(ThirdPartyCode.DNB_SINGLE_ENTITY_SEARCH.ToString(), ThirdPartyProperties.AuthToken.ToString()));

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    result = purgeContent(result);
                    response.ResponseJSON = result;
                    var serializer = new JavaScriptSerializer();
                    objCleanseMatch = serializer.Deserialize<GetCleanseMatchResponseMain>(result);
                    if (objCleanseMatch != null)
                        companyinfo_res = objCleanseMatch.GetCleanseMatchResponse;
                }

                if (companyinfo_res != null)
                {
                    response.APIRequest = endPoint;
                    objResponse.MatchDataCriteriaText = string.Empty;
                    objResponse.MatchedQuantity = 0;
                    if (companyinfo_res.TransactionDetail != null)
                    {
                        objResponse.ServiceTransactionID = companyinfo_res.TransactionDetail.ServiceTransactionID;
                        objResponse.TransactionTimestamp = Convert.ToDateTime(companyinfo_res.TransactionDetail.TransactionTimestamp);
                    }
                    if (companyinfo_res.TransactionResult != null)
                    {
                        objResponse.SeverityText = Convert.ToString(companyinfo_res.TransactionResult.SeverityText);
                        objResponse.ResultID = companyinfo_res.TransactionResult.ResultID;
                        objResponse.ResultText = companyinfo_res.TransactionResult.ResultText;
                    }
                    GetCleanseMatchResponseDetail companyinfo_wrapper = companyinfo_res.GetCleanseMatchResponseDetail;
                    if (companyinfo_wrapper != null)
                    {
                        if (companyinfo_wrapper.MatchResponseDetail != null && companyinfo_wrapper.MatchResponseDetail.MatchDataCriteriaText != null)
                        {
                            objResponse.MatchDataCriteriaText = companyinfo_wrapper.MatchResponseDetail.MatchDataCriteriaText._param;
                            objResponse.MatchedQuantity = companyinfo_wrapper.MatchResponseDetail.CandidateMatchedQuantity;
                        }
                        else
                        {
                            objResponse.MatchDataCriteriaText = string.Empty;
                            objResponse.MatchedQuantity = 0;
                        }
                        if (companyinfo_wrapper.MatchResponseDetail != null)
                        {
                            companyinfo_wrapper.MatchResponseDetail.MatchCandidate.ToList().
                           ForEach(n =>
                           {
                               string streetAddress = string.Empty;
                               MatchEntity model = new MatchEntity();
                               model.TransactionTimestamp = Convert.ToDateTime(objResponse.TransactionTimestamp);
                               model.DnBDUNSNumber = n.DUNSNumber;
                               if (n.OrganizationPrimaryName != null && n.OrganizationPrimaryName._OrganizationName != null)
                                   model.DnBOrganizationName = n.OrganizationPrimaryName._OrganizationName._param;
                               if (n.PrimaryAddress != null)
                               {
                                   if (n.PrimaryAddress.StreetAddressLine != null && n.PrimaryAddress.StreetAddressLine.Any())
                                   {
                                       n.PrimaryAddress.StreetAddressLine.ToList().ForEach(s =>
                                       {
                                           streetAddress += s.LineText + ",";
                                       });
                                       model.DnBStreetAddressLine = streetAddress.TrimEnd(',');
                                   }

                                   model.DnBPrimaryTownName = n.PrimaryAddress.PrimaryTownName;
                                   model.DnBPostalCode = n.PrimaryAddress.PostalCode;
                                   model.DnBPostalCodeExtensionCode = n.PrimaryAddress.PostalCodeExtensionCode;
                                   model.DnBCountryISOAlpha2Code = Convert.ToString(n.PrimaryAddress.CountryISOAlpha2Code);
                                   model.DnBTerritoryAbbreviatedName = n.PrimaryAddress.TerritoryAbbreviatedName;
                                   model.DnBAddressUndeliverable = Convert.ToString(n.PrimaryAddress.UndeliverableIndicator);
                               }

                               if (n.TelephoneNumber != null)
                               {
                                   model.DnBTelephoneNumber = n.TelephoneNumber.TelecommunicationNumber;
                                   model.DnBTelephoneNumberUnreachableIndicator = n.TelephoneNumber.UnreachableIndicator;
                               }

                               model.DnBStandaloneOrganization = Convert.ToString(n.StandaloneOrganizationIndicator);
                               if (n.StandaloneOrganizationIndicator)
                               {
                                   model.DnBFamilyTreeMemberRole = "Single Location";
                               }
                               else if (n.FamilyTreeMemberRole != null && n.FamilyTreeMemberRole.Any())
                               {
                                   model.DnBFamilyTreeMemberRole = n.FamilyTreeMemberRole[0].FamilyTreeMemberRoleText._param;
                               }

                               if (n.OperatingStatusText != null)
                               {
                                   model.DnBOperatingStatus = n.OperatingStatusText._param;
                               }
                               model.IsSelected = false;
                               if (n.MatchQualityInformation != null)
                               {
                                   model.DnBConfidenceCode = n.MatchQualityInformation.ConfidenceCodeValue;
                                   model.DnBMatchGradeText = n.MatchQualityInformation.MatchGradeText;
                                   model.DnBMatchDataProfileText = n.MatchQualityInformation.MatchDataProfileText;
                                   if (!string.IsNullOrWhiteSpace(model.DnBMatchGradeText))
                                   {
                                       string matchgradeText = model.DnBMatchGradeText;
                                       if (!string.IsNullOrEmpty(matchgradeText) && matchgradeText.Length >= 8)
                                       {
                                           model.MGCompanyName = matchgradeText.Substring(0, 1);
                                           model.MGStreetNo = matchgradeText.Substring(1, 1);
                                           model.MGStreetName = matchgradeText.Substring(2, 1);
                                           model.MGCity = matchgradeText.Substring(3, 1);
                                           model.MGState = matchgradeText.Substring(4, 1);
                                           model.MGZipCode = matchgradeText.Substring(7, 1);
                                           model.MGTelephone = matchgradeText.Substring(6, 1);
                                           model.MGMailingAddress = matchgradeText.Substring(5, 1);
                                           model.MGDensity = matchgradeText.Substring(8, 1);
                                           model.MGUniqueness = matchgradeText.Substring(9, 1);
                                           model.MGSIC = matchgradeText.Substring(10, 1);
                                       }
                                       else if (!string.IsNullOrEmpty(matchgradeText) && matchgradeText.Length >= 7)
                                       {
                                           model.MGCompanyName = matchgradeText.Substring(0, 1);
                                           model.MGStreetNo = matchgradeText.Substring(1, 1);
                                           model.MGStreetName = matchgradeText.Substring(2, 1);
                                           model.MGCity = matchgradeText.Substring(3, 1);
                                           model.MGState = matchgradeText.Substring(4, 1);
                                           model.MGMailingAddress = matchgradeText.Substring(5, 1);
                                           model.MGTelephone = matchgradeText.Substring(6, 1);
                                       }
                                   }
                                   if (!string.IsNullOrEmpty(model.DnBMatchDataProfileText))
                                   {
                                       model.DnBMatchDataProfileComponentCount = n.MatchQualityInformation.MatchDataProfileComponentCount;
                                       string matchText = model.DnBMatchDataProfileText;
                                       if (model.DnBMatchDataProfileComponentCount >= 14)
                                       {
                                           model.MDPVCompanyName = matchText.Substring(0, 2);
                                           model.MDPVStreetNo = matchText.Substring(2, 2);
                                           model.MDPVStreetName = matchText.Substring(4, 2);
                                           model.MDPVCity = matchText.Substring(6, 2);
                                           model.MDPVState = matchText.Substring(8, 2);
                                           model.MDPVMailingAddress = matchText.Substring(10, 2);
                                           model.MDPVTelephone = matchText.Substring(12, 2);
                                           model.MDPVZipCode = matchText.Substring(14, 2);
                                           model.MDPVSIC = matchText.Substring(18, 2);
                                           model.MDPVDensity = matchText.Substring(20, 2);
                                           model.MDPVUniqueness = matchText.Substring(22, 2);
                                           model.MDPText = matchText.Substring(0, 2) + " " + matchText.Substring(2, 2) + "-" + matchText.Substring(4, 2) + "-" + matchText.Substring(6, 2) + "-" + matchText.Substring(8, 2) + " " + matchText.Substring(12, 2);
                                       }
                                       else
                                       {
                                           model.MDPVCompanyName = matchText.Substring(0, 2);
                                           model.MDPVStreetNo = matchText.Substring(2, 2);
                                           model.MDPVStreetName = matchText.Substring(4, 2);
                                           model.MDPVCity = matchText.Substring(6, 2);
                                           model.MDPVState = matchText.Substring(8, 2);
                                           model.MDPVMailingAddress = matchText.Substring(10, 2);
                                           model.MDPVTelephone = matchText.Substring(12, 2);
                                           model.MDPText = matchText.Substring(0, 2) + " " + matchText.Substring(2, 2) + "-" + matchText.Substring(4, 2) + "-" + matchText.Substring(6, 2) + "-" + matchText.Substring(8, 2) + " " + matchText.Substring(12, 2);
                                       }
                                   }
                                   if (n.MatchQualityInformation.MatchGradeComponent != null && n.MatchQualityInformation.MatchGradeComponent.Any())
                                   {
                                       model.ScoreCompany = Convert.ToString(n.MatchQualityInformation.MatchGradeComponent.FirstOrDefault().MatchGradeComponentScore);
                                   }
                               }
                               model.DnBDisplaySequence = Convert.ToString(n.DisplaySequence);
                               if (n.TradeStyleName != null && n.TradeStyleName.OrganizationName != null)
                                   model.DnBTradeStyleName = n.TradeStyleName.OrganizationName._param;

                               if (n.SeniorPrincipalName != null)
                                   model.DnBSeniorPrincipalName = n.SeniorPrincipalName.FullName;

                               if (n.MailingAddress != null && n.MailingAddress.StreetAddressLine != null && n.MailingAddress.StreetAddressLine.Any())
                               {
                                   string maddress, TerrAbb = "", PostalCode = "", PostalCodeExt = "";
                                   string LineText = n.MailingAddress.StreetAddressLine.FirstOrDefault().LineText;
                                   if (!string.IsNullOrEmpty(n.MailingAddress.TerritoryAbbreviatedName)) TerrAbb = n.MailingAddress.TerritoryAbbreviatedName;
                                   if (!string.IsNullOrEmpty(n.MailingAddress.PostalCode)) PostalCode = n.MailingAddress.PostalCode;
                                   if (!string.IsNullOrEmpty(n.MailingAddress.PostalCodeExtensionCode)) PostalCodeExt = n.MailingAddress.PostalCodeExtensionCode;
                                   if (!string.IsNullOrEmpty(LineText))
                                   {
                                       maddress = LineText + ", " + n.MailingAddress.PrimaryTownName + " " + TerrAbb + " " + PostalCode + " " + PostalCodeExt + " " + n.MailingAddress.CountryISOAlpha2Code;
                                       model.DnBMailingAddress = maddress;
                                       model.DnBMailingAddressUndeliverable = n.MailingAddress.UndeliverableIndicator;
                                   }
                               }


                               model.DnBMarketabilityIndicator = n.MarketabilityIndicator;

                               response.MatchEntities.Add(model);

                           });
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                response = null;
            }
            if (response != null)
                response.TransactionResponseDetail = objResponse;
            return response;
        }
        public APIResponse GetMatchResult(string DunsNo, string connectionString = "", int confidenceLowerLevelThresholdValue = 4)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            APIResponse response = new APIResponse();
            response.DnbMatchModels = new List<DnbMatchModel>();
            response.MatchEntities = new List<MatchEntity>();
            TransactionResponseDetail objResponse = new TransactionResponseDetail();
            objResponse.DnBDUNSNumber = DunsNo;
            GetCleanseMatchResponseMain objMatch = null;
            GetCleanseMatchResponseDetail match_res = null;

            try
            {
                //string endPoint = DnBApi.MatchURL;
                SettingFacade sfac = new SettingFacade(connectionString);
                string endPoint = sfac.GetURLEncode("", "", "", "", "", "", "", "", "", "", "", "Direct20", confidenceLowerLevelThresholdValue, DunsNo, null, null, null);

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(endPoint);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "GET";
                httpWebRequest.Headers.Add("Authorization", CommonMethod.GetThirdPartyProperty(ThirdPartyCode.DNB_SINGLE_ENTITY_SEARCH.ToString(), ThirdPartyProperties.AuthToken.ToString()));

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    result = purgeContent(result);
                    response.ResponseJSON = result;
                    var serializer = new JavaScriptSerializer();
                    objMatch = serializer.Deserialize<GetCleanseMatchResponseMain>(result);
                    if (objMatch != null && objMatch.GetCleanseMatchResponse != null)
                        match_res = objMatch.GetCleanseMatchResponse.GetCleanseMatchResponseDetail;
                }
                if (objMatch != null && objMatch.GetCleanseMatchResponse != null)
                {
                    if (objMatch.GetCleanseMatchResponse.TransactionDetail != null)
                    {
                        objResponse.ServiceTransactionID = objMatch.GetCleanseMatchResponse.TransactionDetail.ServiceTransactionID;
                        objResponse.TransactionTimestamp = Convert.ToDateTime(objMatch.GetCleanseMatchResponse.TransactionDetail.TransactionTimestamp);
                    }
                    if (objMatch.GetCleanseMatchResponse.TransactionResult != null)
                    {
                        objResponse.SeverityText = Convert.ToString(objMatch.GetCleanseMatchResponse.TransactionResult.SeverityText);
                        objResponse.ResultID = objMatch.GetCleanseMatchResponse.TransactionResult.ResultID;
                        objResponse.ResultText = objMatch.GetCleanseMatchResponse.TransactionResult.ResultText;
                    }
                }
                if (match_res != null)
                {
                    response.APIRequest = endPoint;
                    objResponse.MatchDataCriteriaText = string.Empty;
                    objResponse.MatchedQuantity = 0;

                    MatchResponseDetail companyinfo_wrapper = match_res.MatchResponseDetail;
                    if (companyinfo_wrapper != null)
                    {

                        if (companyinfo_wrapper.MatchDataCriteriaText != null)
                        {
                            objResponse.MatchDataCriteriaText = companyinfo_wrapper.MatchDataCriteriaText._param;
                            objResponse.MatchedQuantity = companyinfo_wrapper.CandidateMatchedQuantity;
                        }
                        else
                        {
                            objResponse.MatchDataCriteriaText = string.Empty;
                            objResponse.MatchedQuantity = 0;
                        }
                        if (companyinfo_wrapper.MatchCandidate != null)
                        {
                            companyinfo_wrapper.MatchCandidate.ToList().
                           ForEach(n =>
                           {

                               string streetAddress = string.Empty;
                               MatchEntity model = new MatchEntity();
                               model.TransactionTimestamp = objResponse.TransactionTimestamp;
                               model.DnBDUNSNumber = n.DUNSNumber;
                               model.DnBOrganizationName = n.OrganizationPrimaryName._OrganizationName._param;
                               if (n.PrimaryAddress != null)
                               {
                                   if (n.PrimaryAddress.StreetAddressLine != null && n.PrimaryAddress.StreetAddressLine.Any())
                                   {
                                       n.PrimaryAddress.StreetAddressLine.ToList().ForEach(s =>
                                       {
                                           streetAddress += s.LineText + ",";
                                       });
                                       model.DnBStreetAddressLine = streetAddress.TrimEnd(',');
                                   }

                                   model.DnBPrimaryTownName = n.PrimaryAddress.PrimaryTownName;
                                   model.DnBPostalCode = n.PrimaryAddress.PostalCode;
                                   model.DnBPostalCodeExtensionCode = n.PrimaryAddress.PostalCodeExtensionCode;
                                   model.DnBCountryISOAlpha2Code = Convert.ToString(n.PrimaryAddress.CountryISOAlpha2Code);
                                   model.DnBTerritoryAbbreviatedName = n.PrimaryAddress.TerritoryAbbreviatedName;
                                   model.DnBAddressUndeliverable = Convert.ToString(n.PrimaryAddress.UndeliverableIndicator);
                               }

                               if (n.TelephoneNumber != null)
                               {
                                   model.DnBTelephoneNumber = n.TelephoneNumber.TelecommunicationNumber;
                                   model.DnBTelephoneNumberUnreachableIndicator = n.TelephoneNumber.UnreachableIndicator;
                               }

                               model.DnBStandaloneOrganization = Convert.ToString(n.StandaloneOrganizationIndicator);
                               if (n.StandaloneOrganizationIndicator)
                               {
                                   model.DnBFamilyTreeMemberRole = "Single Location";
                               }
                               else if (n.FamilyTreeMemberRole != null && n.FamilyTreeMemberRole.Any())
                               {
                                   model.DnBFamilyTreeMemberRole = n.FamilyTreeMemberRole[0].FamilyTreeMemberRoleText._param;
                               }

                               if (n.OperatingStatusText != null)
                               {
                                   model.DnBOperatingStatus = n.OperatingStatusText._param;
                               }
                               model.IsSelected = false;
                               if (n.MatchQualityInformation == null)
                               {
                                   MatchQualityInformation objMatchQuality = new MatchQualityInformation();
                                   objMatchQuality.ConfidenceCodeValue = 10;
                                   objMatchQuality.MatchGradeText = "AAAAAAAAAAA";
                                   objMatchQuality.MatchDataProfileText = "98989898989898";
                                   n.MatchQualityInformation = objMatchQuality;

                               }
                               if (n.MatchQualityInformation != null)
                               {
                                   model.DnBConfidenceCode = n.MatchQualityInformation.ConfidenceCodeValue;
                                   model.DnBMatchGradeText = n.MatchQualityInformation.MatchGradeText;
                                   if (string.IsNullOrEmpty(model.DnBMatchGradeText))
                                       model.DnBMatchGradeText = "AAAAAAAAAAA";

                                   model.DnBMatchDataProfileText = n.MatchQualityInformation.MatchDataProfileText;
                                   if (string.IsNullOrEmpty(model.DnBMatchDataProfileText))
                                       model.DnBMatchDataProfileText = "98989898989898";

                                   if (!string.IsNullOrWhiteSpace(model.DnBMatchGradeText))
                                   {
                                       string matchgradeText = model.DnBMatchGradeText;
                                       if (!string.IsNullOrEmpty(matchgradeText) && matchgradeText.Length >= 8)
                                       {
                                           model.MGCompanyName = matchgradeText.Substring(0, 1);
                                           model.MGStreetNo = matchgradeText.Substring(1, 1);
                                           model.MGStreetName = matchgradeText.Substring(2, 1);
                                           model.MGCity = matchgradeText.Substring(3, 1);
                                           model.MGState = matchgradeText.Substring(4, 1);
                                           model.MGZipCode = matchgradeText.Substring(7, 1);
                                           model.MGTelephone = matchgradeText.Substring(6, 1);
                                           model.MGMailingAddress = matchgradeText.Substring(5, 1);
                                           model.MGDensity = matchgradeText.Substring(8, 1);
                                           model.MGUniqueness = matchgradeText.Substring(9, 1);
                                           model.MGSIC = matchgradeText.Substring(10, 1);
                                       }
                                       else if (!string.IsNullOrEmpty(matchgradeText) && matchgradeText.Length >= 7)
                                       {
                                           model.MGCompanyName = matchgradeText.Substring(0, 1);
                                           model.MGStreetNo = matchgradeText.Substring(1, 1);
                                           model.MGStreetName = matchgradeText.Substring(2, 1);
                                           model.MGCity = matchgradeText.Substring(3, 1);
                                           model.MGState = matchgradeText.Substring(4, 1);
                                           model.MGMailingAddress = matchgradeText.Substring(5, 1);
                                           model.MGTelephone = matchgradeText.Substring(6, 1);
                                       }
                                   }

                                   string matchText = model.DnBMatchDataProfileText;
                                   if (matchText.Length >= 14)
                                   {
                                       model.MDPVCompanyName = matchText.Substring(0, 2);
                                       model.MDPVStreetNo = matchText.Substring(2, 2);
                                       model.MDPVStreetName = matchText.Substring(4, 2);
                                       model.MDPVCity = matchText.Substring(6, 2);
                                       model.MDPVState = matchText.Substring(8, 2);
                                       model.MDPVMailingAddress = matchText.Substring(10, 2);
                                       model.MDPVTelephone = matchText.Substring(12, 2);
                                       model.MDPText = matchText.Substring(0, 2) + " " + matchText.Substring(2, 2) + "-" + matchText.Substring(4, 2) + "-" + matchText.Substring(6, 2) + "-" + matchText.Substring(8, 2) + " " + matchText.Substring(12, 2);
                                   }

                                   if (n.MatchQualityInformation.MatchGradeComponent != null && n.MatchQualityInformation.MatchGradeComponent.Any())
                                   {
                                       model.ScoreCompany = Convert.ToString(n.MatchQualityInformation.MatchGradeComponent.FirstOrDefault().MatchGradeComponentScore);
                                   }

                               }

                               model.DnBDisplaySequence = Convert.ToString(n.DisplaySequence);
                               if (n.TradeStyleName != null && n.TradeStyleName.OrganizationName != null)
                                   model.DnBTradeStyleName = n.TradeStyleName.OrganizationName._param;

                               if (n.SeniorPrincipalName != null)
                                   model.DnBSeniorPrincipalName = n.SeniorPrincipalName.FullName;

                               if (n.MailingAddress != null && n.MailingAddress.StreetAddressLine != null && n.MailingAddress.StreetAddressLine.Any())
                               {
                                   string address, TerrAbb = "", PostalCode = "", PostalCodeExt = "";
                                   string LineText = n.MailingAddress.StreetAddressLine.FirstOrDefault().LineText;
                                   if (!string.IsNullOrEmpty(n.MailingAddress.TerritoryAbbreviatedName)) TerrAbb = n.MailingAddress.TerritoryAbbreviatedName;
                                   if (!string.IsNullOrEmpty(n.MailingAddress.PostalCode)) PostalCode = n.MailingAddress.PostalCode;
                                   if (!string.IsNullOrEmpty(n.MailingAddress.PostalCodeExtensionCode)) PostalCodeExt = n.MailingAddress.PostalCodeExtensionCode;
                                   if (!string.IsNullOrEmpty(LineText))
                                   {
                                       address = LineText + ", " + n.MailingAddress.PrimaryTownName + " " + TerrAbb + " " + PostalCode + " " + PostalCodeExt + " " + n.MailingAddress.CountryISOAlpha2Code;
                                       model.DnBMailingAddress = address;
                                       model.DnBMailingAddressUndeliverable = n.MailingAddress.UndeliverableIndicator;
                                   }
                               }
                               model.DnBMarketabilityIndicator = n.MarketabilityIndicator;
                               response.MatchEntities.Add(model);

                           });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            if (response != null)
                response.TransactionResponseDetail = objResponse;
            return response;

        }
        #endregion //GetCleanseMatchResult Function
        public APIResponse GetMatchByDomainOrEmail(string seachValue, string type, string connectionString = "", int confidenceLowerLevelThresholdValue = 4, string CountryCode = "") // type = URLText or EmailAddress
        {
            APIResponse response = new APIResponse();
            response.DnbMatchModels = new List<DnbMatchModel>();
            response.MatchEntities = new List<MatchEntity>();
            TransactionResponseDetail objResponse = new TransactionResponseDetail();

            GetCleanseMatchResponseMain objMatch = null;
            GetCleanseMatchResponseDetail match_res = null;

            try
            {
                SettingFacade sfac = new SettingFacade(connectionString);
                string endPoint = "";
                if (type.ToLower() == "domain")
                {
                    endPoint = sfac.GetURLEncode("", "", "", "", "", "", "", CountryCode, "", "", "", "Direct20", confidenceLowerLevelThresholdValue, null, seachValue, null);
                }
                else
                {
                    endPoint = sfac.GetURLEncode("", "", "", "", "", "", "", CountryCode, "", "", "", "Direct20", confidenceLowerLevelThresholdValue, null, null, seachValue);
                }
                //string endPoint = "https://direct.dnb.com/V6.0/organizations?URLText=apple.com&cleansematch=true";
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(endPoint);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "GET";
                httpWebRequest.Headers.Add("Authorization", CommonMethod.GetThirdPartyProperty(ThirdPartyCode.DNB_SINGLE_ENTITY_SEARCH.ToString(), ThirdPartyProperties.AuthToken.ToString()));

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    result = purgeContent(result);
                    response.ResponseJSON = result;
                    var serializer = new JavaScriptSerializer();
                    objMatch = serializer.Deserialize<GetCleanseMatchResponseMain>(result);
                    if (objMatch != null && objMatch.GetCleanseMatchResponse != null)
                        match_res = objMatch.GetCleanseMatchResponse.GetCleanseMatchResponseDetail;
                }
                if (objMatch != null && objMatch.GetCleanseMatchResponse != null)
                {
                    if (objMatch.GetCleanseMatchResponse.TransactionDetail != null)
                    {
                        objResponse.ServiceTransactionID = objMatch.GetCleanseMatchResponse.TransactionDetail.ServiceTransactionID;
                        objResponse.TransactionTimestamp = Convert.ToDateTime(objMatch.GetCleanseMatchResponse.TransactionDetail.TransactionTimestamp);
                    }
                    if (objMatch.GetCleanseMatchResponse.TransactionResult != null)
                    {
                        objResponse.SeverityText = Convert.ToString(objMatch.GetCleanseMatchResponse.TransactionResult.SeverityText);
                        objResponse.ResultID = objMatch.GetCleanseMatchResponse.TransactionResult.ResultID;
                        objResponse.ResultText = objMatch.GetCleanseMatchResponse.TransactionResult.ResultText;
                    }
                }
                if (match_res != null)
                {
                    response.APIRequest = endPoint;
                    objResponse.MatchDataCriteriaText = string.Empty;
                    objResponse.MatchedQuantity = 0;

                    MatchResponseDetail companyinfo_wrapper = match_res.MatchResponseDetail;
                    if (companyinfo_wrapper != null)
                    {

                        if (companyinfo_wrapper.MatchDataCriteriaText != null)
                        {
                            objResponse.MatchDataCriteriaText = companyinfo_wrapper.MatchDataCriteriaText._param;
                            objResponse.MatchedQuantity = companyinfo_wrapper.CandidateMatchedQuantity;
                        }
                        else
                        {
                            objResponse.MatchDataCriteriaText = string.Empty;
                            objResponse.MatchedQuantity = 0;
                        }
                        if (companyinfo_wrapper.MatchCandidate != null)
                        {
                            companyinfo_wrapper.MatchCandidate.ToList().
                           ForEach(n =>
                           {
                               string streetAddress = string.Empty;
                               MatchEntity model = new MatchEntity();
                               model.TransactionTimestamp = objResponse.TransactionTimestamp;
                               model.DnBDUNSNumber = n.DUNSNumber;
                               model.DnBOrganizationName = n.OrganizationPrimaryName._OrganizationName._param;
                               if (n.PrimaryAddress != null)
                               {
                                   if (n.PrimaryAddress.StreetAddressLine != null && n.PrimaryAddress.StreetAddressLine.Any())
                                   {
                                       n.PrimaryAddress.StreetAddressLine.ToList().ForEach(s =>
                                       {
                                           streetAddress += s.LineText + ",";
                                       });
                                       model.DnBStreetAddressLine = streetAddress.TrimEnd(',');
                                   }

                                   model.DnBPrimaryTownName = n.PrimaryAddress.PrimaryTownName;
                                   model.DnBPostalCode = n.PrimaryAddress.PostalCode;
                                   model.DnBPostalCodeExtensionCode = n.PrimaryAddress.PostalCodeExtensionCode;
                                   model.DnBCountryISOAlpha2Code = Convert.ToString(n.PrimaryAddress.CountryISOAlpha2Code);
                                   model.DnBTerritoryAbbreviatedName = n.PrimaryAddress.TerritoryAbbreviatedName;
                                   model.DnBAddressUndeliverable = Convert.ToString(n.PrimaryAddress.UndeliverableIndicator);
                               }

                               if (n.TelephoneNumber != null)
                               {
                                   model.DnBTelephoneNumber = n.TelephoneNumber.TelecommunicationNumber;
                                   model.DnBTelephoneNumberUnreachableIndicator = n.TelephoneNumber.UnreachableIndicator;
                               }

                               model.DnBStandaloneOrganization = Convert.ToString(n.StandaloneOrganizationIndicator);
                               if (n.StandaloneOrganizationIndicator)
                               {
                                   model.DnBFamilyTreeMemberRole = "Single Location";
                               }
                               else if (n.FamilyTreeMemberRole != null && n.FamilyTreeMemberRole.Any())
                               {
                                   model.DnBFamilyTreeMemberRole = n.FamilyTreeMemberRole[0].FamilyTreeMemberRoleText._param;
                               }

                               if (n.OperatingStatusText != null)
                               {
                                   model.DnBOperatingStatus = n.OperatingStatusText._param;
                               }
                               model.IsSelected = false;
                               if (n.MatchQualityInformation == null)
                               {
                                   MatchQualityInformation objMatchQuality = new MatchQualityInformation();
                                   objMatchQuality.ConfidenceCodeValue = 10;
                                   objMatchQuality.MatchGradeText = "AAAAAAAAAAA";
                                   objMatchQuality.MatchDataProfileText = "98989898989898";
                                   n.MatchQualityInformation = objMatchQuality;

                               }
                               if (n.MatchQualityInformation != null)
                               {
                                   model.DnBConfidenceCode = n.MatchQualityInformation.ConfidenceCodeValue;
                                   model.DnBMatchGradeText = n.MatchQualityInformation.MatchGradeText;
                                   if (string.IsNullOrEmpty(model.DnBMatchGradeText))
                                       model.DnBMatchGradeText = "AAAAAAAAAAA";

                                   model.DnBMatchDataProfileText = n.MatchQualityInformation.MatchDataProfileText;
                                   if (string.IsNullOrEmpty(model.DnBMatchDataProfileText))
                                       model.DnBMatchDataProfileText = "98989898989898";

                                   if (!string.IsNullOrWhiteSpace(model.DnBMatchGradeText))
                                   {
                                       string matchgradeText = model.DnBMatchGradeText;
                                       if (!string.IsNullOrEmpty(matchgradeText) && matchgradeText.Length >= 8)
                                       {
                                           model.MGCompanyName = matchgradeText.Substring(0, 1);
                                           model.MGStreetNo = matchgradeText.Substring(1, 1);
                                           model.MGStreetName = matchgradeText.Substring(2, 1);
                                           model.MGCity = matchgradeText.Substring(3, 1);
                                           model.MGState = matchgradeText.Substring(4, 1);
                                           model.MGZipCode = matchgradeText.Substring(7, 1);
                                           model.MGTelephone = matchgradeText.Substring(6, 1);
                                           model.MGMailingAddress = matchgradeText.Substring(5, 1);
                                           model.MGDensity = matchgradeText.Substring(8, 1);
                                           model.MGUniqueness = matchgradeText.Substring(9, 1);
                                           model.MGSIC = matchgradeText.Substring(10, 1);
                                       }
                                       else if (!string.IsNullOrEmpty(matchgradeText) && matchgradeText.Length >= 7)
                                       {
                                           model.MGCompanyName = matchgradeText.Substring(0, 1);
                                           model.MGStreetNo = matchgradeText.Substring(1, 1);
                                           model.MGStreetName = matchgradeText.Substring(2, 1);
                                           model.MGCity = matchgradeText.Substring(3, 1);
                                           model.MGState = matchgradeText.Substring(4, 1);
                                           model.MGMailingAddress = matchgradeText.Substring(5, 1);
                                           model.MGTelephone = matchgradeText.Substring(6, 1);
                                       }

                                   }

                                   string matchText = model.DnBMatchDataProfileText;
                                   if (matchText.Length >= 14)
                                   {
                                       model.MDPVCompanyName = matchText.Substring(0, 2);
                                       model.MDPVStreetNo = matchText.Substring(2, 2);
                                       model.MDPVStreetName = matchText.Substring(4, 2);
                                       model.MDPVCity = matchText.Substring(6, 2);
                                       model.MDPVState = matchText.Substring(8, 2);
                                       model.MDPVMailingAddress = matchText.Substring(10, 2);
                                       model.MDPVTelephone = matchText.Substring(12, 2);
                                       model.MDPText = matchText.Substring(0, 2) + " " + matchText.Substring(2, 2) + "-" + matchText.Substring(4, 2) + "-" + matchText.Substring(6, 2) + "-" + matchText.Substring(8, 2) + " " + matchText.Substring(12, 2);
                                   }
                               }

                               model.DnBDisplaySequence = Convert.ToString(n.DisplaySequence);
                               if (n.TradeStyleName != null && n.TradeStyleName.OrganizationName != null)
                                   model.DnBTradeStyleName = n.TradeStyleName.OrganizationName._param;

                               if (n.SeniorPrincipalName != null)
                                   model.DnBSeniorPrincipalName = n.SeniorPrincipalName.FullName;

                               if (n.MailingAddress != null && n.MailingAddress.StreetAddressLine != null && n.MailingAddress.StreetAddressLine.Any())
                               {
                                   string address, TerrAbb = "", PostalCode = "", PostalCodeExt = "";
                                   string LineText = n.MailingAddress.StreetAddressLine.FirstOrDefault().LineText;
                                   if (!string.IsNullOrEmpty(n.MailingAddress.TerritoryAbbreviatedName)) TerrAbb = n.MailingAddress.TerritoryAbbreviatedName;
                                   if (!string.IsNullOrEmpty(n.MailingAddress.PostalCode)) PostalCode = n.MailingAddress.PostalCode;
                                   if (!string.IsNullOrEmpty(n.MailingAddress.PostalCodeExtensionCode)) PostalCodeExt = n.MailingAddress.PostalCodeExtensionCode;
                                   if (!string.IsNullOrEmpty(LineText))
                                   {
                                       address = LineText + ", " + n.MailingAddress.PrimaryTownName + " " + TerrAbb + " " + PostalCode + " " + PostalCodeExt + " " + n.MailingAddress.CountryISOAlpha2Code;
                                       model.DnBMailingAddress = address;
                                       model.DnBMailingAddressUndeliverable = n.MailingAddress.UndeliverableIndicator;
                                   }
                               }

                               model.DnBMarketabilityIndicator = n.MarketabilityIndicator;

                               response.MatchEntities.Add(model);

                           });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            if (response != null)
                response.TransactionResponseDetail = objResponse;
            return response;
        }
        public APIResponse GetMatchByRegistrationNo(string country, string connectionString = "", string RegistrationNumber = "", int CandidateMaximumQuantity = 10)
        {
            APIResponse response = new APIResponse();
            response.DnbMatchModels = new List<DnbMatchModel>();
            response.MatchEntities = new List<MatchEntity>();
            TransactionResponseDetail objResponse = new TransactionResponseDetail();
            GetCleanseMatchResponseMain objCleanseMatch = null;
            GetCleanseMatchResponse companyinfo_res = null;
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            try
            {
                DataTable dt = new DataTable();
                SettingFacade sfac = new SettingFacade(connectionString);
                string endPoint = sfac.GetURLEncode("", "", "", "", "", "", "", country.ToString(), "", "", "", "Direct20", 0, null, null, null, "0", RegistrationNumber);
                //endPoint = endPoint + "&CandidateMaximumQuantity=" + CandidateMaximumQuantity;
                endPoint = endPoint.Replace("CandidateMaximumQuantity=50", "CandidateMaximumQuantity=" + CandidateMaximumQuantity);


                var httpWebRequest = (HttpWebRequest)WebRequest.Create(endPoint);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "GET";
                httpWebRequest.Headers.Add("Authorization", CommonMethod.GetThirdPartyProperty(ThirdPartyCode.DNB_SINGLE_ENTITY_SEARCH.ToString(), ThirdPartyProperties.AuthToken.ToString()));

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    result = purgeContent(result);
                    response.ResponseJSON = result;
                    var serializer = new JavaScriptSerializer();
                    objCleanseMatch = serializer.Deserialize<GetCleanseMatchResponseMain>(result);
                    if (objCleanseMatch != null)
                        companyinfo_res = objCleanseMatch.GetCleanseMatchResponse;
                }

                if (companyinfo_res != null)
                {
                    response.APIRequest = endPoint;
                    objResponse.MatchDataCriteriaText = string.Empty;
                    objResponse.MatchedQuantity = 0;
                    if (companyinfo_res.TransactionDetail != null)
                    {
                        objResponse.ServiceTransactionID = companyinfo_res.TransactionDetail.ServiceTransactionID;
                        objResponse.TransactionTimestamp = Convert.ToDateTime(companyinfo_res.TransactionDetail.TransactionTimestamp);
                    }
                    if (companyinfo_res.TransactionResult != null)
                    {
                        objResponse.SeverityText = Convert.ToString(companyinfo_res.TransactionResult.SeverityText);
                        objResponse.ResultID = companyinfo_res.TransactionResult.ResultID;
                        objResponse.ResultText = companyinfo_res.TransactionResult.ResultText;
                    }
                    GetCleanseMatchResponseDetail companyinfo_wrapper = companyinfo_res.GetCleanseMatchResponseDetail;
                    if (companyinfo_wrapper != null)
                    {
                        if (companyinfo_wrapper.MatchResponseDetail != null && companyinfo_wrapper.MatchResponseDetail.MatchDataCriteriaText != null)
                        {
                            objResponse.MatchDataCriteriaText = companyinfo_wrapper.MatchResponseDetail.MatchDataCriteriaText._param;
                            objResponse.MatchedQuantity = companyinfo_wrapper.MatchResponseDetail.CandidateMatchedQuantity;
                        }
                        else
                        {
                            objResponse.MatchDataCriteriaText = string.Empty;
                            objResponse.MatchedQuantity = 0;
                        }
                        if (companyinfo_wrapper.MatchResponseDetail != null)
                        {
                            companyinfo_wrapper.MatchResponseDetail.MatchCandidate.ToList().
                           ForEach(n =>
                           {
                               string streetAddress = string.Empty;
                               MatchEntity model = new MatchEntity();
                               model.TransactionTimestamp = Convert.ToDateTime(objResponse.TransactionTimestamp);
                               model.DnBDUNSNumber = n.DUNSNumber;
                               if (n.OrganizationPrimaryName != null && n.OrganizationPrimaryName._OrganizationName != null)
                                   model.DnBOrganizationName = n.OrganizationPrimaryName._OrganizationName._param;
                               if (n.PrimaryAddress != null)
                               {
                                   if (n.PrimaryAddress.StreetAddressLine != null && n.PrimaryAddress.StreetAddressLine.Any())
                                   {
                                       n.PrimaryAddress.StreetAddressLine.ToList().ForEach(s =>
                                       {
                                           streetAddress += s.LineText + ",";
                                       });
                                       model.DnBStreetAddressLine = streetAddress.TrimEnd(',');
                                   }

                                   model.DnBPrimaryTownName = n.PrimaryAddress.PrimaryTownName;
                                   model.DnBPostalCode = n.PrimaryAddress.PostalCode;
                                   model.DnBPostalCodeExtensionCode = n.PrimaryAddress.PostalCodeExtensionCode;
                                   model.DnBCountryISOAlpha2Code = Convert.ToString(n.PrimaryAddress.CountryISOAlpha2Code);
                                   model.DnBTerritoryAbbreviatedName = n.PrimaryAddress.TerritoryAbbreviatedName;
                                   model.DnBAddressUndeliverable = Convert.ToString(n.PrimaryAddress.UndeliverableIndicator);
                               }

                               if (n.TelephoneNumber != null)
                               {
                                   model.DnBTelephoneNumber = n.TelephoneNumber.TelecommunicationNumber;
                                   model.DnBTelephoneNumberUnreachableIndicator = n.TelephoneNumber.UnreachableIndicator;
                               }

                               model.DnBStandaloneOrganization = Convert.ToString(n.StandaloneOrganizationIndicator);
                               if (n.StandaloneOrganizationIndicator)
                               {
                                   model.DnBFamilyTreeMemberRole = "Single Location";
                               }
                               else if (n.FamilyTreeMemberRole != null && n.FamilyTreeMemberRole.Any())
                               {
                                   model.DnBFamilyTreeMemberRole = n.FamilyTreeMemberRole[0].FamilyTreeMemberRoleText._param;
                               }

                               if (n.OperatingStatusText != null)
                               {
                                   model.DnBOperatingStatus = n.OperatingStatusText._param;
                               }
                               model.IsSelected = false;
                               if (n.MatchQualityInformation != null)
                               {
                                   model.DnBConfidenceCode = n.MatchQualityInformation.ConfidenceCodeValue;
                                   model.DnBMatchGradeText = n.MatchQualityInformation.MatchGradeText;
                                   model.DnBMatchDataProfileText = n.MatchQualityInformation.MatchDataProfileText;
                                   if (!string.IsNullOrWhiteSpace(model.DnBMatchGradeText))
                                   {
                                       string matchgradeText = model.DnBMatchGradeText;
                                       if (!string.IsNullOrEmpty(matchgradeText) && matchgradeText.Length >= 8)
                                       {
                                           model.MGCompanyName = matchgradeText.Substring(0, 1);
                                           model.MGStreetNo = matchgradeText.Substring(1, 1);
                                           model.MGStreetName = matchgradeText.Substring(2, 1);
                                           model.MGCity = matchgradeText.Substring(3, 1);
                                           model.MGState = matchgradeText.Substring(4, 1);
                                           model.MGZipCode = matchgradeText.Substring(7, 1);
                                           model.MGTelephone = matchgradeText.Substring(6, 1);
                                           model.MGMailingAddress = matchgradeText.Substring(5, 1);
                                           model.MGDensity = matchgradeText.Substring(8, 1);
                                           model.MGUniqueness = matchgradeText.Substring(9, 1);
                                           model.MGSIC = matchgradeText.Substring(10, 1);
                                       }
                                       else if (!string.IsNullOrEmpty(matchgradeText) && matchgradeText.Length >= 7)
                                       {
                                           model.MGCompanyName = matchgradeText.Substring(0, 1);
                                           model.MGStreetNo = matchgradeText.Substring(1, 1);
                                           model.MGStreetName = matchgradeText.Substring(2, 1);
                                           model.MGCity = matchgradeText.Substring(3, 1);
                                           model.MGState = matchgradeText.Substring(4, 1);
                                           model.MGMailingAddress = matchgradeText.Substring(5, 1);
                                           model.MGTelephone = matchgradeText.Substring(6, 1);
                                       }
                                   }
                                   if (!string.IsNullOrEmpty(model.DnBMatchDataProfileText))
                                   {
                                       model.DnBMatchDataProfileComponentCount = n.MatchQualityInformation.MatchDataProfileComponentCount;
                                       string matchText = model.DnBMatchDataProfileText;
                                       if (matchText.Length >= 14)
                                       {
                                           model.MDPVCompanyName = matchText.Substring(0, 2);
                                           model.MDPVStreetNo = matchText.Substring(2, 2);
                                           model.MDPVStreetName = matchText.Substring(4, 2);
                                           model.MDPVCity = matchText.Substring(6, 2);
                                           model.MDPVState = matchText.Substring(8, 2);
                                           model.MDPVMailingAddress = matchText.Substring(10, 2);
                                           model.MDPVTelephone = matchText.Substring(12, 2);
                                           model.MDPText = matchText.Substring(0, 2) + " " + matchText.Substring(2, 2) + "-" + matchText.Substring(4, 2) + "-" + matchText.Substring(6, 2) + "-" + matchText.Substring(8, 2) + " " + matchText.Substring(12, 2);
                                       }
                                   }
                                   if (n.MatchQualityInformation.MatchGradeComponent != null && n.MatchQualityInformation.MatchGradeComponent.Any())
                                   {
                                       model.ScoreCompany = Convert.ToString(n.MatchQualityInformation.MatchGradeComponent.FirstOrDefault().MatchGradeComponentScore);
                                   }
                               }
                               model.DnBDisplaySequence = Convert.ToString(n.DisplaySequence);
                               if (n.TradeStyleName != null && n.TradeStyleName.OrganizationName != null)
                                   model.DnBTradeStyleName = n.TradeStyleName.OrganizationName._param;

                               if (n.SeniorPrincipalName != null)
                                   model.DnBSeniorPrincipalName = n.SeniorPrincipalName.FullName;

                               if (n.MailingAddress != null && n.MailingAddress.StreetAddressLine != null && n.MailingAddress.StreetAddressLine.Any())
                               {
                                   string maddress, TerrAbb = "", PostalCode = "", PostalCodeExt = "";
                                   string LineText = n.MailingAddress.StreetAddressLine.FirstOrDefault().LineText;
                                   if (!string.IsNullOrEmpty(n.MailingAddress.TerritoryAbbreviatedName)) TerrAbb = n.MailingAddress.TerritoryAbbreviatedName;
                                   if (!string.IsNullOrEmpty(n.MailingAddress.PostalCode)) PostalCode = n.MailingAddress.PostalCode;
                                   if (!string.IsNullOrEmpty(n.MailingAddress.PostalCodeExtensionCode)) PostalCodeExt = n.MailingAddress.PostalCodeExtensionCode;
                                   if (!string.IsNullOrEmpty(LineText))
                                   {
                                       maddress = LineText + ", " + n.MailingAddress.PrimaryTownName + " " + TerrAbb + " " + PostalCode + " " + PostalCodeExt + " " + n.MailingAddress.CountryISOAlpha2Code;
                                       model.DnBMailingAddress = maddress;
                                       model.DnBMailingAddressUndeliverable = n.MailingAddress.UndeliverableIndicator;
                                   }
                               }

                               model.DnBMarketabilityIndicator = n.MarketabilityIndicator;

                               response.MatchEntities.Add(model);

                           });
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                response = null;
            }
            if (response != null)
                response.TransactionResponseDetail = objResponse;
            return response;
        }

        #region EncryptDecrypt
        public static string GetEncryptedString(string strValue)
        {
            return StringCipher.Encrypt(strValue, General.passPhrase);
        }
        public static string GetDecryptedString(string strValue)
        {
            return StringCipher.Decrypt(strValue, General.passPhrase);
        }
        #endregion

        #region "Direct +"

        #region "Authorization"      
        public dnb_Authentication DirectPlusAuth(String consumerKey, String consumerSecret, out string Result, out string ErrorMessage)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                Result = string.Empty;
                ErrorMessage = string.Empty;
                string url = DirectPlus.DirectPLUSAuth;
                var webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Method = "POST";
                webRequest.ContentType = "application/json";
                string autorization = consumerKey + ":" + consumerSecret;
                byte[] binaryAuthorization = System.Text.Encoding.Default.GetBytes(autorization);
                autorization = Convert.ToBase64String(binaryAuthorization);
                webRequest.Headers.Add("Authorization", "Basic " + autorization);
                webRequest.Headers.Add("Origin", "www.dnb.com");
                using (var streamWriter = new StreamWriter(webRequest.GetRequestStream(), Encoding.UTF8))
                {
                    string json_str = "{ \"grant_type\" : \"client_credentials\" }";
                    streamWriter.Write(json_str);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                var webResponse = (HttpWebResponse)webRequest.GetResponse();
                if (webResponse.StatusCode != HttpStatusCode.OK) Console.WriteLine("{0}", webResponse.Headers);
                using (StreamReader reader = new StreamReader(webResponse.GetResponseStream()))
                {
                    Result = reader.ReadToEnd();
                    var serializer = new JavaScriptSerializer();
                    return serializer.Deserialize<dnb_Authentication>(Result);

                }
            }
            catch (WebException e)
            {
                using (WebResponse response = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        ErrorMessage = reader.ReadToEnd();
                    }
                    Result = "";
                    var serializer = new JavaScriptSerializer();
                    return serializer.Deserialize<dnb_Authentication>(Result);
                }
            }
        }
        #endregion

        #region "Cleanse Match Identity resolution"
        public APIResponse GetCleanseMatchResultDirectPlus(string companyName, string address, string Address2, string city, string state, CountryGroupModel.CountryISOAlpha2Enum country, string zipcode, string telephon, bool ExcludeNonHeadQuarters = false, bool ExcludeNonMarketable = false, bool ExcludeOutofBusiness = false, bool ExcludeUndeliverable = false, bool ExcludeUnreachable = false, string inLanguage = "", int candidateMaxQuantity = 10, int confidenceLowerLevelThresholdValue = 4, string connectionString = "", string InputId = null)
        {
            APIResponse response = new APIResponse();
            response.DnbMatchModels = new List<DnbMatchModel>();
            response.MatchEntities = new List<MatchEntity>();
            TransactionResponseDetail objResponse = new TransactionResponseDetail();
            IdentityResolutionResponse objCleanseMatch = null;
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            try
            {
                DataTable dt = new DataTable();
                SettingFacade sfac = new SettingFacade(connectionString);
                string endPoint = sfac.GetURLEncode("", companyName, address, Address2, city, state, zipcode, country.ToString(), telephon, "", inLanguage, "DirectPlus", confidenceLowerLevelThresholdValue, null, null, null, InputId);


                string exclusionCriteria = "";
                if (ExcludeNonHeadQuarters) { exclusionCriteria = exclusionCriteria + ",ExcludeNonHeadQuarters"; }
                if (ExcludeNonMarketable) { exclusionCriteria = exclusionCriteria + ",ExcludeNonMarketable"; }
                if (ExcludeOutofBusiness) { exclusionCriteria = exclusionCriteria + ",ExcludeOutofBusiness"; }
                if (ExcludeUndeliverable) { exclusionCriteria = exclusionCriteria + ",ExcludeUndeliverable"; }
                if (ExcludeUnreachable) { exclusionCriteria = exclusionCriteria + ",ExcludeUnreachable"; }

                if (exclusionCriteria != string.Empty)
                    endPoint = endPoint + "&exclusionCriteria=" + exclusionCriteria.TrimStart(',');

                endPoint = endPoint.Replace("candidateMaximumQuantity=50", "candidateMaximumQuantity=" + candidateMaxQuantity);

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(endPoint);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "GET";
                httpWebRequest.Headers.Add("Authorization", CommonMethod.GetThirdPartyProperty(ThirdPartyCode.DNB_SINGLE_ENTITY_SEARCH.ToString(), ThirdPartyProperties.AuthToken.ToString()));

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    result = purgeContent(result);
                    response.ResponseJSON = result;
                    var serializer = new JavaScriptSerializer();
                    var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore };
                    objCleanseMatch = JsonConvert.DeserializeObject<IdentityResolutionResponse>(result, settings);
                }

                if (objCleanseMatch != null)
                {
                    response.APIRequest = endPoint;
                    objResponse.MatchDataCriteriaText = objCleanseMatch.matchDataCriteria;
                    objResponse.MatchedQuantity = objCleanseMatch.candidatesMatchedQuantity;
                    if (objCleanseMatch.transactionDetail != null)
                    {
                        objResponse.ServiceTransactionID = objCleanseMatch.transactionDetail.transactionID;
                        objResponse.TransactionTimestamp = Convert.ToDateTime(objCleanseMatch.transactionDetail.transactionTimestamp);
                    }
                    if (objCleanseMatch.matchCandidates != null)
                    {
                        objCleanseMatch.matchCandidates.ToList().
                       ForEach(n =>
                       {
                           string streetAddress = string.Empty;
                           MatchEntity model = new MatchEntity();
                           model.TransactionTimestamp = Convert.ToDateTime(objResponse.TransactionTimestamp);
                           if (n.organization != null)
                           {
                               model.DnBDUNSNumber = n.organization.duns;
                               model.DnBOrganizationName = n.organization.primaryName;
                               if (n.organization.primaryAddress != null)
                               {
                                   if (n.organization.primaryAddress.streetAddress != null)
                                   {
                                       model.DnBStreetAddressLine = n.organization.primaryAddress.streetAddress.line1 + ", " + n.organization.primaryAddress.streetAddress.line2;
                                   }
                                   if (n.organization.primaryAddress.addressLocality != null)
                                       model.DnBPrimaryTownName = n.organization.primaryAddress.addressLocality.name;

                                   model.DnBPostalCode = n.organization.primaryAddress.postalCode;
                                   model.DnBPostalCodeExtensionCode = Convert.ToString(n.organization.primaryAddress.postalCodeExtension);
                                   if (n.organization.primaryAddress.addressCountry != null)
                                   {
                                       model.DnBCountryISOAlpha2Code = n.organization.primaryAddress.addressCountry.isoAlpha2Code;
                                   }
                                   if (n.organization.primaryAddress.addressRegion != null)
                                   {
                                       model.DnBTerritoryAbbreviatedName = Convert.ToString(n.organization.primaryAddress.addressRegion.abbreviatedName);
                                       if (string.IsNullOrEmpty(model.DnBTerritoryAbbreviatedName))
                                       {
                                           model.DnBTerritoryAbbreviatedName = Convert.ToString(n.organization.primaryAddress.addressRegion.name);
                                       }
                                   }
                               }

                               // need to confirm - model.DnBAddressUndeliverable = Convert.ToString(n.PrimaryAddress.UndeliverableIndicator);
                               if (n.organization.telephone != null && n.organization.telephone.Any())
                               {
                                   model.DnBTelephoneNumber = n.organization.telephone.FirstOrDefault().telephoneNumber; // need to confirm to take first or not
                                   model.DnBTelephoneNumberUnreachableIndicator = n.organization.telephone.FirstOrDefault().isUnreachable; // need to confirm to take first or not
                               }

                               if (Convert.ToBoolean(n.organization.isStandalone))
                               {
                                   model.DnBFamilyTreeMemberRole = "Single Location";
                               }
                               else
                               {
                                   if (n.organization != null && n.organization.corporateLinkage != null && n.organization.corporateLinkage.familytreeRolesPlayed != null && n.organization.corporateLinkage.familytreeRolesPlayed.Any())
                                       model.DnBFamilyTreeMemberRole = n.organization.corporateLinkage.familytreeRolesPlayed.FirstOrDefault().description.ToString();
                               }

                               if (n.organization.tradeStyleNames != null && n.organization.tradeStyleNames.Any())
                                   model.DnBTradeStyleName = n.organization.tradeStyleNames.FirstOrDefault().name.ToString();

                               if (n.organization.mostSeniorPrincipals != null && n.organization.mostSeniorPrincipals.Any())
                                   model.DnBSeniorPrincipalName = n.organization.mostSeniorPrincipals.FirstOrDefault().fullName.ToString();


                               if (n.organization.dunsControlStatus != null)
                               {
                                   if (n.organization.dunsControlStatus.operatingStatus != null)
                                   {
                                       model.DnBOperatingStatus = n.organization.dunsControlStatus.operatingStatus.description;
                                   }
                               }
                           }

                           model.IsSelected = false;
                           if (n.matchQualityInformation != null)
                           {
                               model.ScoreCompany = Convert.ToString(n.matchQualityInformation.nameMatchScore);
                               model.DnBConfidenceCode = n.matchQualityInformation.confidenceCode;
                               model.DnBMatchGradeText = n.matchQualityInformation.matchGrade;
                               model.DnBMatchDataProfileText = n.matchQualityInformation.matchDataProfile;
                               if (!string.IsNullOrWhiteSpace(model.DnBMatchGradeText))
                               {
                                   string matchgradeText = model.DnBMatchGradeText;
                                   if (!string.IsNullOrEmpty(matchgradeText) && matchgradeText.Length >= 8)
                                   {
                                       model.MGCompanyName = matchgradeText.Substring(0, 1);
                                       model.MGStreetNo = matchgradeText.Substring(1, 1);
                                       model.MGStreetName = matchgradeText.Substring(2, 1);
                                       model.MGCity = matchgradeText.Substring(3, 1);
                                       model.MGState = matchgradeText.Substring(4, 1);
                                       model.MGZipCode = matchgradeText.Substring(7, 1);
                                       model.MGTelephone = matchgradeText.Substring(6, 1);
                                       model.MGMailingAddress = matchgradeText.Substring(5, 1);
                                       model.MGDensity = matchgradeText.Substring(8, 1);
                                       model.MGUniqueness = matchgradeText.Substring(9, 1);
                                       model.MGSIC = matchgradeText.Substring(10, 1);
                                   }
                                   else if (!string.IsNullOrEmpty(matchgradeText) && matchgradeText.Length >= 7)
                                   {
                                       model.MGCompanyName = matchgradeText.Substring(0, 1);
                                       model.MGStreetNo = matchgradeText.Substring(1, 1);
                                       model.MGStreetName = matchgradeText.Substring(2, 1);
                                       model.MGCity = matchgradeText.Substring(3, 1);
                                       model.MGState = matchgradeText.Substring(4, 1);
                                       model.MGMailingAddress = matchgradeText.Substring(5, 1);
                                       model.MGTelephone = matchgradeText.Substring(6, 1);
                                   }
                               }
                               if (!string.IsNullOrEmpty(model.DnBMatchDataProfileText))
                               {
                                   model.DnBMatchDataProfileComponentCount = n.matchQualityInformation.matchDataProfileComponentsCount;
                                   string matchText = model.DnBMatchDataProfileText;
                                   if (model.DnBMatchDataProfileComponentCount >= 14)
                                   {
                                       model.MDPVCompanyName = matchText.Substring(0, 2);
                                       model.MDPVStreetNo = matchText.Substring(2, 2);
                                       model.MDPVStreetName = matchText.Substring(4, 2);
                                       model.MDPVCity = matchText.Substring(6, 2);
                                       model.MDPVState = matchText.Substring(8, 2);
                                       model.MDPVMailingAddress = matchText.Substring(10, 2);
                                       model.MDPVTelephone = matchText.Substring(12, 2);
                                       model.MDPVZipCode = matchText.Substring(14, 2);
                                       model.MDPVSIC = matchText.Substring(18, 2);
                                       model.MDPVDensity = matchText.Substring(20, 2);
                                       model.MDPVUniqueness = matchText.Substring(22, 2);
                                       model.MDPText = matchText.Substring(0, 2) + " " + matchText.Substring(2, 2) + "-" + matchText.Substring(4, 2) + "-" + matchText.Substring(6, 2) + "-" + matchText.Substring(8, 2) + " " + matchText.Substring(12, 2);
                                   }
                                   else
                                   {
                                       model.MDPVCompanyName = matchText.Substring(0, 2);
                                       model.MDPVStreetNo = matchText.Substring(2, 2);
                                       model.MDPVStreetName = matchText.Substring(4, 2);
                                       model.MDPVCity = matchText.Substring(6, 2);
                                       model.MDPVState = matchText.Substring(8, 2);
                                       model.MDPVMailingAddress = matchText.Substring(10, 2);
                                       model.MDPVTelephone = matchText.Substring(12, 2);
                                       model.MDPText = matchText.Substring(0, 2) + " " + matchText.Substring(2, 2) + "-" + matchText.Substring(4, 2) + "-" + matchText.Substring(6, 2) + "-" + matchText.Substring(8, 2) + " " + matchText.Substring(12, 2);
                                   }
                               }

                           }
                           model.DnBDisplaySequence = Convert.ToString(n.displaySequence);


                           if (n.organization.mailingAddress != null && n.organization.mailingAddress.streetAddress != null)
                           {
                               string maddress, TerrAbb = "", PostalCode = "", PrimaryTown = "", Country = "";
                               string LineText = n.organization.mailingAddress.streetAddress.line1;
                               if (n.organization.mailingAddress.addressRegion != null) TerrAbb = Convert.ToString(n.organization.mailingAddress.addressRegion.abbreviatedName);
                               if (!string.IsNullOrEmpty(n.organization.primaryAddress.postalCode)) PostalCode = n.organization.mailingAddress.postalCode;

                               if (n.organization.mailingAddress.addressLocality != null) PrimaryTown = n.organization.mailingAddress.addressLocality.name;
                               if (n.organization.mailingAddress.addressCountry != null) Country = n.organization.mailingAddress.addressCountry.isoAlpha2Code;
                               if (!string.IsNullOrEmpty(LineText))
                               {
                                   maddress = LineText + ", " + PrimaryTown + " " + TerrAbb + " " + PostalCode + " " + Country;
                                   model.DnBMailingAddress = maddress;
                                   // model.DnBMailingAddressUndeliverable = n.organization.mailingAddress.;
                               }
                           }

                           if (n.organization != null && n.organization.registrationNumbers.Any())
                           {
                               model.RegistrationNumbers = n.organization.registrationNumbers.FirstOrDefault().registrationNumber;
                           }

                           if (n.organization != null && n.organization.websiteAddress.Any())
                           {
                               model.WebsiteURL = n.organization.websiteAddress.FirstOrDefault().url;
                           }

                           //model.DnBMarketabilityIndicator = n.MarketabilityIndicator ? "Marketable" : "Non Marketable";

                           response.MatchEntities.Add(model);

                       });
                    }
                }
            }
            catch (WebException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                response = null;
            }
            if (response != null)
                response.TransactionResponseDetail = objResponse;
            return response;
        }
        #endregion

        #region "Match - Search By DUNS"

        public APIResponse GetMatchResultDirectPlus(string dunsNumber, string connectionString = "", int confidenceLowerLevelThresholdValue = 4)
        {
            APIResponse response = new APIResponse();
            response.DnbMatchModels = new List<DnbMatchModel>();
            response.MatchEntities = new List<MatchEntity>();
            TransactionResponseDetail objResponse = new TransactionResponseDetail();
            IdentityResolutionResponse objMatch = null;
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            try
            {

                //string endPoint = DirectPlus.CleanseMatchURL;
                SettingFacade sfac = new SettingFacade(connectionString);
                string endPoint = sfac.GetURLEncode("", "", "", "", "", "", "", "", "", "", "", "DirectPlus", confidenceLowerLevelThresholdValue, dunsNumber, null, null);

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(endPoint);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "GET";
                httpWebRequest.Headers.Add("Authorization", CommonMethod.GetThirdPartyProperty(ThirdPartyCode.DNB_SINGLE_ENTITY_SEARCH.ToString(), ThirdPartyProperties.AuthToken.ToString()));

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    result = purgeContent(result);
                    response.ResponseJSON = result;
                    var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore };
                    objMatch = JsonConvert.DeserializeObject<IdentityResolutionResponse>(result, settings);
                }

                if (objMatch != null)
                {
                    response.APIRequest = endPoint;
                    objResponse.MatchDataCriteriaText = objMatch.matchDataCriteria;
                    objResponse.MatchedQuantity = objMatch.candidatesMatchedQuantity;
                    if (objMatch.transactionDetail != null)
                    {
                        objResponse.ServiceTransactionID = objMatch.transactionDetail.transactionID;
                        objResponse.TransactionTimestamp = Convert.ToDateTime(objMatch.transactionDetail.transactionTimestamp);
                    }
                    if (objMatch.matchCandidates != null)
                    {
                        objMatch.matchCandidates.ToList().
                   ForEach(n =>
                   {
                       string streetAddress = string.Empty;
                       MatchEntity model = new MatchEntity();
                       model.TransactionTimestamp = Convert.ToDateTime(objResponse.TransactionTimestamp);
                       if (n.organization != null)
                       {
                           model.DnBDUNSNumber = n.organization.duns;
                           model.DnBOrganizationName = n.organization.primaryName;
                           if (n.organization.primaryAddress != null)
                           {
                               if (n.organization.primaryAddress.streetAddress != null)
                               {
                                   model.DnBStreetAddressLine = n.organization.primaryAddress.streetAddress.line1 + ", " + n.organization.primaryAddress.streetAddress.line2;
                               }
                               if (n.organization.primaryAddress.addressLocality != null)
                                   model.DnBPrimaryTownName = n.organization.primaryAddress.addressLocality.name;

                               model.DnBPostalCode = n.organization.primaryAddress.postalCode;
                               model.DnBPostalCodeExtensionCode = Convert.ToString(n.organization.primaryAddress.postalCodeExtension);
                               if (n.organization.primaryAddress.addressCountry != null)
                               {
                                   model.DnBCountryISOAlpha2Code = n.organization.primaryAddress.addressCountry.isoAlpha2Code;
                               }
                               if (n.organization.primaryAddress.addressRegion != null)
                               {
                                   model.DnBTerritoryAbbreviatedName = Convert.ToString(n.organization.primaryAddress.addressRegion.abbreviatedName);
                               }
                           }

                           // need to confirm - model.DnBAddressUndeliverable = Convert.ToString(n.PrimaryAddress.UndeliverableIndicator);

                           if (n.organization.telephone != null && n.organization.telephone.Any())
                           {
                               model.DnBTelephoneNumber = n.organization.telephone.FirstOrDefault().telephoneNumber; // need to confirm to take first or not
                               model.DnBTelephoneNumberUnreachableIndicator = n.organization.telephone.FirstOrDefault().isUnreachable; // need to confirm to take first or not
                           }

                           if (Convert.ToBoolean(n.organization.isStandalone))
                           {
                               model.DnBFamilyTreeMemberRole = "Single Location";
                           }
                           else
                           {
                               if (n.organization != null && n.organization.corporateLinkage != null && n.organization.corporateLinkage.familytreeRolesPlayed != null && n.organization.corporateLinkage.familytreeRolesPlayed.Any())
                                   model.DnBFamilyTreeMemberRole = n.organization.corporateLinkage.familytreeRolesPlayed.FirstOrDefault().description.ToString();
                           }

                           if (n.organization.tradeStyleNames != null && n.organization.tradeStyleNames.Any())
                               model.DnBTradeStyleName = n.organization.tradeStyleNames.FirstOrDefault().name.ToString();

                           if (n.organization.mostSeniorPrincipals != null && n.organization.mostSeniorPrincipals.Any())
                               model.DnBSeniorPrincipalName = n.organization.mostSeniorPrincipals.FirstOrDefault().fullName.ToString();


                           if (n.organization.dunsControlStatus != null)
                           {
                               if (n.organization.dunsControlStatus.operatingStatus != null)
                               {
                                   model.DnBOperatingStatus = n.organization.dunsControlStatus.operatingStatus.description;
                               }
                           }
                       }

                       model.IsSelected = false;
                       if (n.matchQualityInformation == null)
                       {
                           SBISCCMWeb.Utility.IdentityResolution.MatchQualityInformation objMatchQuality = new SBISCCMWeb.Utility.IdentityResolution.MatchQualityInformation();
                           objMatchQuality.confidenceCode = 10;
                           objMatchQuality.matchGrade = "AAAAAAAAAAA";
                           objMatchQuality.matchDataProfile = "98989898989898";
                           n.matchQualityInformation = objMatchQuality;

                       }
                       if (n.matchQualityInformation != null)
                       {
                           model.ScoreCompany = Convert.ToString(n.matchQualityInformation.nameMatchScore);
                           model.DnBConfidenceCode = n.matchQualityInformation.confidenceCode;
                           model.DnBMatchGradeText = n.matchQualityInformation.matchGrade;
                           if (string.IsNullOrEmpty(model.DnBMatchGradeText))
                               model.DnBMatchGradeText = "AAAAAAAAAAA";
                           model.DnBMatchDataProfileText = n.matchQualityInformation.matchDataProfile;
                           if (string.IsNullOrEmpty(model.DnBMatchDataProfileText))
                               model.DnBMatchDataProfileText = "98989898989898";

                           if (!string.IsNullOrWhiteSpace(model.DnBMatchGradeText))
                           {
                               string matchgradeText = model.DnBMatchGradeText;
                               if (!string.IsNullOrEmpty(matchgradeText) && matchgradeText.Length >= 8)
                               {
                                   model.MGCompanyName = matchgradeText.Substring(0, 1);
                                   model.MGStreetNo = matchgradeText.Substring(1, 1);
                                   model.MGStreetName = matchgradeText.Substring(2, 1);
                                   model.MGCity = matchgradeText.Substring(3, 1);
                                   model.MGState = matchgradeText.Substring(4, 1);
                                   model.MGZipCode = matchgradeText.Substring(7, 1);
                                   model.MGTelephone = matchgradeText.Substring(6, 1);
                                   model.MGMailingAddress = matchgradeText.Substring(5, 1);
                                   model.MGDensity = matchgradeText.Substring(8, 1);
                                   model.MGUniqueness = matchgradeText.Substring(9, 1);
                                   model.MGSIC = matchgradeText.Substring(10, 1);
                               }
                               else if (!string.IsNullOrEmpty(matchgradeText) && matchgradeText.Length >= 7)
                               {
                                   model.MGCompanyName = matchgradeText.Substring(0, 1);
                                   model.MGStreetNo = matchgradeText.Substring(1, 1);
                                   model.MGStreetName = matchgradeText.Substring(2, 1);
                                   model.MGCity = matchgradeText.Substring(3, 1);
                                   model.MGState = matchgradeText.Substring(4, 1);
                                   model.MGMailingAddress = matchgradeText.Substring(5, 1);
                                   model.MGTelephone = matchgradeText.Substring(6, 1);
                               }
                           }
                           if (!string.IsNullOrEmpty(model.DnBMatchDataProfileText))
                           {
                               model.DnBMatchDataProfileComponentCount = n.matchQualityInformation.matchDataProfileComponentsCount;
                               string matchText = model.DnBMatchDataProfileText;
                               if (matchText.Length >= 14)
                               {
                                   model.MDPVCompanyName = matchText.Substring(0, 2);
                                   model.MDPVStreetNo = matchText.Substring(2, 2);
                                   model.MDPVStreetName = matchText.Substring(4, 2);
                                   model.MDPVCity = matchText.Substring(6, 2);
                                   model.MDPVState = matchText.Substring(8, 2);
                                   model.MDPVMailingAddress = matchText.Substring(10, 2);
                                   model.MDPVTelephone = matchText.Substring(12, 2);
                                   model.MDPText = matchText.Substring(0, 2) + " " + matchText.Substring(2, 2) + "-" + matchText.Substring(4, 2) + "-" + matchText.Substring(6, 2) + "-" + matchText.Substring(8, 2) + " " + matchText.Substring(12, 2);
                               }
                           }
                       }
                       model.DnBDisplaySequence = Convert.ToString(n.displaySequence);


                       if (n.organization.mailingAddress != null && n.organization.mailingAddress.streetAddress != null)
                       {
                           string maddress, TerrAbb = "", PostalCode = "", PrimaryTown = "", Country = "";
                           string LineText = n.organization.mailingAddress.streetAddress.line1;
                           if (n.organization.mailingAddress.addressRegion != null) TerrAbb = Convert.ToString(n.organization.mailingAddress.addressRegion.abbreviatedName);
                           if (!string.IsNullOrEmpty(n.organization.primaryAddress.postalCode)) PostalCode = n.organization.mailingAddress.postalCode;

                           if (n.organization.mailingAddress.addressLocality != null) PrimaryTown = n.organization.mailingAddress.addressLocality.name;
                           if (n.organization.mailingAddress.addressCountry != null) Country = n.organization.mailingAddress.addressCountry.isoAlpha2Code;

                           if (!string.IsNullOrEmpty(LineText))
                           {
                               maddress = LineText + ", " + PrimaryTown + " " + TerrAbb + " " + PostalCode + " " + Country;
                               model.DnBMailingAddress = maddress;
                               // model.DnBMailingAddressUndeliverable = n.organization.mailingAddress.;
                           }
                       }

                       //model.DnBMarketabilityIndicator = n.MarketabilityIndicator ? "Marketable" : "Non Marketable";
                       response.MatchEntities.Add(model);

                   });
                    }
                }
            }
            catch (WebException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                response = null;
            }
            if (response != null)
                response.TransactionResponseDetail = objResponse;
            return response;
        }

        public APIResponse SearchByDomainOrEmail(string searchValue, string type, string connectionString = "", int confidenceLowerLevelThresholdValue = 4, string CountryCode = "")
        {
            APIResponse response = new APIResponse();
            response.MatchEntities = new List<MatchEntity>();
            TransactionResponseDetail objResponse = new TransactionResponseDetail();
            IdentityResolutionResponse objCleanseMatch = null;
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            try
            {
                //string endPoint = string.Format(DirectPlus.SearchByDomainOrEmail, type.ToLower(), searchValue);
                SettingFacade sfac = new SettingFacade(connectionString);
                string endPoint = "";
                if (type.ToLower() == "domain")
                {
                    endPoint = sfac.GetURLEncode("", "", "", "", "", "", "", CountryCode, "", "", "", "Directplus", confidenceLowerLevelThresholdValue, null, searchValue, null);
                }
                else
                {
                    endPoint = sfac.GetURLEncode("", "", "", "", "", "", "", CountryCode, "", "", "", "Directplus", confidenceLowerLevelThresholdValue, null, null, searchValue);
                }
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(endPoint);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "GET";
                httpWebRequest.Headers.Add("Authorization", CommonMethod.GetThirdPartyProperty(ThirdPartyCode.DNB_SINGLE_ENTITY_SEARCH.ToString(), ThirdPartyProperties.AuthToken.ToString()));
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    result = purgeContent(result);
                    response.ResponseJSON = result;
                    var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore };
                    objCleanseMatch = JsonConvert.DeserializeObject<IdentityResolutionResponse>(result, settings);
                }

                if (objCleanseMatch != null)
                {
                    response.APIRequest = endPoint;
                    objResponse.MatchDataCriteriaText = objCleanseMatch.matchDataCriteria;
                    objResponse.MatchedQuantity = objCleanseMatch.candidatesMatchedQuantity;
                    if (objCleanseMatch.transactionDetail != null)
                    {
                        objResponse.ServiceTransactionID = objCleanseMatch.transactionDetail.transactionID;
                        objResponse.TransactionTimestamp = Convert.ToDateTime(objCleanseMatch.transactionDetail.transactionTimestamp);
                    }
                    if (objCleanseMatch.matchCandidates != null)
                    {
                        objCleanseMatch.matchCandidates.ToList().
                       ForEach(n =>
                       {
                           string streetAddress = string.Empty;
                           MatchEntity model = new MatchEntity();
                           model.TransactionTimestamp = Convert.ToDateTime(objResponse.TransactionTimestamp);
                           if (n.organization != null)
                           {
                               model.DnBDUNSNumber = n.organization.duns;
                               model.DnBOrganizationName = n.organization.primaryName;
                               if (n.organization.primaryAddress != null)
                               {
                                   if (n.organization.primaryAddress.streetAddress != null)
                                   {
                                       model.DnBStreetAddressLine = n.organization.primaryAddress.streetAddress.line1 + ", " + n.organization.primaryAddress.streetAddress.line2;
                                   }
                                   if (n.organization.primaryAddress.addressLocality != null)
                                       model.DnBPrimaryTownName = n.organization.primaryAddress.addressLocality.name;

                                   model.DnBPostalCode = n.organization.primaryAddress.postalCode;
                                   model.DnBPostalCodeExtensionCode = Convert.ToString(n.organization.primaryAddress.postalCodeExtension);
                                   if (n.organization.primaryAddress.addressCountry != null)
                                   {
                                       model.DnBCountryISOAlpha2Code = n.organization.primaryAddress.addressCountry.isoAlpha2Code;
                                   }
                                   if (n.organization.primaryAddress.addressRegion != null)
                                   {
                                       model.DnBTerritoryAbbreviatedName = Convert.ToString(n.organization.primaryAddress.addressRegion.abbreviatedName);
                                   }
                               }

                               // need to confirm - model.DnBAddressUndeliverable = Convert.ToString(n.PrimaryAddress.UndeliverableIndicator);
                               if (n.organization.telephone != null && n.organization.telephone.Any())
                               {
                                   model.DnBTelephoneNumber = n.organization.telephone.FirstOrDefault().telephoneNumber; // need to confirm to take first or not
                                   model.DnBTelephoneNumberUnreachableIndicator = n.organization.telephone.FirstOrDefault().isUnreachable; // need to confirm to take first or not
                               }

                               if (Convert.ToBoolean(n.organization.isStandalone))
                               {
                                   model.DnBFamilyTreeMemberRole = "Single Location";
                               }
                               else
                               {
                                   if (n.organization != null && n.organization.corporateLinkage != null && n.organization.corporateLinkage.familytreeRolesPlayed != null && n.organization.corporateLinkage.familytreeRolesPlayed.Any())
                                       model.DnBFamilyTreeMemberRole = n.organization.corporateLinkage.familytreeRolesPlayed.FirstOrDefault().description.ToString();
                               }

                               if (n.organization.tradeStyleNames != null && n.organization.tradeStyleNames.Any())
                                   model.DnBTradeStyleName = n.organization.tradeStyleNames.FirstOrDefault().name.ToString();

                               if (n.organization.mostSeniorPrincipals != null && n.organization.mostSeniorPrincipals.Any())
                                   model.DnBSeniorPrincipalName = n.organization.mostSeniorPrincipals.FirstOrDefault().fullName.ToString();


                               if (n.organization.dunsControlStatus != null)
                               {
                                   if (n.organization.dunsControlStatus.operatingStatus != null)
                                   {
                                       model.DnBOperatingStatus = n.organization.dunsControlStatus.operatingStatus.description;
                                   }
                               }
                           }

                           model.IsSelected = false;
                           if (n.matchQualityInformation != null)
                           {
                               model.ScoreCompany = Convert.ToString(n.matchQualityInformation.nameMatchScore);
                               model.DnBConfidenceCode = n.matchQualityInformation.confidenceCode;
                               model.DnBMatchGradeText = n.matchQualityInformation.matchGrade;
                               model.DnBMatchDataProfileText = n.matchQualityInformation.matchDataProfile;
                               if (!string.IsNullOrWhiteSpace(model.DnBMatchGradeText))
                               {
                                   string matchgradeText = model.DnBMatchGradeText;
                                   if (!string.IsNullOrEmpty(matchgradeText) && matchgradeText.Length >= 8)
                                   {
                                       model.MGCompanyName = matchgradeText.Substring(0, 1);
                                       model.MGStreetNo = matchgradeText.Substring(1, 1);
                                       model.MGStreetName = matchgradeText.Substring(2, 1);
                                       model.MGCity = matchgradeText.Substring(3, 1);
                                       model.MGState = matchgradeText.Substring(4, 1);
                                       model.MGZipCode = matchgradeText.Substring(7, 1);
                                       model.MGTelephone = matchgradeText.Substring(6, 1);
                                       model.MGMailingAddress = matchgradeText.Substring(5, 1);
                                       model.MGDensity = matchgradeText.Substring(8, 1);
                                       model.MGUniqueness = matchgradeText.Substring(9, 1);
                                       model.MGSIC = matchgradeText.Substring(10, 1);
                                   }
                                   else if (!string.IsNullOrEmpty(matchgradeText) && matchgradeText.Length >= 7)
                                   {
                                       model.MGCompanyName = matchgradeText.Substring(0, 1);
                                       model.MGStreetNo = matchgradeText.Substring(1, 1);
                                       model.MGStreetName = matchgradeText.Substring(2, 1);
                                       model.MGCity = matchgradeText.Substring(3, 1);
                                       model.MGState = matchgradeText.Substring(4, 1);
                                       model.MGMailingAddress = matchgradeText.Substring(5, 1);
                                       model.MGTelephone = matchgradeText.Substring(6, 1);
                                   }
                               }
                               if (!string.IsNullOrEmpty(model.DnBMatchDataProfileText))
                               {
                                   model.DnBMatchDataProfileComponentCount = n.matchQualityInformation.matchDataProfileComponentsCount;
                                   string matchText = model.DnBMatchDataProfileText;
                                   if (matchText.Length >= 14)
                                   {
                                       model.MDPVCompanyName = matchText.Substring(0, 2);
                                       model.MDPVStreetNo = matchText.Substring(2, 2);
                                       model.MDPVStreetName = matchText.Substring(4, 2);
                                       model.MDPVCity = matchText.Substring(6, 2);
                                       model.MDPVState = matchText.Substring(8, 2);
                                       model.MDPVMailingAddress = matchText.Substring(10, 2);
                                       model.MDPVTelephone = matchText.Substring(12, 2);
                                       model.MDPText = matchText.Substring(0, 2) + " " + matchText.Substring(2, 2) + "-" + matchText.Substring(4, 2) + "-" + matchText.Substring(6, 2) + "-" + matchText.Substring(8, 2) + " " + matchText.Substring(12, 2);
                                   }
                               }

                               //if (n.matchQualityInformation.matchGradeComponents != null && n.matchQualityInformation.matchGradeComponents.Any())
                               //{
                               //    model.ScoreCompany = Convert.ToString(n.matchQualityInformation.matchGradeComponents.FirstOrDefault().componentRating);
                               //}

                           }
                           model.DnBDisplaySequence = Convert.ToString(n.displaySequence);


                           if (n.organization.mailingAddress != null && n.organization.mailingAddress.streetAddress != null)
                           {
                               string maddress, TerrAbb = "", PostalCode = "", PrimaryTown = "", Country = "";
                               string LineText = n.organization.mailingAddress.streetAddress.line1;
                               if (n.organization.mailingAddress.addressRegion != null) TerrAbb = Convert.ToString(n.organization.mailingAddress.addressRegion.abbreviatedName);
                               if (!string.IsNullOrEmpty(n.organization.primaryAddress.postalCode)) PostalCode = n.organization.mailingAddress.postalCode;

                               if (n.organization.mailingAddress.addressLocality != null) PrimaryTown = n.organization.mailingAddress.addressLocality.name;
                               if (n.organization.mailingAddress.addressCountry != null) Country = n.organization.mailingAddress.addressCountry.isoAlpha2Code;
                               if (!string.IsNullOrEmpty(LineText))
                               {
                                   maddress = LineText + ", " + PrimaryTown + " " + TerrAbb + " " + PostalCode + " " + Country;
                                   model.DnBMailingAddress = maddress;
                                   // model.DnBMailingAddressUndeliverable = n.organization.mailingAddress.;
                               }
                           }

                           //model.DnBMarketabilityIndicator = n.MarketabilityIndicator ? "Marketable" : "Non Marketable";

                           response.MatchEntities.Add(model);

                       });
                    }
                }
            }
            catch (WebException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                response = null;
            }
            if (response != null)
                response.TransactionResponseDetail = objResponse;
            return response;
        }


        #endregion


        #region "Build List"
        public SearchCriteriaResponse BuildAList(SearchCriteriaRequest objRequest)
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            string endpoint = DnBApi.DnbBuildAListURL;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(endpoint);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.Headers.Add("Authorization", CommonMethod.GetThirdPartyProperty(ThirdPartyCode.DNB_BUILD_A_LIST.ToString(), ThirdPartyProperties.AuthToken.ToString()));
            DataContractJsonSerializer ser = new DataContractJsonSerializer(objRequest.GetType());
            StreamWriter writer = new StreamWriter(httpWebRequest.GetRequestStream());

            string data = JsonConvert.SerializeObject(objRequest,
                              Newtonsoft.Json.Formatting.None,
                              new JsonSerializerSettings
                              {
                                  NullValueHandling = NullValueHandling.Ignore
                              });
            writer.Write(data);
            writer.Close();
            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var serializer = new JavaScriptSerializer();
                    SearchCriteriaResponse objResponse = serializer.Deserialize<SearchCriteriaResponse>(result);
                    return objResponse;
                }
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response.GetResponseStream())
                using (var streamReader = new StreamReader(stream))
                {
                    var result = streamReader.ReadToEnd();
                    if (result != null)
                    {
                        var serializer = new JavaScriptSerializer();
                        SearchCriteriaResponse objResponse = JsonConvert.DeserializeObject<SearchCriteriaResponse>(result);
                        return objResponse;
                    }
                }
            }
            return null;
        }
        #endregion

        #region "Match by registration no"
        public APIResponse GetMatchByRegistrationNoDirectPlus(string country, string connectionString = "", string RegistrationNo = "", int candidateMaxQuantity = 10)
        {
            APIResponse response = new APIResponse();
            response.DnbMatchModels = new List<DnbMatchModel>();
            response.MatchEntities = new List<MatchEntity>();
            TransactionResponseDetail objResponse = new TransactionResponseDetail();
            IdentityResolutionResponse objCleanseMatch = null;

            try
            {
                DataTable dt = new DataTable();
                SettingFacade sfac = new SettingFacade(connectionString);
                string endPoint = sfac.GetURLEncode("", "", "", "", "", "", "", country.ToString(), "", "", "", "DirectPlus", 1, null, null, null, "0", RegistrationNo);

                endPoint = endPoint.Replace("candidateMaximumQuantity=50", "candidateMaximumQuantity=" + candidateMaxQuantity);
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(endPoint);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "GET";
                httpWebRequest.Headers.Add("Authorization", CommonMethod.GetThirdPartyProperty(ThirdPartyCode.DNB_SINGLE_ENTITY_SEARCH.ToString(), ThirdPartyProperties.AuthToken.ToString()));

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    result = purgeContent(result);
                    response.ResponseJSON = result;
                    var serializer = new JavaScriptSerializer();
                    var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore };
                    objCleanseMatch = JsonConvert.DeserializeObject<IdentityResolutionResponse>(result, settings);
                }

                if (objCleanseMatch != null)
                {
                    response.APIRequest = endPoint;
                    objResponse.MatchDataCriteriaText = objCleanseMatch.matchDataCriteria;
                    objResponse.MatchedQuantity = objCleanseMatch.candidatesMatchedQuantity;
                    if (objCleanseMatch.transactionDetail != null)
                    {
                        objResponse.ServiceTransactionID = objCleanseMatch.transactionDetail.transactionID;
                        objResponse.TransactionTimestamp = Convert.ToDateTime(objCleanseMatch.transactionDetail.transactionTimestamp);
                    }
                    if (objCleanseMatch.matchCandidates != null)
                    {
                        objCleanseMatch.matchCandidates.ToList().
                       ForEach(n =>
                       {
                           string streetAddress = string.Empty;
                           MatchEntity model = new MatchEntity();
                           model.TransactionTimestamp = Convert.ToDateTime(objResponse.TransactionTimestamp);
                           if (n.organization != null)
                           {
                               model.DnBDUNSNumber = n.organization.duns;
                               model.DnBOrganizationName = n.organization.primaryName;
                               if (n.organization.primaryAddress != null)
                               {
                                   if (n.organization.primaryAddress.streetAddress != null)
                                   {
                                       model.DnBStreetAddressLine = n.organization.primaryAddress.streetAddress.line1 + ", " + n.organization.primaryAddress.streetAddress.line2;
                                   }
                                   if (n.organization.primaryAddress.addressLocality != null)
                                       model.DnBPrimaryTownName = n.organization.primaryAddress.addressLocality.name;

                                   model.DnBPostalCode = n.organization.primaryAddress.postalCode;
                                   model.DnBPostalCodeExtensionCode = Convert.ToString(n.organization.primaryAddress.postalCodeExtension);
                                   if (n.organization.primaryAddress.addressCountry != null)
                                   {
                                       model.DnBCountryISOAlpha2Code = n.organization.primaryAddress.addressCountry.isoAlpha2Code;
                                   }
                                   if (n.organization.primaryAddress.addressRegion != null)
                                   {
                                       model.DnBTerritoryAbbreviatedName = Convert.ToString(n.organization.primaryAddress.addressRegion.abbreviatedName);
                                   }
                               }

                               // need to confirm - model.DnBAddressUndeliverable = Convert.ToString(n.PrimaryAddress.UndeliverableIndicator);
                               if (n.organization.telephone != null && n.organization.telephone.Any())
                               {
                                   model.DnBTelephoneNumber = n.organization.telephone.FirstOrDefault().telephoneNumber; // need to confirm to take first or not
                                   model.DnBTelephoneNumberUnreachableIndicator = n.organization.telephone.FirstOrDefault().isUnreachable; // need to confirm to take first or not
                               }

                               if (Convert.ToBoolean(n.organization.isStandalone))
                               {
                                   model.DnBFamilyTreeMemberRole = "Single Location";
                               }
                               else
                               {
                                   if (n.organization != null && n.organization.corporateLinkage != null && n.organization.corporateLinkage.familytreeRolesPlayed != null && n.organization.corporateLinkage.familytreeRolesPlayed.Any())
                                       model.DnBFamilyTreeMemberRole = n.organization.corporateLinkage.familytreeRolesPlayed.FirstOrDefault().description.ToString();
                               }

                               if (n.organization.tradeStyleNames != null && n.organization.tradeStyleNames.Any())
                                   model.DnBTradeStyleName = n.organization.tradeStyleNames.FirstOrDefault().name.ToString();

                               if (n.organization.mostSeniorPrincipals != null && n.organization.mostSeniorPrincipals.Any())
                                   model.DnBSeniorPrincipalName = n.organization.mostSeniorPrincipals.FirstOrDefault().fullName.ToString();


                               if (n.organization.dunsControlStatus != null)
                               {
                                   if (n.organization.dunsControlStatus.operatingStatus != null)
                                   {
                                       model.DnBOperatingStatus = n.organization.dunsControlStatus.operatingStatus.description;
                                   }
                               }
                           }

                           model.IsSelected = false;
                           if (n.matchQualityInformation != null)
                           {
                               model.ScoreCompany = Convert.ToString(n.matchQualityInformation.nameMatchScore);
                               model.DnBConfidenceCode = n.matchQualityInformation.confidenceCode;
                               model.DnBMatchGradeText = n.matchQualityInformation.matchGrade;
                               model.DnBMatchDataProfileText = n.matchQualityInformation.matchDataProfile;
                               if (!string.IsNullOrWhiteSpace(model.DnBMatchGradeText))
                               {
                                   string matchgradeText = model.DnBMatchGradeText;
                                   if (!string.IsNullOrEmpty(matchgradeText) && matchgradeText.Length >= 8)
                                   {
                                       model.MGCompanyName = matchgradeText.Substring(0, 1);
                                       model.MGStreetNo = matchgradeText.Substring(1, 1);
                                       model.MGStreetName = matchgradeText.Substring(2, 1);
                                       model.MGCity = matchgradeText.Substring(3, 1);
                                       model.MGState = matchgradeText.Substring(4, 1);
                                       model.MGZipCode = matchgradeText.Substring(7, 1);
                                       model.MGTelephone = matchgradeText.Substring(6, 1);
                                       model.MGMailingAddress = matchgradeText.Substring(5, 1);
                                       model.MGDensity = matchgradeText.Substring(8, 1);
                                       model.MGUniqueness = matchgradeText.Substring(9, 1);
                                       model.MGSIC = matchgradeText.Substring(10, 1);
                                   }
                                   else if (!string.IsNullOrEmpty(matchgradeText) && matchgradeText.Length >= 7)
                                   {
                                       model.MGCompanyName = matchgradeText.Substring(0, 1);
                                       model.MGStreetNo = matchgradeText.Substring(1, 1);
                                       model.MGStreetName = matchgradeText.Substring(2, 1);
                                       model.MGCity = matchgradeText.Substring(3, 1);
                                       model.MGState = matchgradeText.Substring(4, 1);
                                       model.MGMailingAddress = matchgradeText.Substring(5, 1);
                                       model.MGTelephone = matchgradeText.Substring(6, 1);
                                   }
                               }
                               if (!string.IsNullOrEmpty(model.DnBMatchDataProfileText))
                               {
                                   model.DnBMatchDataProfileComponentCount = n.matchQualityInformation.matchDataProfileComponentsCount;
                                   string matchText = model.DnBMatchDataProfileText;
                                   if (matchText.Length >= 14)
                                   {
                                       model.MDPVCompanyName = matchText.Substring(0, 2);
                                       model.MDPVStreetNo = matchText.Substring(2, 2);
                                       model.MDPVStreetName = matchText.Substring(4, 2);
                                       model.MDPVCity = matchText.Substring(6, 2);
                                       model.MDPVState = matchText.Substring(8, 2);
                                       model.MDPVMailingAddress = matchText.Substring(10, 2);
                                       model.MDPVTelephone = matchText.Substring(12, 2);
                                       model.MDPText = matchText.Substring(0, 2) + " " + matchText.Substring(2, 2) + "-" + matchText.Substring(4, 2) + "-" + matchText.Substring(6, 2) + "-" + matchText.Substring(8, 2) + " " + matchText.Substring(12, 2);
                                   }
                               }

                               //if (n.matchQualityInformation.matchGradeComponents != null && n.matchQualityInformation.matchGradeComponents.Any())
                               //{
                               //    model.ScoreCompany = Convert.ToString(n.matchQualityInformation.matchGradeComponents.FirstOrDefault().componentRating);
                               //}

                           }
                           model.DnBDisplaySequence = Convert.ToString(n.displaySequence);


                           if (n.organization.mailingAddress != null && n.organization.mailingAddress.streetAddress != null)
                           {
                               string maddress, TerrAbb = "", PostalCode = "", PrimaryTown = "", Country = "";
                               string LineText = n.organization.mailingAddress.streetAddress.line1;
                               if (n.organization.mailingAddress.addressRegion != null) TerrAbb = Convert.ToString(n.organization.mailingAddress.addressRegion.abbreviatedName);
                               if (!string.IsNullOrEmpty(n.organization.primaryAddress.postalCode)) PostalCode = n.organization.mailingAddress.postalCode;

                               if (n.organization.mailingAddress.addressLocality != null) PrimaryTown = n.organization.mailingAddress.addressLocality.name;
                               if (n.organization.mailingAddress.addressCountry != null) Country = n.organization.mailingAddress.addressCountry.isoAlpha2Code;
                               if (!string.IsNullOrEmpty(LineText))
                               {
                                   maddress = LineText + ", " + PrimaryTown + " " + TerrAbb + " " + PostalCode + " " + Country;
                                   model.DnBMailingAddress = maddress;
                                   // model.DnBMailingAddressUndeliverable = n.organization.mailingAddress.;
                               }
                           }

                           //model.DnBMarketabilityIndicator = n.MarketabilityIndicator ? "Marketable" : "Non Marketable";

                           response.MatchEntities.Add(model);

                       });
                    }

                }
            }
            catch (WebException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                response = null;
            }
            if (response != null)
                response.TransactionResponseDetail = objResponse;
            return response;
        }
        #endregion

        #region "Direct+ Monitoring"

        /// <summary>
        /// Get All Direct+ Registrions 
        /// </summary>
        /// <param name="dunsNumber"></param>
        /// <param name="connectionString"></param>
        /// <param name="confidenceLowerLevelThresholdValue"></param>
        /// <returns></returns>
        public ListMonitoringRegistrationResponse ListMonitoringRegistrations(string AuthToken)
        {
            var objResponse = new ListMonitoringRegistrationResponse();

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(DirectPlus.ListMonitoringRegistrations);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";
            httpWebRequest.Headers.Add("Authorization", AuthToken);
            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var serializer = new JavaScriptSerializer();
                    objResponse = serializer.Deserialize<ListMonitoringRegistrationResponse>(result);
                    //MonitoringRegistrationDetailResponse(objResponse.messages.references.FirstOrDefault());
                    return objResponse;
                }
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response.GetResponseStream())
                using (var streamReader = new StreamReader(stream))
                {
                    var result = streamReader.ReadToEnd();
                    if (result != null)
                    {
                        var serializer = new JavaScriptSerializer();
                        objResponse = JsonConvert.DeserializeObject<ListMonitoringRegistrationResponse>(result);
                        return objResponse;
                    }
                }
            }
            return null;
        }
        public MonitoringRegistrationDetailResponse MonitoringRegistrationDetailResponse(string referenceName, string AuthToken)
        {
            var objResponse = new MonitoringRegistrationDetailResponse();
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(string.Format(DirectPlus.DetailMonitoringRegistration, referenceName));
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";
            httpWebRequest.Headers.Add("Authorization", AuthToken);
            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var serializer = new JavaScriptSerializer();
                    objResponse = serializer.Deserialize<MonitoringRegistrationDetailResponse>(result);
                    return objResponse;
                }
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response.GetResponseStream())
                using (var streamReader = new StreamReader(stream))
                {
                    var result = streamReader.ReadToEnd();
                    if (result != null)
                    {
                        var serializer = new JavaScriptSerializer();
                        objResponse = JsonConvert.DeserializeObject<MonitoringRegistrationDetailResponse>(result);
                        return objResponse;
                    }
                }
            }
            return new MonitoringRegistrationDetailResponse();
        }
        /// <summary>
        /// Add DUNS to Existing Registration 
        /// </summary>
        /// <param name="referenceName"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public AddRemoveDUNSToMonitoringResponse AddDunsToMonitoring(string referenceName, string filePath, string AuthToken)
        {
            string boundaryString = String.Format("----------{0:N}", Guid.NewGuid());
            var objResponse = new AddRemoveDUNSToMonitoringResponse();
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(string.Format(DirectPlus.AddDunsToMonitoringRegistration, referenceName));
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "PATCH";
            httpWebRequest.Headers.Add("Authorization", AuthToken);
            httpWebRequest.ContentType = "multipart/form-data; boundary=" + boundaryString;
            httpWebRequest.KeepAlive = true;

            MemoryStream postDataStream = new MemoryStream();
            StreamWriter postDataWriter = new StreamWriter(postDataStream);
            postDataWriter.Write("\r\n--" + boundaryString + "\r\n");
            postDataWriter.Write("Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}", "duns1", Path.GetFileName(filePath));

            postDataWriter.Write("\r\n--" + boundaryString + "\r\n");
            postDataWriter.Write("Content-Disposition: form-data;" + "name=\"{0}\";" + "filename=\"{1}\"" + "\r\nContent-Type: {2}\r\n\r\n", "duns", Path.GetFileName(filePath), "text/plain");
            postDataWriter.Flush();

            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[1024];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                postDataStream.Write(buffer, 0, bytesRead);
            }
            fileStream.Close();

            postDataWriter.Write("\r\n--" + boundaryString + "--\r\n");
            postDataWriter.Flush();
            httpWebRequest.ContentLength = postDataStream.Length;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            using (Stream s = httpWebRequest.GetRequestStream())
            {
                postDataStream.WriteTo(s);
            }
            postDataStream.Close();

            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var serializer = new JavaScriptSerializer();
                    objResponse = serializer.Deserialize<AddRemoveDUNSToMonitoringResponse>(result);
                    return objResponse;
                }
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response.GetResponseStream())
                using (var streamReader = new StreamReader(stream))
                {
                    var result = streamReader.ReadToEnd();
                    if (result != null)
                    {
                        var serializer = new JavaScriptSerializer();
                        objResponse = JsonConvert.DeserializeObject<AddRemoveDUNSToMonitoringResponse>(result);
                        return objResponse;
                    }
                }
            }
            return new AddRemoveDUNSToMonitoringResponse();
        }

        /// <summary>
        ///  Remove DUNS from Registartion
        /// </summary>
        /// <param name="referenceName"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public AddRemoveDUNSToMonitoringResponse RemoveDunsToMonitoring(string referenceName, string filePath, string AuthToken)
        {
            string boundaryString = String.Format("----------{0:N}", Guid.NewGuid());
            var objResponse = new AddRemoveDUNSToMonitoringResponse();
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(string.Format(DirectPlus.RemoveDunsToMonitoringRegistration, referenceName));
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "PATCH";
            httpWebRequest.Headers.Add("Authorization", AuthToken);
            httpWebRequest.ContentType = "multipart/form-data; boundary=" + boundaryString;
            httpWebRequest.KeepAlive = true;

            MemoryStream postDataStream = new MemoryStream();
            StreamWriter postDataWriter = new StreamWriter(postDataStream);
            postDataWriter.Write("\r\n--" + boundaryString + "\r\n");
            postDataWriter.Write("Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}", "duns1", Path.GetFileName(filePath));

            postDataWriter.Write("\r\n--" + boundaryString + "\r\n");
            postDataWriter.Write("Content-Disposition: form-data;" + "name=\"{0}\";" + "filename=\"{1}\"" + "\r\nContent-Type: {2}\r\n\r\n", "duns", Path.GetFileName(filePath), "text/plain");
            postDataWriter.Flush();

            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[1024];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                postDataStream.Write(buffer, 0, bytesRead);
            }
            fileStream.Close();

            postDataWriter.Write("\r\n--" + boundaryString + "--\r\n");
            postDataWriter.Flush();
            httpWebRequest.ContentLength = postDataStream.Length;
            using (Stream s = httpWebRequest.GetRequestStream())
            {
                postDataStream.WriteTo(s);
            }
            postDataStream.Close();

            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var serializer = new JavaScriptSerializer();
                    objResponse = serializer.Deserialize<AddRemoveDUNSToMonitoringResponse>(result);
                    return objResponse;
                }
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response.GetResponseStream())
                using (var streamReader = new StreamReader(stream))
                {
                    var result = streamReader.ReadToEnd();
                    if (result != null)
                    {
                        var serializer = new JavaScriptSerializer();
                        objResponse = JsonConvert.DeserializeObject<AddRemoveDUNSToMonitoringResponse>(result);
                        return objResponse;
                    }
                }
            }
            return new AddRemoveDUNSToMonitoringResponse();
        }

        public MonitoringRegistrationResponse EditRegistration(string referenceName)
        {
            var objResponse = new MonitoringRegistrationResponse();
            EditRegistrationRequest objRequest = new EditRegistrationRequest();
            objRequest.email = "rmehta@sequelbisolutions.com,support@matchbookservices.com,knarkhi@sequelbisolutions.com";

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(string.Format(DirectPlus.EditMonitoringRegistration, referenceName));
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            httpWebRequest.Accept = "application/json";
            httpWebRequest.Method = "PATCH";
            httpWebRequest.ContentType = "multipart/form-data;";
            httpWebRequest.Headers.Add("Authorization", string.Empty);

            string postData = JsonConvert.SerializeObject(objRequest,
                                  Newtonsoft.Json.Formatting.None,
                                  new JsonSerializerSettings
                                  {
                                      NullValueHandling = NullValueHandling.Ignore
                                  });

            //var postData = "email=rmehta@sequelbisolutions.com,support@matchbookservices.com,knarkhi@sequelbisolutions.com";


            var data = Encoding.ASCII.GetBytes(postData);
            httpWebRequest.ContentLength = data.Length;
            using (var stream = httpWebRequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var serializer = new JavaScriptSerializer();
                    objResponse = serializer.Deserialize<MonitoringRegistrationResponse>(result);
                    return objResponse;
                }
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response.GetResponseStream())
                using (var streamReader = new StreamReader(stream))
                {
                    var result = streamReader.ReadToEnd();
                    if (result != null)
                    {
                        var serializer = new JavaScriptSerializer();
                        objResponse = JsonConvert.DeserializeObject<MonitoringRegistrationResponse>(result);
                        return objResponse;
                    }
                }
            }
            return objResponse;

        }
        #endregion
        public MonitoringRegistrationResponse ExportRegistration(string referenceName)
        {
            var objResponse = new MonitoringRegistrationResponse();
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(string.Format(DirectPlus.ExportMonitoringRegistration, referenceName));
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";
            httpWebRequest.Headers.Add("Authorization", string.Empty);
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var serializer = new JavaScriptSerializer();
                    objResponse = serializer.Deserialize<MonitoringRegistrationResponse>(result);
                    return objResponse;
                }
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response.GetResponseStream())
                using (var streamReader = new StreamReader(stream))
                {
                    var result = streamReader.ReadToEnd();
                    if (result != null)
                    {
                        var serializer = new JavaScriptSerializer();
                        objResponse = JsonConvert.DeserializeObject<MonitoringRegistrationResponse>(result);
                        return objResponse;
                    }
                }
            }
            return null;
        }
        #endregion
        /// <summary>
        /// Supress & Unsupress for notifications
        /// </summary>
        /// <param name="referenceName"></param>
        /// <param name="isSuprressed"></param>
        /// <returns></returns>
        public MonitoringRegistrationResponse SuppressUnSupressRegistration(string referenceName, bool isSuprressed, string AuthToken)
        {
            var objResponse = new MonitoringRegistrationResponse();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(string.Format(DirectPlus.SuppressUnSuppressMonitoringRegistration, referenceName));
            if (isSuprressed)
            {
                httpWebRequest.Method = "POST";
            }
            else
            {
                httpWebRequest.Method = "DELETE";
            }

            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Headers.Add("Authorization", AuthToken);
            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var serializer = new JavaScriptSerializer();
                    objResponse = serializer.Deserialize<MonitoringRegistrationResponse>(result);
                    return objResponse;
                }
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response.GetResponseStream())
                using (var streamReader = new StreamReader(stream))
                {
                    var result = streamReader.ReadToEnd();
                    if (result != null)
                    {
                        var serializer = new JavaScriptSerializer();
                        objResponse = JsonConvert.DeserializeObject<MonitoringRegistrationResponse>(result);
                        return objResponse;
                    }
                }
            }
            return null;
        }
        #region "Other"
        private string purgeContent(string fileContent)
        {
            var result = fileContent.Replace("{\"OrganizationName\":", "{\"_OrganizationName\":")
                                .Replace("{\"FindCompetitorResponse\":", "{\"OrderProductResponse\":")
                                .Replace("\"IndustryCode\":[", "\"_IndustryCode\":[")
                                .Replace("\"IndustryCode\":{\"$\"", "\"IndustryCode\":{\"$\"")
                                .Replace("\"@type\":", "\"_type\":")
                                .Replace("\"$\"", "\"_param\"")
                                .Replace("@", string.Empty);

            return result;
        }
        public static string[] SplitString(string Value)
        {
            string separator = Convert.ToString(ConfigurationManager.AppSettings["separator"]);
            string[] strValue = Value.Split(new string[] { separator }, StringSplitOptions.None);
            return strValue;
        }
        public static string SplitParameters(string Parameters, char separator, int wordNumber, int index)
        {
            string value = "";
            try
            {
                if (!string.IsNullOrEmpty(Parameters))
                {
                    string Myseparator = Convert.ToString(ConfigurationManager.AppSettings["separator"]);
                    //Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.urlseparator, "+"), General.passPhrase);
                    string[] strValue = Parameters.Split(new string[] { Myseparator }, StringSplitOptions.None);

                    if (strValue.Length - 1 >= wordNumber)
                    {
                        if (strValue[wordNumber].Split(separator).Length - 1 >= index)
                        {
                            if (strValue[wordNumber].Split(separator).Length > 2)
                            {
                                var splitedArr = strValue[wordNumber].Split(separator);
                                for (int i = 0; i < splitedArr.Length; i++)
                                {
                                    if (i >= index)
                                        value = value + (string.IsNullOrEmpty(value) ? "" : separator.ToString()) + splitedArr[i];
                                }
                                value = HttpContext.Current.Server.UrlDecode(value);
                            }
                            else
                            {
                                value = HttpContext.Current.Server.UrlDecode(strValue[wordNumber].Split(separator)[index]);
                            }

                            if (string.IsNullOrEmpty(value))
                            {
                                value = "";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                value = "";
            }
            return value;
        }
        #endregion


        #region Mini iResearch Investigation
        public ResearchInvestigationResponseEntity iResearchInvestigate(iResearchEntity objRequest, string token = "")
        {
            ResearchInvestigationResponseEntity objResponse = new ResearchInvestigationResponseEntity();
            ResearchInvestigationRequestEntity objiResearchRequest = new ResearchInvestigationRequestEntity();
            RequestorEmail objRequestorEmail = new RequestorEmail();
            objiResearchRequest.requestorEmails = new List<RequestorEmail>();
            objiResearchRequest.organization = new SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters.Organizations();
            ResearchComment objResearchComment = new ResearchComment();
            objiResearchRequest.researchComments = new List<ResearchComment>();
            objiResearchRequest.organization.primaryAddress = new SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters.PrimaryAddress();
            objiResearchRequest.organization.primaryAddress.addressCountry = new SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters.AddressCountry();
            objiResearchRequest.organization.primaryAddress.addressRegion = new SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters.AddressRegion();
            objiResearchRequest.organization.primaryAddress.addressLocality = new SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters.AddressLocality();
            objiResearchRequest.organization.primaryAddress.streetAddress = new SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters.StreetAddress();
            string endpoint = iResearchInvestigation.iResearchInvestigationURL;
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(endpoint);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            if (!string.IsNullOrEmpty(token))
            {
                httpWebRequest.Headers.Add("Authorization", token);
            }
            else
            {
                string authToken = CommonMethod.GetThirdPartyProperty(ThirdPartyCode.DNB_INVESTIGATIONS.ToString(), ThirdPartyProperties.AuthToken.ToString());
                httpWebRequest.Headers.Add("Authorization", authToken);
            }

            if (objRequest != null)
            {
                objiResearchRequest.customerTransactionID = objRequest.SrcRecordId;
                objiResearchRequest.customerReference = objRequest.CustomerTransactionReference;
                objiResearchRequest.researchRequestType = objRequest.ResearchRequestType;
                objRequestorEmail.email = objRequest.CustomerRequestorEmail;
                objRequestorEmail.roleType = objRequest.roleType;
                objiResearchRequest.requestorEmails.Add(objRequestorEmail);
                objiResearchRequest.organization.primaryName = objRequest.BusinessName;
                objiResearchRequest.organization.countryISOAlpha2Code = objRequest.CountryISOAlpha2Code;
                objiResearchRequest.organization.primaryAddress.addressCountry.isoAlpha2Code = objRequest.CountryISOAlpha2Code;
                objiResearchRequest.organization.primaryAddress.addressRegion.name = objRequest.AddressRegion;
                objiResearchRequest.organization.primaryAddress.addressLocality.name = objRequest.AddressLocality;
                objiResearchRequest.organization.primaryAddress.streetAddress.line1 = objRequest.StreetAddress;
                objiResearchRequest.organization.primaryAddress.postalCode = objRequest.PostalCode;
                objResearchComment.typeDnBCode = objRequest.typeDnBCode;
                objResearchComment.comment = objRequest.ResearchComments;
                if (!string.IsNullOrEmpty(objRequest.Phone))
                    objResearchComment.comment = objResearchComment.comment + " Telephone- " + objRequest.Phone;
                if (!string.IsNullOrEmpty(objRequest.Website))
                    objResearchComment.comment = objResearchComment.comment + " Website- " + objRequest.Website;
                objiResearchRequest.researchComments.Add(objResearchComment);
            }

            string json = string.Empty;
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                json = JsonConvert.SerializeObject(objiResearchRequest,
                                  Newtonsoft.Json.Formatting.None,
                                  new JsonSerializerSettings
                                  {
                                      NullValueHandling = NullValueHandling.Ignore
                                  });
                //string json = JsonConvert.SerializeObject(objiResearchRequest, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                //objResponse.requestJSON = json;
                if (ConfigurationManager.AppSettings["IgnoreNonPrintableCharcters"].ToString() == "true")
                {
                    json = json.Replace("/", string.Empty);
                    if (json.IndexOf("http:", 0, StringComparison.InvariantCultureIgnoreCase) > -1)
                    {
                        json = json.Remove(json.IndexOf("http:", 0, StringComparison.InvariantCultureIgnoreCase) + 4, 1);
                    }
                    if (json.IndexOf("https:", 0, StringComparison.InvariantCultureIgnoreCase) > -1)
                    {
                        json = json.Remove(json.IndexOf("https:", 0, StringComparison.InvariantCultureIgnoreCase) + 5, 1);
                    }
                }
                streamWriter.Write(json.Replace("\r\n", string.Empty).Replace("\n", string.Empty).Replace("\r", string.Empty).Replace("\t", string.Empty));
                streamWriter.Flush();
                streamWriter.Close();
            }
            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var serializer = new JavaScriptSerializer();
                    objResponse = serializer.Deserialize<ResearchInvestigationResponseEntity>(result);
                    objResponse.requestJSON = json;
                    objResponse.responseJSON = result;
                    return objResponse;
                }
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response.GetResponseStream())
                using (var streamReader = new StreamReader(stream))
                {
                    var result = streamReader.ReadToEnd();
                    //"{\"transactionDetail\":{\"transactionID\":\"rrt-0aab8d4bf8d3936a1-b-se-20993-5252199-1\",\"customerTransactionID\":\"CPL0003\",\"transactionTimestamp\":\"2020-03-31T09:48:59.587Z\",\"serviceVersion\":\"1\"},\"error\":{\"errorCode\":\"05007\",\"errorMessage\":\"One or more mandatory parameters are missing.\",\"errorDetails\":[{\"parameter\":\"researchComments[0].comment\",\"description\":\"Request missing required element: 'researchComments[0].comment'\"}]},\"inquiryDetail\":{\"researchRequestType\":\"Mini\",\"organization\":{\"primaryName\":\"Dun & Bradstreet\"}}}"
                    if (result != null)
                    {
                        objResponse.responseJSON = result;
                        var serializer = new JavaScriptSerializer();
                        objResponse = JsonConvert.DeserializeObject<ResearchInvestigationResponseEntity>(result);
                        objResponse.requestJSON = json;
                        objResponse.responseJSON = result;
                        return objResponse;
                    }
                }
            }
            return objResponse;
        }
        public string GetResearchInvestigationStatus(int researchRequestId, string token = "")
        {
            string ResponseJson = string.Empty;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(iResearchInvestigation.iResearchInvestigationURL + "?researchRequestID=" + researchRequestId);
            httpWebRequest.Method = "GET";
            httpWebRequest.ContentType = "application/json";
            if (!string.IsNullOrEmpty(token))
                httpWebRequest.Headers.Add("Authorization", token);
            else
                httpWebRequest.Headers.Add("Authorization", CommonMethod.GetThirdPartyProperty(ThirdPartyCode.DNB_INVESTIGATIONS.ToString(), ThirdPartyProperties.AuthToken.ToString()));
            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    ResponseJson = result;
                }
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response.GetResponseStream())
                using (var streamReader = new StreamReader(stream))
                {
                    var result = streamReader.ReadToEnd();
                    ResponseJson = result;
                }
            }
            return ResponseJson;
        }

        #endregion
        #region Targeted iResearch Investigation
        public ResearchInvestigationTargetedResponseEntity iResearchInvestigateTargeted(iResearchEntityTargetedEntity objRequest, string token = "")
        {
            try
            {
                ResearchInvestigationTargetedResponseEntity objResponse = new ResearchInvestigationTargetedResponseEntity();
                ResearchInvestigationTargetedRequestEntity objiResearchRequest = new ResearchInvestigationTargetedRequestEntity();
                objiResearchRequest.organization = new SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters.Organizations();
                ResearchComment objResearchComment1 = new ResearchComment();
                ResearchComment objResearchComment2 = new ResearchComment();
                objiResearchRequest.researchComments = new List<ResearchComment>();
                string endpoint = iResearchInvestigation.iResearchInvestigationURL;
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(endpoint);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                if (!string.IsNullOrEmpty(token))
                {
                    httpWebRequest.Headers.Add("Authorization", token);
                }
                else
                {
                    string authToken = CommonMethod.GetThirdPartyProperty(ThirdPartyCode.DNB_INVESTIGATIONS.ToString(), ThirdPartyProperties.AuthToken.ToString());
                    httpWebRequest.Headers.Add("Authorization", authToken);
                }



                if (objRequest != null)
                {
                    objiResearchRequest.customerTransactionID = objRequest.SrcRecordId;
                    objiResearchRequest.customerReference = objRequest.CustomerTransactionReference;
                    if (!string.IsNullOrEmpty(objRequest.CustomerRequestorEmail))
                    {
                        RequestorEmail objRequestorEmail = new RequestorEmail();
                        objiResearchRequest.requestorEmails = new List<RequestorEmail>();
                        objRequestorEmail.email = objRequest.CustomerRequestorEmail;
                        objRequestorEmail.roleType = objRequest.roleType;
                        objiResearchRequest.requestorEmails.Add(objRequestorEmail);

                    }
                    objiResearchRequest.researchRequestType = objRequest.ResearchRequestType;
                    objiResearchRequest.researchSubTypes = objRequest.ResearchSubTypes?.Split(',').Select<string, int>(int.Parse).ToArray();

                    objiResearchRequest.organization.duns = objRequest.Duns;
                    if (objiResearchRequest.researchSubTypes.Contains(33550))
                        objiResearchRequest.organization.primaryName = objRequest.BusinessName;
                    if (objiResearchRequest.researchSubTypes.Contains(33776))
                        objiResearchRequest.organization.tradeStyleName = objRequest.TradeStyle;
                    if (objiResearchRequest.researchSubTypes.Contains(33558))
                        objiResearchRequest.organization.isActiveBusiness = objRequest.Status;
                    if (!string.IsNullOrEmpty(objRequest.DuplicateDuns) && objiResearchRequest.researchSubTypes.Contains(33560))
                        objiResearchRequest.organization.duplicateDUNS = objRequest.DuplicateDuns.Split(',');

                    if (objiResearchRequest.researchSubTypes.Contains(33549))
                    {
                        objiResearchRequest.organization.primaryAddress = new SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters.PrimaryAddress
                        {
                            streetAddress = new SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters.StreetAddress
                            {
                                line1 = objRequest.StreetAddress
                            },
                            addressLocality = new SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters.AddressLocality
                            {
                                name = objRequest.AddressLocality
                            },
                            postalCode = objRequest.PostalCode,
                            addressCountry = new SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters.AddressCountry
                            {
                                isoAlpha2Code = objRequest.CountryISOAlpha2Code
                            }
                        };
                    }

                    if (objiResearchRequest.researchSubTypes.Contains(33769) && !string.IsNullOrEmpty(objRequest.ResearchComments1))
                    {
                        objResearchComment1.typeDnBCode = 33814;/*objRequest.typeDnBCode;*/
                        objResearchComment1.comment = objRequest.ResearchComments1;
                        objiResearchRequest.researchComments.Add(objResearchComment1);
                    }

                    if (objiResearchRequest.researchSubTypes.Contains(33770) && !string.IsNullOrEmpty(objRequest.ResearchComments2))
                    {
                        objResearchComment2.typeDnBCode = 33815;/*objRequest.typeDnBCode;*/
                        objResearchComment2.comment = objRequest.ResearchComments2;
                        objiResearchRequest.researchComments.Add(objResearchComment2);
                    }
                }

                string json = string.Empty;
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    json = JsonConvert.SerializeObject(objiResearchRequest,
                                      Newtonsoft.Json.Formatting.None,
                                      new JsonSerializerSettings
                                      {
                                          NullValueHandling = NullValueHandling.Ignore
                                      });
                    if (ConfigurationManager.AppSettings["IgnoreNonPrintableCharcters"].ToString() == "true")
                    {
                        json = json.Replace("/", string.Empty);
                        if (json.IndexOf("http:", 0, StringComparison.InvariantCultureIgnoreCase) > -1)
                        {
                            json = json.Remove(json.IndexOf("http:", 0, StringComparison.InvariantCultureIgnoreCase) + 4, 1);
                        }
                        if (json.IndexOf("https:", 0, StringComparison.InvariantCultureIgnoreCase) > -1)
                        {
                            json = json.Remove(json.IndexOf("https:", 0, StringComparison.InvariantCultureIgnoreCase) + 5, 1);
                        }
                    }
                    //string json = JsonConvert.SerializeObject(objiResearchRequest, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                    streamWriter.Write(json.Replace("\r\n", string.Empty).Replace("\n", string.Empty).Replace("\r", string.Empty).Replace("\t", string.Empty));
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                try
                {
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        var serializer = new JavaScriptSerializer();
                        objResponse = serializer.Deserialize<ResearchInvestigationTargetedResponseEntity>(result);
                        objResponse.requestJSON = json;
                        objResponse.responseJSON = result;
                        return objResponse;
                    }
                }
                catch (WebException ex)
                {
                    using (var stream = ex.Response.GetResponseStream())
                    using (var streamReader = new StreamReader(stream))
                    {
                        var result = streamReader.ReadToEnd();
                        //"{\"transactionDetail\":{\"transactionID\":\"rrt-0aab8d4bf8d3936a1-b-se-20993-5252199-1\",\"customerTransactionID\":\"CPL0003\",\"transactionTimestamp\":\"2020-03-31T09:48:59.587Z\",\"serviceVersion\":\"1\"},\"error\":{\"errorCode\":\"05007\",\"errorMessage\":\"One or more mandatory parameters are missing.\",\"errorDetails\":[{\"parameter\":\"researchComments[0].comment\",\"description\":\"Request missing required element: 'researchComments[0].comment'\"}]},\"inquiryDetail\":{\"researchRequestType\":\"Mini\",\"organization\":{\"primaryName\":\"Dun & Bradstreet\"}}}"
                        if (result != null)
                        {
                            objResponse.responseJSON = result;
                            var serializer = new JavaScriptSerializer();
                            objResponse = JsonConvert.DeserializeObject<ResearchInvestigationTargetedResponseEntity>(result);
                            objResponse.requestJSON = json;
                            objResponse.responseJSON = result;
                            return objResponse;
                        }
                    }
                }
                return objResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region Type Ahead Functionality Implementation
        public string SearchDataTypeAheadAPI(string searchTerm, string countryISOAlpha2Code)
        {
            string endpoint = string.Empty;
            if (countryISOAlpha2Code != null)
            {
                endpoint = TypeAhead.SearchDataTypeAheadWithCountry + searchTerm + "&countryISOAlpha2Code=" + countryISOAlpha2Code;
            }
            else
            {
                endpoint = TypeAhead.SearchDataTypeAheadWithoutCountry + searchTerm + "&countryISOAlpha2Code=" + countryISOAlpha2Code;
            }
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                var webRequest = (HttpWebRequest)WebRequest.Create(endpoint);
                webRequest.Method = "GET";
                webRequest.ContentType = "application/json";
                webRequest.Headers.Add("Authorization", CommonMethod.GetThirdPartyProperty(ThirdPartyCode.DNB_TYPEAHEAD_SEARCH.ToString(), ThirdPartyProperties.AuthToken.ToString()));
                //webRequest.Headers.Add("Origin", "www.dnb.com");

                var webResponse = (HttpWebResponse)webRequest.GetResponse();
                using (StreamReader reader = new StreamReader(webResponse.GetResponseStream()))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (WebException e)
            {
                using (WebResponse response = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }
        #endregion

        #region "Multiple MINI requests"
        public async Task<int> SubmitMultipleMiniRequest(List<iResearchEntity> iResearches, string connectionString, string authToken)
        {
            iResearchFacade fac = new iResearchFacade(connectionString);
            foreach (var item in iResearches)
            {
                if (!string.IsNullOrEmpty(item.BusinessName) && !string.IsNullOrEmpty(item.SrcRecordId) && !string.IsNullOrEmpty(item.StreetAddress) && !string.IsNullOrEmpty(item.AddressLocality) && !string.IsNullOrEmpty(item.PostalCode) && !string.IsNullOrEmpty(item.CountryISOAlpha2Code) && !string.IsNullOrEmpty(item.ResearchComments))
                {
                    ResearchInvestigationResponseEntity modelSubmit = new ResearchInvestigationResponseEntity();
                    item.roleType = 19117;
                    item.typeDnBCode = 33588;
                    modelSubmit = this.iResearchInvestigate(item, authToken);
                    string Message = string.Empty;
                    if (modelSubmit.researchRequestID > 0)
                    {
                        item.ResearchRequestId = modelSubmit.researchRequestID;
                        item.ResearchRequestType = item.ResearchRequestType;
                        item.InputId = item.InputId;
                        item.SrcRecordId = item.SrcRecordId;
                        item.Tags = item.Tags;
                        item.RequestResponseJSON = modelSubmit.responseJSON;
                        item.RequestBody = modelSubmit.requestJSON;
                        Message = fac.InsertResearchInvestigation(item);
                    }
                    else
                    {
                        dynamic data = JObject.Parse(modelSubmit.responseJSON);
                        if (data.error != null && !string.IsNullOrEmpty(data.error.errorMessage.Value))
                        {
                            data = data.error.errorMessage.Value;
                        }
                        item.RequestResponseJSON = modelSubmit.responseJSON;
                        item.RequestBody = modelSubmit.requestJSON;
                        Message = fac.InsertiResearchInvestigationFailedCalls(item);
                    }
                }
            }
            return 0;
        }
        #endregion

        #region "Multiple Targeted Requests"
        public async Task<int> SubmitMultipleTargetedRequest(List<iResearchEntityTargetedEntity> iResearches, string connectionString, string authToken)
        {
            iResearchFacade fac = new iResearchFacade(connectionString);
            foreach (var item in iResearches)
            {
                ResearchInvestigationTargetedResponseEntity modelSubmit = new ResearchInvestigationTargetedResponseEntity();
                item.roleType = 19117;
                modelSubmit = this.iResearchInvestigateTargeted(item, authToken);
                string Message = string.Empty;
                if (modelSubmit.researchRequestID > 0)
                {
                    item.ResearchRequestId = modelSubmit.researchRequestID;
                    item.InputId = !string.IsNullOrEmpty(item.InputId) ? item.InputId : "0";
                    item.SrcRecordId = string.IsNullOrEmpty(item.SrcRecordId) ? "" : item.SrcRecordId.Trim();
                    item.Tags = item.Tags;
                    item.RequestResponseJSON = modelSubmit.responseJSON;
                    item.RequestBody = modelSubmit.requestJSON;
                    Message = fac.InsertResearchInvestigation(item);
                }
                else
                {
                    dynamic data = JObject.Parse(modelSubmit.responseJSON);
                    if (data.error != null && !string.IsNullOrEmpty(data.error.errorMessage.Value))
                    {
                        data = data.error.errorMessage.Value;
                    }
                    item.RequestResponseJSON = modelSubmit.responseJSON;
                    item.RequestBody = modelSubmit.requestJSON;
                    Message = fac.InsertiResearchInvestigationFailedCalls(item);
                }
            }
            return 0;
        }
        #endregion

        #region "Multiple GetResearchInvestigationStatus"
        public async Task<int> SubmitMultipleGetResearchInvestigationStatus(List<IResearchInvestigationEntity> iResearches, string connectionString, string token)
        {
            iResearchFacade fac = new iResearchFacade(connectionString);
            foreach (var item in iResearches)
            {
                string result = GetResearchInvestigationStatus(item.ResearchRequestId, token);
                if (!string.IsNullOrEmpty(result))
                {
                    dynamic data = JObject.Parse(result);
                    if (data != null && data.error != null && !string.IsNullOrEmpty(data.error.errorMessage.Value))
                    {
                    }
                    else
                    {
                        bool resultStatus = fac.iResearchUpdateRequestStatus(item.ResearchRequestId, result);
                    }
                }
            }
            return 0;
        }
        #endregion

        #region "Challenge Investigation"
        public ResearchInvestigationResponseEntity ChallengeInvestigation(iResearchChallengeEntity iResearchChallenge)
        {
            ResearchInvestigationResponseEntity objResponse = new ResearchInvestigationResponseEntity();
            ResearchInvestigationChallengeRequestEntity objiResearchRequest = new ResearchInvestigationChallengeRequestEntity();
            string endpoint = iResearchInvestigation.iResearchInvestigationURL;
            endpoint = endpoint + "/" + iResearchChallenge.researchRequestID + "/cases/" + iResearchChallenge.caseID + "/challenges";
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(endpoint);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            string authToken = CommonMethod.GetThirdPartyProperty(ThirdPartyCode.DNB_INVESTIGATIONS.ToString(), ThirdPartyProperties.AuthToken.ToString());
            httpWebRequest.Headers.Add("Authorization", authToken);
            if(iResearchChallenge != null)
            {
                objiResearchRequest.customerTransactionID = iResearchChallenge.customerTransactionID;
                objiResearchRequest.customerReference = iResearchChallenge.customerReference;
                objiResearchRequest.challengeReason = iResearchChallenge.challengeReason;
                objiResearchRequest.researchComments = new List<ResearchComment>()
                {
                    new ResearchComment()
                    {
                        comment = iResearchChallenge.comment,
                        typeDnBCode = iResearchChallenge.typeDnBCode
                    }
                };

            }

            string json = string.Empty;
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                json = JsonConvert.SerializeObject(objiResearchRequest,
                                  Newtonsoft.Json.Formatting.None,
                                  new JsonSerializerSettings
                                  {
                                      NullValueHandling = NullValueHandling.Ignore
                                  });
                if (ConfigurationManager.AppSettings["IgnoreNonPrintableCharcters"].ToString() == "true")
                {
                    json = json.Replace("/", string.Empty);
                    if (json.IndexOf("http:", 0, StringComparison.InvariantCultureIgnoreCase) > -1)
                    {
                        json = json.Remove(json.IndexOf("http:", 0, StringComparison.InvariantCultureIgnoreCase) + 4, 1);
                    }
                    if (json.IndexOf("https:", 0, StringComparison.InvariantCultureIgnoreCase) > -1)
                    {
                        json = json.Remove(json.IndexOf("https:", 0, StringComparison.InvariantCultureIgnoreCase) + 5, 1);
                    }
                }
                streamWriter.Write(json.Replace("\r\n", string.Empty).Replace("\n", string.Empty).Replace("\r", string.Empty).Replace("\t", string.Empty));
                streamWriter.Flush();
                streamWriter.Close();
            }
            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var serializer = new JavaScriptSerializer();
                    objResponse = serializer.Deserialize<ResearchInvestigationResponseEntity>(result);
                    objResponse.requestJSON = json;
                    objResponse.responseJSON = result;
                    objResponse.researchRequestID = objResponse.inquiryDetail.researchRequestID;
                    return objResponse;
                }
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response.GetResponseStream())
                using (var streamReader = new StreamReader(stream))
                {
                    var result = streamReader.ReadToEnd();
                    if (result != null)
                    {
                        objResponse.responseJSON = result;
                        var serializer = new JavaScriptSerializer();
                        objResponse = JsonConvert.DeserializeObject<ResearchInvestigationResponseEntity>(result);
                        objResponse.requestJSON = json;
                        objResponse.responseJSON = result;
                        return objResponse;
                    }
                }
            }
            return objResponse;
        }
        #endregion
    }


}
