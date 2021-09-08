using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdylAPI.Models.Master
{
    [Table("Customer")]
    public class Customer : BaseEntity
    {
        [Key]
        public int CustomerNo { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Position { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public int CompanyNo { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDelete { get; set; }
        public bool? IsMaintainance { get; set; }
        public string CustomerCode { get; set; }
        public string CompanyName { get; set; }
        public int? SectionNo { get; set; }
        public bool? IsActive { get; set; }
        public int? IndustryTypeNo { get; set; }
        public decimal? Rate { get; set; }
        public int? CraftTypeNo { get; set; }
        public string LineToken { get; set; }
        public bool? IsSendLine { get; set; }
        public bool? IsSendEmail { get; set; }
        public bool? IsHeadSection { get; set; }
        public bool? IsHeadCraft { get; set; }
        public string IndustryType { get; set; }
        public DateTime? CreatedDate { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string CustomerName { get; private set; }

        [ForeignKey("CompanyNo")]
        public virtual Site Site { get; set; }
        public int? CreateBy { get; set; }
    }
}
