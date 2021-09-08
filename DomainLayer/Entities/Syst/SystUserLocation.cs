using IdylAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Syst
{
    [Table("_systUser_Location")]
    public class SystUserLocation : BaseEntity
    {
        public int LocationNo { get; set; }
        public int UserNo { get; set; }
    }
}
