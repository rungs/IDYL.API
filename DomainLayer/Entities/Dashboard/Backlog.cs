using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdylAPI.Models.Dashboard
{
    public class Backlog
    {
        public string WOTypeCode { get; set; }
        public int BacklogCnt { get; set; }
        public int OverDueCnt { get; set; }
    }
}
