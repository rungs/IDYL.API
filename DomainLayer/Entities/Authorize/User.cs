using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdylAPI.Models.Authorize
{
    public class User
    {
        public int CustomerNo { get; set; }
        public int UserNo { get; set; }
        public string Username { get; set; }
        public string CustomerName { get; set; }
        public int? SectionNo { get; set; }
        public bool IsRequester { get; set; }
        public string SectionName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Token { get; set; }
        public string CompanyName_EN { get; set; }
        public string Platform { get; set; }
        public string CustomerType { get; set; }
        public int? UserGroupId { get; set; }
        public string UserGroupCode { get; set; }
        public bool IsSuperUser { get; set; }
        public string UserFixed { get; set; }
        public int? CompanyNo { get; set; }

    }
}
