using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class WR
    {
        //public string secretKey { get; set; }
        public string clientId { get; set; }
        public string wr_date { get; set; }
        public string wo_date { get; set; }
        public string ref_code { get; set; }
        public string work_desc { get; set; }
        public string problem_type_code { get; set; }
        public string eq_code { get; set; }
        public string location_code { get; set; }
      
    }

    public class WRResponse
    {
        public int? wr_no { get; set; }
        public string wr_code { get; set; }
    }
}
