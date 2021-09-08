using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdylAPI.Models.Syst
{
    [Table("_systUserGroup")]
    public partial class UserGroup : BaseEntity
    {
        public UserGroup()
        {
           //UserGroupPermissions = new HashSet<UserGroupPermission>();
        }
        [Key]
        public int UserGroupNo { get; set; }
        public string UserGroupCode { get; set; }
        public string UserGroupName { get; set; }
        public bool UseWeb { get; set; }
        public bool UseMobile { get; set; }

        //public virtual ICollection<UserGroupPermission> UserGroupPermissions { get; set; }
    }
}

   
