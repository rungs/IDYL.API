using IdylAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities.PM
{
    [Table("PMSchedHead")]
    public class PMSchedHead : BaseEntity
    {
        public int PMDocPlanNo { get; set; }
        public string PMDocPlanCode { get; set; }
        public int PMDocStatusNo { get; set; }
        public int YearNo { get; set; }
        public int CompanyNo { get; set; }
        public bool IsFixed { get; set; }
        //public string PMPlanStatusCode { get; set; }
        //public string PMPlanStatusName { get; set; }
    }
}
