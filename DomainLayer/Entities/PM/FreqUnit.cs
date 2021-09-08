using IdylAPI.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.PM
{
    [Table("FreqUnit")]
    public  class FreqUnit : BaseEntity
    {
        [Key]
        public int FreqUnitNo { get; set; }
        public string FreqUnitCode { get; set; }
        public string FreqUnitName { get; set; }
        public int? FreqCnv { get; set; }
        public string FreqUnitCnv { get; set; }
        public int? FreqUnitConvNo { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
