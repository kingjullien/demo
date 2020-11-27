using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using static SBISCompanyCleanseMatchBusiness.Objects.Helpers.EnumHelper;

namespace SBISCompanyCleanseMatchBusiness.Objects.Repositories
{
    internal class CompanyRepository : RepositoryParent
    {
        public CompanyRepository(string connectionString) : base(connectionString) { }

        #region "Company Match & Bad"

        internal List<CompanyEntity> GetLCMCompany()
        {
            List<CompanyEntity> results = new List<CompanyEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.StewGetLCMCompany";

                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new CompanyAdapter().Adapt(dt);
                    foreach (CompanyEntity comp in results)
                    {
                        comp.Matches = new MatchRepository(Connection).GetMatches(comp.InputId, comp.SrcRecordId);
                    }
                }
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return results;
        }

        internal List<CompanyEntity> GetBIDCompany()
        {
            List<CompanyEntity> results = new List<CompanyEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.StewGetBIDCompany";

                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new CompanyAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return results;
        }

        internal void AcceptLCMMatches(List<MatchEntity> Matches, string UserName)
        {
            string InpAsXML = "";
            InpAsXML = "'<LCMAccept>";
            InpAsXML += "<UserId>" + UserName + "</UserId>";
            foreach (MatchEntity match in Matches)
            {
                InpAsXML += "<Rec><SrcRecordId>" + match.SrcRecordId + "</SrcRecordId><DnBDUNSNumber>" + match.DnBDUNSNumber + "</DnBDUNSNumber></Rec>";
            }
            InpAsXML += "</LCMAccept>'";

            StoredProcedureEntity sproc = new StoredProcedureEntity();
            sproc.StoredProcedureName = "dnb.StewAcceptLowConfidenceMatches";
            StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
            param.ParameterName = "@inp";
            Regex badAmpersand = new Regex("&(?![a-zA-Z]{2,6};|#[0-9]{2,4};)");
            string res = badAmpersand.Replace(InpAsXML, "&amp;");
            param._ParameterValue = res;
            sproc.StoredProceduresParameter.Add(param);

            sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
        }
        /// <summary>
        /// overrider after feature added for approver - Build 120
        /// </summary>
        /// <param name="Matches"></param>
        /// <param name="UserName"></param>
        internal void AcceptLCMMatches(List<MatchEntity> Matches, string UserName, int UserId, bool IsApprove = true)
        {
            string InpAsXML = "";
            InpAsXML = "'<LCMAccept>";
            InpAsXML += "<UserId>" + UserName + "</UserId>";
            foreach (MatchEntity match in Matches)
            {
                InpAsXML += "<Rec><InputId>" + match.InputId + "</InputId><SrcRecordId>" + match.SrcRecordId + "</SrcRecordId><DnBDUNSNumber>" + match.DnBDUNSNumber + "</DnBDUNSNumber><StewardshipNotes>" + match.StewardshipNotes + "</StewardshipNotes></Rec>";
            }
            InpAsXML += "</LCMAccept>'";

            StoredProcedureEntity sproc = new StoredProcedureEntity();
            sproc.StoredProcedureName = "dnb.StewAcceptLowConfidenceMatches";
            StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
            param.ParameterName = "@inp";
            Regex badAmpersand = new Regex("&(?![a-zA-Z]{2,6};|#[0-9]{2,4};)");
            string res = badAmpersand.Replace(InpAsXML, "&amp;");
            param._ParameterValue = res;
            sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString(), SQLServerDatatype.IntDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@Approve", IsApprove.ToString(), SQLServerDatatype.BitDataType));
            sproc.StoredProceduresParameter.Add(param);
            sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
        }
        internal void RejectLCMMatches(List<CompanyEntity> Companies, string UserName)
        {
            string InpAsXML = "";
            InpAsXML = "'<LCMReject>";
            InpAsXML += "<UserId>" + UserName + "</UserId>";
            foreach (CompanyEntity company in Companies)
            {
                InpAsXML += "<Rec><SrcRecordId>" + company.SrcRecordId + "</SrcRecordId></Rec>";
            }
            InpAsXML += "</LCMReject>'";

            StoredProcedureEntity sproc = new StoredProcedureEntity();
            sproc.StoredProcedureName = "dnb.StewRejectLowConfidenceMatches";
            StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
            param.ParameterName = "@inp";
            Regex badAmpersand = new Regex("&(?![a-zA-Z]{2,6};|#[0-9]{2,4};)");
            string res = badAmpersand.Replace(InpAsXML, "&amp;");
            param._ParameterValue = res;
            sproc.StoredProceduresParameter.Add(param);
            sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
        }
        /// <summary>
        /// overrider after feature added for approver - Build 120
        /// </summary>
        /// <param name="Matches"></param>
        /// <param name="UserName"></param>
        internal void RejectLCMMatches(List<CompanyEntity> Companies, string UserName, int UserId, bool IsApprove = true)
        {
            string InpAsXML = "";
            InpAsXML = "'<LCMReject>";
            InpAsXML += "<UserId>" + UserName + "</UserId>";
            foreach (CompanyEntity company in Companies)
            {
                InpAsXML += "<Rec><InputId>" + company.InputId + "</InputId><SrcRecordId>" + company.SrcRecordId + "</SrcRecordId><StewardshipNotes>" + company.StewardshipNotes + "</StewardshipNotes></Rec>";
            }
            InpAsXML += "</LCMReject>'";

            StoredProcedureEntity sproc = new StoredProcedureEntity();
            sproc.StoredProcedureName = "dnb.StewRejectLowConfidenceMatches";
            StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
            param.ParameterName = "@inp";
            Regex badAmpersand = new Regex("&(?![a-zA-Z]{2,6};|#[0-9]{2,4};)");
            string res = badAmpersand.Replace(InpAsXML, "&amp;");
            param._ParameterValue = res;
            sproc.StoredProceduresParameter.Add(param);
            sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString(), SQLServerDatatype.IntDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@Approve", IsApprove.ToString(), SQLServerDatatype.BitDataType));
            sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
        }

        internal void AcceptBIDMatch(CompanyEntity company, MatchEntity matchEntity, string UserName)
        {
            string InpAsXML = "";
            InpAsXML = "'<BIDMatch>";
            InpAsXML += "<UserId>" + UserName + "</UserId>";
            InpAsXML += "<Rec><InputId>" + company.InputId + "</InputId><SrcRecordId>" + company.SrcRecordId + "</SrcRecordId>";
            InpAsXML += "<CompanyName>" + company.CompanyName + "</CompanyName>";

            if (!String.IsNullOrEmpty(company.Address))
            {
                InpAsXML += "<Address>" + company.Address + "</Address>";
            }
            else
            {
                InpAsXML += "<Address></Address>";
            }

            if (!String.IsNullOrEmpty(company.City))
            {
                InpAsXML += "<City>" + company.City + "</City>";
            }
            else
            {
                InpAsXML += "<City></City>";
            }

            if (!String.IsNullOrEmpty(company.State))
            {
                InpAsXML += "<State>" + company.State + "</State>";
            }
            else
            {
                InpAsXML += "<State></State>";
            }

            if (!String.IsNullOrEmpty(company.PostalCode))
            {
                InpAsXML += "<PostalCode>" + company.PostalCode + "</PostalCode>";
            }
            else
            {
                InpAsXML += "<PostalCode></PostalCode>";
            }

            InpAsXML += "<CountryISOAlpha2Code>" + company.CountryISOAlpha2Code + "</CountryISOAlpha2Code>";

            if (!String.IsNullOrEmpty(company.PhoneNbr))
            {
                InpAsXML += "<PhoneNbr>" + company.PhoneNbr + "</PhoneNbr>";
            }
            else
            {
                InpAsXML += "<PhoneNbr></PhoneNbr>";
            }

            if (matchEntity.TransactionTimestamp != null)
            {
                InpAsXML += "<TransactionTimestamp>" + matchEntity.TransactionTimestamp + "</TransactionTimestamp>";
            }
            else
            {
                InpAsXML += "<TransactionTimestamp></TransactionTimestamp>";
            }

            if (!string.IsNullOrWhiteSpace(matchEntity.DnBDUNSNumber))
            {
                InpAsXML += "<DnBDUNSNumber>" + matchEntity.DnBDUNSNumber + "</DnBDUNSNumber>";
            }
            else
            {
                InpAsXML += "<DnBDUNSNumber></DnBDUNSNumber>";
            }

            if (!string.IsNullOrWhiteSpace(matchEntity.DnBOrganizationName))
            {
                InpAsXML += "<DnBOrganizationName>" + matchEntity.DnBOrganizationName + "</DnBOrganizationName>";
            }
            else
            {
                InpAsXML += "<DnBOrganizationName></DnBOrganizationName>";
            }

            if (!string.IsNullOrWhiteSpace(matchEntity.DnBTradeStyleName))
            {
                InpAsXML += "<DnBTradeStyleName>" + matchEntity.DnBTradeStyleName + "</DnBTradeStyleName>";
            }
            else
            {
                InpAsXML += "<DnBTradeStyleName></DnBTradeStyleName>";
            }

            if (!string.IsNullOrWhiteSpace(matchEntity.DnBSeniorPrincipalName))
            {
                InpAsXML += "<DnBSeniorPrincipalName>" + matchEntity.DnBSeniorPrincipalName + "</DnBSeniorPrincipalName>";
            }
            else
            {
                InpAsXML += "<DnBSeniorPrincipalName></DnBSeniorPrincipalName>";
            }

            if (!string.IsNullOrWhiteSpace(matchEntity.DnBStreetAddressLine))
            {
                InpAsXML += "<DnBStreetAddressLine>" + matchEntity.DnBStreetAddressLine + "</DnBStreetAddressLine>";
            }
            else
            {
                InpAsXML += "<DnBStreetAddressLine></DnBStreetAddressLine>";
            }

            if (!string.IsNullOrWhiteSpace(matchEntity.DnBPrimaryTownName))
            {
                InpAsXML += "<DnBPrimaryTownName>" + matchEntity.DnBPrimaryTownName + "</DnBPrimaryTownName>";
            }
            else
            {
                InpAsXML += "<DnBPrimaryTownName></DnBPrimaryTownName>";
            }

            if (!string.IsNullOrWhiteSpace(matchEntity.DnBCountryISOAlpha2Code))
            {
                InpAsXML += "<DnBCountryISOAlpha2Code>" + matchEntity.DnBCountryISOAlpha2Code + "</DnBCountryISOAlpha2Code>";
            }
            else
            {
                InpAsXML += "<DnBCountryISOAlpha2Code></DnBCountryISOAlpha2Code>";
            }

            if (!string.IsNullOrWhiteSpace(matchEntity.DnBPostalCode))
            {
                InpAsXML += "<DnBPostalCode>" + matchEntity.DnBPostalCode + "</DnBPostalCode>";
            }
            else
            {
                InpAsXML += "<DnBPostalCode></DnBPostalCode>";
            }

            if (!string.IsNullOrWhiteSpace(matchEntity.DnBPostalCodeExtensionCode))
            {
                InpAsXML += "<DnBPostalCodeExtensionCode>" + matchEntity.DnBPostalCodeExtensionCode + "</DnBPostalCodeExtensionCode>";
            }
            else
            {
                InpAsXML += "<DnBPostalCodeExtensionCode></DnBPostalCodeExtensionCode>";
            }

            if (!string.IsNullOrWhiteSpace(matchEntity.DnBTerritoryAbbreviatedName))
            {
                InpAsXML += "<DnBTerritoryAbbreviatedName>" + matchEntity.DnBTerritoryAbbreviatedName + "</DnBTerritoryAbbreviatedName>";
            }
            else
            {
                InpAsXML += "<DnBTerritoryAbbreviatedName></DnBTerritoryAbbreviatedName>";
            }

            if (!string.IsNullOrWhiteSpace(matchEntity.DnBAddressUndeliverable))
            {
                InpAsXML += "<DnBAddressUndeliverable>" + matchEntity.DnBAddressUndeliverable + "</DnBAddressUndeliverable>";
            }
            else
            {
                InpAsXML += "<DnBAddressUndeliverable></DnBAddressUndeliverable>";
            }

            if (!string.IsNullOrWhiteSpace(matchEntity.DnBTelephoneNumber))
            {
                InpAsXML += "<DnBTelephoneNumber>" + matchEntity.DnBTelephoneNumber + "</DnBTelephoneNumber>";
            }
            else
            {
                InpAsXML += "<DnBTelephoneNumber></DnBTelephoneNumber>";
            }

            if (!string.IsNullOrWhiteSpace(matchEntity.DnBOperatingStatus))
            {
                InpAsXML += "<DnBOperatingStatus>" + matchEntity.DnBOperatingStatus + "</DnBOperatingStatus>";
            }
            else
            {
                InpAsXML += "<DnBOperatingStatus></DnBOperatingStatus>";
            }

            if (!string.IsNullOrWhiteSpace(matchEntity.DnBFamilyTreeMemberRole))
            {
                InpAsXML += "<DnBFamilyTreeMemberRole>" + matchEntity.DnBFamilyTreeMemberRole + "</DnBFamilyTreeMemberRole>";
            }
            else
            {
                InpAsXML += "<DnBFamilyTreeMemberRole></DnBFamilyTreeMemberRole>";
            }

            if (!string.IsNullOrWhiteSpace(matchEntity.DnBStandaloneOrganization))
            {
                InpAsXML += "<DnBStandaloneOrganization>" + matchEntity.DnBStandaloneOrganization + "</DnBStandaloneOrganization>";
            }
            else
            {
                InpAsXML += "<DnBStandaloneOrganization></DnBStandaloneOrganization>";
            }

            InpAsXML += "<DnBConfidenceCode>" + matchEntity.DnBConfidenceCode + "</DnBConfidenceCode>";

            if (!string.IsNullOrWhiteSpace(matchEntity.DnBMatchGradeText))
            {
                InpAsXML += "<DnBMatchGradeText>" + matchEntity.DnBMatchGradeText + "</DnBMatchGradeText>";
            }
            else
            {
                InpAsXML += "<DnBMatchGradeText></DnBMatchGradeText>";
            }

            if (!string.IsNullOrWhiteSpace(matchEntity.DnBMatchDataProfileText))
            {
                InpAsXML += "<DnBMatchDataProfileText>" + matchEntity.DnBMatchDataProfileText + "</DnBMatchDataProfileText>";
            }
            else
            {
                InpAsXML += "<DnBMatchDataProfileText></DnBMatchDataProfileText>";
            }

            InpAsXML += "<DnBMatchDataProfileComponentCount>" + matchEntity.DnBMatchDataProfileComponentCount + "</DnBMatchDataProfileComponentCount>";
            InpAsXML += "<DnBDisplaySequence>" + matchEntity.DnBDisplaySequence + "</DnBDisplaySequence></Rec>";

            InpAsXML += "</BIDMatch>'";

            StoredProcedureEntity sproc = new StoredProcedureEntity();
            sproc.StoredProcedureName = "dnb.StewAcceptBIDMatch";
            StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
            param.ParameterName = "@inp";
            Regex badAmpersand = new Regex("&(?![a-zA-Z]{2,6};|#[0-9]{2,4};)");
            string res = badAmpersand.Replace(InpAsXML, "&amp;");
            param._ParameterValue = res;

            sproc.StoredProceduresParameter.Add(param);

            sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);

        }

        internal void UpdateBIDRecord(List<CompanyEntity> Companies, string UserName)
        {

            string InpAsXML = "";
            InpAsXML = "'<BIDUpdate>";
            InpAsXML += "<UserId>" + UserName + "</UserId>";
            foreach (CompanyEntity company in Companies)
            {
                InpAsXML += "<Rec><InputId>" + company.InputId + "</InputId><SrcRecordId>" + company.SrcRecordId + "</SrcRecordId>";
                InpAsXML += "<CompanyName>" + company.CompanyName + "</CompanyName>";

                if (!String.IsNullOrEmpty(company.Address))
                {
                    InpAsXML += "<Address>" + company.Address + "</Address>";
                }
                else
                {
                    InpAsXML += "<Address></Address>";
                }

                if (!String.IsNullOrEmpty(company.City))
                {
                    InpAsXML += "<City>" + company.City + "</City>";
                }
                else
                {
                    InpAsXML += "<City></City>";
                }

                if (!String.IsNullOrEmpty(company.State))
                {
                    InpAsXML += "<State>" + company.State + "</State>";
                }
                else
                {
                    InpAsXML += "<State></State>";
                }

                if (!String.IsNullOrEmpty(company.PostalCode))
                {
                    InpAsXML += "<PostalCode>" + company.PostalCode + "</PostalCode>";
                }
                else
                {
                    InpAsXML += "<PostalCode></PostalCode>";
                }

                InpAsXML += "<CountryISOAlpha2Code>" + company.CountryISOAlpha2Code + "</CountryISOAlpha2Code>";

                if (!String.IsNullOrEmpty(company.PhoneNbr))
                {
                    InpAsXML += "<PhoneNbr>" + company.PhoneNbr + "</PhoneNbr>";
                }
                else
                {
                    InpAsXML += "<PhoneNbr></PhoneNbr>";
                }

                if (!String.IsNullOrEmpty(company.inLanguage))
                {
                    InpAsXML += "<inLanguage>" + company.inLanguage + "</inLanguage></Rec>";
                }
                else
                {
                    InpAsXML += "<inLanguage></inLanguage></Rec>";
                }
            }
            InpAsXML += "</BIDUpdate>'";

            StoredProcedureEntity sproc = new StoredProcedureEntity();
            sproc.StoredProcedureName = "dnb.StewUpdateBadInputData";
            StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
            param.ParameterName = "@inp";
            Regex badAmpersand = new Regex("&(?![a-zA-Z]{2,6};|#[0-9]{2,4};)");
            string res = badAmpersand.Replace(InpAsXML, "&amp;");
            param._ParameterValue = res;
            sproc.StoredProceduresParameter.Add(param);

            sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
        }

        internal void DeleteBIDData(List<CompanyEntity> Companies, string UserName)
        {
            string InpAsXML = "";
            InpAsXML = "'<BIDReject>";
            InpAsXML += "<UserId>" + UserName + "</UserId>";
            foreach (CompanyEntity company in Companies)
            {
                InpAsXML += "<Rec><InputId>" + company.InputId + "</InputId><SrcRecordId>" + company.SrcRecordId + "</SrcRecordId></Rec>";
            }
            InpAsXML += "</BIDReject>'";

            StoredProcedureEntity sproc = new StoredProcedureEntity();
            sproc.StoredProcedureName = "dnb.StewDeleteBadInputData";
            StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
            param.ParameterName = "@inp";
            Regex badAmpersand = new Regex("&(?![a-zA-Z]{2,6};|#[0-9]{2,4};)");
            string res = badAmpersand.Replace(InpAsXML, "&amp;");
            param._ParameterValue = res;
            sproc.StoredProceduresParameter.Add(param);

            sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
        }

        internal Tuple<List<CompanyEntity>, string> GetLCMCompany(int UserID, int PgaeIndex, int PageSize, out int TotalCount, bool IsApprove = false)
        {
            TotalCount = 0;

            List<CompanyEntity> lstCompany = new List<CompanyEntity>();
            string Message = "";

            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.StewGetLCMCompanyPage";

                StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserID.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PageSize", PageSize.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PageNumber", PgaeIndex.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@TotalRecords", TotalCount.ToString(), SQLServerDatatype.IntDataType, ParameterDirection.Output));
                sproc.StoredProceduresParameter.Add(GetParam("@GetForApproval", IsApprove.ToString(), SQLServerDatatype.BitDataType));
                string outParam = "";
                DataSet ds;
                DataTable dt = new DataTable(), dtMessage = new DataTable();
                ds = sql.ExecuteDataSetWithOutputParam(CommandType.StoredProcedure, sproc, out outParam);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (ds.Tables.Count > 1)
                    {
                        Message = Convert.ToString(ds.Tables[1].Rows[0][0]);
                    }
                }
                if (dt != null && dt.Rows.Count > 0)
                {
                    lstCompany = new CompanyAdapter().Adapt(dt);
                    foreach (CompanyEntity comp in lstCompany)
                    {
                        comp.Matches = new MatchRepository(Connection).GetMatches(comp.InputId, comp.SrcRecordId);
                    }
                    TotalCount = Convert.ToInt32(outParam);

                }
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return Tuple.Create(lstCompany, Message);
        }



        internal Tuple<List<CompanyEntity>, string> GetBIDCompany(int userId, int PgaeIndex, int PageSize, out int TotalCount)
        {
            TotalCount = 0;
            List<CompanyEntity> results = new List<CompanyEntity>();
            string Message = "";
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.StewGetBIDCompanyPage";

                StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", userId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PageSize", PageSize.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PageNumber", PgaeIndex.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@TotalRecords", TotalCount.ToString(), SQLServerDatatype.IntDataType, ParameterDirection.Output));

                string outParam = "";
                DataSet ds;
                DataTable dt = new DataTable(), dtMessage = new DataTable();
                ds = sql.ExecuteDataSetWithOutputParam(CommandType.StoredProcedure, sproc, out outParam, "", DBIntent.ReadWrite.ToString());
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (ds.Tables.Count > 1)
                    {
                        Message = Convert.ToString(ds.Tables[1].Rows[0][0]);
                    }
                }
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new CompanyAdapter().Adapt(dt);
                    TotalCount = Convert.ToInt32(outParam);
                }
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return Tuple.Create(results, Message); ;
        }

        #endregion

        #region "Login"

        internal UsersEntity StewUserLogIn(string EmailAddress, string DomainId, bool IsWebLogin = false)
        {

            List<UsersEntity> results = new List<UsersEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.StewUserLogIn";

                sproc.StoredProceduresParameter.Add(GetParam("@EmailAddress", EmailAddress.ToString(), SQLServerDatatype.NvarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@DomainId", Convert.ToString(DomainId), SQLServerDatatype.NvarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IsWebLogin", IsWebLogin.ToString(), SQLServerDatatype.BitDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new UsersAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                //Put log to db here
                //throw;
            }
            return results.FirstOrDefault();
        }
        internal UsersEntity GetUserByEmail(string EmailId)
        {

            List<UsersEntity> results = new List<UsersEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetUserByEmail";
                sproc.StoredProceduresParameter.Add(GetParam("@EmailId", EmailId.ToString(), SQLServerDatatype.NvarcharDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "");
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new UsersAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return results.FirstOrDefault();
        }


        internal UsersEntity GetUserBySAMLUserName(string UserName)
        {

            List<UsersEntity> results = new List<UsersEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetUserBySMALUser";
                sproc.StoredProceduresParameter.Add(GetParam("@SSOUser", UserName.ToString(), SQLServerDatatype.NvarcharDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new UsersAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return results.FirstOrDefault();
        }
        internal UsersEntity GetUserByLoginId(string EmailAddress)
        {

            List<UsersEntity> results = new List<UsersEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetUserByLoginId";
                sproc.StoredProceduresParameter.Add(GetParam("@EmailAddress", EmailAddress.ToString(), SQLServerDatatype.NvarcharDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new UsersAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return results.FirstOrDefault();
        }
        internal void StewUserLogIn(int userId)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.StewUserLogOut";

                StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
                param.ParameterName = "@UserId";
                param.ParameterValue = userId.ToString();
                sproc.StoredProceduresParameter.Add(param);
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);

            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
        }

        #endregion

        #region User Session
        internal void InsertOrUpdateUserSession(UsersEntity usersObj)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                if (usersObj.UserId > 0)
                {
                    sproc.StoredProcedureName = "dnb.UpdateUser";
                    sproc.StoredProceduresParameter.Add(GetParam("@UserId", usersObj.UserId.ToString(), SQLServerDatatype.IntDataType));
                }
                else
                {
                    sproc.StoredProcedureName = "dnb.InsertUser";
                }

                sproc.StoredProceduresParameter.Add(GetParam("@UserName", usersObj.UserName.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                //sproc.StoredProceduresParameter.Add(GetParam("@LoginId", usersObj.LoginId.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserStatusCode", usersObj.UserStatusCode.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserTypeCode", usersObj.UserTypeCode.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                throw;
            }
        }
        internal void InsertuserLoginAudit(int UserId, string IpAddress, int LoginStatus, string BrowserToken, string BrowserAgent)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.InsertuserLoginAudit";
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString().Trim(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IpAddress", IpAddress.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@LoginStatus", LoginStatus.ToString().Trim(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@BROWSERTOKEN", BrowserToken != null ? BrowserToken.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@BROWSERAGENT", BrowserAgent.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                throw;
            }
        }
        internal void DeleteUser(int UserId)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.DeleteUser";
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString(), SQLServerDatatype.IntDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {

                throw;
            }
        }

        internal void DeleteUserSessionFilter(int UserId)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.DeleteUserSessionFilter";
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString(), SQLServerDatatype.IntDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region "Report"
        internal DataTable GetFirstMatchCCMG()
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "rpt.GetFirstMatchCCMG";

                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                return dt;
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
        }

        internal DataTable GetCompanyAuditData()
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "rpt.GetCompanyAuditData";

                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                return dt;
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
        }

        internal DataTable GetInputCompanyData()
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "rpt.GetInputCompanyData";

                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                return dt;
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
        }

        internal DataTable GetOutputCompanyData()
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "rpt.GetOutputCompanyData";

                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                return dt;
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
        }

        internal DataTable GetAPIUsage()
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "rpt.GetAPIUsage";

                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                return dt;
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
        }
        internal DataTable GetTopMatchGrades(bool flag)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "rpt.GetTopMatchGrades";
                StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
                sproc.StoredProceduresParameter.Add(GetParam("@Top1Match", flag.ToString(), SQLServerDatatype.BitDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                return dt;
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
        }

        internal DataTable GetAPIUsageCount()
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.DashboardGetAPIUsageCount";

                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                return dt;
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
        }
        internal DataSet GetReport(string strReportType)
        {
            StoredProcedureEntity sproc = new StoredProcedureEntity();
            //DatabaseGateway da = new DatabaseGateway();
            DataSet ds = new DataSet();
            switch (strReportType)
            {
                case "1":
                    sproc.StoredProcedureName = "rpt.GetInputCompanyData";
                    ds = sql.ExecuteDataSet(CommandType.StoredProcedure, sproc);
                    break;
                case "2":
                    sproc.StoredProcedureName = "rpt.GetCompanyAuditData";
                    ds = sql.ExecuteDataSet(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                    break;
                case "3":
                    sproc.StoredProcedureName = "rpt.GetStewardshipStatistics";
                    ds = sql.ExecuteDataSet(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());

                    break;
                case "4":
                    sproc.StoredProcedureName = "rpt.GetOutputCompanyData";
                    ds = sql.ExecuteDataSet(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                    break;
                case "6":
                    sproc.StoredProcedureName = "rpt.GetInvestigateData";
                    ds = sql.ExecuteDataSet(CommandType.StoredProcedure, sproc);
                    break;
            }
            return ds;
        }
        internal DataSet GetActiveDataStatistics(string LOBTag)
        {
            StoredProcedureEntity sproc = new StoredProcedureEntity();
            //DatabaseGateway da = new DatabaseGateway();
            DataSet ds = new DataSet();
            sproc.StoredProcedureName = "rpt.GetActiveDataStatistics";
            sproc.StoredProceduresParameter.Add(GetParam("@LOBTag", !string.IsNullOrEmpty(LOBTag) ? LOBTag.ToString() : null, SQLServerDatatype.VarcharDataType));
            ds = sql.ExecuteDataSet(CommandType.StoredProcedure, sproc, "", DBIntent.ReadWrite.ToString());
            return ds;
        }
        internal DataTable GetAPIDataCount()
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "[dnb].[GetAPIDataCount]";
                StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                return dt;
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
        }
        internal DataTable GetDashboardGetDataQueueStatistics(string LOBTag, string Tag, int UserId)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "[dnb].[DashboardGetDataQueueStatistics]";
                sproc.StoredProceduresParameter.Add(GetParam("@LOBTag", !string.IsNullOrEmpty(LOBTag) ? LOBTag : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Tag", !string.IsNullOrEmpty(Tag) ? Tag : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString(), SQLServerDatatype.IntDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.ReadWrite.ToString());
                return dt;
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
        }

        internal DataSet DashboardGetLowConfidenceMatchStatistics(string LOBTag, string Tag, int UserId)
        {
            DataSet ds = new DataSet();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "[dnb].[DashboardGetLowConfidenceMatchStatistics]";
                sproc.StoredProceduresParameter.Add(GetParam("@LOBTag", !string.IsNullOrEmpty(LOBTag) ? LOBTag : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Tag", !string.IsNullOrEmpty(Tag) ? Tag : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString(), SQLServerDatatype.IntDataType));
                ds = sql.ExecuteDataSet(CommandType.StoredProcedure, sproc, "", DBIntent.ReadWrite.ToString());

                return ds;
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
        }
        internal DataSet GetStewardshipStatistics(string LOBTag, string Tag, int UserId, int ImportProcessId)
        {
            DataSet ds = new DataSet();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.DashboardGetStewardshipStatistics";
                sproc.StoredProceduresParameter.Add(GetParam("@LOBTag", !string.IsNullOrEmpty(LOBTag) ? LOBTag : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Tag", !string.IsNullOrEmpty(Tag) ? Tag : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ImportProcessId", ImportProcessId == 0 ? null : ImportProcessId.ToString(), SQLServerDatatype.IntDataType));
                ds = sql.ExecuteDataSet(CommandType.StoredProcedure, sproc, "", DBIntent.ReadWrite.ToString());

                return ds;
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
        }
        #endregion

        #region "Other Methods"

        private StoredProceduresParameterEntity GetParam(string ParameterName, string ParameterValue, SQLServerDatatype DataType, ParameterDirection Direction = ParameterDirection.Input)
        {
            StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
            param.ParameterName = ParameterName;
            param.ParameterValue = ParameterValue;
            param.Datatype = DataType;
            param.Direction = Direction;
            return param;
        }
        internal void StewUserActivityCloseWindow(int UserId)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.StewUserActivityCloseWindow";
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString(), SQLServerDatatype.IntDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Set Layout prefreances 
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="UserLayout"></param>
        internal void SetUserLayoutPreference(int UserId, string UserLayout)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.SetUserLayoutPreference";
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserLayoutPreference", UserLayout.ToString(), SQLServerDatatype.NvarcharDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                throw;
            }
        }




        internal void ExecuteCleanseMatchETL()
        {
            StoredProcedureEntity sproc = new StoredProcedureEntity();
            sproc.StoredProcedureName = "dnb.ExecuteCleanseMatchETL";
            sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
        }
        internal void AuthenticateApplication(string ClientGUID)
        {
            SqlHelper asql = new SqlHelper(General.authConnectionString);

            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "auth.AuthenticateApplication";
                sproc.StoredProceduresParameter.Add(GetParam("@ClientGUID", Convert.ToString(ClientGUID), SQLServerDatatype.NvarcharDataType));
                asql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
        }

        internal object GetETLJobStatus()
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetETLJobStatus";
                return sql.ExecuteScalar(CommandType.StoredProcedure, sproc, DBIntent.ReadWrite.ToString());
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal DataTable GetETLJobStatusMessage()
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetETLJobStatus";
                DataTable dt = new DataTable();
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.ReadWrite.ToString());
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }
        internal DataTable GetActiveUserData()
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetActiveUsersList";
                DataTable dt = new DataTable();
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }
        internal void InsertAPILogs(string ServiceTransactionID, DateTime? TransactionTimestamp, string SeverityText, string ResultID, string ResultText, string MatchDataCriteriaText, int MatchedQuantity, string DnBDUNSNumber = null)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();

                sproc.StoredProcedureName = "dnb.LogSearchApiCall";

                sproc.StoredProceduresParameter.Add(GetParam("@ServiceTransactionID", !string.IsNullOrEmpty(ServiceTransactionID) ? ServiceTransactionID.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@TransactionTimestamp", TransactionTimestamp != null ? TransactionTimestamp.ToString() : Convert.ToString(DateTime.Now), SQLServerDatatype.DateTimeDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SeverityText", !string.IsNullOrEmpty(SeverityText) ? SeverityText.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ResultID", !string.IsNullOrEmpty(ResultID) ? ResultID.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ResultText", !string.IsNullOrEmpty(ResultText) ? ResultText.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MatchDataCriteriaText", !string.IsNullOrEmpty(MatchDataCriteriaText) ? MatchDataCriteriaText.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MatchedQuantity", MatchedQuantity.ToString().Trim(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@DnBDUNSNumber", !string.IsNullOrEmpty(DnBDUNSNumber) ? Convert.ToString(DnBDUNSNumber) : "", SQLServerDatatype.VarcharDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region "Review data - Build 119"
        internal DataTable GetReviewData(int ConfidenceCode, string MatchGrade, string MDP, bool IsTopMatch, int CountryGroupId, string OrderByColumn, int PageSize, int PgaeIndex, out int TotalRecord)
        {
            TotalRecord = 0; DataTable dt;

            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.StewGetMatchesByCCMG";

                StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
                sproc.StoredProceduresParameter.Add(GetParam("@ConfidenceCode", ConfidenceCode.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MatchGradeText", MatchGrade.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MatchDataProfileText", MDP.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@FirstMatch", IsTopMatch.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CountryGroupId", CountryGroupId.ToString(), SQLServerDatatype.TinyintDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@OrderByColumn", OrderByColumn.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PageSize", PageSize.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PageNumber", PgaeIndex.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@TotalRecords", TotalRecord.ToString(), SQLServerDatatype.IntDataType, ParameterDirection.Output));

                string outParam = "";
                dt = sql.ExecuteDataTableWithOutputParam(CommandType.StoredProcedure, sproc, out outParam, "", DBIntent.ReadWrite.ToString());
                outParam = string.IsNullOrEmpty(outParam) ? "0" : outParam;
                TotalRecord = Convert.ToInt32(outParam);
            }
            catch (Exception)
            {
                //Put log to db here
                TotalRecord = 0;
                throw;
            }
            return dt;
        }
        #endregion

        #region "Review All data- Build 121"
        internal DataTable GetReviewAllData(bool IsTopMatch, int CountryGroupId, string LOBTag, string Tags, string ConfidenceCodes, int UserId, string OrderBy, int PageNumber, int PageSize, out int TotalRecords)
        {
            DataTable dt;
            try
            {
                TotalRecords = 0;
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.StewGetAllMatchesForReview";
                StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
                sproc.StoredProceduresParameter.Add(GetParam("@FirstMatch", IsTopMatch.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CountryGroupId", CountryGroupId.ToString(), SQLServerDatatype.TinyintDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@LOBTag", string.IsNullOrEmpty(LOBTag) ? null : LOBTag.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Tag", string.IsNullOrEmpty(Tags) ? null : Tags.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ConfidenceCodes", ConfidenceCodes.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@OrderBy", string.IsNullOrEmpty(OrderBy) ? null : OrderBy.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PageSize", PageSize.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PageNumber", PageNumber.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@TotalRecords", TotalRecords.ToString(), SQLServerDatatype.IntDataType, ParameterDirection.Output));
                string outParam = "";
                dt = sql.ExecuteDataTableWithOutputParam(CommandType.StoredProcedure, sproc, out outParam, "", DBIntent.ReadWrite.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    TotalRecords = Convert.ToInt32(outParam);
                }
            }
            catch (Exception)
            {
                //Put log to db here

                throw;
            }
            return dt;
        }
        #endregion

        #region "User Details"

        internal List<UsersEntity> GetSecurityQuestion()
        {
            List<UsersEntity> results = new List<UsersEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetSecurityQuestion";

                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new UsersAdapter().AdaptforSecurity(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }
        internal void UpdateSecurityQue(int userid, int securityQueId, string securityAnswer)
        {
            StoredProcedureEntity sproc = new StoredProcedureEntity();

            sproc.StoredProcedureName = "dnb.updateUserforSecurityQue";
            sproc.StoredProceduresParameter.Add(GetParam("@SecurityQuestionId", securityQueId.ToString().Trim(), SQLServerDatatype.IntDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@SecurityAnswer", securityAnswer.ToString().Trim(), SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@UserId", userid.ToString().Trim(), SQLServerDatatype.IntDataType));

            sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
        }
        public void updateUserLoginFirstTime(string EmailAddress, bool IsUserLoginFirstTime, string VerificationCode)
        {
            StoredProcedureEntity sproc = new StoredProcedureEntity();
            sproc.StoredProcedureName = "dnb.updateUserLoginFirstTime";
            sproc.StoredProceduresParameter.Add(GetParam("@EmailAddress", EmailAddress.ToString().Trim(), SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@IsUserLoginFirstTime", IsUserLoginFirstTime.ToString(), SQLServerDatatype.BitDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@VerificationCode", VerificationCode.ToString(), SQLServerDatatype.VarcharDataType));
            sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
        }
        public void updateUserEULA(DateTime EULAAcceptedDateTime, string EULAAcceptedIPAddress, int Id)
        {
            StoredProcedureEntity sproc = new StoredProcedureEntity();
            sproc.StoredProcedureName = "dnb.updateUserEULA";
            sproc.StoredProceduresParameter.Add(GetParam("@EULAAcceptedDateTime", EULAAcceptedDateTime.ToString().Trim(), SQLServerDatatype.DateTimeDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@EULAAcceptedIPAddress", EULAAcceptedIPAddress.ToString(), SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@Id", Id.ToString(), SQLServerDatatype.IntDataType));
            sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
        }
        #endregion

        #region "Import Data"
        internal int InsertStgInputCompany(InpCompanyEntity objCompany)
        {
            int result = 0;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "ext.InsertStgInputCompany";
                sproc.StoredProceduresParameter.Add(GetParam("@ImportProcessId", objCompany.ImportProcessId.ToString().Trim(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SrcRecordId", objCompany.SrcRecordId.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CompanyName", !string.IsNullOrEmpty(objCompany.CompanyName) ? objCompany.CompanyName.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Address", !string.IsNullOrEmpty(objCompany.Address) ? objCompany.Address.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Address1", !string.IsNullOrEmpty(objCompany.Address1) ? objCompany.Address1.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@City", !string.IsNullOrEmpty(objCompany.City) ? objCompany.City.ToString().Trim() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@State", !string.IsNullOrEmpty(objCompany.State) ? objCompany.State.ToString().Trim() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PostalCode", !string.IsNullOrEmpty(objCompany.PostalCode) ? objCompany.PostalCode.ToString().Trim() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Country", !string.IsNullOrEmpty(objCompany.Country) ? objCompany.Country.ToString().Trim() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PhoneNbr", !string.IsNullOrEmpty(objCompany.PhoneNbr) ? objCompany.PhoneNbr.ToString().Trim() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Tags", !string.IsNullOrEmpty(objCompany.Tags) ? objCompany.Tags.ToString().Trim() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@DUNSNumber", !string.IsNullOrEmpty(objCompany.DUNSNumber) ? objCompany.DUNSNumber.ToString().Trim() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CEOName", !string.IsNullOrEmpty(objCompany.CEOName) ? objCompany.CEOName.ToString().Trim() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Website", !string.IsNullOrEmpty(objCompany.Website) ? objCompany.Website.ToString().Trim() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@AltCompanyName", !string.IsNullOrEmpty(objCompany.AltCompanyName) ? objCompany.AltCompanyName.ToString().Trim() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@AltAddress", !string.IsNullOrEmpty(objCompany.AltAddress) ? objCompany.AltAddress.ToString().Trim() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@AltAddress1", !string.IsNullOrEmpty(objCompany.AltAddress1) ? objCompany.AltAddress1.ToString().Trim() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@AltCity", !string.IsNullOrEmpty(objCompany.AltCity) ? objCompany.AltCity.ToString().Trim() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@AltState", !string.IsNullOrEmpty(objCompany.AltState) ? objCompany.AltState.ToString().Trim() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@AltPostalCode", !string.IsNullOrEmpty(objCompany.AltPostalCode) ? objCompany.AltPostalCode.ToString().Trim() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@AltCountry", !string.IsNullOrEmpty(objCompany.AltCountry) ? objCompany.AltCountry.ToString().Trim() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Email", !string.IsNullOrEmpty(objCompany.Email) ? objCompany.Email.ToString().Trim() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@RegistrationNbr", !string.IsNullOrEmpty(objCompany.RegistrationNbr) ? objCompany.RegistrationNbr.ToString().Trim() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@RegistrationType", !string.IsNullOrEmpty(objCompany.RegistrationType) ? objCompany.RegistrationType.ToString().Trim() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@inLanguage", !string.IsNullOrEmpty(objCompany.inLanguage) ? objCompany.inLanguage.ToString().Trim() : null, SQLServerDatatype.VarcharDataType));
                result = Convert.ToInt32(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
        internal int InsertDataImportProcess(DataImportProcessEntity objDataImport)
        {
            int result = 0;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "aud.InsertDataImportProcess";
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", objDataImport.UserId.ToString().Trim(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SourceType", objDataImport.SourceType.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Source", objDataImport.Source.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ImportedRowCount", objDataImport.ImportedRowCount.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ProcessedRowCount", objDataImport.ProcessedRowCount.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ProcessStatusId", objDataImport.ProcessStatusId.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                result = Convert.ToInt32(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
        internal string ProcessDataImport(int ImportProcessId)
        {
            string Message = string.Empty;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.ProcessDataImport";
                sproc.StoredProceduresParameter.Add(GetParam("@ImportProcessId", ImportProcessId.ToString(), SQLServerDatatype.IntDataType));
                Message = Convert.ToString(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
                return Message;
            }
            catch (Exception ex)
            {
                return Message = ex.Message;
            }
        }

        internal string ImportDataRefresh(int ImportProcessId)
        {
            string Message = string.Empty;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "ext.ImportDataRefresh";
                sproc.StoredProceduresParameter.Add(GetParam("@ImportProcessId", ImportProcessId.ToString(), SQLServerDatatype.IntDataType));
                Message = Convert.ToString(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
                return Message;
            }
            catch (Exception ex)
            {
                return Message = ex.Message;
            }
        }
        #endregion

        #region search data
        public void InsertCleanseMatchCallResults(string SrcRecordId, string responseJson, string Apirequest, int UserId, string InputId)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "aud.InsertCleanseMatchCallResults";
                sproc.StoredProceduresParameter.Add(GetParam("@SrcRecordId", SrcRecordId != null ? SrcRecordId.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ResponseJSON", responseJson != null ? responseJson.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@APIRequest", Apirequest != null ? Apirequest.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId != 0 ? UserId.ToString().Trim() : "", SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@InputId", InputId != null ? InputId.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));

                sql.ExecuteScalar(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion


        #region Reject All
        internal DataTable GetStewTags(bool IsMatchData, string LOBTag)
        {
            DataTable result = new DataTable();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetStewTags";
                sproc.StoredProceduresParameter.Add(GetParam("@IsMatchData", IsMatchData.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@LOBTag", string.IsNullOrEmpty(LOBTag) ? null : LOBTag.ToString(), SQLServerDatatype.VarcharDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }
        internal DataTable GetImportProcessesByQueue(string Queue, bool IsMatchdata)
        {
            DataTable result = new DataTable();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetImportProcessesByQueue";
                sproc.StoredProceduresParameter.Add(GetParam("@Queue", string.IsNullOrEmpty(Queue) ? null : Queue.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IsMatchdata", IsMatchdata.ToString().Trim(), SQLServerDatatype.BitDataType));

                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }
        internal int StewRejectAllLCMRecords(string CountryISO2AlphaCode, int CountryGroupId, string Tag, string ImportProcess, int HighestConfidenceCode, string SrcRecordId, string City, string State, bool CityExactMatch, bool StateExactMatch, int UserId, bool GetCountsOnly, bool Purge)
        {
            try
            {
                int count = 0;
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.StewRejectAllLCMRecords";
                sproc.StoredProceduresParameter.Add(GetParam("@CountryISO2AlphaCode", string.IsNullOrEmpty(CountryISO2AlphaCode) ? null : CountryISO2AlphaCode.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CountryGroupId", CountryGroupId == 0 ? null : CountryGroupId.ToString().Trim(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Tag", string.IsNullOrEmpty(Tag) ? null : Tag.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ImportProcess", string.IsNullOrEmpty(ImportProcess) ? null : ImportProcess.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@HighestConfidenceCode", HighestConfidenceCode == 0 ? null : HighestConfidenceCode.ToString(), SQLServerDatatype.BigintDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SrcRecordId", string.IsNullOrEmpty(SrcRecordId) ? null : SrcRecordId.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@City", string.IsNullOrEmpty(City) ? null : City.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@State", string.IsNullOrEmpty(State) ? null : State.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CityExactMatch", CityExactMatch.ToString().Trim(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@StateExactMatch", StateExactMatch.ToString().Trim(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString().Trim(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@GetCountsOnly", GetCountsOnly.ToString().Trim(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Purge", Purge.ToString().Trim(), SQLServerDatatype.BitDataType));
                if (GetCountsOnly == false)
                {
                    sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
                }
                else
                {
                    count = (Int32)sql.ExecuteScalar(CommandType.StoredProcedure, sproc);
                }
                return count;
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
        }
        internal int PurgeAllBIDRecords(string CountryISO2AlphaCode, int CountryGroupId, string Tag, string ImportProcess, string SrcRecordId, string City, string State, bool CityExactMatch, bool StateExactMatch, int UserId, bool GetCountsOnly)
        {
            try
            {
                int count = 0;
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.PurgeAllBIDRecords";
                sproc.StoredProceduresParameter.Add(GetParam("@CountryISO2AlphaCode", string.IsNullOrEmpty(CountryISO2AlphaCode) ? null : CountryISO2AlphaCode.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CountryGroupId", CountryGroupId == 0 ? null : CountryGroupId.ToString().Trim(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Tag", string.IsNullOrEmpty(Tag) ? null : Tag.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ImportProcess", string.IsNullOrEmpty(ImportProcess) ? null : ImportProcess.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SrcRecordId", string.IsNullOrEmpty(SrcRecordId) ? null : SrcRecordId.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@City", string.IsNullOrEmpty(City) ? null : City.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@State", string.IsNullOrEmpty(State) ? null : State.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CityExactMatch", CityExactMatch.ToString().Trim(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@StateExactMatch", StateExactMatch.ToString().Trim(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString().Trim(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@GetCountsOnly", GetCountsOnly.ToString().Trim(), SQLServerDatatype.BitDataType));

                if (GetCountsOnly == false)
                {
                    sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
                }
                else
                {
                    count = (Int32)sql.ExecuteScalar(CommandType.StoredProcedure, sproc);
                }
                return count;
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
        }

        internal int InsertStgImportDataRefresh(InpCompanyDataRefershEntity objCompany)
        {
            int result = 0;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "ext.InsertStgImportDataRefresh";
                sproc.StoredProceduresParameter.Add(GetParam("@ImportProcessId", objCompany.ImportProcessId.ToString().Trim(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SrcRecordId", objCompany.SrcRecordId.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Country", !string.IsNullOrEmpty(objCompany.Country) ? objCompany.Country.ToString().Trim() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Tags", !string.IsNullOrEmpty(objCompany.Tags) ? objCompany.Tags.ToString().Trim() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@DUNSNumber", !string.IsNullOrEmpty(objCompany.DUNSNumber) ? objCompany.DUNSNumber.ToString().Trim() : null, SQLServerDatatype.VarcharDataType));
                result = Convert.ToInt32(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }


        public void StewPurgeSingleRecord(string InputId, string SrcRecordId, int UserId, string Queue)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.StewPurgeSingleRecord";
                sproc.StoredProceduresParameter.Add(GetParam("@InputId", InputId != null ? InputId.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SrcRecordId", SrcRecordId != null ? SrcRecordId.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId != 0 ? UserId.ToString().Trim() : "", SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Queue", Queue != null ? Queue.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                sql.ExecuteScalar(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region "Export Data"

        internal SqlDataReader ExportCompanyDataReader(string Tag, string ImportProcess, string LOBTag, bool FlagExport, string SrcRecID, bool SrcRecIdExactMatch, int UserId)
        {
            SqlDataReader reader;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.ExportCompanyData";
                sproc.StoredProceduresParameter.Add(GetParam("@Tag", !string.IsNullOrEmpty(Tag) ? Tag : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ImportProcess", !string.IsNullOrEmpty(ImportProcess) ? ImportProcess : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@LOBTag", !string.IsNullOrEmpty(LOBTag) ? LOBTag : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@FlagExport", FlagExport.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SrcRecordId ", string.IsNullOrEmpty(SrcRecID) ? null : SrcRecID.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SrcRecIdExactMatch", SrcRecIdExactMatch.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString(), SQLServerDatatype.IntDataType)); //D&B - Inclusive tags on ,while exporting data issue (MP-647)
                reader = sql.ExecuteDataReader(CommandType.StoredProcedure, sproc);
                return reader;
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal SqlDataReader ExportActiveQueueOutputDataReader(string Tag, string ImportProcess, string LOBTag, bool FlagExport, string SrcRecID, bool SrcRecIdExactMatch, int UserId)
        {
            SqlDataReader reader;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.ExportActiveDataQueue";
                sproc.StoredProceduresParameter.Add(GetParam("@Tag", !string.IsNullOrEmpty(Tag) ? Tag.ToString() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ImportProcess", !string.IsNullOrEmpty(ImportProcess) ? ImportProcess.ToString() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@LOBTag", !string.IsNullOrEmpty(LOBTag) ? LOBTag.ToString() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@FlagExport", FlagExport.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SrcRecordId ", string.IsNullOrEmpty(SrcRecID) ? null : SrcRecID.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SrcRecIdExactMatch", SrcRecIdExactMatch.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString(), SQLServerDatatype.IntDataType));//D&B - Inclusive tags on ,while exporting data issue (MP-719)
                reader = sql.ExecuteDataReader(CommandType.StoredProcedure, sproc);
                return reader;
            }
            catch (Exception)
            {
                throw;
            }
        }
        internal DataTable GetExportedDataImportProcess()
        {
            DataTable result = new DataTable();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetExportedDataImportProcess";

                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }
        internal SqlDataReader GetExportDUNSTransferDataReader(bool FlagExport)
        {
            SqlDataReader reader;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.ExportDUNSTransferData";
                sproc.StoredProceduresParameter.Add(GetParam("@FlagExport", FlagExport.ToString(), SQLServerDatatype.BitDataType));
                reader = sql.ExecuteDataReader(CommandType.StoredProcedure, sproc);
                return reader;
            }
            catch (Exception)
            {
                throw;
            }
        }
        internal int GetExportActiveQueueOutputCnt()
        {
            int count = 0;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetExportActiveQueueOutputCnt";
                count = (Int32)sql.ExecuteScalar(CommandType.StoredProcedure, sproc, DBIntent.Read.ToString());
            }
            catch (Exception)
            {
                throw;
            }
            return count;
        }
        internal SqlDataReader ExportLCMQueueDataReader(string Tag, string ImportProcess, string LOBTag, bool FlagExport, string SrcRecID, bool SrcRecIdExactMatch)
        {
            SqlDataReader reader;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.ExportLowConfidenceMatchQueue";
                sproc.StoredProceduresParameter.Add(GetParam("@Tag", !string.IsNullOrEmpty(Tag) ? Tag : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ImportProcess", !string.IsNullOrEmpty(ImportProcess) ? ImportProcess : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@LOBTag", !string.IsNullOrEmpty(LOBTag) ? LOBTag : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@FlagExport", FlagExport.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SrcRecordId ", string.IsNullOrEmpty(SrcRecID) ? null : SrcRecID.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SrcRecIdExactMatch", SrcRecIdExactMatch.ToString(), SQLServerDatatype.BitDataType));
                reader = sql.ExecuteDataReader(CommandType.StoredProcedure, sproc);
                return reader;
            }
            catch (Exception)
            {
                throw;
            }
        }
        internal SqlDataReader ExportBadInputDataQueueExportBadInputDataQueueDataReader(string Tag, string ImportProcess, string LOBTag, bool FlagExport, string SrcRecID, bool SrcRecIdExactMatch)
        {
            SqlDataReader reader;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.ExportBadInputDataQueue";
                sproc.StoredProceduresParameter.Add(GetParam("@Tag", !string.IsNullOrEmpty(Tag) ? Tag : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ImportProcess", !string.IsNullOrEmpty(ImportProcess) ? ImportProcess : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@LOBTag", !string.IsNullOrEmpty(LOBTag) ? LOBTag : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@FlagExport", FlagExport.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SrcRecordId ", string.IsNullOrEmpty(SrcRecID) ? null : SrcRecID.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SrcRecIdExactMatch", SrcRecIdExactMatch.ToString(), SQLServerDatatype.BitDataType));
                reader = sql.ExecuteDataReader(CommandType.StoredProcedure, sproc);
                return reader;
            }
            catch (Exception)
            {
                throw;
            }
        }
        internal void FinalizeCompanyDataExport(bool FlagExport)
        {
            StoredProcedureEntity sproc = new StoredProcedureEntity();
            sproc.StoredProcedureName = "dnb.FinalizeCompanyDataExport";
            sproc.StoredProceduresParameter.Add(GetParam("@FlagExport", FlagExport.ToString(), SQLServerDatatype.BitDataType));
            sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
        }
        #endregion

        #region Report Data

        internal DataTable GetDataQueueDashboardReport()
        {
            DataTable dt;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "rpt.GetDataQueueDashboard";
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }

        internal DataSet GetdtDataStewardStatisticsReport(string UserGroup)
        {
            StoredProcedureEntity sproc = new StoredProcedureEntity();
            //DatabaseGateway da = new DatabaseGateway();
            DataSet ds = new DataSet();
            sproc.StoredProcedureName = "rpt.GetDataStewardshipStatistics";
            sproc.StoredProceduresParameter.Add(GetParam("@UserGroup", string.IsNullOrEmpty(UserGroup) ? null : UserGroup.ToString(), SQLServerDatatype.VarcharDataType));
            ds = sql.ExecuteDataSet(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
            return ds;
        }
        internal DataSet GetdtAPIReport()
        {
            StoredProcedureEntity sproc = new StoredProcedureEntity();
            DataSet ds = new DataSet();
            sproc.StoredProcedureName = "rpt.GetAPIUsageStatistics";
            ds = sql.ExecuteDataSet(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
            return ds;
        }

        #endregion

        #region Build List
        internal object InsertBuildSearch(int userId, string RequestedJson, string ResponseJson, DateTime? RequestedDate, DateTime? ResponseDate)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "aud.InsertSearchList";
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", userId.ToString().Trim(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@RequestJson", RequestedJson.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ResponseJson", ResponseJson.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@RequestDateTime", RequestedDate.ToString().Trim(), SQLServerDatatype.DateTimeDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ResponseDateTime", ResponseDate.ToString().Trim(), SQLServerDatatype.DateTimeDataType));
                return sql.ExecuteScalar(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                throw;
            }
        }
        internal DataTable GetSearchListResults(int UserId)
        {
            try
            {
                List<SearchListEntity> results = new List<SearchListEntity>();

                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "aud.GetSearchListResult";
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString(), SQLServerDatatype.IntDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }
        internal DataTable ExportBuildResult(long SearchResultsId)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "ext.Out_DP_BuildList_Base";
                sproc.StoredProceduresParameter.Add(GetParam("@SearchResultsId", SearchResultsId.ToString(), SQLServerDatatype.BigintDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }
        internal DataTable ViewHistory(long Id)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "aud.GetSearchListById";
                StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
                sproc.StoredProceduresParameter.Add(GetParam("@Id", Id.ToString(), SQLServerDatatype.BigintDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion


        #region "Re Match Records"
        //Implement re-match queue (MP-14)
        public string StewReMatchBadInputData(ReMatchRecordsEntity model)
        {
            string message = string.Empty;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.StewReMatchBadInputData";
                sproc.StoredProceduresParameter.Add(GetParam("@CountryISO2AlphaCode", string.IsNullOrEmpty(model.CountryISOAlpha2Code) ? null : model.CountryISOAlpha2Code.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CountryGroupId", model.CountryGroupId == 0 ? null : model.CountryGroupId.ToString().Trim(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Tag", string.IsNullOrEmpty(model.Tag) ? null : model.Tag.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ImportProcess", string.IsNullOrEmpty(model.ImportProcess) ? null : model.ImportProcess.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SrcRecordId", string.IsNullOrEmpty(model.SrcRecordId) ? null : model.SrcRecordId.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@City", string.IsNullOrEmpty(model.City) ? null : model.City.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@State", string.IsNullOrEmpty(model.State) ? null : model.State.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CityExactMatch", model.CityExactMatch.ToString().Trim(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@StateExactMatch", model.StateExactMatch.ToString().Trim(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", model.UserId.ToString().Trim(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@GetCountsOnly", model.GetCountsOnly.ToString().Trim(), SQLServerDatatype.BitDataType));
                message = Convert.ToString(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
                return message;
            }
            catch (Exception ex)
            {
                //Put log to db here
                return ex.Message;
            }
        }
        #endregion





        public DataTable GetComanyAttribute(string SrcRec)
        {
            DataTable dt;

            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetCompanyAttributes";
                StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
                sproc.StoredProceduresParameter.Add(GetParam("@SrcRecordId", SrcRec.ToString(), SQLServerDatatype.VarcharDataType));
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());

            }
            catch (Exception)
            {
                //Put log to db here

                throw;
            }
            return dt;

        }






        internal string GetAPILayer(string DnBAPICode)
        {
            string Message = string.Empty;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetAPILayer";
                sproc.StoredProceduresParameter.Add(GetParam("@DnBAPICode", DnBAPICode.ToString(), SQLServerDatatype.VarcharDataType));
                Message = Convert.ToString(sql.ExecuteScalar(CommandType.StoredProcedure, sproc, DBIntent.Read.ToString()));
                return Message;
            }
            catch (Exception ex)
            {
                return Message = ex.Message;
            }
        }

        /// <summary>
        /// Get Family Tree
        /// </summary>
        /// <returns></returns>
        internal DataTable GetFamilyTree(int FamilyTreeId)
        {
            DataTable dt;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetFamilyTreeDetailById";
                sproc.StoredProceduresParameter.Add(GetParam("@FamilyTreeId", FamilyTreeId.ToString(), SQLServerDatatype.VarcharDataType));
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }
        internal DataTable GetFamilyTreeChild(string ParentFamilyTreeDetailId)
        {
            DataTable dt;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetFamilyTreeChildDetailById";
                sproc.StoredProceduresParameter.Add(GetParam("@ParentFamilyTreeDetailId", ParentFamilyTreeDetailId.ToString(), SQLServerDatatype.VarcharDataType));
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.ReadWrite.ToString());
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }
        internal DataTable GetListFamilyTree()
        {
            DataTable dt;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.ListAllFamilyTrees";
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }

        internal DataTable GetCorporateLinkageDuns()
        {
            DataTable dt;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetCorporateLinkageDuns";
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }

        internal string PopulateCorporateLinkageDuns(string duns)
        {
            string result = string.Empty;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.MergeFamilyTreeFromCORPLINK";
                sproc.StoredProceduresParameter.Add(GetParam("@DnBDUNSNumber", duns, SQLServerDatatype.VarcharDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
                result = Convert.ToString(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        internal string DeleteFamilyTreeNode(int sourceFamilyTreeId, int sourceFamilyTreeDetailId, int userId)
        {
            string result = string.Empty;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.DeleteFamilyTreeNode";
                sproc.StoredProceduresParameter.Add(GetParam("@FamilyTreeId", sourceFamilyTreeId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@FamilyTreeDetailId", sourceFamilyTreeDetailId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", userId.ToString(), SQLServerDatatype.IntDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        internal string AddFamilyTreeNode(int familyTreeId, string detailId, string nodeName, string nodeDisplayDetail, string nodeType, int? parentFamilyTreeDetailId, int userId)
        {
            string result = string.Empty;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.AddNewFamilyTreeNode";
                sproc.StoredProceduresParameter.Add(GetParam("@FamilyTreeId", familyTreeId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@DetailId", detailId.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@NodeName", nodeName.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@NodeDisplayDetail", nodeDisplayDetail.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@NodeType", nodeType.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ParentFamilyTreeDetailId", (parentFamilyTreeDetailId == null ? null : parentFamilyTreeDetailId.ToString()), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", userId.ToString(), SQLServerDatatype.IntDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        internal string AddFamilyTree(string familyTreeName, string familyTreeType, string alternateId, int userId)
        {
            string result = string.Empty;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.CreateNewFamilyTree";
                sproc.StoredProceduresParameter.Add(GetParam("@FamilyTreeName", familyTreeName.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@FamilyTreeType", familyTreeType.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@AlternateId", alternateId.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", userId.ToString(), SQLServerDatatype.IntDataType));
                sql.ExecuteScalar(CommandType.StoredProcedure, sproc);
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        internal string DeleteFamilyTree(int familyTreeId, int userId)
        {
            string result = string.Empty;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.DeleteFamilyTree";
                sproc.StoredProceduresParameter.Add(GetParam("@FamilyTreeId", familyTreeId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", userId.ToString(), SQLServerDatatype.IntDataType));
                result = Convert.ToString(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        internal string DuplicaeFamilyTree(int familyTreeId, string familyTreeName, string familyTreeType, int userId)
        {
            string result = string.Empty;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.DuplicateFamilyTree";
                sproc.StoredProceduresParameter.Add(GetParam("@FamilyTreeId", familyTreeId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@FamilyTreeName", familyTreeName.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@FamilyTreeType", familyTreeType.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", userId.ToString(), SQLServerDatatype.IntDataType));
                sql.ExecuteScalar(CommandType.StoredProcedure, sproc);
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        internal bool MoveFamilyTree(int sourceFamilyTreeId, int sourceFamilyTreeDetailId, int destinationFamilyTreeId, int destinationFamilyTreeDetailId, string operation, int userId)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.MoveFamilyTreeNode";
                sproc.StoredProceduresParameter.Add(GetParam("@SourceFamilyTreeId", sourceFamilyTreeId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SourceFamilyTreeDetailId", sourceFamilyTreeDetailId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@DestinationFamilyTreeId", destinationFamilyTreeId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@DestinationFamilyTreeDetailId", destinationFamilyTreeDetailId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Operation", operation, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", userId.ToString(), SQLServerDatatype.IntDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal DataTable GetFamilyTreeById(string id)
        {
            DataTable dt;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetFamilyTreeById";
                sproc.StoredProceduresParameter.Add(GetParam("@FamilyTreeId", id, SQLServerDatatype.IntDataType));
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());

            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }

        #region OI Build List
        // Inserting in the view history list
        internal object InsertOIBuildSearch(int userId, string RequestedJson, string ResponseJson, DateTime? RequestedDate, DateTime? ResponseDate, int ResultCount)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "oi.InsertUpdateBuildSearch";
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", userId.ToString().Trim(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@RequestJson", RequestedJson.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ResponseJson", ResponseJson.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@RequestDateTime", RequestedDate.ToString().Trim(), SQLServerDatatype.DateTimeDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ResponseDateTime", ResponseDate.ToString().Trim(), SQLServerDatatype.DateTimeDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ResultCount", ResultCount.ToString().Trim(), SQLServerDatatype.IntDataType));
                return sql.ExecuteScalar(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                throw;
            }
        }
        // Getting OI Build search list
        internal DataTable GetOIBuildSearchListResults(int UserId)
        {
            try
            {
                List<SearchListEntity> results = new List<SearchListEntity>();

                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "oi.GetBuilldSearchResult";
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString(), SQLServerDatatype.IntDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }
        // View the records selected from history popup
        internal DataTable ViewOIHistory(long Id)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "oi.GetOIBuildSearchListById";
                StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
                sproc.StoredProceduresParameter.Add(GetParam("@Id", Id.ToString(), SQLServerDatatype.BigintDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region Remove Data From File
        public string RemoveFileData(int ImportProcessId, int UserId)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.RemoveFileData";
                sproc.StoredProceduresParameter.Add(GetParam("@ImportProcessId", ImportProcessId.ToString().Trim(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId != 0 ? UserId.ToString().Trim() : "", SQLServerDatatype.IntDataType));
                string Message = Convert.ToString(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
                return Message;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Preview Enrichment Data
        public DataTable UXGetFirmographicsURL(string DnBDUNSNumber, string CountryISO2AlphaCode)
        {
            DataTable dt;

            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.UXGetFirmographicsURL";
                StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
                sproc.StoredProceduresParameter.Add(GetParam("@DnBDUNSNumber", DnBDUNSNumber.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CountryISO2AlphaCode", CountryISO2AlphaCode.ToString(), SQLServerDatatype.VarcharDataType));
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return dt;
        }
        public void ProcessDataForEnrichment(int APIRequestId, int DnBAPIId, string DnBDUNSNumber, string CountryISOAlpha2Code, string JSONResponse, string ResultID, DateTime TransactionTimestamp, int CredentialId)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.apiProcessDataEnrichment";
                sproc.StoredProceduresParameter.Add(GetParam("@APIRequestId", APIRequestId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@DnBAPIId", DnBAPIId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@DnBDUNSNumber", DnBDUNSNumber != null ? DnBDUNSNumber.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CountryISOAlpha2Code", CountryISOAlpha2Code != null ? CountryISOAlpha2Code.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@JSONResponse", JSONResponse != null ? JSONResponse.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ResultID", ResultID != null ? ResultID.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@TransactionTimestamp", TransactionTimestamp != null ? TransactionTimestamp.ToString().Trim() : "", SQLServerDatatype.DateTimeDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CredentialId", CredentialId.ToString(), SQLServerDatatype.IntDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc, "", DBIntent.ReadWrite.ToString());
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
        }
        #endregion
    }
}
