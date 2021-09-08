using Dapper;
using IdylAPI.Helper;
using IdylAPI.Models;
using IdylAPI.Models.Authorize;
using IdylAPI.Models.Master;
using IdylAPI.Models.Notify;
using IdylAPI.Models.WO;
using IdylAPI.Services.Interfaces.Authorize;
using Microsoft.Extensions.Configuration;
using PAUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using static System.Data.CommandType;
namespace IdylAPI.Services.Repository.Notify
{
    public class NotifyRepository : INotifyRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connStr;
        public NotifyRepository(IConfiguration configuration)
        {

            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("IDYLConnection");
        }
        public Result Insert(string token, User user)
        {
            Result result = new Result();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    conn.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@UserNo", user.UserNo);
                    parameters.Add("@Token", token);
                    parameters.Add("@No", 0, DbType.Int32, ParameterDirection.Output);
                    SqlMapper.Execute(conn, "msp_NotifyToken_Insert", parameters, commandType: StoredProcedure);
                    result.StatusCode = 200;
                    result.Data = InputVal.ToString(parameters.Get<int>("@No"));
                }
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.ErrMsg = ex.Message;
            }
            return result;
        }
        public Result RetriveTokenByUser(int userNo)
        {
            Result result = new Result();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    conn.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@UserNo", userNo);
                    NotifyToken obj = SqlMapper.QueryFirstOrDefault<NotifyToken>(conn, "msp_NotifyToken_RetriveByPerson", parameters, commandType: StoredProcedure);

                    result.Data = obj;
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
        public Result PushMsg(int companyNo, int woNo, int customerNo, string action)
        {
            Result result = new Result();
            try
            {
                Send(woNo, action, companyNo, customerNo);
                result.StatusCode = 200;
            }
            catch (Exception ex)
            {

                result.StatusCode = 500;
                result.ErrMsg = ex.Message;
            }
            return result;
        }
        public Result RetriveInboxByUser(WhereParameter whereParameter, int userNo)
        {
            Result result = new Result();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    conn.Open();
                    string condition = "";
                    condition += $" where me.AssignTo = {userNo}";

                    if (!string.IsNullOrEmpty(InputVal.ToString(whereParameter.Filter)))
                    {
                        condition += $" and(me.AssignMessage like '%{whereParameter.Filter}%'";
                        condition += $" or wo.wocode like '%{whereParameter.Filter}%')";
                    }
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@WhereSel", condition);
                    parameters.Add("@StartRow", whereParameter.StartRow);
                    parameters.Add("@EndRow", whereParameter.EndRow);
                    IEnumerable<DocumentHistory> wOInprogress = conn.Query<DocumentHistory>("msp_DocumentHistory_Retrive", parameters, commandType: StoredProcedure);

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

        #region Logic
        public void InsertDocumentHistory(DocumentHistory obj)
        {
            Result result = new Result();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    conn.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@LinkDocNo", obj.LinkDocNo);
                    parameters.Add("@LinkDocType", obj.LinkDocType);
                    parameters.Add("@Description", obj.Description);
                    parameters.Add("@AssignFrom", obj.AssignFrom);
                    parameters.Add("@AssignTo", obj.AssignTo);
                    parameters.Add("@DocFrom", obj.DocFrom);
                    parameters.Add("@DocTypeFrom", obj.DocTypeFrom);
                    parameters.Add("@DocTo", obj.DocTo);
                    parameters.Add("@DocTypeTo", obj.DocTypeTo);
                    parameters.Add("@CreatedBy", obj.CreateBy);
                    parameters.Add("@CompanyNo", obj.CompanyNo);
                    parameters.Add("@AssignMessage", obj.AssignMessage);
                    conn.Execute("DocumentHistory_Insert", parameters, commandType: StoredProcedure);

                    result.StatusCode = 200;
                }
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.ErrMsg = ex.Message;
            }

        }
        public IEnumerable<NotifyToken> RetriveTokenMaintenanceLead(int sectionNo)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();
                DynamicParameters parameters = new DynamicParameters();
                string condition = $" where Customer.sectionno={sectionNo} and  isnull(IsHeadSection,0)=1 and Customer.IsDelete=0 ";
                parameters.Add("@WhereSel", condition);
                IEnumerable<Person> people = SqlMapper.Query<Person>(conn, "msp_Person_Retrive", parameters, commandType: StoredProcedure);
                if (people.Count() > 0)
                {
                    condition = "";
                    foreach (var item in people)
                    {
                        condition += $"{item.CustomerNo},";
                    }
                    if (condition.Length > 0)
                    {
                        condition = condition.Substring(0, condition.Length - 1);
                    }
                    parameters = new DynamicParameters();
                    parameters.Add("@WhereSel", $" where u.customerno in({condition})");
                    return SqlMapper.Query<NotifyToken>(conn, "msp_NotifyToken_Retrive", parameters, commandType: StoredProcedure);
                }

                return new List<NotifyToken>().AsEnumerable();
            }
        }

        public string RetriveAssigneeMsg(int woNo)
        {
            string msg = string.Empty;
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();
                string cmd = " select c.customerno, c.firstname from WOResource w inner join Customer c on w.RescNo = c.CustomerNo " +
                    $" where Type = 'P' and ResTypeCode = 'L' and wono={woNo} group by c.customerno, c.firstname ";

                DynamicParameters parameters = new DynamicParameters();
                IEnumerable<Person> people = SqlMapper.Query<Person>(conn, cmd, null, commandType: Text);
                List<string> name = new List<string>();
                foreach (Person p in people)
                {
                    string firstname = p.Firstname;
                    if (!name.Contains(firstname))
                    {
                        name.Add(firstname);
                        msg += $" {firstname},";
                    }
                }
               
            }
            if(!string.IsNullOrEmpty(msg)) msg = msg.Substring(0, msg.Length - 1);
            return msg;
        }
        
        public IEnumerable<NotifyToken> RetriveAssignee(int woNo)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();
                string cmd = " select c.customerno, c.firstname from WOResource w inner join Customer c on w.RescNo = c.CustomerNo " +
                    " inner join _systuser_customer s on s.CustomerNo = c.CustomerNo " +
                    $" where Type = 'P' and ResTypeCode = 'L' and wono={woNo} group by c.customerno, c.firstname ";

                DynamicParameters parameters = new DynamicParameters();
                IEnumerable<Person> people = SqlMapper.Query<Person>(conn, cmd, null, commandType: Text);
                string name = "";
                if (people.Count() > 0)
                {
                    string condition = "";
                    foreach (var item in people)
                    {
                        condition += $"{item.CustomerNo},";
                        name += $"{item.Firstname},";
                    }
                    if (condition.Length > 0)
                    {
                        condition = condition.Substring(0, condition.Length - 1);
                        name = name.Substring(0, name.Length - 1);
                    }
                    parameters = new DynamicParameters();
                    parameters.Add("@WhereSel", $" where u.customerno in({condition})");

                    IEnumerable<NotifyToken> notifyToken = SqlMapper.Query<NotifyToken>(conn, "msp_NotifyToken_Retrive", parameters, commandType: StoredProcedure);
                    if (notifyToken.Count() > 0)
                    {
                        notifyToken.First().FirstName = name;
                    }
                    else
                    {
                        NotifyToken notify = new NotifyToken()
                        {
                            FirstName = name
                        };
                        List<NotifyToken> l = new List<NotifyToken>();
                        l.Add(notify);
                        notifyToken = l.AsEnumerable();
                    }

                    return notifyToken;
                }

                return new List<NotifyToken>().AsEnumerable();
            }
        }

        public IEnumerable<Person> RetriveAssigneeName(int woNo)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();
                string cmd = " select c.customerno, c.firstname from WOResource w inner join Customer c on w.RescNo = c.CustomerNo " +
                    $" where Type = 'P' and ResTypeCode = 'L' and wono={woNo} group by c.customerno, c.firstname ";

                DynamicParameters parameters = new DynamicParameters();
                return SqlMapper.Query<Person>(conn, cmd, null, commandType: Text);
            }
        }

        public IEnumerable<NotifyToken> RetriveByCustomer(int customerNo)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@WhereSel", $" where u.customerno in({customerNo})");
                return SqlMapper.Query<NotifyToken>(conn, "msp_NotifyToken_Retrive", parameters, commandType: StoredProcedure);
            }
        }
        public IEnumerable<NotifyToken> RetriveLastSender(int docNo)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();
                DynamicParameters parameters = new DynamicParameters();
                string cmd = $"   select top 1 NotifyToken.Token, _systUser_Customer.CustomerNo from DocumentHistory inner join _systUser_Customer on _systUser_Customer.CustomerNo = DocumentHistory.AssignFrom " +
                     $" inner join NotifyToken on _systUser_Customer.UserNo = NotifyToken.UserNo where DocFrom = {docNo} ";
                return SqlMapper.Query<NotifyToken>(conn, cmd, null, commandType: Text);
            }
        }

        public NotifyMsg RetriveMsgByAction(string action, int companyNo)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();
                string condition = $" where me.action='{action}' and me.companyno={companyNo}";

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@WhereSel", condition);
                return SqlMapper.QueryFirstOrDefault<NotifyMsg>(conn, "msp_NotifyMsg_Retrive", parameters, commandType: StoredProcedure);
            }
        }
        public IEnumerable<NotifyMsg> RetriveRole(int notifyNo)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@NotifyNo", notifyNo);
                return SqlMapper.Query<NotifyMsg>(conn, "msp_NotifyMsg_Role_ByNotifyNo_ForPushMsg", parameters, commandType: StoredProcedure);
            }
        }
        public void RetriveLeadNextTask(int wono, ref int leadNo, ref WOTask wOTask)
        {

            using (SqlConnection conn = new SqlConnection(_connStr))
            {

                conn.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@WONo", wono);
                parameters.Add("@LeadNo", 0, DbType.Int32, ParameterDirection.Output);
                parameters.Add("@WODNo", 0, DbType.Int32, ParameterDirection.Output);
                conn.Query("sp_WOD_GetLeadNextTask", parameters, commandType: StoredProcedure);
                leadNo = parameters.Get<int>("@LeadNo");
                int wodNo = parameters.Get<int>("@WODNo");

                wOTask = SqlMapper.QueryFirstOrDefault<WOTask>(conn, $" select * from wod where wodno={wodNo} ", null, commandType: Text);
            }

        }
        public void RetriveLeadFirstTask(int wono, ref int leadNo)
        {

            using (SqlConnection conn = new SqlConnection(_connStr))
            {

                conn.Open();
                WOTask wOTask = SqlMapper.QueryFirstOrDefault<WOTask>(conn, $" select top 1 * from wod where wono={wono} order by OPNO ", null, commandType: Text);
                if (wOTask != null)
                {
                    leadNo = wOTask.HeadCraftTypeNo.HasValue ? wOTask.HeadCraftTypeNo.Value : 0;
                }
            }
        }

        public void InsertNotifyLog(int sendNo, int receiveNo, string msg, DateTime sendDate, string receiveToken, Task<HttpResponseMessage> result, string roleName)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();
                string cmd = $" insert into msg_NotifyLog values({sendNo},{receiveNo},'{msg}','{sendDate}','{receiveToken}','{result.Result.ReasonPhrase}','{roleName}') ";
                SqlMapper.Execute(conn, cmd, null, commandType: Text);
            }
        }

        public void Send(int docNo, string action, int companyNo, int customerNo)
        {
            Models.WO.WO wO = new Models.WO.WO();
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@WONo", docNo);
                wO = conn.QueryFirstOrDefault<Models.WO.WO>("sp_WO_GetByNo", parameters, commandType: StoredProcedure);
            }
            if (wO == null) return;

            NotifyMsg notifyMsg = RetriveMsgByAction(action, wO.CompanyNo);
            if (notifyMsg.IsActive)
            {
                IEnumerable<NotifyMsg> roleList = RetriveRole(notifyMsg.NotifyNo);

                List<NotifyToken> tokens = new List<NotifyToken>();

                string title = wO.WOCode;
                string msg = "";
                int leadNo = 0;

                if (notifyMsg != null)
                {
                    WOTask wOTask = new WOTask();
                    RetriveLeadNextTask(wO.WONo, ref leadNo, ref wOTask);
                    if (action == "SERVICEREQUEST")
                    {
                        RetriveLeadFirstTask(wO.WONo, ref leadNo);
                    }

                    msg = ReplaceTagMsg(notifyMsg.Message, wO, wOTask);
                    if (notifyMsg.Title != null) title += ":" + ReplaceTagMsg(notifyMsg.Title, wO, wOTask);


                    foreach (var roleItem in roleList)
                    {
                        if (roleItem.RoleName.Contains("Maintenance Manager"))
                        {
                            foreach (NotifyToken item in RetriveTokenMaintenanceLead(wO.SectionNo))
                            {
                                item.RoleName = roleItem.RoleName;
                                tokens.Add(item);
                            }
                            //SendMsg(tokens, title, msg, customerNo, companyNo, action, wO.WONo, wO.WOCode, roleItem.RoleName);
                        }
                        else if (roleItem.RoleName.Contains("Requester"))
                        {
                            foreach (NotifyToken item in RetriveByCustomer(wO.Requester))
                            {
                                item.RoleName = roleItem.RoleName;
                                tokens.Add(item);
                            }
                            //SendMsg(tokens, title, msg, customerNo, companyNo, action, wO.WONo, wO.WOCode, roleItem.RoleName);
                        }
                        else if (roleItem.RoleName.Contains("Responsible Person") && wO.CraftNo.HasValue)
                        {
                            IEnumerable<NotifyToken> receive = RetriveByCustomer(wO.CraftNo.Value);
                            foreach (NotifyToken item in receive)
                            {
                                item.RoleName = roleItem.RoleName;
                                tokens.Add(item);
                            }
                            if (action == "ASSIGNTO[PLAN]" && receive != null)
                            {
                                msg += receive.First().FirstName;
                            }
                            //SendMsg(tokens, title, msg, customerNo, companyNo, action, wO.WONo, wO.WOCode, roleItem.RoleName);
                        }
                        else if (roleItem.RoleName.Trim().Contains("WorkCenterLeadofnexttask"))
                        {
                            foreach (NotifyToken item in RetriveByCustomer(leadNo))
                            {
                                item.RoleName = roleItem.RoleName;
                                tokens.Add(item);
                            }
                            //SendMsg(tokens, title, msg, customerNo, companyNo, action, wO.WONo, wO.WOCode, roleItem.RoleName);
                        }
                        else if (roleItem.RoleName.Contains("Work Center Lead"))
                        {
                            foreach (NotifyToken item in RetriveByCustomer(leadNo))
                            {
                                item.RoleName = roleItem.RoleName;
                                tokens.Add(item);
                            }
                            //SendMsg(tokens, title, msg, customerNo, companyNo, action, wO.WONo, wO.WOCode, roleItem.RoleName);
                        }
                        else if (roleItem.RoleName.Contains("Last Sender"))
                        {
                            foreach (NotifyToken item in RetriveLastSender(wO.WONo))
                            {
                                item.RoleName = roleItem.RoleName;
                                tokens.Add(item);
                            }
                            //SendMsg(tokens, title, msg, customerNo, companyNo, action, wO.WONo, wO.WOCode, roleItem.RoleName);
                        }
                    }

 
                    if(action != "INPROGRESS[PLAN]") SendMsg(tokens, title, msg, customerNo, companyNo, action, wO.WONo, wO.WOCode);
                }

                if (action == "INPROGRESS[PLAN]")
                {
                    msg += RetriveAssigneeMsg(wO.WONo);
                    foreach (NotifyToken item in RetriveAssignee(wO.WONo))
                    {
                        item.RoleName = "INPROGRESS[PLAN]";
                        tokens.Add(item);
                    }
                    SendMsg(tokens, title, msg, customerNo, companyNo, action, wO.WONo, wO.WOCode);//, "INPROGRESS[PLAN]"
                }
            }
        }

        public void SendMsg(IEnumerable<NotifyToken> tokens, string title, string msg, int from, int companyNo, string action, int woNo, string woCode)
        {
            if (action == "REQUEST")
            {
                action = DocumentHistory.WORKREQEST_CREATE;
            }
            else if (action == "INPROGRESS")
            {
                action = DocumentHistory.WORKREQEST_ACKNOWLEDGE;
            }
            else if (action == "INPROGRESS[PLAN]")
            {
                action = DocumentHistory.WORKORDER_PLAN;
            }
            else if (action == "FINISH")
            {
                action = DocumentHistory.WORKORDER_FINISH;
            }
            else if (action == "EVALUATE")
            {
                action = DocumentHistory.WORKORDER_EVALUATE;
            }
            else if (action == "HISTORY")
            {
                action = DocumentHistory.WORKORDER_CLOSE;
            }
            else if (action == "CANCEL")
            {
                action = DocumentHistory.WORKORDER_CANCEL;
            }
            else if (action == "ASSIGNTO")
            {
                action = DocumentHistory.WORKORDER_PLAN;
            }
            else if (action == "DONETASK")
            {
                action = DocumentHistory.DONETASK;
            }

            List<string> tokenUse = new List<string>();

            foreach (NotifyToken item in tokens)
            {
                if (!tokenUse.Contains(item.Token))
                {
                    tokenUse.Add(item.Token);
                    NotiData notiData = new NotiData()
                    {
                        docType = "WO",
                        docCode = woCode,
                        docNo = woNo,
                        siteNo = companyNo
                    };

                    DocumentHistory documentHistoryInfo = new DocumentHistory
                    {
                        AssignFrom = from,
                        AssignTo = item.CustomerNo,
                        AssignMessage = msg,
                        DocTypeFrom = "WO",
                        DocFrom = woNo,
                        Description = action,
                        CompanyNo = companyNo,
                        CreateBy = from
                    };

                    InsertDocumentHistory(documentHistoryInfo);
                    Task<HttpResponseMessage> resultMsg = new SendNotify(_configuration).Send(title, msg, item.Token, notiData);
                    InsertNotifyLog(from, item.CustomerNo, msg, DateTime.Now, item.Token, resultMsg, item.RoleName);
                }
            }
        }
        public string ReplaceTagMsg(string template, Models.WO.WO wO, WOTask wOTask)
        {
            if (template.Contains("[EQCODE]"))
            {
                template = template.Replace("[EQCODE]", wO.EQCode);
            }
            if (template.Contains("[SENDER]"))
            {
                template = template.Replace("[SENDER]", wO.ReqFirstName);
            }
            if (template.Contains("[PROBLEM]"))
            {
                template = template.Replace("[PROBLEM]", wO.WorkDesc);
            }
            if (template.Contains("[WOCODE]"))
            {
                template = template.Replace("[WOCODE]", wO.WOCode);
            }
            if (template.Contains("[DEPTNAME]"))
            {
                template = template.Replace("[DEPTNAME]", wO.SecRespName);
            }
            if (template.Contains("[TASKNO]") && wOTask != null)
            {
                template = template.Replace("[TASKNO]", wOTask.OPNo.ToString());
            }
            if (template.Contains("[TASKDESC]") && wOTask != null)
            {
                template = template.Replace("[TASKDESC]", wOTask.Description);
            }

            //if (template.Contains("[REMARK]"))
            //{
            //    template = template.Replace("[REMARK]", wO.WORE);
            //}
            return template;
        }

        public Result PushMsg(NotiObj notiObj)
        {
            Result result = new Result();
            try
            {

                IEnumerable<NotifyToken> tokens = RetriveByCustomer(notiObj.to);
                foreach (var item in tokens)
                {
                    NotiData notiData = new NotiData()
                    {
                        docType = notiObj.docType,
                        docNo = notiObj.docNo,
                        docCode = notiObj.docCode,
                        siteNo = notiObj.siteNo
                    };

                    Task<HttpResponseMessage> resultMsg = new SendNotify(_configuration).Send(notiObj.title, notiObj.body, item.Token, notiData);

                }

                result.StatusCode = 200;
                return result;
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.ErrMsg = ex.Message;
            }
            return result;
        }

        public Result SendAssignMsg(IEnumerable<NotiObj> notiObjs, User user)
        {
            Result result = new Result();
            try
            {
                foreach (var item in notiObjs)
                {
                    NotifyToken obj = new NotifyToken();
                    using (SqlConnection conn = new SqlConnection(_connStr))
                    {
                        conn.Open();
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("@WhereSel", $" where u.customerno = {item.to}");
                        obj = SqlMapper.QueryFirstOrDefault<NotifyToken>(conn, "msp_NotifyToken_Retrive", parameters, commandType: StoredProcedure);
                    }

                    if (obj != null)
                    {
                        NotiData notiData = new NotiData()
                        {
                            docType = "WO",
                            docCode = item.docCode,
                            docNo = item.docNo,
                            siteNo = item.siteNo
                        };

                        DocumentHistory documentHistoryInfo = new DocumentHistory
                        {
                            AssignFrom = user.CustomerNo,
                            AssignTo = obj.CustomerNo,
                            AssignMessage = "เพื่อทราบ-" + item.body,
                            DocTypeFrom = "WO",
                            DocFrom = item.docNo,
                            Description = "Assign to",
                            CompanyNo = item.siteNo,
                            CreateBy = user.CustomerNo
                        };
                        InsertDocumentHistory(documentHistoryInfo);
                        Task<HttpResponseMessage> resultMsg = new SendNotify(_configuration).Send(item.docCode, "เพื่อทราบ-" + item.body, obj.Token, notiData);
                        InsertNotifyLog(user.CustomerNo, obj.CustomerNo, documentHistoryInfo.AssignMessage, DateTime.Now, obj.Token, resultMsg, "เพื่อทราบ");

                    }


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

#endregion

