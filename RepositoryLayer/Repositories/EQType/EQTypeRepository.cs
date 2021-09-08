using Dapper;
using IdylAPI.Models;
using IdylAPI.Models.Master;
using IdylAPI.Services.Interfaces.Master;
using Microsoft.Extensions.Configuration;
using PAUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using static System.Data.CommandType;
namespace IdylAPI.Services.Repository.Master
{
    public class EQTypeRepository : IEQTypeRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connStr;
        public EQTypeRepository(IConfiguration configuration)
        {

            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("IDYLConnection");
        }

        public Result Retrive(WhereParameter whereParameter)
        {
            Result result = new Result();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    conn.Open();

                    DynamicParameters parameters = new DynamicParameters();
                    string condition = $" where me.companyno = {whereParameter.SiteNo}";
                    condition += $" and me.isdelete = 0 ";
                    if (!string.IsNullOrEmpty(InputVal.ToString(whereParameter.Filter)))
                    {
                        condition += $" and(me.eqtypename like '%{whereParameter.Filter}%'";
                        condition += $" or me.eqtypecode like '%{whereParameter.Filter}%')";
                    }
                    
                    parameters.Add("@WhereSel", condition);
                    parameters.Add("@StartRow", whereParameter.StartRow);
                    parameters.Add("@EndRow", whereParameter.EndRow);

                    IEnumerable<EQType> eQs = SqlMapper.Query<EQType>(conn, "msp_EQType_Retrive", parameters, commandType: StoredProcedure);
                    result.Data = eQs;
                    result.StatusCode = 200;
                }
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.ErrMsg = ex.Message;
            }
            return result;
        }
        public Result Insert(EQType eQType, Models.Authorize.User user)
        {
            Result result = new Result();
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        DynamicParameters parameters = new DynamicParameters();

                        if (eQType.EQTypeNo == 0)
                        {
                            parameters.Add("@EQTypeNo", eQType.EQTypeNo, DbType.Int32, ParameterDirection.Output);
                            parameters.Add("@EQTypeCode", eQType.EQTypeCode);
                            parameters.Add("@EQTypeName", eQType.EQTypeName);
                            parameters.Add("@CompanyNo", eQType.CompanyNo);
                            parameters.Add("@CreatedBy", user.CustomerNo);

                            conn.Query<int>("sp_EQType_Insert", parameters, commandType: StoredProcedure, transaction: trans);
                            eQType.EQTypeNo = parameters.Get<int>("@EQTypeNo");
                        }
                        else
                        {
                            //parameters = new DynamicParameters();
                            //parameters.Add("@EQNo", eQ.EQNo);
                            //EQ eQOld = conn.QueryFirst<EQ>("sp_EQ_GetByNo", parameters, commandType: StoredProcedure, transaction: trans);

                            //parameters = new DynamicParameters();
                            //parameters.Add("@EQNo", eQ.EQNo);
                            //parameters.Add("@EQCode", eQ.EQCode);
                            //parameters.Add("@EQName", eQ.EQName);
                            //parameters.Add("@LocationNo", eQ.Location);
                            //parameters.Add("@LocationName", eQ.LocationName);
                            //parameters.Add("@EQTypeNo", eQ.EQType);
                            //parameters.Add("@EQTypeName", eQ.EQTypeName);
                            //parameters.Add("@Manufacturer", eQ.Manufacturer);
                            //parameters.Add("@Model", eQ.Model);
                            //parameters.Add("@SerialNo", eQ.SerialNo);
                            //parameters.Add("@VendorNo", eQ.VendorNo);
                            //parameters.Add("@Phone", eQOld.Phone);
                            //parameters.Add("@Email", eQOld.Email);
                            //parameters.Add("@Attach", null);
                            //parameters.Add("@Remark", eQOld.Remark);
                            //parameters.Add("@CompanyNo", eQOld.CompanyNo);
                            //parameters.Add("@UpdatedBy", user.CustomerNo);
                            //parameters.Add("@Criticality", eQ.Criticality);
                            //parameters.Add("@InstalledDate", eQ.InstalledDate);
                            //parameters.Add("@WarrantyDate", eQ.WarrantyDate);
                            //parameters.Add("@Capacity", eQ.Capacity);

                            //conn.Execute("sp_EQ_Update", parameters, commandType: StoredProcedure, transaction: trans);

                        }

                        trans.Commit();
                        result.Data = eQType.EQTypeNo;
                        result.ErrMsg = "บันทึกเรียบร้อย";
                        result.StatusCode = 200;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        result.StatusCode = 500;
                        if (ex.Message == "duplicate")
                        {
                            result.ErrMsg = "ไม่สามารถบันทึกได้ เนื่องจากรหัสซ้ำ";
                        }
                        else
                        {
                            result.ErrMsg = ex.Message;
                        }
                    }



                }

                return result;
            }
        }
    }
}
