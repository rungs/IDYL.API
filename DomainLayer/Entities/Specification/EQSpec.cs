using IdylAPI.Models.Master;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdylAPI.Models.Specification
{
    [Table("EQ_Specification")]
    public class EQSpec : BaseEntity
    {
        public int EQNo { get; set; }
        [ForeignKey("EQNo")]
        public virtual EQ EQ { get; set; }
        public int SpecNo { get; set; }
        [ForeignKey("SpecNo")]
        public virtual Spec Specification { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Value { get; set; }
    }
}
