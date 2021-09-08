using Dapper;
using Domain.Entities.PM;
using Domain.Interfaces;
using IdylAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Persistence.Contexts;
using System;
using System.Data.SqlClient;
using System.Linq;
using static System.Data.CommandType;
using System.Data;
using IdylAPI.Models.Authorize;

namespace IdylAPI.Services.Repository.Company
{
    public class PMRepository : BaseRepositoryV2<PM>, IPMRepository
    {
        private readonly AppDBContext _context;
        private readonly IConfiguration _configuration;
        private readonly string _connStr;
        public PMRepository(IConfiguration configuration, AppDBContext context) : base(context)
        {
            _context = context;
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("IDYLConnection");
        }

        public Result GetPmAllByEq(WhereParameter whereParameter)
        {
            Result result = new Result();
            try
            {
                // && (x.Pmname.Contains(whereParameter.Filter) || x.Pmcode.Contains(whereParameter.Filter))
                var obj = _entities.Where(x => x.EqhistoryNo == whereParameter.EQNo)
                .Include(i => i.FreqUnitObj)
                .Include(i => i.EqhistoryObj)
                .OrderBy(on => on.Pmcode)
                .Skip(whereParameter.StartRow - 1)
                .Take(whereParameter.EndRow - (whereParameter.StartRow - 1));
                result.Data = obj;
            }
            catch (System.Exception ex)
            {
                result.ErrMsg = ex.Message;
                result.StatusCode = 500;
                throw;
            }
            return result;
        }

        public PM GetPmByCode(string code, int companyNo)
        {
            return _entities.Where(t => t.Pmcode == code && t.CompanyNo == companyNo).FirstOrDefault();
        }

        public PM GetPmById(int pmNo)
        {
            return _entities
                .Include(t=>t.LocationHistoryObj)
                .Include(t=>t.EqhistoryObj)
                .Include(t=>t.SectionObj)
                .Include(t=>t.CustomerObj)
                .Where(t => t.Pmno == pmNo).FirstOrDefault();
        }

        public Result UpdatePm(PM PMInfo, User user)
        {
            Result result = new Result();
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                DateTime updatedDate = DateTime.Now;
                conn.Open();
             
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        DynamicParameters parameters = new DynamicParameters();
                        if (PMInfo.Pmno == 0)
                        {
                            parameters.Add("@PMNo", dbType: DbType.Int32, direction: ParameterDirection.Output);
                            parameters.Add("@PMCode", PMInfo.Pmcode);
                            parameters.Add("@PMName", PMInfo.Pmname);
                            parameters.Add("@FreqUnitNo", PMInfo.FreqUnitNo);
                            parameters.Add("@Frequency", PMInfo.Frequency);
                            parameters.Add("@EQDown", true);
                            parameters.Add("@PlantDown", true);
                            parameters.Add("@Freeze", false);
                            parameters.Add("@EQNo", PMInfo.Eqno);
                            parameters.Add("@LocationNo", PMInfo.LocationNo);
                            parameters.Add("@Duration", 0);
                            parameters.Add("@ManHours", 0);
                            parameters.Add("@DeptNo", PMInfo.SectionNo);
                            parameters.Add("@PersonNo", PMInfo.UpdatedBy);
                            parameters.Add("@Remark", null);
                            parameters.Add("@CompanyNo", PMInfo.CompanyNo);
                            parameters.Add("@CreatedBy", PMInfo.UpdatedBy);
                            parameters.Add("@EQHistoryNo", PMInfo.Eqno);
                            parameters.Add("@JobPlanNo", 0);
                            parameters.Add("@IsCopy", false);
                            parameters.Add("@PMNoCopy", 0);
                            parameters.Add("@LocationHisNo", PMInfo.LocationNo);
                            parameters.Add("@AssignGroup", null);
                            parameters.Add("@InspectionExcelConfig", null);

                            conn.Execute("sp_PM_Insert", parameters, commandType: StoredProcedure, transaction: trans);
                            PMInfo.Pmno = parameters.Get<int>("@PMNo");

                            PM newPm = conn.QueryFirstOrDefault<PM>($"select * from pm where pmno = {PMInfo.Pmno}", commandType: Text, transaction: trans);
                            PMInfo.Pmcode = newPm.Pmcode;

                            parameters = new DynamicParameters();
                            parameters.Add("@PMNo", PMInfo.Pmno);
                            parameters.Add("@PMCode", PMInfo.Pmcode);
                            parameters.Add("@PMName", PMInfo.Pmname);
                            parameters.Add("@FreqUnit", PMInfo.FreqUnitNo);
                            parameters.Add("@Freq", PMInfo.Frequency);
                            parameters.Add("@NextDue", PMInfo.NextDue_D);
                            conn.Execute("sp_PM_UpdateNextDue", parameters, commandType: StoredProcedure, transaction: trans);   
                        }
                        else
                        {
                            parameters.Add("@PMNo", PMInfo.Pmno);
                            parameters.Add("@PMCode", PMInfo.Pmcode);
                            parameters.Add("@PMName", PMInfo.Pmname);
                            parameters.Add("@FreqUnitNo", PMInfo.FreqUnitNo);
                            parameters.Add("@Frequency", PMInfo.Frequency);
                            parameters.Add("@EQDown", true);
                            parameters.Add("@PlantDown", true);
                            parameters.Add("@Freeze", false);
                            parameters.Add("@EQNo", PMInfo.Eqno);
                            parameters.Add("@LocationNo", PMInfo.LocationNo);
                            parameters.Add("@Duration", 0);
                            parameters.Add("@ManHours", 0);
                            parameters.Add("@DeptNo", PMInfo.SectionNo);
                            parameters.Add("@PersonNo", PMInfo.UpdatedBy);
                            parameters.Add("@Remark", null);
                            parameters.Add("@UpdatedBy", PMInfo.UpdatedBy);
                            parameters.Add("@EQHistoryNo", PMInfo.Eqno);
                            parameters.Add("@JobPlanNo", 0);
                            parameters.Add("@QuotePrice", 0);
                            parameters.Add("@QuoteActDuration", 0);
                            parameters.Add("@QuoteManhours", 0);
                            parameters.Add("@QuoteDesc", null);
                            parameters.Add("@LocationHisNo", PMInfo.LocationNo);
                            parameters.Add("@AssignGroup", null);
                            parameters.Add("@InspectionExcelConfig", null);
                            conn.Execute("sp_PM_Update", parameters, commandType: StoredProcedure, transaction: trans);

                            parameters = new DynamicParameters();
                            parameters.Add("@PMNo", PMInfo.Pmno);
                            parameters.Add("@PMCode", PMInfo.Pmcode);
                            parameters.Add("@PMName", PMInfo.Pmname);
                            parameters.Add("@FreqUnit", PMInfo.FreqUnitNo);
                            parameters.Add("@Freq", PMInfo.Frequency);
                            parameters.Add("@NextDue", PMInfo.NextDue_D);
                            conn.Execute("sp_PM_UpdateNextDue", parameters, commandType: StoredProcedure, transaction: trans);
                        }


                        string p_strWhereSel = string.Empty;
                        p_strWhereSel += " WHERE PMSchedHead.YearNo=" + updatedDate.Year;
                        p_strWhereSel += " AND PMSchedHead.CompanyNo=" + PMInfo.CompanyNo;
                        parameters = new DynamicParameters();
                        parameters.Add("@WhereSel", p_strWhereSel);
                        PMSchedHead pMSchedHead = SqlMapper.QueryFirstOrDefault<PMSchedHead>(conn, "sp_PMSchedHead_Retrive", parameters, commandType: StoredProcedure, transaction: trans);

                        int pmDocPlanNo = 0;
                        if (pMSchedHead == null)
                        {
                            parameters = new DynamicParameters();
                            parameters.Add("@PMDocPlanNo", pmDocPlanNo, DbType.Int32, ParameterDirection.Output);
                            parameters.Add("@YearNo", updatedDate.Year);
                            parameters.Add("@UpdateUser", PMInfo.UpdatedBy);
                            parameters.Add("@CompanyNo", PMInfo.CompanyNo);
                            conn.Execute("sp_PMSchedHead_Insert", parameters, commandType: StoredProcedure, transaction: trans);
                            pmDocPlanNo = parameters.Get<int>("@PMDocPlanNo");
                        }
                        else
                        {
                            pmDocPlanNo = pMSchedHead.PMDocPlanNo;
                        }

                        parameters = new DynamicParameters();
                        parameters.Add("@PMDocPlanNo", pmDocPlanNo);
                        parameters.Add("@PMNO", PMInfo.Pmno);
                        parameters.Add("@Year", updatedDate.Year);
                        parameters.Add("@UpdateUser", PMInfo.UpdatedBy);
                        conn.Execute("sp_PMPlan_GenNextDue", parameters, commandType: StoredProcedure, transaction: trans);

                     
                        result.StatusCode = 200;
                        result.Data = PMInfo;
                        trans.Commit();
                    }

                    catch (System.Exception ex)
                    {
                        result.StatusCode = 500;
                        result.ErrMsg = ex.Message;
                        trans.Rollback();
                    }
                }

                return result;
            }
        }
    }
}
