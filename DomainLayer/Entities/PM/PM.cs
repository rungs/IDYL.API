using IdylAPI.Models;
using IdylAPI.Models.Master;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.PM
{
    [Table("PM")]
    public class PM : BaseEntity
    {
        [Key]
        public int Pmno { get; set; }
        public string Pmcode { get; set; }
        public string Pmname { get; set; }
        public int? FreqUnitNo { get; set; }
        [ForeignKey("FreqUnitNo")]
        public virtual FreqUnit FreqUnitObj { get; set; }
        public double? Frequency { get; set; }
        public bool? Eqdown { get; set; }
        public bool? PlantDown { get; set; }
        public bool? Freeze { get; set; }
        public int? Eqno { get; set; }
        public int? LocationNo { get; set; }
        public double? Duration { get; set; }
        public double? ManHours { get; set; }
        public int? SectionNo { get; set; }
        [ForeignKey("SectionNo")]
        public virtual Section SectionObj { get; set; }
        public int? CraftNo { get; set; }
        [ForeignKey("CraftNo")]
        public virtual Customer CustomerObj { get; set; }
        public string Remark { get; set; }
        public int CompanyNo { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? Wono { get; set; }
        public string FlagWo { get; set; }
        public int? SubSetSeq { get; set; }
        public int? SchTypeNo { get; set; }
        public int? DueMethodNo { get; set; }
        public DateTime? NextDue_D { get; set; }
        public DateTime? LastDue_D { get; set; }
        public int? EqhistoryNo { get; set; }
        [ForeignKey("EqhistoryNo")]
        public virtual EQ EqhistoryObj { get; set; }
        public bool? FlagJobPlan { get; set; }
        public int? JobPlanNo { get; set; }
        public decimal? QuotePrice { get; set; }
        public double? QuoteActDuration { get; set; }
        public double? QuoteManhours { get; set; }
        public string QuoteDesc { get; set; }
        public int? LocationHisNo { get; set; }
        [ForeignKey("LocationHisNo")]
        public virtual Location LocationHistoryObj { get; set; }
        public string AssignGroup { get; set; }
        public string InspectionExcelConfig { get; set; }
    }
}
