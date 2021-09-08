using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdylAPI.Models.Syst
{
    [Table("_systAction")]
    public class SystAction : BaseEntity
    {
        [Key]
        public int ActionID { get; set; }
        public string ActionNameTH { get; set; }
        public string ActionNameEN { get; set; }
        public int MenuID { get; set; }
        public bool FlagAdminOnly { get; set; }
    }
}
