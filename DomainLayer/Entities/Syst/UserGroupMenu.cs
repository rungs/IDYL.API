using System.ComponentModel.DataAnnotations.Schema;

namespace IdylAPI.Models.Syst
{
    [Table("_systUserGroup_Menu")]
    public class UserGroupMenu : BaseEntity
    {
        public int UserGroupNo { get; set; }
        public int MenuId { get; set; }
        public bool IsView { get; set; }
      
        [ForeignKey("UserGroupNo")]
        public UserGroup UserGroup { get; set; }
        [ForeignKey("MenuId")]
        public Menu Menu { get; set; }

    }
}
