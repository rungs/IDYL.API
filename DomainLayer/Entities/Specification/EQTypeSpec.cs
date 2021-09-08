using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdylAPI.Models.Specification
{
    [Table("EQType_Specification")]
    public class EqTypeSpec : BaseEntity
    {
        public int EQTypeNo { get; set; }
        public int SpecNo { get; set; }
        [ForeignKey("SpecNo")]
        public virtual Spec Specification { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
