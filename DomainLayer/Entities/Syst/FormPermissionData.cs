using System.ComponentModel.DataAnnotations.Schema;

namespace IdylAPI.Models.Syst
{
    [Table("_systPermissionsData")]
    public class FormPermissionData : BaseEntity
    {
        public int UserId { get; set; }
        public int MenuId { get; set; }
        public string Type { get; set; }
    }
}
