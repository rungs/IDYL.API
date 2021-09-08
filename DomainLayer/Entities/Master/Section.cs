using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdylAPI.Models.Master
{
    [Table("Section")]
    public class Section : BaseEntity
    {
        [Key]
        public int SectionNo { get; set; }
        public string SectionName { get; set; }
        public string SectionCode { get; set; }
        [NotMapped]
        public string CompanyName { get; set; }
        public int? CompanyNo { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool IsDelete { get; set; }
    }
}
