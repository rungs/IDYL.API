using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdylAPI.Models
{
    public class Result
    {
        public int StatusCode { get; set; }
        public object Data { get; set; }
        public string ErrMsg { get; set; }
        public int TotalCount { get; set; }

        public Result()
        {
            StatusCode = 200;
            Data = null;
            ErrMsg = "";
            TotalCount = 0;
        }

        public void SetResult(int statusCode, object data, string errMsg, int totalCount, out Result result)
        {
            result = new Result()
            {
                StatusCode = statusCode,
                Data = data,
                ErrMsg = errMsg,
                TotalCount = totalCount
            };
        }
    }
}
