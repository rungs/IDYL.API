using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdylAPI.Models.Master
{
    [Table("Location")]
    public class Location : BaseEntity
    {
        [Key]
        public int LocationNo { get; set; }
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public int CompanyNo { get; set; }
       
    }
}
