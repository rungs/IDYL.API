using Dapper;
using IdylAPI.Models;
using IdylAPI.Models.Img;
using IdylAPI.Services.Interfaces.WR;
using IdylAPI.Services.Repository.DT;
using Microsoft.Extensions.Configuration;
using PAUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using static System.Data.CommandType;
using Microsoft.AspNetCore.Hosting;
using IdylAPI.Helper;
using DomainLayer.Entities;
using IdylAPI.Models.Master;

namespace IdylAPI.Services.Repository.WR
{
    public class WRRepository : IWRRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connStr;

        [Obsolete]
        private readonly IHostingEnvironment _host;

        [Obsolete]
        public WRRepository(IConfiguration configuration, IHostingEnvironment host)
        {
            _host = host;
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("IDYLConnection");
        }

        public Result CreateWR(Models.WO.WO wo)
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

                        parameters.Add("@SubsiteCode", wo.SubsiteCode);
                        parameters.Add("@CompanyName", wo.CompanyName);
                        parameters.Add("@SystemNo", wo.SystemNo);
                        parameters.Add("@ProblemTypeName", wo.ProblemTypeName);
                        parameters.Add("@Email", wo.Email);
                        parameters.Add("@WorkDesc", wo.WorkDesc);
                        parameters.Add("@SiteNo", wo.CompanyNo);
                        conn.Execute("sp_WR_CreateWRFromReportProblem", parameters, commandType: StoredProcedure, transaction: trans);
                        trans.Commit();
                        //result.Data = wo;
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

        public Result CreateWR(DomainLayer.Entities.WR wr)
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
                        WRResponse wRResponse = new WRResponse();
                        parameters.Add("@WONo", 0, DbType.Int32, ParameterDirection.Output);
                        parameters.Add("@WRDate", ValDB.GetDateTime(wr.wr_date));
                        parameters.Add("@WorkDesc", wr.work_desc ?? Convert.DBNull);
                        parameters.Add("@WRCode", wr.ref_code ?? Convert.DBNull);
                        parameters.Add("@ProblemTypeCode", wr.problem_type_code ?? Convert.DBNull);
                        parameters.Add("@EQCode", ValDB.GetString(wr.eq_code));
                        parameters.Add("@LocationCode", ValDB.GetString(wr.location_code));
                        parameters.Add("@CompanyNo", wr.clientId);
                        parameters.Add("@WOCode", "", DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);

                        conn.Query<int>("WO_InsertAPI", parameters, commandType: StoredProcedure, transaction: trans);

                        wRResponse.wr_no = parameters.Get<int>("@WONo");
                        wRResponse.wr_code = conn.QueryFirst<string>("select wocode from wo where wono = " + wRResponse.wr_no, commandType: Text, transaction: trans);

                        result.Data = wRResponse;
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

        [Obsolete]
        public Result Insert(Models.WO.WO wo, Models.Authorize.User user)
        {
            bool isNew = false;
            Result result = new Result();
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        DynamicParameters parameters = new DynamicParameters();

                        if (wo.WONo == 0)
                        {
                            parameters.Add("@WONo", wo.WONo, DbType.Int32, ParameterDirection.Output);
                            parameters.Add("@WOStatusNo", wo.WOStatusNo);
                            parameters.Add("@WODate", wo.WODate);
                            parameters.Add("@WRDate", wo.WRDate);
                            parameters.Add("@EQNo", wo.EQNo);
                            parameters.Add("@LocationNo", wo.LocationNo);
                            parameters.Add("@WorkDesc", wo.WorkDesc);
                            parameters.Add("@SectReq", user.SectionNo);
                            parameters.Add("@Requester", user.CustomerNo);
                            parameters.Add("@CompanyNo", wo.CompanyNo);
                            parameters.Add("@CreatedBy", user.CustomerNo);
                            parameters.Add("@SectionNo", wo.SectionNo);
                            parameters.Add("@WOTypeNo", wo.WOTypeNo);
                            parameters.Add("@ProblemTypeNo", wo.ProblemTypeNo);
                            parameters.Add("@ReqDate", wo.ReqDate);

                            conn.Query<int>("msp_WO_Insert", parameters, commandType: StoredProcedure, transaction: trans);
                            wo.WONo = parameters.Get<int>("@WONo");
                            isNew = true;

                            parameters = new DynamicParameters();
                            parameters.Add("@WorkDesc", wo.WorkDesc);
                            parameters.Add("@WONo", wo.WONo);
                            parameters.Add("@CreatedBy", user.CustomerNo);
                            parameters.Add("@CompanyNo", wo.CompanyNo);
                            parameters.Add("@EQNo", wo.EQNo);
                            conn.Query<int>("sp_WOD_InsertTaskDefault", parameters, commandType: StoredProcedure, transaction: trans);
                        }
                        else
                        {
                            parameters = new DynamicParameters();
                            parameters.Add("@WONo", wo.WONo);
                            parameters.Add("@WODate", wo.WODate);
                            parameters.Add("@WRDate", wo.WRDate);
                            parameters.Add("@EQNo", wo.EQNo);
                            parameters.Add("@LocationNo", wo.LocationNo);
                            parameters.Add("@WorkDesc", wo.WorkDesc);
                            parameters.Add("@UpdatedBy", user.CustomerNo);
                            parameters.Add("@SectionNo", wo.SectionNo);
                            parameters.Add("@ProblemTypeNo", wo.ProblemTypeNo);
                            parameters.Add("@ReqDate", wo.ReqDate);

                            conn.Query<int>("msp_WO_Update", parameters, commandType: StoredProcedure, transaction: trans);

                        }

                        if (wo.IsDowntime)
                        {
                            Models.WO.DT dtObj = new Models.WO.DT()
                            {
                                DTDate = wo.WRDate,
                                EQNo = wo.EQNo,
                                LocationNo = wo.LocationNo,
                                WONo = wo.WONo,
                                DownTime = 0,
                                CompanyNo = wo.CompanyNo
                            };
                            new DTRepository().Update(dtObj, user, conn, trans);
                        }

                        parameters = new DynamicParameters();
                        parameters.Add("@WONo", wo.WONo);
                        Models.WO.WO woOld = conn.QueryFirst<Models.WO.WO>("sp_WO_GetByNo", parameters, commandType: StoredProcedure, transaction: trans);

                        if (wo.Attachment != null && wo.Attachment.Count() > 0)
                        {
                            foreach (AttachFileObject item in wo.Attachment)
                            {
                                string ext = Path.GetExtension(item.DocName); ;
                                int id = wo.WONo;
                                string path = $"{UploadFiles.GenerateFolderUploadPath("WO", wo.CompanyName)}/{woOld.WOCode}/{item.FileName}";

                                string pPath = _host.ContentRootPath;
                                int pathLevel = InputVal.ToInt(_configuration["PathLevel"]);
                                for (int j = 0; j <= pathLevel; j++)
                                {
                                    pPath = Directory.GetParent(pPath).ToString();
                                }

                                pPath += _configuration["AttPath"] + "\\[" + wo.CompanyName.Replace(" ", "").Trim() + "]\\WO\\" + woOld.WOCode + "";
                                bool exists = Directory.Exists(pPath);
                                if (!exists)
                                {
                                    Directory.CreateDirectory(pPath);
                                }
                                pPath += "\\" + item.FileName;
                                File.Copy(item.Path, pPath);
                                File.Delete(item.Path);
                                parameters = new DynamicParameters();
                                parameters.Add("@LinkNo", id);
                                parameters.Add("@FileName", item.DocName);
                                parameters.Add("@Path", path);
                                parameters.Add("@FileType", "WO");
                                parameters.Add("@CompanyNo", wo.CompanyNo);
                                parameters.Add("@CreatedBy", user.CustomerNo);
                                parameters.Add("@IsUrl", false);
                                parameters.Add("@Extension", ext);
                                parameters.Add("@AttachFileNo", 0, DbType.Int32, ParameterDirection.Output);

                                SqlMapper.Execute(conn, "sp_AttachFile_Insert", parameters, commandType: StoredProcedure, transaction: trans);

                            }
                        }

                        trans.Commit();
                        if (isNew)
                        {
                            new Notify.NotifyRepository(_configuration).Send(wo.WONo, "REQUEST", wo.CompanyNo, user.CustomerNo);

                            string cmd = $" select * from section where sectionno={wo.SectionNo}";
                            Section section = conn.QueryFirst<Section>(cmd, parameters, commandType: Text);

                            if (section.IsSendLine.HasValue && section.IsSendLine.Value)
                            {
                                string msg = string.Format("IDYL: {2}| รหัส/ชื่ออุปกรณ์:{7}| อาการ/ปัญหา: {0}| วันที่แจ้ง: {1}" +
                                    "| วันที่เกิดปัญหา: {3} | หน่วยงานแจ้ง: {4} | ผู้แจ้ง: {5}| {6}"
                                    , wo.WorkDesc
                                    , woOld.WRDate != null ? Convert.ToDateTime(woOld.WRDate).ToString("dd/MM/yyyy HH:mm") : ""
                                    , woOld.WOCode
                                    , woOld.WODate != null ? Convert.ToDateTime(woOld.WODate).ToString("dd/MM/yyyy HH:mm") : ""
                                    , section.SectionName
                                , woOld.ReqName
                                , $"{_configuration["IdylWeb"]}/Form/WO/WOEdit.aspx?WONo={wo.WONo}"
                                , $"{woOld.EQCode};{woOld.EQName}");

                                new SendNotify(_configuration).LineNotify(msg, section.LineToken);
                            }
                            if (section.IsSendEmail.HasValue && section.IsSendEmail.Value)
                            {
                                Mail.SendEmail(_configuration, new MailReq()
                                {
                                    Content = string.Format("{3}<br/><b>อาการ/ปัญหา: </b>{0}<br/><br/><b>วันที่แจ้ง: </b>{1}<br/>" +
                                        "<b>วันที่เกิดปัญหา: </b>{2}<br/><b>หน่วยงานแจ้ง: </b>{4}<br/><b>ผู้แจ้ง: </b>{5}<br/>"
                                        , wo.WorkDesc
                                        , woOld.WRDate != null ? Convert.ToDateTime(woOld.WRDate).ToString("dd/MM/yyyy HH:mm") : ""
                                        , woOld.WODate != null ? Convert.ToDateTime(woOld.WODate).ToString("dd/MM/yyyy HH:mm") : ""
                                        , woOld.WOCode
                                        , section.SectionName
                                        , woOld.ReqName),
                                    DocCode = woOld.WOCode,
                                    DocLink = $"{_configuration["IdylWeb"]}/Form/WO/WOEdit.aspx?WONo={wo.WONo}",
                                    FromPage = "WR",
                                    Receive = section.Email
                                });
                            }
                        }

                        result.Data = wo;
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

        public Result Retrive(WhereParameter whereParameter, Models.Authorize.User user)
        {
            Result result = new Result();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    conn.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    string condition = " where wo.PMNo is null";
                    condition += $" and wo.companyno = {whereParameter.SiteNo}";
                    if (whereParameter.Type == "HISTORY")
                    {
                        condition += "and wo.wostatusno in (1,4)";
                    }
                    else if (whereParameter.Type == "BACKLOG")
                    {
                        condition += "and wo.wostatusno not in (1,4)";
                    }

                    if ("D" == whereParameter.DataType)
                    {
                        condition += " and wo.SectReq =" + user.SectionNo;
                    }
                    else if ("O" == whereParameter.DataType)
                    {
                        condition += string.Format(" and wo.Requester = {0}", user.CustomerNo);
                    }
                    condition += "and _SystUser_Location.userno = " + user.UserNo;

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
                    parameters.Add("@WhereSel", condition);
                    parameters.Add("@StartRow", whereParameter.StartRow);
                    parameters.Add("@EndRow", whereParameter.EndRow);

                    IEnumerable<IdylAPI.Models.WO.WO> wOs = conn.Query<IdylAPI.Models.WO.WO>("msp_WOMain_Retrive", parameters, commandType: StoredProcedure);
                    result.Data = wOs;
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

        public Result RetriveById(int id)
        {
            Result result = new Result();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    conn.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@WONo", id);
                    IdylAPI.Models.WO.WO wOs = conn.QueryFirst<IdylAPI.Models.WO.WO>("sp_WO_GetByNo", parameters, commandType: StoredProcedure);

                    parameters = new DynamicParameters();
                    parameters.Add("@WONo", id);
                    Models.WO.DT dT = conn.QueryFirstOrDefault<Models.WO.DT>("sp_DownTime_GetByWONo", parameters, commandType: StoredProcedure);
                    wOs.DTs = dT;
                    if (dT != null)
                    {
                        wOs.IsDowntime = true;
                    }

                    parameters = new DynamicParameters();
                    parameters.Add("@LinkNo", id);
                    parameters.Add("@FileType", "WO");
                    IEnumerable<AttachFileObject> attachFileResult = conn.Query<AttachFileObject>("sp_AttachFile_GetByLinkNoFileType", parameters, commandType: StoredProcedure);
                    wOs.AttachmentObj = new List<string>();
                    foreach (AttachFileObject item in attachFileResult)
                    {
                        wOs.AttachmentObj.Add(string.Format("{0}/{1}", _configuration["IdylWeb"], item.Path));
                    }

                    result.Data = wOs;
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

        public Result RetriveForReportProblem(int siteNo, int systemNo)
        {
            Result result = new Result();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    conn.Open();
                    DynamicParameters parameters = new DynamicParameters();

                    parameters.Add("@SystemNo", systemNo);
                    parameters.Add("@SiteNo", siteNo);

                    IEnumerable<IdylAPI.Models.WO.WO> wOs = conn.Query<IdylAPI.Models.WO.WO>("sp_ReportProblem_Retrive", parameters, commandType: StoredProcedure);
                    result.Data = wOs;
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
