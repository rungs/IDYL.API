using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdylAPI.Models.Syst
{
    [Table("_systUserGroup_Permission")]
    public class UserGroupPermission
    {
        public int UserGroupNo { get; set; }
        public string FormId { get; set; }
        public bool IsView { get; set; }
        public bool IsNew { get; set; }
        public bool IsEdit { get; set; }
        public bool IsDelete { get; set; }
        public bool IsDisable { get; set; }

        [ForeignKey("UserGroupNo")]
        public UserGroup UserGroup { get; set; }
        [ForeignKey("FormId")]
        public Form Form { get; set; }

    }
}
