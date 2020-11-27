using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using static SBISCompanyCleanseMatchBusiness.Objects.Helpers.EnumHelper;

namespace SBISCompanyCleanseMatchBusiness.Objects.Repositories
{
    internal class MatchRepository : RepositoryParent
    {
        public MatchRepository(string connectionString) : base(connectionString) { }


        internal List<MatchEntity> GetMatches(int InputId, string SrcRecordId)
        {
            List<MatchEntity> results = new List<MatchEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.StewGetLCMMatchExtended";
                StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
                sproc.StoredProceduresParameter.Add(GetParam("@InputId", InputId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SrcRecordId", SrcRecordId.ToString(), SQLServerDatatype.VarcharDataType));
                //sproc.StoredProceduresParameter.Add(param);

                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new MatchAdapter().Adapt(dt);

                }
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return results;
        }

        internal string EncodeURL(string value)
        {
            return sql.ExecuteText("SELECT dbo.fn_EncodeString('" + value.TrimEnd() + "')");
        }

        internal List<MDPCodeEntity> GetMDPCodes()
        {
            List<MDPCodeEntity> results = new List<MDPCodeEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetMDPCodes";

                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new MDPCodeAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return results;
        }
        internal List<MatchCodeEntity> GetMDPValues()
        {
            List<MatchCodeEntity> results = new List<MatchCodeEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetMDPValue";

                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new MatchCodeAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return results;
        }
        internal void AddCompanyRecord(MatchEntity Match, int UserId)
        {
            string InpAsXML = "";
            InpAsXML = "'<CreateMatch>";
            InpAsXML += "<UserId>" + UserId + "</UserId>";
            InpAsXML += "<Rec><SrcRecordId>" + Match.SrcRecordId + "</SrcRecordId>";
            InpAsXML += "<Tags>" + Match.Tags + "</Tags>";
            InpAsXML += "<OriginalSrcRecordId>" + Match.OriginalSrcRecordId + "</OriginalSrcRecordId>";
            InpAsXML += "<TransactionTimestamp>" + DateTime.Now.Date + "</TransactionTimestamp>";
            InpAsXML += "<DnBDUNSNumber>" + Match.DnBDUNSNumber + "</DnBDUNSNumber>";
            InpAsXML += "<DnBOrganizationName>" + Match.DnBOrganizationName + "</DnBOrganizationName>";
            InpAsXML += "<DnBTradeStyleName>" + Match.DnBTradeStyleName + "</DnBTradeStyleName>";
            InpAsXML += "<DnBSeniorPrincipalName>" + Match.DnBSeniorPrincipalName + "</DnBSeniorPrincipalName>";
            InpAsXML += "<DnBStreetAddressLine>" + Match.DnBStreetAddressLine + "</DnBStreetAddressLine>";
            InpAsXML += "<DnBPrimaryTownName>" + Match.DnBPrimaryTownName + "</DnBPrimaryTownName>";
            InpAsXML += "<DnBCountryISOAlpha2Code>" + Match.DnBCountryISOAlpha2Code + "</DnBCountryISOAlpha2Code>";
            InpAsXML += "<DnBPostalCode>" + Match.DnBPostalCode + "</DnBPostalCode>";
            InpAsXML += "<DnBPostalCodeExtensionCode>" + Match.DnBPostalCodeExtensionCode + "</DnBPostalCodeExtensionCode>";
            InpAsXML += "<DnBTerritoryAbbreviatedName>" + Match.DnBTerritoryAbbreviatedName + "</DnBTerritoryAbbreviatedName>";
            InpAsXML += "<DnBAddressUndeliverable>" + Match.DnBAddressUndeliverable + "</DnBAddressUndeliverable>";
            InpAsXML += "<DnBTelephoneNumber>" + Match.DnBTelephoneNumber + "</DnBTelephoneNumber>";
            InpAsXML += "<DnBOperatingStatus>" + Match.DnBOperatingStatus + "</DnBOperatingStatus>";
            InpAsXML += "<DnBFamilyTreeMemberRole>" + Match.DnBFamilyTreeMemberRole + "</DnBFamilyTreeMemberRole>";
            InpAsXML += "<DnBStandaloneOrganization>" + Match.DnBStandaloneOrganization + "</DnBStandaloneOrganization>";
            InpAsXML += "<DnBConfidenceCode>" + Match.DnBConfidenceCode + "</DnBConfidenceCode>";
            InpAsXML += "<DnBMatchGradeText>" + Match.DnBMatchGradeText + "</DnBMatchGradeText>";
            InpAsXML += "<DnBMatchDataProfileText>" + Match.DnBMatchDataProfileText + "</DnBMatchDataProfileText>";
            InpAsXML += "<DnBMatchDataProfileComponentCount>" + Match.DnBMatchDataProfileComponentCount + "</DnBMatchDataProfileComponentCount>";
            InpAsXML += "<DnBDisplaySequence>" + Match.DnBDisplaySequence + "</DnBDisplaySequence>";
            InpAsXML += "</Rec ></CreateMatch>";

            StoredProcedureEntity sproc = new StoredProcedureEntity();
            sproc.StoredProcedureName = "dnb.StewCreateNewCompanyFromMatch";
            StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
            param.ParameterName = "@inp";
            Regex badAmpersand = new Regex("&(?![a-zA-Z]{2,6};|#[0-9]{2,4};)");
            string res = badAmpersand.Replace(InpAsXML, "&amp;");
            param._ParameterValue = res;
            sproc.StoredProceduresParameter.Add(param);

            sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
        }
        // Validate SrcId for checking duplicate records at "Add Match as a new Company".
        internal bool ValidateCompanySrcId(string SrcRecordId)
        {
            try
            {

                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.StewCheckNewSrcRecordId";
                StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
                sproc.StoredProceduresParameter.Add(GetParam("@SrcRecordId", SrcRecordId.ToString(), SQLServerDatatype.VarcharDataType));
                sql.ExecuteScalar(CommandType.StoredProcedure, sproc);
                return false;
            }
            catch (SqlException)
            {
                throw;
            }

        }

        private StoredProceduresParameterEntity GetParam(string ParameterName, string ParameterValue, SQLServerDatatype DataType, ParameterDirection Direction = ParameterDirection.Input)
        {
            StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
            param.ParameterName = ParameterName;
            param.ParameterValue = ParameterValue;
            param.Datatype = DataType;
            param.Direction = Direction;
            return param;
        }
    }
}
