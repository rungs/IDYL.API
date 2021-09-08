using Dapper;
using IdylAPI.Models;
using IdylAPI.Models.Master;
using IdylAPI.Services.Interfaces.Master;
using PAUtility;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using static System.Data.CommandType;
namespace IdylAPI.Services.Repository.Master
{
    public class LocationRepository : BaseRepositoryV2<Location>, ILocationRepository
    {
        private readonly AppDBContext _context;
        public LocationRepository(AppDBContext context) : base(context)
        {
            _context = context;
        }

        public Result Retrive(WhereParameter whereParameter, Models.Authorize.User user)
        {
            Result result = new Result();
            try
            {
                using (SqlConnection conn = new SqlConnection(_context.Connection.ConnectionString))
                {
                    conn.Open();

                    DynamicParameters parameters = new DynamicParameters();
                    string condition = $" where location.companyno = {whereParameter.SiteNo}";
                    condition += $" and location.isdelete = 0 ";
                    condition += $" and _SystUser_Location.userno =  " + user.UserNo;

                    if (!string.IsNullOrEmpty(InputVal.ToString(whereParameter.Filter)))
                    {
                        condition += $" and(location.locationname like '%{whereParameter.Filter}%'";
                        condition += $" or location.locationcode like '%{whereParameter.Filter}%')";
                    }

                    parameters.Add("@WhereSel", condition);
                    parameters.Add("@StartRow", whereParameter.StartRow);
                    parameters.Add("@EndRow", whereParameter.EndRow);

                    IEnumerable<Location> locations = SqlMapper.Query<Location>(conn, "msp_Location_Retrive", parameters, commandType: StoredProcedure);
                    result.Data = locations;
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

        public Result Insert(Location location, Models.Authorize.User user)
        {
            Result result = new Result();
            using (SqlConnection conn = new SqlConnection(_context.Connection.ConnectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        DynamicParameters parameters = new DynamicParameters();

                        if (location.LocationNo == 0)
                        {
                            parameters.Add("@LocationNo", location.LocationNo, DbType.Int32, ParameterDirection.Output);
                            parameters.Add("@LocationCode", location.LocationCode);
                            parameters.Add("@LocationName", location.LocationName);
                            parameters.Add("@CompanyNo", location.CompanyNo);
                            parameters.Add("@CreatedBy", user.CustomerNo);

                            conn.Query<int>("sp_Location_Insert", parameters, commandType: StoredProcedure, transaction: trans);
                            location.LocationNo = parameters.Get<int>("@LocationNo");
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
                        result.Data = location.LocationNo;
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

        public IEnumerable<Location> GetByCompany(int companyNo)
        {
            return _entities.Where(x => x.CompanyNo == companyNo);
        }

        //public PagedList<Location> GetLocations(WhereParameter parameters)
        //{
        //    return PagedList<Location>.ToPagedList(_entities.Where(wh => wh.LocationCode.Contains(parameters.Filter) || wh.LocationName.Contains(parameters.Filter)).OrderBy(on => on.LocationCode), parameters.Skip, parameters.Take);
        //}
    }
}
