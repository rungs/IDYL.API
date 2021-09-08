using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdylAPI.Models.Syst
{
    [Table("_systMenus")]
    public partial class Menu : BaseEntity
    {
        public Menu()
        {
            //UserGroupPermissions = new HashSet<UserGroupPermission>();
        }

        [Key]
        public int MenuID { get; set; }
        public string MenuName { get; set; }
        public string LocalLang { get; set; }
        public int OrderNo { get; set; }
        public bool FlagAdminOnly { get; set; }
        public bool Need_Lock { get; set; }
        

        //public virtual ICollection<UserGroupPermission> UserGroupPermissions { get; set; }
    }
}
