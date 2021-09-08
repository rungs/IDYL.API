using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdylAPI.Models.Syst
{
    [Table("NotifyMsgDefault")]
    public class NotifyMsgDefault : BaseEntity
    {
        public int IndexNo { get; set; }
        public int RoleNo { get; set; }
        public bool? IsSystem { get; set; }

    
    }
}
