using System.ComponentModel.DataAnnotations.Schema;

namespace IdylAPI.Models.Syst
{
    [Table("_systPermissionsAction")]
    public class FormPermissionAction : BaseEntity
    {
        public int UserNo { get; set; }
        public int ActionId { get; set; }
        public int MenuId { get; set; }
        public bool IsActive { get; set; }
    }
}
