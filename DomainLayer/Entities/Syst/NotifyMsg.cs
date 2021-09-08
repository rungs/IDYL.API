using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdylAPI.Models.Syst
{
    [Table("NotifyMsg")]
    public class NotifyMsg : BaseEntity
    {
        [Key]
        public int NotifyNo { get; set; }
        public string Message { get; set; }
        public int? WOStatusNo { get; set; }
        public int CompanyNo { get; set; }
        public bool IsActive { get; set; }
        public string DefaultTo { get; set; }
        public string Action { get; set; }
        public string Title { get; set; }
        public string ActionName { get; set; }
        public bool IsDelete { get; set; }
        public int IndexNo { get; set; }
    }
}
