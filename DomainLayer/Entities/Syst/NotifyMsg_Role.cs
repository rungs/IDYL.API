using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdylAPI.Models.Syst
{
    [Table("NotifyMsg_Role")]
    public class NotifyMsg_Role : BaseEntity
    {
        [Key]
        public int NotifyMsgNo { get; set; }
        [Key]
        public int RoleNo { get; set; }
        public bool? IsSystem { get; set; }
    }
}
