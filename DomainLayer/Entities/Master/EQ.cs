using IdylAPI.Models.Img;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdylAPI.Models.Master
{
    [Table("EQ")]
    public class EQ : BaseEntity
    {
        [Key]
        public int EQNo { get; set; }
        public string EQCode { get; set; }
        public string EQName { get; set; }
        public int Location { get; set; }
        [ForeignKey("Location")]
        public virtual Location LocationObj { get; set; }
        public int EQType { get; set; }
        [ForeignKey("EQType")]
        public virtual EQType EQTypeObj { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string SerialNo { get; set; }
        public string Capacity { get; set; }
        [NotMapped]
        public string LocationCode { get; set; }
        [NotMapped]
        public string LocationName { get; set; }
        [NotMapped]
        public string EQTypeCode { get; set; }
        [NotMapped]
        public string EQTypeName { get; set; }
        public string Criticality { get; set; }
        public int? VendorNo { get; set; }
        [NotMapped]
        public string VendorCode { get; set; }
        [NotMapped]
        public string VendorName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Remark { get; set; }
        public string Img { get; set; }
        [NotMapped]
        public int InProgressCnt { get; set; }
        [NotMapped]
        public int HistoryCnt { get; set; }
        public DateTime? InstalledDate { get; set; }
        public DateTime? WarrantyDate { get; set; }
        [NotMapped]
        public AttachFileObject[] Attachment { get; set; }
        [NotMapped]
        public List<string> AttachmentObj { get; set; }
        [NotMapped]
        public IEnumerable<AttachFileObject> InspectFiles { get; set; }
        [NotMapped]
        public IEnumerable<MA> mAs { get; set; }
        [NotMapped]
        public IEnumerable<PartUsage> partUsages{ get; set; }
        public DateTime? LastPMDate { get; set; }
        [NotMapped]
        public string LastPMWorkOrderCode { get; set; }
        public int CompanyNo { get; set; }
        [NotMapped]
        public string CompanyName { get; set; }
        public bool IsDelete { get; set; }

    }

    public class MA
    {
        public int Items { get; set; }
        public int WONo { get; set; }
        public string WOCode { get; set; }
        public string WOStatusName { get; set; }
        public DateTime? WODate { get; set; }
        public DateTime? WRDate { get; set; }
        public string WorkDesc { get; set; }
        public string RequesterName { get; set; }
        public string SectionReq { get; set; }
        public DateTime? PlnDate { get; set; }
        public decimal PlnDuration { get; set; }
        public decimal PlnManHours { get; set; }
        public string WOTypeCode { get; set; }
        public string WOAction { get; set; }
        public string WOCause { get; set; }
        public string ResponseName { get; set; }
        public string SectionRes { get; set; }
        public DateTime? ActDate { get; set; }
        public decimal ActDuration { get; set; }
        public decimal ActManHours { get; set; }
        public decimal Meter { get; set; }
        public decimal CostStock { get; set; }
        public decimal CostDirectPurchase { get; set; }
        public decimal CostTools { get; set; }
        public decimal CostOutsources { get; set; }
        public decimal CostCrafts { get; set; }
        public decimal CostTotal { get; set; }
        public string WRCode { get; set; }
        public string EQCode { get; set; }
        public string EQName { get; set; }
        public string EQTypeName { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string SerialNo { get; set; }
        public string Remark { get; set; }
        public string Criticality { get; set; }
        public decimal DownTime { get; set; }
        public DateTime? DateTimeStop { get; set; }
        public string Component { get; set; }
        public string ProCode { get; set; }
        public string Problem { get; set; }
        public string CauseCode{ get; set; }
        public string Cause { get; set; }
        public string ActionCode{ get; set; }
        public string Action { get; set; }
        public string ProblemType { get; set; }
    }
    public class PartUsage
    {
        public int WOResourceNo { get; set; }
        public DateTime? TRDATE { get; set; }
        public decimal METERREAD { get; set; }
        public string WOTYPE_NO { get; set; }
        public int EQNo { get; set; }
        public string EQCode { get; set; }
        public string EQName { get; set; }
        public string EQTYPE { get; set; }
        public string MANUF { get; set; }
        public string MODEL { get; set; }
        public string RESCTYPE { get; set; }
        public string RESCCODE { get; set; }
        public string RESCNAME { get; set; }
        public string WOCODE { get; set; }
        public int WONO { get; set; }
        public decimal QTY { get; set; }
        public string WOStatusName { get; set; }
        public string WorkDesc { get; set; }
        public string WOAction { get; set; }
        public string WOCause { get; set; }
        public string Component { get; set; }
        public string MaintenanceSection { get; set; }
        public string ProCode { get; set; }
        public string CauseCode { get; set; }
        public string ActionCode { get; set; }
        public string ProblemType { get; set; }
        public DateTime? ActDate { get; set; }
    }
}
