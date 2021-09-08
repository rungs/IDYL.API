using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdylAPI.Models.WO
{
    public class FM
    {
        public int FMNo { get; set; }
        public DateTime Date { get; set; }
        public decimal MeterRead { get; set; }
        public int? WOTypeNo { get; set; }
        public int? EQNo { get; set; }
        public string EQCode { get; set; }
        public string EQName { get; set; }
        public int? LocationNo { get; set; }
        public int? EQTypeNo { get; set; }
        public string Manuf { get; set; }
        public string Model { get; set; }
        public int? ProblemNo { get; set; }
        public object Problem { get; set; }
        public string ProblemCode { get; set; }
        public string ProblemName { get; set; }
        public string ProblemDesc { get; set; }
        public int? ComponentNo { get; set; }
        public object Component { get; set; }
        public string ComponentCode { get; set; }
        public string ComponentName { get; set; }
        public string ComponentDesc { get; set; }
        public int? CauseNo { get; set; }
        public object Cause { get; set; }
        public string CauseCode { get; set; }
        public string CauseName { get; set; }
        public string CauseDesc { get; set; }
        public int? ActionNo { get; set; }
        public object Action { get; set; }
        public string ActionCode { get; set; }
        public string ActionName { get; set; }
        public string ActionDesc { get; set; }
        public int WONo { get; set; }
        public int OPNo { get; set; }
        public bool IsInsert { get; set; }
        public int CompanyNo { get; set; }
    }
}
