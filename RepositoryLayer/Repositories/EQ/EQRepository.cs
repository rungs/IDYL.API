using Dapper;
using IdylAPI.Helper;
using IdylAPI.Models;
using IdylAPI.Models.Authorize;
using IdylAPI.Models.Img;
using IdylAPI.Models.Master;
using IdylAPI.Services.Interfaces.Master;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PAUtility;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using static System.Data.CommandType;
namespace IdylAPI.Services.Repository.Master
{
    public class EQRepository : IEQRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connStr;
        private readonly AppDBContext _context;
        public EQRepository(IConfiguration configuration, AppDBContext context)
        {
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("IDYLConnection");
            _context = context;
        }

        public Result RetriveEf(WhereParameter whereParameter, Models.Authorize.User user)
        {
            Result result = new Result();
            try
            {
                var locationList = _context.SystUserLocation.Where(w => w.UserNo == user.UserNo).Select(e=>e.LocationNo).ToList();
                var eqlocation = _context.EQ.Where(w => w.CompanyNo == whereParameter.SiteNo && w.IsDelete == false && locationList.Contains(w.Location));
                var eq_result = eqlocation
                    .Include(e => e.LocationObj)
                    .Include(e => e.EQTypeObj).Take(whereParameter.EndRow).Skip(whereParameter.StartRow -1);
                
                
                result.Data = eq_result;
                result.StatusCode = 200;



                //var query = from eq in _context.EQ
                //            join location in _context.Location on eq.Location equals location.LocationNo
                //            join eq_type in _context.EQType on eq.EQType equals eq_type.EQTypeNo


                //            where eq.CompanyNo == companyNo && !eq.IsDelete
                //            select eqspec;
                //using (SqlConnection conn = new SqlConnection(_connStr))
                //{
                //    conn.Open();

                //    DynamicParameters parameters = new DynamicParameters();
                //    string condition = $" where EQ.companyno = {whereParameter.SiteNo}";
                //    condition += $" and EQ.isdelete = 0 ";
                //    condition += $" and _SystUser_Location.userno =  " + user.UserNo;

                //    if (!string.IsNullOrEmpty(InputVal.ToString(whereParameter.Filter)))
                //    {
                //        condition += $" and(EQ.EQname like '%{whereParameter.Filter}%'";
                //        condition += $" or EQ.EQCode like '%{whereParameter.Filter}%'";
                //        condition += $" or Location.LocationCode like '%{whereParameter.Filter}%'";
                //        condition += $" or Location.LocationName like '%{whereParameter.Filter}%'";
                //        condition += $" or EQ.SerialNo like '%{whereParameter.Filter}%'";
                //        condition += $" or EQ.Model like '%{whereParameter.Filter}%'";
                //        condition += $" or Vendor.VendorName like '%{whereParameter.Filter}%'";
                //        condition += $" or Vendor.VendorCode like '%{whereParameter.Filter}%')";
                //    }
                //    if (whereParameter.PUNo != 0)
                //    {
                //        condition += $" and EQ.location = {whereParameter.PUNo} ";
                //    }

                //    parameters.Add("@WhereSel", condition);
                //    parameters.Add("@StartRow", whereParameter.StartRow);
                //    parameters.Add("@EndRow", whereParameter.EndRow);

                //    IEnumerable<EQ> eQs = SqlMapper.Query<EQ>(conn, "msp_EQ_Retrive", parameters, commandType: StoredProcedure);

                //}
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.ErrMsg = ex.Message;
            }
            return result;
        }
        public Result RetriveById(int eqNo)
        {
            Result result = new Result();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    conn.Open();

                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@EQNo", eqNo);
                    EQ eQs = SqlMapper.QueryFirst<EQ>(conn, "sp_EQ_GetByNo", parameters, commandType: StoredProcedure);

                    //string where = "";
                    //where += $" where eq.eqno = {eqNo} ";
                    //parameters = new DynamicParameters();
                    //parameters.Add("@WhereSel", where);
                    //eQs.partUsages = SqlMapper.Query<PartUsage>(conn, "msp_Grid_EQResource_Retrive", parameters, commandType: StoredProcedure);

                    //parameters = new DynamicParameters();
                    //parameters.Add("@WhereSel", where);
                    //eQs.mAs = SqlMapper.Query<MA>(conn, "msp_Grid_Maintainance_Retrive", parameters, commandType: StoredProcedure);

                    parameters = new DynamicParameters();
                    parameters.Add("@LinkNo", eqNo);
                    parameters.Add("@FileType", "EQ");
                    IEnumerable<AttachFileObject> attachFileResult = conn.Query<AttachFileObject>("sp_AttachFile_GetByLinkNoFileType", parameters, commandType: StoredProcedure);
                    eQs.AttachmentObj = new List<string>();
                    foreach (AttachFileObject item in attachFileResult)
                    {
                        eQs.AttachmentObj.Add(string.Format("{0}/{1}", _configuration["IdylWeb"], item.Path));
                    }

                    string cmd = $" select AttachFile.*, WOCode as LinkCode from AttachFile inner join WO on AttachFile.LinkNo = WO.WONo where WO.EQNo = {eqNo} and AttachFile.IsInspectionFile = 1 ";
                    IEnumerable<AttachFileObject> inspecFiles = conn.Query<AttachFileObject>(cmd, commandType: Text);
                    foreach (AttachFileObject item in inspecFiles)
                    {
                        item.WebAddress = _configuration["IdylWeb"];
                    }
                    eQs.InspectFiles = inspecFiles;

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
        public Result RetriveByCode(WhereParameter whereParameter, Models.Authorize.User user)
        {
            Result result = new Result();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    conn.Open();

                    DynamicParameters parameters = new DynamicParameters();
                    string condition = $" where EQ.eqcode = '{whereParameter.Filter}'";
                    condition += $" and EQ.isdelete = 0 ";
                    condition += $" and _SystUser_Location.Userno = {user.UserNo} ";
                    condition += $" and EQ.companyNo = {whereParameter.SiteNo} ";

                    parameters.Add("@WhereSel", condition);
                    EQ eQs = SqlMapper.QueryFirstOrDefault<EQ>(conn, "msp_EQ_Retrive", parameters, commandType: StoredProcedure);

                    if (eQs != null)
                    {
                        parameters = new DynamicParameters();
                        parameters.Add("@CompanyNo", eQs.CompanyNo);
                        Site site = SqlMapper.QueryFirstOrDefault<Site>(conn, "sp_Company_GetByID", parameters, commandType: StoredProcedure);

                        if (!string.IsNullOrEmpty(eQs.Img))
                        {
                            eQs.Img = $"{ _configuration["IdylWeb"]}/Files/[{ site.CompanyName.Replace(" ", "").Trim()}]/EQ/{eQs.Img}";
                        }


                        eQs.InProgressCnt = 0;
                        eQs.HistoryCnt = 0;
                        condition = "";
                        condition += $" where wo.companyno = {eQs.CompanyNo}";
                        condition += $" and eq.eqno = {eQs.EQNo}";
                        condition += $" and wo.WOStatusNo not in (1,4)";
                        parameters = new DynamicParameters();
                        parameters.Add("@WhereSel", condition);
                        IEnumerable<Models.WO.WO> wOInprogress = conn.Query<Models.WO.WO>("msp_WOMain_Retrive_All", parameters, commandType: StoredProcedure);

                        eQs.InProgressCnt = wOInprogress.AsList().Count;

                        condition = "";
                        condition += $" where wo.companyno = {eQs.CompanyNo}";
                        condition += $" and eq.eqno = {eQs.EQNo}";
                        condition += $" and wo.WOStatusNo = 1";
                        parameters = new DynamicParameters();
                        parameters.Add("@WhereSel", condition);
                        IEnumerable<Models.WO.WO> wOHistory = conn.Query<Models.WO.WO>("msp_WOMain_Retrive_All", parameters, commandType: StoredProcedure);

                        eQs.HistoryCnt = wOHistory.AsList().Count;
                    }

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
        public Result RetriveInProgrss(WhereParameter whereParameter)
        {
            Result result = new Result();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    conn.Open();
                    string condition = "";
                    condition += $" where eq.eqno = {whereParameter.EQNo}";
                    condition += $" and wo.WOStatusNo = 2";
                    if (!string.IsNullOrEmpty(InputVal.ToString(whereParameter.Filter)))
                    {
                        condition += $" and(wo.wocode like '%{whereParameter.Filter}%'";
                        condition += $" or wo.workdesc like '%{whereParameter.Filter}%'";
                        condition += $" or location.locationname like '%{whereParameter.Filter}%'";
                        condition += $" or location.locationcode like '%{whereParameter.Filter}%'";
                        condition += $" or eq.eqcode like '%{whereParameter.Filter}%'";
                        condition += $" or eq.eqname like '%{whereParameter.Filter}%'";
                        condition += $" or WOStatus.WOStatusName like '%{whereParameter.Filter}%')";
                    }
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@WhereSel", condition);
                    parameters.Add("@StartRow", whereParameter.StartRow);
                    parameters.Add("@EndRow", whereParameter.EndRow);
                    IEnumerable<Models.WO.WO> wOInprogress = conn.Query<Models.WO.WO>("msp_WOMain_Retrive", parameters, commandType: StoredProcedure);

                    result.Data = wOInprogress;
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
        public Result RetriveHistory(WhereParameter whereParameter)
        {
            Result result = new Result();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    conn.Open();
                    string condition = "";
                    condition += $" where eq.eqno = {whereParameter.EQNo}";
                    condition += $" and wo.WOStatusNo = 1";
                    if (!string.IsNullOrEmpty(InputVal.ToString(whereParameter.Filter)))
                    {
                        condition += $" and(wo.wocode like '%{whereParameter.Filter}%'";
                        condition += $" or wo.workdesc like '%{whereParameter.Filter}%'";
                        condition += $" or location.locationname like '%{whereParameter.Filter}%'";
                        condition += $" or location.locationcode like '%{whereParameter.Filter}%'";
                        condition += $" or eq.eqcode like '%{whereParameter.Filter}%'";
                        condition += $" or eq.eqname like '%{whereParameter.Filter}%'";
                        condition += $" or WOStatus.WOStatusName like '%{whereParameter.Filter}%')";
                    }
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@WhereSel", condition);
                    parameters.Add("@StartRow", whereParameter.StartRow);
                    parameters.Add("@EndRow", whereParameter.EndRow);
                    IEnumerable<Models.WO.WO> wOInprogress = conn.Query<Models.WO.WO>("msp_WOMain_Retrive", parameters, commandType: StoredProcedure);

                    result.Data = wOInprogress;
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
        public Result RetriveEqResource(WhereParameter whereParameter)
        {
            Result result = new Result();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    conn.Open();

                    DynamicParameters parameters = new DynamicParameters();
                    string condition = $" where EQ.eqno = {whereParameter.EQNo}";
                    condition += $" and  WORESOURCE.ResTypeCode <> 'L'";
                    parameters.Add("@WhereSel", condition);
                    parameters.Add("@StartRow", whereParameter.StartRow);
                    parameters.Add("@EndRow", whereParameter.EndRow);

                    IEnumerable<PartUsage> eQs = SqlMapper.Query<PartUsage>(conn, "msp_Grid_EQResource_Retrive", parameters, commandType: StoredProcedure);
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
        public Result RetriveMaintainance(WhereParameter whereParameter)
        {
            Result result = new Result();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    conn.Open();

                    DynamicParameters parameters = new DynamicParameters();
                    string condition = $" where EQ.eqno = {whereParameter.EQNo}";
                    if (!string.IsNullOrEmpty(InputVal.ToString(whereParameter.Filter)))
                    {
                        condition += $" and(wo.wocode like '%{whereParameter.Filter}%'";
                        condition += $" or wo.workdesc like '%{whereParameter.Filter}%'";
                        condition += $" or WOStatus.WOStatusName like '%{whereParameter.Filter}%'";
                        condition += $" or WOType.WOTypeCode like '%{whereParameter.Filter}%'";
                        condition += $" or wo.woaction like '%{whereParameter.Filter}%'";
                        condition += $" or wo.wocause  like '%{whereParameter.Filter}%')";
                    }
                    parameters.Add("@WhereSel", condition);
                    parameters.Add("@StartRow", whereParameter.StartRow);
                    parameters.Add("@EndRow", whereParameter.EndRow);

                    IEnumerable<MA> eQs = SqlMapper.Query<MA>(conn, "msp_Grid_Maintainance_Retrive", parameters, commandType: StoredProcedure);
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
        public Result Insert(EQ eQ, Models.Authorize.User user)
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

                        if (eQ.EQNo == 0)
                        {
                            int? limitRow =_context.Site.Where(t => t.CompanyNo == eQ.CompanyNo).Select(s => s.LimitRow).First();
                            if (_context.EQ.Where(t => t.CompanyNo == eQ.CompanyNo).Count() >= limitRow && limitRow > 0)
                            {
                                throw new Exception($"ไม่สามารถสร้างรายการใหม่ได้ เนื่องจากคุณสามารถสร้างได้ไม่เกิน {limitRow} รายการ");
                            }
                            //if((int)UserGroupEnum.UserGroup.Free == user.UserGroupId)
                            //{
                            //    int maxEq = InputVal.ToInt(_configuration["MaxEq"]);
                            //}
                            parameters.Add("@EQNo", eQ.EQNo, DbType.Int32, ParameterDirection.Output);
                            parameters.Add("@EQCode", eQ.EQCode);
                            parameters.Add("@EQName", eQ.EQName);
                            parameters.Add("@LocationNo", eQ.Location);
                            parameters.Add("@LocationName", eQ.LocationName);
                            parameters.Add("@EQTypeNo", eQ.EQType);
                            parameters.Add("@EQTypeName", eQ.EQTypeName);
                            parameters.Add("@Manufacturer", eQ.Manufacturer);
                            parameters.Add("@Model", eQ.Model);
                            parameters.Add("@SerialNo", eQ.SerialNo);
                            parameters.Add("@VendorNo", eQ.VendorNo);
                            parameters.Add("@Phone", null);
                            parameters.Add("@Email", null);
                            parameters.Add("@Attach", null);
                            parameters.Add("@Remark", null);
                            parameters.Add("@CompanyNo", eQ.CompanyNo);
                            parameters.Add("@CreatedBy", user.CustomerNo);
                            parameters.Add("@Criticality", eQ.Criticality);
                            parameters.Add("@InstalledDate", eQ.InstalledDate);
                            parameters.Add("@WarrantyDate", eQ.WarrantyDate);
                            parameters.Add("@Capacity", eQ.Capacity);

                            conn.Query<int>("sp_EQ_Insert", parameters, commandType: StoredProcedure, transaction: trans);
                            eQ.EQNo = parameters.Get<int>("@EQNo");


                        }
                        else
                        {
                            parameters = new DynamicParameters();
                            parameters.Add("@EQNo", eQ.EQNo);
                            EQ eQOld = conn.QueryFirst<EQ>("sp_EQ_GetByNo", parameters, commandType: StoredProcedure, transaction: trans);

                            parameters = new DynamicParameters();
                            parameters.Add("@EQNo", eQ.EQNo);
                            parameters.Add("@EQCode", eQ.EQCode);
                            parameters.Add("@EQName", eQ.EQName);
                            parameters.Add("@LocationNo", eQ.Location);
                            parameters.Add("@LocationName", eQ.LocationName);
                            parameters.Add("@EQTypeNo", eQ.EQType);
                            parameters.Add("@EQTypeName", eQ.EQTypeName);
                            parameters.Add("@Manufacturer", eQ.Manufacturer);
                            parameters.Add("@Model", eQ.Model);
                            parameters.Add("@SerialNo", eQ.SerialNo);
                            parameters.Add("@VendorNo", eQ.VendorNo);
                            parameters.Add("@Phone", eQOld.Phone);
                            parameters.Add("@Email", eQOld.Email);
                            parameters.Add("@Attach", null);
                            parameters.Add("@Remark", eQOld.Remark);
                            parameters.Add("@CompanyNo", eQOld.CompanyNo);
                            parameters.Add("@UpdatedBy", user.CustomerNo);
                            parameters.Add("@Criticality", eQ.Criticality);
                            parameters.Add("@InstalledDate", eQ.InstalledDate);
                            parameters.Add("@WarrantyDate", eQ.WarrantyDate);
                            parameters.Add("@Capacity", eQ.Capacity);

                            conn.Execute("sp_EQ_Update", parameters, commandType: StoredProcedure, transaction: trans);

                        }

                        int cnt = 0;
                        if (eQ.Attachment != null && eQ.Attachment.Length > 0)
                        {

                            foreach (AttachFileObject item in eQ.Attachment)
                            {

                                string ext = Path.GetExtension(item.DocName); ;
                                int id = eQ.EQNo;
                                if (cnt == 0)
                                {
                                    string cmd = $" update eq set IMG = '{item.UidName}{ext}' where eqno = {eQ.EQNo}";
                                    SqlMapper.Execute(conn, cmd, null, commandType: Text, transaction: trans);
                                    cnt++;
                                }

                                string path = $"{UploadFiles.GenerateFolderUploadPath("EQ", eQ.CompanyName)}/{item.FileName}";
                                parameters = new DynamicParameters();
                                parameters.Add("@LinkNo", id);
                                parameters.Add("@FileName", item.DocName);
                                parameters.Add("@Path", path);
                                parameters.Add("@FileType", "EQ");
                                parameters.Add("@CompanyNo", eQ.CompanyNo);
                                parameters.Add("@CreatedBy", user.CustomerNo);
                                parameters.Add("@IsUrl", false);
                                parameters.Add("@Extension", ext);
                                parameters.Add("@AttachFileNo", 0, DbType.Int32, ParameterDirection.Output);

                                SqlMapper.Execute(conn, "sp_AttachFile_Insert", parameters, commandType: StoredProcedure, transaction: trans);
                            }
                        }

                        trans.Commit();
                        result.Data = eQ.EQNo;
                        result.ErrMsg = "บันทึกเรียบร้อย";
                        result.StatusCode = 200;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        result.StatusCode = 500;
                        result.ErrMsg = ex.Message;
                    }
                }

                return result;
            }
        }
        //query for select
        public IEnumerable<EQ> Retrive2(WhereParameter whereParameter, User user)
        {
            var locationList = _context.SystUserLocation.Where(w => w.UserNo == user.UserNo).Select(e => e.LocationNo).ToList();
            var eqlocation = _context.EQ.Where(w => w.CompanyNo == whereParameter.SiteNo && w.IsDelete == false && locationList.Contains(w.Location));
            var eq_result = eqlocation
                .Include(e => e.LocationObj)
                .Include(e => e.EQTypeObj).Where(w => w.EQName.Contains(whereParameter.Filter) || w.EQCode.Contains(whereParameter.Filter));
          
            return eq_result;

            //condition += $" and(EQ.EQname like '%{whereParameter.Filter}%'";
            //        condition += $" or EQ.EQCode like '%{whereParameter.Filter}%'";
            //        condition += $" or Location.LocationCode like '%{whereParameter.Filter}%'";
            //        condition += $" or Location.LocationName like '%{whereParameter.Filter}%'";
            //        condition += $" or EQ.SerialNo like '%{whereParameter.Filter}%'";
            //        condition += $" or EQ.Model like '%{whereParameter.Filter}%'";
            //        condition += $" or Vendor.VendorName like '%{whereParameter.Filter}%'";
            //        condition += $" or Vendor.VendorCode like '%{whereParameter.Filter}%')";
            //    }
        }
        //query for eqlist into
        //query id
        public Result Retrive(WhereParameter whereParameter, User user)
        {
            Result result = new Result();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    conn.Open();

                    DynamicParameters parameters = new DynamicParameters();
                    string condition = $" where EQ.companyno = {whereParameter.SiteNo}";
                    condition += $" and EQ.isdelete = 0 ";
                    condition += $" and _SystUser_Location.userno =  " + user.UserNo;

                    if (!string.IsNullOrEmpty(InputVal.ToString(whereParameter.Filter)))
                    {
                        condition += $" and(EQ.EQname like '%{whereParameter.Filter}%'";
                        condition += $" or EQ.EQCode like '%{whereParameter.Filter}%'";
                        condition += $" or Location.LocationCode like '%{whereParameter.Filter}%'";
                        condition += $" or Location.LocationName like '%{whereParameter.Filter}%'";
                        condition += $" or EQ.SerialNo like '%{whereParameter.Filter}%'";
                        condition += $" or EQ.Model like '%{whereParameter.Filter}%'";
                        condition += $" or Vendor.VendorName like '%{whereParameter.Filter}%'";
                        condition += $" or Vendor.VendorCode like '%{whereParameter.Filter}%')";
                    }
                    if (whereParameter.PUNo != 0)
                    {
                        condition += $" and EQ.location = {whereParameter.PUNo} ";
                    }

                    parameters.Add("@WhereSel", condition);
                    parameters.Add("@StartRow", whereParameter.StartRow);
                    parameters.Add("@EndRow", whereParameter.EndRow);

                    IEnumerable<EQ> eQs = SqlMapper.Query<EQ>(conn, "msp_EQ_Retrive", parameters, commandType: StoredProcedure);
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
    }
}
