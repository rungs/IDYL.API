using Dapper;
using IdylAPI.Models;
using IdylAPI.Models.Master;
using IdylAPI.Services.Interfaces.Master;
using Microsoft.Extensions.Configuration;
using PAUtility;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using static System.Data.CommandType;
namespace IdylAPI.Services.Repository.Master
{
    public class PersonRepository : IPersonRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connStr;
        public PersonRepository(IConfiguration configuration)
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
                    string condition = $" where customer.companyno = {whereParameter.SiteNo}";
                    condition += $" and customer.isdelete = 0 ";
                    if (!string.IsNullOrEmpty(InputVal.ToString(whereParameter.Filter)))
                    {
                        condition += $" and(customer.customername like '%{whereParameter.Filter}%'";
                        condition += $" or section.sectioncode like '%{whereParameter.Filter}%'";
                        condition += $" or section.sectionname like '%{whereParameter.Filter}%'";
                        condition += $" or crafttype.crafttypename like '%{whereParameter.Filter}%'";
                        condition += $" or customer.customercode like '%{whereParameter.Filter}%')";
                    }

                    if (whereParameter.IsMaintainance)
                    {
                        condition += $" and customer.IsMaintainance= 1";
                    }

                    parameters.Add("@WhereSel", condition);
                    parameters.Add("@StartRow", whereParameter.StartRow);
                    parameters.Add("@EndRow", whereParameter.EndRow);
                    parameters.Add("@WONo", whereParameter.WONo);
                    parameters.Add("@Type", whereParameter.DataType);

                    IEnumerable<Person> people = SqlMapper.Query<Person>(conn, "msp_Person_Retrive", parameters, commandType: StoredProcedure);
                    result.Data = people;
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
