using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdylAPI.Models.Syst
{
    [Table("_systPermissionsAction_Company")]
    public class SystPermissionsActionCompany : BaseEntity
    {
       
        public int CompanyNo { get; set; }
   
        public int ActionID { get; set; }
        public int MenuID { get; set; }
        public bool IsActive { get; set; }

    }
}
