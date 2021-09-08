using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdylAPI.Models.Notify
{
    public class NotifyMsg
    {
        public int NotifyNo { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }
        public string DefaultTo { get; set; }
        public bool IsActive { get; set; }
        public string RoleName { get; set; }
    }
   
}
