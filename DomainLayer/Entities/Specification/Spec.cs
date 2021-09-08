using IdylAPI.Models.Specification;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdylAPI.Models
{
    [Table("Specification")]
    public class Spec : BaseEntity
    {
        [Key]
        public int SpecNo { get; set; }
        public string SpecCode { get; set; }
        public string SpecName { get; set; }
        public string ValueType { get; set; }
        public string Unit { get; set; }
        public string Remark { get; set; }
        public int CompanyNo { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool isUse { get; set; }

    }
}
