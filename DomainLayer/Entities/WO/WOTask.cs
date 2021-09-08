using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdylAPI.Models.WO
{
    public class WOTask
    {
        public int WODNo { get; set; }
        public int OPNo { get; set; }
        public int? EQNo { get; set; }
        public string EQCode { get; set; }
        public string EQName { get; set; }
        public int? WONo { get; set; }
        public int? ComponentNo { get; set; }
        public string Component { get; set; }
        public string ComponentCode { get; set; }
        public int? ActNo { get; set; }
        public string Action { get; set; }
        public string ActionCode { get; set; }
        public int? CauseNo { get; set; }
        public string Cause { get; set; }
        public string CauseCode { get; set; }
        public string Method { get; set; }
        public string Standard { get; set; }
        public bool IsFinish { get; set; }
        public bool IsAbNormal { get; set; }
        public string Result { get; set; }
        public int? EQType { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public int? CraftNo { get; set; }
        public decimal UnitCost { get; set; }
        public decimal Amount { get; set; }
        public decimal Qty { get; set; }
        public bool IsExternal { get; set; }
        public decimal MH { get; set; }
        public int? CraftTypeNo { get; set; }
        public int? HeadCraftTypeNo { get; set; }
        public bool IsCurrent { get; set; }
        public DateTime? AcknowledgeDate { get; set; }
        public bool IsGenWO { get; set; }
        public int? GenWONo { get; set; }
    }
}
