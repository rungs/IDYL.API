using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdylAPI.Models.WO
{
    public class WOResource : Base
    {
        public int WOResourceNo { get; set; }
        public DateTime? DocDate { get; set; }
        public int RescNo { get; set; }
        public string ResTypeCode { get; set; }
        public string RescCode { get; set; }
        public string RescName { get; set; }
        public decimal PlnQtyMH { get; set; }
        public decimal QtyMH { get; set; }
        public decimal? UnitCost { get; set; }
        public decimal? Amount { get; set; }
        public int WONo { get; set; }
        public int WODNo { get; set; }
        public int EQNo { get; set; }
        public string DocNo { get; set; }
        public string Manuf { get; set; }
        public string Model { get; set; }
        public string ResCode { get; set; }
        public bool IsExternal { get; set; }
        public DateTime? WarrantyDate { get; set; }
        public int? VendorNo { get; set; }
        public string Type { get; set; }
        public string MHType { get; set; }
        public int? Qty { get; set; }
        public int? MH { get; set; }
        public int? CraftTypeNo { get; set; }
        public string CraftTypeName { get; set; }
    }
}
