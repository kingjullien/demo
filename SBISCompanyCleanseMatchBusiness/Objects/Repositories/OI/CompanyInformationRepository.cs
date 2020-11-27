using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.Repositories
{
    internal class CompanyInformationRepository : RepositoryParent
    {
        public CompanyInformationRepository(string connectionString) : base(connectionString) { }
        #region "Comman Method"
        private StoredProceduresParameterEntity GetParam(string ParameterName, string ParameterValue, SQLServerDatatype DataType, ParameterDirection Direction = ParameterDirection.Input)
        {
            StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
            param.ParameterName = ParameterName;
            param.ParameterValue = ParameterValue;
            param.Datatype = DataType;
            param.Direction = Direction;
            return param;
        }
        #endregion

        internal int InsertCompanyInformation(OICompanyInformationEntity obj)
        {
            int result = 0;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "OI.InsertCompanyInformation";
                sproc.StoredProceduresParameter.Add(GetParam("@SrcRecordId", obj.SrcRecordId.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CompanyName", string.IsNullOrEmpty(obj.CompanyName) ? "" : obj.CompanyName.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Address1", string.IsNullOrEmpty(obj.Address1) ? null : obj.Address1.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Address2", string.IsNullOrEmpty(obj.Address2) ? null : obj.Address2.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@City", string.IsNullOrEmpty(obj.City) ? null : obj.City.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@State", string.IsNullOrEmpty(obj.State) ? null : obj.State.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PostalCode", string.IsNullOrEmpty(obj.PostalCode) ? null : obj.PostalCode.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CountryISOAlpha2Code", string.IsNullOrEmpty(obj.CountryISOAlpha2Code) ? null : obj.CountryISOAlpha2Code.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PhoneNbr", string.IsNullOrEmpty(obj.PhoneNbr) ? null : obj.PhoneNbr.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@OrbNum", string.IsNullOrEmpty(obj.OrbNum) ? null : obj.OrbNum.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@EIN", string.IsNullOrEmpty(obj.EIN) ? null : obj.EIN.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Website", string.IsNullOrEmpty(obj.Website) ? null : obj.Website.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Email", string.IsNullOrEmpty(obj.Email) ? null : obj.Email.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Subdomain", obj.Subdomain.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserName", string.IsNullOrEmpty(obj.UserName) ? null : obj.UserName.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserEmail", string.IsNullOrEmpty(obj.UserEmail) ? null : obj.UserEmail.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Message", string.IsNullOrEmpty(obj.Message) ? null : obj.Message.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MatchOrbNumber", string.IsNullOrEmpty(obj.MatchOrbNumber) ? null : obj.MatchOrbNumber.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@InputId", obj.InputId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Tags", string.IsNullOrEmpty(obj.Tags) ? null : obj.Tags.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@TicketNumber", string.IsNullOrEmpty(obj.TicketNumber) ? null : obj.TicketNumber.ToString(), SQLServerDatatype.VarcharDataType));

                result = Convert.ToInt32(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
    }
}
