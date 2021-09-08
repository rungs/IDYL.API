using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdylAPI.Models.Master
{
    [Table("ProblemType")]
    public class ProblemType : BaseEntity
    {
        [Key]
        public int ProblemTypeNo { get; set; }
        public string ProblemTypeCode { get; set; }
        public string ProblemTypeName { get; set; }
        public int SectionNo { get; set; }
        public int CompanyNo { get; set; }
        public bool IsDelete { get; set; }
        [ForeignKey("SectionNo")]
        public virtual Section Section { get; set; }
        [NotMapped]
        public string SectionCode { get; set; }
        [NotMapped]
        public string SectionName { get; set; }
    }
}
