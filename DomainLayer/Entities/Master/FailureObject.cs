using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdylAPI.Models.Master
{
    [Table("FailureObject")]
    public class FailureObject : BaseEntity
    {
        [Key]
        public int ObjectNo { get; set; }
        public string ObjectType { get; set; }
        public string ObjectCode { get; set; }
        public string ObjectName { get; set; }
        public int CompanyNo { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatdBy { get; set; }
        public bool IsDelete { get; set; }
        public string Description { get; set; }
        public string CauseOfTrouble { get; set; }

        [NotMapped]
        public int? EQTypeNo { get; set; }
        [NotMapped]
        public string CompanyName { get; set; }
    }
}
