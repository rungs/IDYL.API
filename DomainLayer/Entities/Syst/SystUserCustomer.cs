using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdylAPI.Models.Syst
{
    [Table("_systUser_Customer")]
    public class SystUserCustomer : BaseEntity
    {
        public int UserNo { get; set; }
        public int CustomerNo { get; set; }
    }
}
