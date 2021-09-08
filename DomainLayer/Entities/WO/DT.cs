using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdylAPI.Models.WO
{
    public class DT: Base
    {
        public int DTNo { get; set; }
        public DateTime? DTDate { get; set; }
        public int WONo { get; set; }
        public bool EQDown { get; set; }
        public bool PlantDown { get; set; }
        public float DownTime { get; set; }
        public int EQNo { get; set; }
        public int LocationNo { get; set; }
        public decimal LossAmount { get; set; }
        public int LossQty { get; set; }
        public DateTime? DateTimeStop { get; set; }
        public DateTime? DateTimeStopEnd { get; set; }
    }
}
