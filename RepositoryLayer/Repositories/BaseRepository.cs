using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace IdylAPI.Services.Repository
{
    public class BaseRepository
    {

        public T QueryFirst<T>(string query, object parameters = null)
        {
            try
            {
                using (SqlConnection conn
                       = new SqlConnection("Your Connection String"))
                {
                    return conn.QueryFirst<T>(query, parameters);
                }
            }
            catch (Exception ex)
            {
                //Handle the exception
                return default; //Or however you want to handle the return
            }
        }

        public T QueryFirstOrDefault<T>(string query, object parameters = null)
        {
            try
            {
                using (SqlConnection conn
                       = new SqlConnection("Your Connection String"))
                {
                    return conn.QueryFirstOrDefault<T>(query, parameters);
                }
            }
            catch (Exception ex)
            {
                //Handle the exception
                return default; //Or however you want to handle the return
            }
        }

        public T QuerySingle<T>(string query, object parameters = null)
        {
            try
            {
                using (SqlConnection conn
                       = new SqlConnection("Your Connection String"))
                {
                    return conn.QuerySingle<T>(query, parameters);
                }
            }
            catch (Exception ex)
            {
                //Handle the exception
                return default; //Or however you want to handle the return
            }
        }

        public T QuerySingleOrDefault<T>(string query, object parameters = null)
        {
            try
            {
                using (SqlConnection conn
                       = new SqlConnection("Your Connection String"))
                {
                    return conn.QuerySingleOrDefault<T>(query, parameters);
                }
            }
            catch (Exception ex)
            {
                //Handle the exception
                return default; //Or however you want to handle the return
            }
        }
        //protected readonly IConfiguration _configuration;
        //protected readonly string _connStr;
        //public BaseRepository(IConfiguration configuration)
        //{
        //    _connStr = _configuration.GetConnectionString("IDYLConnection");
        //}
        //#region Helpers

        //private void ExecuteCommand(string connStr, Action<SqlConnection> task)
        //{
        //    using (var conn = new SqlConnection(connStr))
        //    {
        //        conn.Open();

        //        task(conn);
        //    }
        //}
        //private T ExecuteCommand<T>(string connStr, Func<SqlConnection, T> task)
        //{
        //    using (var conn = new SqlConnection(connStr))
        //    {
        //        conn.Open();

        //        return task(conn);
        //    }
        //}
        //#endregion
    }
}
