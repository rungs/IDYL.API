using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdylAPI.Models.Master
{
    [Table("CraftType")]
    public class CraftType : BaseEntity
    {
        [Key]
        public int CraftTypeNo { get; set; }
        public string CraftTypeName { get; set; }
        public string CraftTypeCode { get; set; }
        public int? CompanyNo { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool IsDelete { get; set; }
        public decimal? Rate { get; set; }
    }
}
