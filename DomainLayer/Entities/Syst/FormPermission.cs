using System.ComponentModel.DataAnnotations.Schema;

namespace IdylAPI.Models.Syst
{
    [Table("_systPermissions")]
    public class FormPermission : BaseEntity
    {
        public int UserNo { get; set; }
        public int MenuId { get; set; }
        public bool IsView { get; set; }
    }
}
