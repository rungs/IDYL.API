using Dapper;
using IdylAPI.Helper;
using IdylAPI.Models;
using IdylAPI.Models.Authorize;
using IdylAPI.Models.Img;
using IdylAPI.Models.Master;
using IdylAPI.Services.Interfaces.Master;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using PAUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using static System.Data.CommandType;
namespace IdylAPI.Services.Repository.Master
{
    public class EvaluateRepository : IEvaluateRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connStr;
        [Obsolete]
        private readonly IHostingEnvironment _host;

        [Obsolete]
        public EvaluateRepository(IConfiguration configuration, IHostingEnvironment host)
        {
            _host = host;
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("IDYLConnection");
        }

        [Obsolete]
        public Result Insert(Sign sign, User user)
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
                        foreach (Evaluate e in sign.Evaluate)
                        {
                            parameters = new DynamicParameters();
                            parameters.Add("@EvaluateNo", e.EvaluateNo);
                            parameters.Add("@WONo", sign.WONo);
                            parameters.Add("@CustomerNo", user.CustomerNo);
                            parameters.Add("@CompanyNo", sign.CompanyNo);
                            parameters.Add("@Score", e.Score);
                            conn.Execute("sp_EvaluateScore_Insert", parameters, commandType: StoredProcedure, transaction: trans);
                        }

                        parameters = new DynamicParameters();
                        parameters.Add("@WONo", sign.WONo);
                        parameters.Add("@CustomerNo", user.CustomerNo);
                        parameters.Add("@CompanyNo", sign.CompanyNo);
                        parameters.Add("@Recommend", sign.Recommend);
                        conn.Execute("sp_EvaluateRecommend_Insert", parameters, commandType: StoredProcedure, transaction: trans);


                        if (sign.SignatureIMG != null)
                        {
                            parameters = new DynamicParameters();
                            parameters.Add("@WONo", sign.WONo);
                            Models.WO.WO woOld = conn.QueryFirst<Models.WO.WO>("sp_WO_GetByNo", parameters, commandType: StoredProcedure, transaction: trans);

                            string ext = Path.GetExtension(sign.Attachment.DocName);
                            int id = sign.WONo;

                            string path = $"{UploadFiles.GenerateFolderUploadPath("WO", sign.CompanyName)}/{woOld.WOCode}/{sign.Attachment.FileName}";
                            string pPath = _host.ContentRootPath;
                            int pathLevel = InputVal.ToInt(_configuration["PathLevel"]);
                            for (int j = 0; j <= pathLevel; j++)
                            {
                                pPath = Directory.GetParent(pPath).ToString();
                            }

                            pPath += _configuration["AttPath"] + "\\[" + sign.CompanyName.Replace(" ", "").Trim() + "]\\WO\\" + woOld.WOCode;
                            bool exists = System.IO.Directory.Exists(pPath);
                            if (!exists)
                            {
                                Directory.CreateDirectory(pPath);
                            }
                            pPath += "\\" + sign.Attachment.FileName;
                            File.Copy(sign.Attachment.Path, pPath);
                            File.Delete(sign.Attachment.Path);
                            parameters = new DynamicParameters();
                            parameters.Add("@LinkNo", id);
                            parameters.Add("@FileName", sign.Attachment.DocName);
                            parameters.Add("@Path", path);
                            parameters.Add("@FileType", "WO");
                            parameters.Add("@CompanyNo", sign.CompanyNo);
                            parameters.Add("@CreatedBy", user.CustomerNo);
                            parameters.Add("@IsUrl", false);
                            parameters.Add("@Extension", ext);
                            parameters.Add("@AttachFileNo", sign.SignNo, DbType.Int32, ParameterDirection.Output);

                            conn.Query<int>("sp_AttachFile_Insert", parameters, commandType: StoredProcedure, transaction: trans);
                            sign.SignNo = parameters.Get<int>("@AttachFileNo");


                            string cmd = $" update wo set SignatureIMG={sign.SignNo}, SignatureDateTime=getdate() where wo.wono = {sign.WONo}";
                            conn.Execute(cmd, commandType: Text, transaction: trans);
                        }

                        trans.Commit();
                        new Notify.NotifyRepository(_configuration).Send(sign.WONo, "EVALUATE", sign.CompanyNo, user.CustomerNo);
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

        public Result RetriveByWONo(int woNo, int companyNo)
        {
            Result result = new Result();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    conn.Open();

                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@WONo", woNo);
                    IEnumerable<Evaluate> evaluates = SqlMapper.Query<Evaluate>(conn, "sp_EvaluateScore_GetByWONo", parameters, commandType: StoredProcedure);

                    if (evaluates.AsList().Count == 0)
                    {
                        parameters = new DynamicParameters();
                        parameters.Add("@CompanyNo", companyNo);
                        evaluates = SqlMapper.Query<Evaluate>(conn, "sp_Evaluate_GetAll", parameters, commandType: StoredProcedure);
                    }

                    parameters = new DynamicParameters();
                    parameters.Add("@WONo", woNo);
                    Sign signObj = SqlMapper.QueryFirstOrDefault<Sign>(conn, "sp_EvaluateRecommend_GetByWONo", parameters, commandType: StoredProcedure);


                    if (signObj == null)
                    {
                        signObj = new Sign();
                    }
                    else
                    {
                        string cmd = $" select * from AttachFile where AttachNo={signObj.SignatureIMG}";
                        AttachFileObject attachFileObject = SqlMapper.QueryFirstOrDefault<AttachFileObject>(conn, cmd, commandType: Text);
                        if (attachFileObject != null)
                        {
                            signObj.SignatureIMG = string.Format("{0}/{1}", _configuration["IdylWeb"], attachFileObject.Path);
                        }
                    }


                    signObj.Evaluate = evaluates;

                    result.Data = signObj;
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
