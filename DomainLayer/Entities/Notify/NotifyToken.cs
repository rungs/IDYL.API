using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdylAPI.Models.Notify
{
    public class NotifyToken
    {
        public int UserNo { get; set; }
        public string Token { get; set; }
        public int CustomerNo { get; set; }
        public string FirstName { get; set; }
        public string RoleName { get; set; }
    }
}
