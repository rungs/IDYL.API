using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdylAPI.Models.Master
{
    [Table("EQType")]
    public class EQType
    {
        [Key]
        public int EQTypeNo { get; set; }
        public string EQTypeCode { get; set; }
        public string EQTypeName { get; set; }
        public int CompanyNo { get; set; }

    }
}
