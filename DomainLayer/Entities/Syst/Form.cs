using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdylAPI.Models.Syst
{
    [Table("_systForms")]
    public partial class Form
    {
        public Form()
        {
            //UserGroupPermissions = new HashSet<UserGroupPermission>();
        }

        [Key]
        public string FormID { get; set; }
        public int MenuID { get; set; }
        public string FormName { get; set; }
        public string LocalLang { get; set; }

        [ForeignKey("MenuID")]
        public Menu Menu { get; set; }
        //public virtual ICollection<UserGroupPermission> UserGroupPermissions { get; set; }
    }
}
