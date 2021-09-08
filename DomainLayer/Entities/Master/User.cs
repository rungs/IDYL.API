using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdylAPI.Models.Master
{
    public class SystUser
    {
        public int UserNo { get; set; }
        public string Username { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CompanyName { get; set; }
    }
}
