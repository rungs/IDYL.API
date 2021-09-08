using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdylAPI.Models.Syst
{
    [Table("_sysConfig")]
    public class SysConfig : BaseEntity
    {
      
        public string ConfigType { get; set; }
      
        public string ConfigName { get; set; }
       
        public string ConfigValue { get; set; }
      
        public int CompanyNo { get; set; }
        
        
    }
}
