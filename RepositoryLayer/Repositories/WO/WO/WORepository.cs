using Dapper;
using IdylAPI.Helper;
using IdylAPI.Models;
using IdylAPI.Models.Authorize;
using IdylAPI.Models.Img;
using IdylAPI.Models.Master;
using IdylAPI.Models.WO;
using IdylAPI.Services.Interfaces.WO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using PAUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using static System.Data.CommandType;
namespace IdylAPI.Services.Repository.WO
{
    public class WORepository : IWORepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connStr;
        [Obsolete]
        private readonly IHostingEnvironment _host;

        [Obsolete]
        public WORepository(IConfiguration configuration, IHostingEnvironment host)
        {
            _host = host;
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("IDYLConnection");
        }

        public Result RetriveWO(WhereParameter whereParameter, Models.Authorize.User user, LoadOptions loadOption)
        {
            Result result = new Result();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    conn.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    string condition = "";
                    condition += $" where wo.companyno = {whereParameter.SiteNo}";
                    if (whereParameter.Type == "CM")
                    {
                        condition += " and isnull(wo.wotypeno,0) <> 1";

                    }
                    else if (whereParameter.Type == "PM")
                    {
                        condition += " and isnull(wo.wotypeno,0) = 1";
                    }

                    if (whereParameter.IsHistory)
                    {
                        condition += " and wo.wostatusno in (1,4)";
                    }
                    else
                    {
                        if (whereParameter.IsOffline)
                        {
                            condition += " and wo.wostatusno = 9";
                        }
                        else
                        {
                            condition += " and wo.wostatusno not in (1,4)";
                        }
                    }

                    if (!string.IsNullOrEmpty(InputVal.ToString(whereParameter.WOTypeCode)))
                    {
                        if (whereParameter.WOTypeCode != "null" && whereParameter.WOTypeCode != "0")
                        {
                            condition += $" and wotype.wotypecode = '{whereParameter.WOTypeCode}'";
                        }
                        else
                        {
                            condition += $" and isnull(wo.wotypeno,0) = 0 ";
                        }



                        if (whereParameter.IsOverdue)
                        {
                            condition += $" and CAST(wo.PlnDate AS DATE) < CAST(GETDATE() AS DATE)";
                        }
                        condition += $" and DocumentHistory.AssignTo = '{user.CustomerNo}'";

                    }

                    if ("D" == whereParameter.DataType)
                    {
                        condition += " and wo.SectionNo =" + user.SectionNo;
                    }
                    else if ("O" == whereParameter.DataType)
                    {
                        condition += string.Format(" and (wo.CraftNo ={0} or wo.assignto={0} OR WO.WONo IN (SELECT WOD.WONo FROM WOD WHERE WOD.HeadCraftTypeNo = {0}) OR WO.WONo IN (SELECT WOResource.WONo FROM WOResource WHERE RescNo = {0} and ResTypeCode = 'L'))", user.CustomerNo);
                    }
                    condition += " and _SystUser_Location.userno = " + user.UserNo;

                    if (user.UserGroupId.HasValue && user.UserGroupId.Value == 4)
                    {
                        condition += " and wo.wostatusno <> 5 ";
                    }

                    if (whereParameter.PMNo.HasValue && whereParameter.PMNo.Value != 0)
                    {
                        condition += " and wo.pmno =  " + whereParameter.PMNo.Value;
                    }

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
                    condition += GenFilter(loadOption);

                    parameters.Add("@WhereSel", condition);
                    parameters.Add("@StartRow", whereParameter.StartRow);
                    parameters.Add("@EndRow", whereParameter.EndRow);
                    IEnumerable<Models.WO.WO> wOs = conn.Query<Models.WO.WO>("msp_WOMain_Retrive", parameters, commandType: StoredProcedure);
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

        public Result RetriveById(int id, User user)
        {
            Result result = new Result();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    conn.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@WONo", id);
                    Models.WO.WO wOs = conn.QueryFirst<Models.WO.WO>("sp_WO_GetByNo", parameters, commandType: StoredProcedure);

                    if (wOs.WOStatusNo == 5)
                    {
                        parameters = new DynamicParameters();
                        parameters.Add("@WONo", id);
                        parameters.Add("@WOStatusNo", 2);
                        parameters.Add("@UpdatedBy", user.CustomerNo);
                        conn.Execute("sp_WO_UpdateWOStatus", parameters, commandType: StoredProcedure);

                        parameters = new DynamicParameters();
                        parameters.Add("@WONo", id);
                        wOs = conn.QueryFirst<Models.WO.WO>("sp_WO_GetByNo", parameters, commandType: StoredProcedure);
                        new Notify.NotifyRepository(_configuration).Send(wOs.WONo, "INPROGRESS", wOs.CompanyNo, user.CustomerNo);
                    }

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
                    wOs.AttachmentAfterObj = new List<string>();
                    foreach (AttachFileObject item in attachFileResult)
                    {
                        if (item.FileName.IndexOf("AFTER") > -1)
                        {
                            wOs.AttachmentAfterObj.Add(string.Format("{0}/{1}", _configuration["IdylWeb"], item.Path));
                        }
                        if (item.FileName.IndexOf("BEFORE") > -1)
                        {
                            wOs.AttachmentObj.Add(string.Format("{0}/{1}", _configuration["IdylWeb"], item.Path));
                        }

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

        public Result RetrivePlan(int woNo)
        {
            Result result = new Result();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    conn.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@WONo", woNo);
                    Models.WO.WO wOs = conn.QueryFirst<Models.WO.WO>("sp_WO_GetByNo", parameters, commandType: StoredProcedure);

                    string p_strWhere = "";
                    parameters = new DynamicParameters();
                    p_strWhere += $" WHERE WOResource.WONo = {woNo}";
                    p_strWhere += $" AND WOResource.Type = 'P'";
                    p_strWhere += $" AND WOResource.ResTypeCode = 'L'";
                    parameters.Add("@Where", p_strWhere);
                    IEnumerable<Models.WO.WOResource> wOResLabor = conn.Query<Models.WO.WOResource>("msp_WOResource_GetByType", parameters, commandType: StoredProcedure);
                    wOs.WOResPlanLabor = wOResLabor;

                    p_strWhere = "";
                    parameters = new DynamicParameters();
                    p_strWhere += $" WHERE WOResource.WONo = {woNo}";
                    p_strWhere += $" AND WOResource.Type = 'P'";
                    p_strWhere += $" AND WOResource.ResTypeCode <> 'L'";
                    parameters.Add("@Where", p_strWhere);
                    IEnumerable<Models.WO.WOResource> wOResMat = conn.Query<Models.WO.WOResource>("msp_WOResource_GetByType", parameters, commandType: StoredProcedure);
                    wOs.WOResPlanMat = wOResMat;

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

        public Result RetriveActual(int woNo)
        {
            Result result = new Result();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    conn.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@WONo", woNo);
                    Models.WO.WO wOs = conn.QueryFirst<Models.WO.WO>("sp_WO_GetByNo", parameters, commandType: StoredProcedure);

                    parameters = new DynamicParameters();
                    parameters.Add("@WONo", woNo);
                    Models.WO.DT dT = conn.QueryFirstOrDefault<Models.WO.DT>("sp_DownTime_GetByWONo", parameters, commandType: StoredProcedure);
                    wOs.DTs = dT;

                    if (dT != null)
                    {
                        wOs.IsDowntime = true;
                    }

                    parameters = new DynamicParameters();
                    parameters.Add("@WONo", woNo);
                    Models.WO.FM fM = conn.QueryFirstOrDefault<Models.WO.FM>("sp_FM_GetByWONo", parameters, commandType: StoredProcedure);
                    wOs.fM = fM;

                    string p_strWhere = "";
                    parameters = new DynamicParameters();
                    p_strWhere += $" WHERE WOResource.WONo = {woNo}";
                    p_strWhere += $" AND WOResource.Type = 'P'";
                    p_strWhere += $" AND WOResource.ResTypeCode = 'L'";
                    parameters.Add("@Where", p_strWhere);
                    IEnumerable<WOResource> wOResLabor = conn.Query<Models.WO.WOResource>("msp_WOResource_GetByType", parameters, commandType: StoredProcedure);
                    wOs.WOResPlanLabor = wOResLabor;

                    p_strWhere = "";
                    parameters = new DynamicParameters();
                    p_strWhere += $" WHERE WOResource.WONo = {woNo}";
                    p_strWhere += $" AND WOResource.Type = 'A'";
                    p_strWhere += $" AND WOResource.ResTypeCode = 'L'";
                    parameters.Add("@Where", p_strWhere);
                    IEnumerable<Models.WO.WOResource> wOResLaborAct = conn.Query<Models.WO.WOResource>("msp_WOResource_GetByType", parameters, commandType: StoredProcedure);
                    wOs.WOResActLabor = wOResLaborAct;

                    p_strWhere = "";
                    parameters = new DynamicParameters();
                    p_strWhere += $" WHERE WOResource.WONo = {woNo}";
                    p_strWhere += $" AND WOResource.Type = 'P'";
                    p_strWhere += $" AND WOResource.ResTypeCode <> 'L'";
                    parameters.Add("@Where", p_strWhere);
                    IEnumerable<Models.WO.WOResource> wOResMat = conn.Query<Models.WO.WOResource>("msp_WOResource_GetByType", parameters, commandType: StoredProcedure);
                    wOs.WOResPlanMat = wOResMat;

                    p_strWhere = "";
                    parameters = new DynamicParameters();
                    p_strWhere += $" WHERE WOResource.WONo = {woNo}";
                    p_strWhere += $" AND WOResource.Type = 'A'";
                    p_strWhere += $" AND WOResource.ResTypeCode <> 'L'";
                    parameters.Add("@Where", p_strWhere);
                    IEnumerable<Models.WO.WOResource> wOResMatAct = conn.Query<Models.WO.WOResource>("msp_WOResource_GetByType", parameters, commandType: StoredProcedure);
                    wOs.WOResActMat = wOResMatAct;

                    parameters = new DynamicParameters();
                    parameters.Add("@LinkNo", woNo);
                    parameters.Add("@FileType", "WO");
                    IEnumerable<AttachFileObject> attachFileResult = conn.Query<AttachFileObject>("sp_AttachFile_GetByLinkNoFileType", parameters, commandType: StoredProcedure);
                    wOs.AttachmentObj = new List<string>();
                    wOs.AttachmentAfterObj = new List<string>();
                    foreach (AttachFileObject item in attachFileResult)
                    {
                        if (item.FileName.IndexOf("AFTER") > -1)
                        {
                            wOs.AttachmentAfterObj.Add(string.Format("{0}/{1}", _configuration["IdylWeb"], item.Path));
                        }
                        if (item.FileName.IndexOf("BEFORE") > -1)
                        {
                            wOs.AttachmentObj.Add(string.Format("{0}/{1}", _configuration["IdylWeb"], item.Path));
                        }

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

        public Result RetriveEvaluate(int id)
        {
            throw new NotImplementedException();
        }

        public Result RetriveInspecFiles(int id)
        {
            Result result = new Result();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    conn.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@LinkNo", id);
                    parameters.Add("@FileType", "WO");
                    parameters.Add("@IsInspection", true);

                    IEnumerable<AttachFileObject> inspecFiles = conn.Query<AttachFileObject>("sp_AttachFile_GetByLinkNoFileType", parameters, commandType: StoredProcedure);
                    foreach (AttachFileObject item in inspecFiles)
                    {
                        item.WebAddress = _configuration["IdylWeb"];
                    }

                    result.Data = inspecFiles;
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

        public Result RetriveViewFilter(WhereParameter whereParameter, User user)
        {
            Result result = new Result();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    conn.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    string condition = "";
                    condition += $" where wo.companyno = {whereParameter.SiteNo}";
                    condition += " and wo.wostatusno in (9)";

                    if ("D" == whereParameter.DataType)
                    {
                        condition += " and wo.SectionNo =" + user.SectionNo;
                    }
                    else if ("O" == whereParameter.DataType)
                    {
                        condition += string.Format(" and (wo.CraftNo ={0} or wo.assignto={0} OR WO.WONo IN (SELECT WOD.WONo FROM WOD WHERE WOD.HeadCraftTypeNo = {0}) OR WO.WONo IN (SELECT WOResource.WONo FROM WOResource WHERE RescNo = {0} and ResTypeCode = 'L'))", user.CustomerNo);
                    }
                    condition += " and _SystUser_Location.userno = " + user.UserNo;

                    parameters.Add("@WhereSel", condition);
                    IEnumerable<Models.WO.WO> wOs = conn.Query<Models.WO.WO>("msp_WOMain_Retrive_ViewFilter", parameters, commandType: StoredProcedure);

                    List<GroupFilter> groupFilters = new List<GroupFilter>();

                    FilterHelper.AddFilter(ref groupFilters, InitFilterMaster.CreateEQ(wOs), "eq", "อุปกรณ์");
                    FilterHelper.AddFilter(ref groupFilters, InitFilterMaster.CreateLocation(wOs), "location", "สถานที่");
                    FilterHelper.AddFilter(ref groupFilters, InitFilterMaster.CreatePlnDate(), "plndate", "วางแผน");
                    FilterHelper.AddFilter(ref groupFilters, InitFilterMaster.CreateWoType(wOs), "wotype", "ประเภทงาน");

                    result.Data = groupFilters;
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

        public Result RetriveWOToLocal(List<Models.WO.WO> wos, User user)
        {
            Result result = new Result();
            try
            {
                List<Models.WO.WO> newWos = new List<Models.WO.WO>();
                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    conn.Open();
                    foreach (var item in wos)
                    {
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("@WONo", item.WONo);

                        Models.WO.WO wOs = conn.QueryFirst<Models.WO.WO>("sp_WO_GetByNo", parameters, commandType: StoredProcedure);
                        AttachFileObject[] attachment = { new AttachFileObject() { } };
                        wOs.Attachment = attachment;
                        wOs.AttachmentObj = new List<string>();
                        wOs.AttachmentAfterObj = new List<string>();
                        wOs.AttachmentBefore = attachment;
                        wOs.AttachmentAfter = attachment;
                        wOs.DTs = new Models.WO.DT();
                        wOs.fM = new Models.WO.FM();
                        wOs.InspectFiles = new List<string>();
                        wOs.WOResPlanLabor = new List<WOResource>();
                        wOs.WOResPlanMat = new List<WOResource>();
                        wOs.WOResActLabor = new List<WOResource>();
                        wOs.WOResActMat = new List<WOResource>();
                        ////Plan
                        //string p_strWhere = "";
                        //parameters = new DynamicParameters();
                        //p_strWhere += $" WHERE WOResource.WONo = {item.WONo}";
                        //p_strWhere += $" AND WOResource.Type = 'P'";
                        //p_strWhere += $" AND WOResource.ResTypeCode = 'L'";
                        //parameters.Add("@Where", p_strWhere);
                        //IEnumerable<WOResource> wOResLabor = conn.Query<WOResource>("msp_WOResource_GetByType", parameters, commandType: StoredProcedure);
                        //wOs.WOResPlanLabor = wOResLabor;

                        //p_strWhere = "";
                        //parameters = new DynamicParameters();
                        //p_strWhere += $" WHERE WOResource.WONo = {item.WONo}";
                        //p_strWhere += $" AND WOResource.Type = 'P'";
                        //p_strWhere += $" AND WOResource.ResTypeCode <> 'L'";
                        //parameters.Add("@Where", p_strWhere);
                        //IEnumerable<WOResource> wOResMat = conn.Query<WOResource>("msp_WOResource_GetByType", parameters, commandType: StoredProcedure);
                        //wOs.WOResPlanMat = wOResMat;


                        ////Actual
                        //p_strWhere = "";
                        //parameters = new DynamicParameters();
                        //p_strWhere += $" WHERE WOResource.WONo = {item.WONo}";
                        //p_strWhere += $" AND WOResource.Type = 'A'";
                        //p_strWhere += $" AND WOResource.ResTypeCode = 'L'";
                        //p_strWhere += $" AND WOResource.QtyMH > 0";
                        //parameters.Add("@Where", p_strWhere);
                        //IEnumerable<Models.WO.WOResource> wOResLaborAct = conn.Query<WOResource>("msp_WOResource_GetByType", parameters, commandType: StoredProcedure);
                        //wOs.WOResActLabor = wOResLaborAct;

                        //p_strWhere = "";
                        //parameters = new DynamicParameters();
                        //p_strWhere += $" WHERE WOResource.WONo = {item.WONo}";
                        //p_strWhere += $" AND WOResource.Type = 'A'";
                        //p_strWhere += $" AND WOResource.ResTypeCode <> 'L'";
                        //p_strWhere += $" AND WOResource.QtyMH > 0";
                        //parameters.Add("@Where", p_strWhere);
                        //IEnumerable<Models.WO.WOResource> wOResMatAct = conn.Query<WOResource>("msp_WOResource_GetByType", parameters, commandType: StoredProcedure);
                        //wOs.WOResActMat = wOResMatAct;

                        //parameters = new DynamicParameters();
                        //parameters.Add("@WONo", item.WONo);
                        //Models.WO.DT dT = conn.QueryFirstOrDefault<Models.WO.DT>("sp_DownTime_GetByWONo", parameters, commandType: StoredProcedure);
                        //wOs.DTs = dT;
                        //if (dT != null)
                        //{
                        //    wOs.IsDowntime = true;
                        //}

                        //parameters = new DynamicParameters();
                        //parameters.Add("@LinkNo", item.WONo);
                        //parameters.Add("@FileType", "WO");
                        //IEnumerable<AttachFileObject> attachFileResult = conn.Query<AttachFileObject>("sp_AttachFile_GetByLinkNoFileType", parameters, commandType: StoredProcedure);
                        //wOs.AttachmentObj = new List<string>();
                        //wOs.AttachmentAfterObj = new List<string>();
                        //foreach (AttachFileObject attachFileObject in attachFileResult)
                        //{
                        //    if (attachFileObject.FileName.IndexOf("AFTER") > -1)
                        //    {
                        //        wOs.AttachmentAfterObj.Add(string.Format("{0}/{1}", _configuration["IdylWeb"], attachFileObject.Path));
                        //    }
                        //    if (attachFileObject.FileName.IndexOf("BEFORE") > -1)
                        //    {
                        //        wOs.AttachmentObj.Add(string.Format("{0}/{1}", _configuration["IdylWeb"], attachFileObject.Path));
                        //    }
                        //}

                        newWos.Add(wOs);
                    }
                }

                result.Data = newWos;
                result.StatusCode = 200;
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.ErrMsg = ex.Message;
            }
            return result;
        }

        [Obsolete]
        public Result UpdatePlan(Models.WO.WO wo, User user)
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
                        parameters.Add("@WONo", wo.WONo);
                        Models.WO.WO woOld = conn.QueryFirst<Models.WO.WO>("sp_WO_GetByNo", parameters, commandType: StoredProcedure, transaction: trans);

                        bool isPlan = false;
                        if (woOld.PlnDate != wo.PlnDate || woOld.CraftNo != wo.CraftNo)
                        {
                            isPlan = true;
                        }

                        parameters = new DynamicParameters();
                        parameters.Add("@WONo", woOld.WONo);
                        parameters.Add("@WOStatusNo", woOld.WOStatusNo);
                        parameters.Add("@WRCode", woOld.WRCode);
                        parameters.Add("@WRDate", woOld.WRDate);
                        parameters.Add("@EQNo", woOld.EQNo);
                        parameters.Add("@LocationNo", woOld.LocationNo);
                        parameters.Add("@WorkDesc", woOld.WorkDesc);
                        parameters.Add("@SectReq", woOld.SectReq);
                        parameters.Add("@Requester", woOld.Requester);
                        parameters.Add("@CompanyNo", woOld.CompanyNo);
                        parameters.Add("@UpdatedBy", user.CustomerNo);
                        parameters.Add("@SectionNo", wo.SectionNo);
                        parameters.Add("@CraftNo", wo.CraftNo);
                        parameters.Add("@PlnDate", wo.PlnDate);
                        parameters.Add("@PlnDuration", wo.PlnDuration);
                        parameters.Add("@PlnManHours", woOld.PlnManHours);
                        parameters.Add("@WOAction", woOld.WOAction);
                        parameters.Add("@ActDate", woOld.ActDate);
                        parameters.Add("@ActTime", woOld.ActTime);
                        parameters.Add("@ActDuration", woOld.ActDuration);
                        parameters.Add("@ActManHours", woOld.ActManHours);
                        parameters.Add("@Meter", woOld.Meter);
                        parameters.Add("@WOCause", woOld.WOCause);
                        parameters.Add("@WOTypeNo", wo.WOTypeNo);
                        parameters.Add("@CostMH", woOld.CostMH);
                        parameters.Add("@CostStock", woOld.CostStock);
                        parameters.Add("@CostDirectPurchase", woOld.CostDirectPurchase);
                        parameters.Add("@CostTools", woOld.CostTools);
                        parameters.Add("@CostOutsources", woOld.CostOutsources);
                        parameters.Add("@CostCrafts", woOld.CostCrafts);


                        conn.Execute("sp_WO_Update", parameters, commandType: StoredProcedure, transaction: trans);

                        trans.Commit();
                        result.ErrMsg = "บันทึกเรียบร้อย";
                        result.StatusCode = 200;
                        if (isPlan)
                        {
                            string cmd = $" select * from customer where customerno={wo.CraftNo}";
                            Customer craft = conn.QueryFirst<Customer>(cmd, parameters, commandType: Text);

                            if (craft.IsSendLine.HasValue && craft.IsSendLine.Value)
                            {
                                string msg = string.Format("IDYL: {2}| อาการ/ปัญหา: {0}| ประมาณวันเริ่มเวลา: {1}| {3}"
                                    , wo.WorkDesc
                                    , wo.PlnDate!= null ? Convert.ToDateTime(wo.PlnDate).ToString("dd/MM/yyyy HH:mm") : ""
                                    , woOld.WOCode
                                    , $"{_configuration["IdylWeb"]}/Form/WO/WOEdit.aspx?WONo={wo.WONo}");

                                new SendNotify(_configuration).LineNotify(msg, craft.LineToken);
                            }
                            if (craft.IsSendEmail.HasValue && craft.IsSendEmail.Value)
                            {
                                string dateTime = wo.PlnDate != null ? Convert.ToDateTime(wo.PlnDate).ToString("dd/MM/yyyy HH:mm") : "";
                                Mail.SendEmail(_configuration, new MailReq()
                                {
                                    Content = $"<br/><b>อาการ/ปัญหา: </b>{woOld.WorkDesc}<br/><br/><b>ประมาณวันเริ่มเวลา: </b>{dateTime}<br/>",
                                    DocCode = woOld.WOCode,
                                    DocLink = $"{_configuration["IdylWeb"]}/Form/WO/WOEdit.aspx?WONo={woOld.WONo}",
                                    FromPage = "WO",
                                    Receive = craft.Email
                                });
                            }
                        }
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
        public Result UpdateActual(Models.WO.WO wo, User user)
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
                        parameters.Add("@WONo", wo.WONo);
                        Models.WO.WO woOld = conn.QueryFirst<Models.WO.WO>("sp_WO_GetByNo", parameters, commandType: StoredProcedure, transaction: trans);

                        parameters = new DynamicParameters();
                        parameters.Add("@WONo", woOld.WONo);
                        parameters.Add("@WOStatusNo", woOld.WOStatusNo);
                        parameters.Add("@WRCode", woOld.WRCode);
                        parameters.Add("@WRDate", woOld.WRDate);
                        parameters.Add("@EQNo", woOld.EQNo);
                        parameters.Add("@LocationNo", woOld.LocationNo);
                        parameters.Add("@WorkDesc", woOld.WorkDesc);
                        parameters.Add("@SectReq", woOld.SectReq);
                        parameters.Add("@Requester", woOld.Requester);
                        parameters.Add("@CompanyNo", woOld.CompanyNo);
                        parameters.Add("@UpdatedBy", user.CustomerNo);
                        parameters.Add("@SectionNo", woOld.SectionNo);
                        parameters.Add("@CraftNo", woOld.CraftNo);
                        parameters.Add("@PlnDate", woOld.PlnDate);
                        parameters.Add("@PlnDuration", woOld.PlnDuration);
                        parameters.Add("@PlnManHours", woOld.PlnManHours);
                        parameters.Add("@WOAction", wo.WOAction);
                        parameters.Add("@ActDateStart", PAUtility.ValDBDapper.GetDateTime(wo.ActDateStart));
                        parameters.Add("@ActDate", wo.ActDate);
                        parameters.Add("@ActTime", woOld.ActTime);
                        parameters.Add("@ActDuration", wo.ActDuration);
                        parameters.Add("@ActManHours", woOld.ActManHours);
                        parameters.Add("@Meter", wo.Meter);
                        parameters.Add("@WOCause", wo.WOCause);
                        parameters.Add("@WOTypeNo", woOld.WOTypeNo);
                        parameters.Add("@CostMH", woOld.CostMH);
                        parameters.Add("@CostStock", woOld.CostStock);
                        parameters.Add("@CostDirectPurchase", woOld.CostDirectPurchase);
                        parameters.Add("@CostTools", woOld.CostTools);
                        parameters.Add("@CostOutsources", woOld.CostOutsources);
                        parameters.Add("@CostCrafts", woOld.CostCrafts);
                        parameters.Add("@SystemNo", wo.SystemNo);

                        conn.Execute("sp_WO_Update", parameters, commandType: StoredProcedure, transaction: trans);

                        if (wo.IsDowntime)
                        {
                            parameters = new DynamicParameters();
                            parameters.Add("@WONo", wo.WONo);
                            Models.WO.DT dTOld = conn.QueryFirstOrDefault<Models.WO.DT>("sp_DownTime_GetByWONo", parameters, commandType: StoredProcedure, transaction: trans);

                            if (wo.DTs.DateTimeStop.HasValue && wo.DTs.DateTimeStopEnd.HasValue)
                            {
                                wo.DTs.DownTime = Convert.ToSingle(wo.DTs.DateTimeStopEnd.Value.Subtract(wo.DTs.DateTimeStop.Value).TotalMinutes / 60);
                            }

                            if (dTOld != null)
                            {
                                parameters = new DynamicParameters();
                                parameters.Add("@DTNo", dTOld.DTNo);
                                parameters.Add("@DTDate", dTOld.DTDate);
                                parameters.Add("@EQDown", true);
                                parameters.Add("@PlantDown", true);
                                parameters.Add("@DownTime", wo.DTs.DownTime);
                                parameters.Add("@EQNo", wo.EQNo);
                                parameters.Add("@LocationNo", wo.LocationNo);
                                parameters.Add("@UpdatedBy", user.CustomerNo);
                                parameters.Add("@LossAmount", dTOld.LossAmount);
                                parameters.Add("@LossQty", dTOld.LossQty);
                                parameters.Add("@DateTimeStop", wo.DTs.DateTimeStop);
                                parameters.Add("@DateTimeStopEnd", wo.DTs.DateTimeStopEnd);
                                conn.Execute("sp_DownTime_Update", parameters, commandType: StoredProcedure, transaction: trans);
                            }
                            else
                            {

                                parameters = new DynamicParameters();
                                parameters.Add("@DTNo", wo.DTs.DTNo, DbType.Int32, ParameterDirection.Output);
                                parameters.Add("@DTDate", woOld.WODate);
                                parameters.Add("@WONo", wo.WONo);
                                parameters.Add("@EQDown", true);
                                parameters.Add("@PlantDown", true);
                                parameters.Add("@DownTime", wo.DTs.DownTime);
                                parameters.Add("@EQNo", wo.EQNo);
                                parameters.Add("@LocationNo", wo.LocationNo);
                                parameters.Add("@CompanyNo", wo.CompanyNo);
                                parameters.Add("@CreatedBy", user.CustomerNo);
                                parameters.Add("@LossAmount", wo.DTs.LossAmount);
                                parameters.Add("@LossQty", wo.DTs.LossQty);
                                parameters.Add("@DateTimeStop", wo.DTs.DateTimeStop);
                                parameters.Add("@DateTimeStopEnd", wo.DTs.DateTimeStopEnd);
                                conn.Query<int>("sp_DownTime_Insert", parameters, commandType: StoredProcedure, transaction: trans);
                            }
                        }
                        else
                        {
                            parameters = new DynamicParameters();
                            parameters.Add("@WONo", wo.WONo);
                            conn.Execute("sp_DownTime_DeleteByWONo", parameters, commandType: StoredProcedure, transaction: trans);
                        }


                        parameters = new DynamicParameters();
                        parameters.Add("@WONo", wo.WONo);
                        Models.WO.FM fMOld = conn.QueryFirstOrDefault<Models.WO.FM>("sp_FM_GetByWONo", parameters, commandType: StoredProcedure, transaction: trans);
                        if (fMOld == null)
                        {
                            parameters = new DynamicParameters();
                            parameters.Add("@FMNo", 0, DbType.Int32, ParameterDirection.Output);
                            parameters.Add("@Date", woOld.WODate);
                            parameters.Add("@MeterRead", wo.Meter);
                            parameters.Add("@WOTypeNo", woOld.WOTypeNo);
                            parameters.Add("@EQNo", wo.EQNo);
                            parameters.Add("@LocationNo", wo.LocationNo);
                            parameters.Add("@EQTypeNo", null);
                            parameters.Add("@Manuf", "");
                            parameters.Add("@Model", "");
                            parameters.Add("@ProblemNo", wo.fM.ProblemNo);
                            parameters.Add("@Problem", "");
                            parameters.Add("@ComponentNo", wo.fM.ComponentNo);
                            parameters.Add("@Component", "");
                            parameters.Add("@CauseNo", wo.fM.CauseNo);
                            parameters.Add("@Cause", "");
                            parameters.Add("@ActionNo", wo.fM.ActionNo);
                            parameters.Add("@Action", "");
                            parameters.Add("@WONo", wo.WONo);
                            parameters.Add("@OPNo", 10);
                            parameters.Add("@CompanyNo", woOld.CompanyNo);
                            parameters.Add("@CreatedBy", user.CustomerNo);
                            conn.Execute("sp_FM_Insert", parameters, commandType: StoredProcedure, transaction: trans);

                        }
                        else
                        {
                            parameters = new DynamicParameters();
                            parameters.Add("@FMNo", fMOld.FMNo);
                            parameters.Add("@Date", woOld.WODate);
                            parameters.Add("@MeterRead", wo.Meter);
                            parameters.Add("@WOTypeNo", woOld.WOTypeNo);
                            parameters.Add("@EQNo", wo.EQNo);
                            parameters.Add("@LocationNo", wo.LocationNo);
                            parameters.Add("@EQTypeNo", fMOld.EQTypeNo);
                            parameters.Add("@Manuf", fMOld.Manuf);
                            parameters.Add("@Model", fMOld.Model);
                            parameters.Add("@ProblemNo", wo.fM.ProblemNo);
                            parameters.Add("@Problem", "");
                            parameters.Add("@ComponentNo", wo.fM.ComponentNo);
                            parameters.Add("@Component", "");
                            parameters.Add("@CauseNo", wo.fM.CauseNo);
                            parameters.Add("@Cause", "");
                            parameters.Add("@ActionNo", wo.fM.ActionNo);
                            parameters.Add("@Action", "");
                            parameters.Add("@CompanyNo", woOld.CompanyNo);
                            parameters.Add("@UpdatedBy", user.CustomerNo);
                            conn.Execute("sp_FM_Update", parameters, commandType: StoredProcedure, transaction: trans);
                        }

                        if (wo.AttachmentBefore != null && wo.AttachmentBefore.Length > 0)
                        {
                            foreach (AttachFileObject item in wo.AttachmentBefore)
                            {
                                string ext = Path.GetExtension(item.DocName); ;
                                int id = wo.WONo;
                                string path = $"{UploadFiles.GenerateFolderUploadPath("WO", woOld.CompanyName)}/{woOld.WOCode}/{item.FileName}";

                                string pPath = _host.ContentRootPath;
                                int pathLevel = InputVal.ToInt(_configuration["PathLevel"]);
                                for (int j = 0; j <= pathLevel; j++)
                                {
                                    pPath = Directory.GetParent(pPath).ToString();
                                }

                                pPath += _configuration["AttPath"] + "\\[" + woOld.CompanyName.Replace(" ", "").Trim() + "]\\WO\\" + woOld.WOCode + "";
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
                                parameters.Add("@CompanyNo", woOld.CompanyNo);
                                parameters.Add("@CreatedBy", user.CustomerNo);
                                parameters.Add("@IsUrl", false);
                                parameters.Add("@Extension", ext);
                                parameters.Add("@AttachFileNo", 0, DbType.Int32, ParameterDirection.Output);

                                SqlMapper.Execute(conn, "sp_AttachFile_Insert", parameters, commandType: StoredProcedure, transaction: trans);

                            }
                        }
                        if (wo.AttachmentAfter != null && wo.AttachmentAfter.Length > 0)
                        {
                            foreach (AttachFileObject item in wo.AttachmentAfter)
                            {
                                string ext = Path.GetExtension(item.DocName); ;
                                int id = wo.WONo;
                                string path = $"{UploadFiles.GenerateFolderUploadPath("WO", woOld.CompanyName)}/{woOld.WOCode}/{item.FileName}";
                                string pPath = _host.ContentRootPath;
                                int pathLevel = InputVal.ToInt(_configuration["PathLevel"]);
                                for (int j = 0; j <= pathLevel; j++)
                                {
                                    pPath = Directory.GetParent(pPath).ToString();
                                }

                                pPath += _configuration["AttPath"] + "\\[" + woOld.CompanyName.Replace(" ", "").Trim() + "]\\WO\\" + woOld.WOCode + "";
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
                        if (wo.ActDate.HasValue && woOld.WOStatusNo != 3)
                        {
                            new Notify.NotifyRepository(_configuration).Send(wo.WONo, "FINISH", wo.CompanyNo, user.CustomerNo);

                            string cmd = $" select * from customer where customerno={woOld.CraftNo}";
                            Customer section = conn.QueryFirst<Customer>(cmd, parameters, commandType: Text);

                            if (section.IsSendLine.HasValue && section.IsSendLine.Value)
                            {
                                string msg = string.Format("IDYL: {2}| รหัส/ชื่ออุปกรณ์:{4}| อาการ/ปัญหา: {0}| วันที่เสร็จงาน: {1}| {3}"
                                    , woOld.WorkDesc
                                    , wo.ActDate.Value.ToString("dd/MM/yyyy HH:mm")
                                    , woOld.WOCode
                                    , $"{_configuration["IdylWeb"]}/Form/WO/WOEdit.aspx?WONo={wo.WONo}"
                                    , $"{woOld.EQCode};{woOld.EQName}");

                                new SendNotify(_configuration).LineNotify(msg, section.LineToken);
                            }
                            if (section.IsSendEmail.HasValue && section.IsSendEmail.Value)
                            {
                                string dateTime = wo.ActDate.Value.ToString("dd/MM/yyyy HH:mm");
                                Mail.SendEmail(_configuration, new MailReq()
                                {
                                    Content = $"<br/><b>อาการ/ปัญหา: </b>{woOld.WorkDesc}<br/><br/><b>วันที่เสร็จงาน: </b>{dateTime}<br/>",
                                    DocCode = woOld.WOCode,
                                    DocLink = $"{_configuration["IdylWeb"]}/Form/WO/WOEdit.aspx?WONo={wo.WONo}",
                                    FromPage = "WO",
                                    Receive = section.Email
                                });
                            }
                        }

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

        public Result UpdateStatus(int woNo, int woStatusNo, int updatedBy)
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

                        parameters.Add("@WONo", woNo);
                        parameters.Add("@WOStatusNo", woStatusNo);
                        parameters.Add("@UpdatedBy", updatedBy);
                        conn.Execute("sp_WO_UpdateWOStatus", parameters, commandType: StoredProcedure, transaction: trans);

                        trans.Commit();
                        string action = woStatusNo == 1 ? "HISTORY" : "CANCEL";
                        new Notify.NotifyRepository(_configuration).Send(woNo, action, 0, updatedBy);
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

        public string GenFilter(LoadOptions loadOption)
        {
            string pNewStrWhere = "";
            if (loadOption != null && loadOption.filters != null && loadOption.filters.Count > 0)
            {
                var filterOrder = loadOption.filters.OrderBy(o => o.filtertype);

                string oldType = "";
                foreach (var item in filterOrder)
                {
                    int cnt = 0;
                    if (oldType == "")
                    {
                        cnt = 0;
                        oldType = item.filtertype;
                        pNewStrWhere += " and ( ";
                    }
                    else if (oldType != item.filtertype)
                    {
                        oldType = item.filtertype;
                        pNewStrWhere += ") and ( ";
                    }
                    else
                    {
                        pNewStrWhere += " or ";
                        cnt++;
                    }

                    if (item.filtertype == "wotype")
                    {
                        pNewStrWhere += $" WO.WOTYPENO = {item.id}";
                    }
                    else if (item.filtertype == "plndate")
                    {
                        string date = "";
                        if (item.id == "today")
                        {
                            date = DateTime.Now.ToString("yyyyMMdd");
                        }
                        else if (item.id == "tomorrow")
                        {
                            date = DateTime.Now.AddDays(1).ToString("yyyyMMdd");
                        }
                        else
                        {
                            date = item.options;
                        }
                        pNewStrWhere += $" WO.PlnDate = '{date}'";
                    }
                    else if (item.filtertype == "location")
                    {
                        pNewStrWhere += $" WO.LocationNo = {item.id}";
                    }
                    else if (item.filtertype == "eq")
                    {
                        pNewStrWhere += $" WO.EQNo = {item.id}";
                    }
                }
                pNewStrWhere += ") ";
            }


            return pNewStrWhere;
        }
    }
}
