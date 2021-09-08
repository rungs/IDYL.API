using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdylAPI.Models.Master
{
    public class Resource : Base
    {
        public int RescNo { get; set; }
        public string RescType { get; set; }
        public string RescCode { get; set; }
        public string RescName { get; set; }
        public string Manuf { get; set; }
        public string Model { get; set; }
        public int? EQTypeNo { get; set; }
        public bool StockItem { get; set; }
        public string Unit { get; set; }
        public decimal UnitCost { get; set; }
        public decimal Amount { get; set; }
        public string MatGroup { get; set; }
        public string Location { get; set; }
        public decimal Qonhand { get; set; }
        public decimal Qpending { get; set; }
        public decimal CostEstimate { get; set; }
        public decimal QtyMin { get; set; }
        public decimal QtyMax { get; set; }
        public int WONo { get; set; }
        public string DocType { get; set; }
    }
}
