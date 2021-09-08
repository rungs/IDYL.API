using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdylAPI.Models.Syst
{
    [Table("Sys_Parameter1")]
    public class SysParameter1: BaseEntity
    {
        [Key]
        public string PARANO { get; set; }
        public short PARAGROUP { get; set; }
        public string PARANAME { get; set; }
        public string MODUL_ { get; set; }
        public string FLAGSYS { get; set; }
        public string PREFIX { get; set; }
        public string UseYear { get; set; }       
        public string UseMonth { get; set; }
        public string SeparetChar { get; set; }
        public string YearNo { get; set; }
        public int LAST_RUNNO { get; set; }
        public short ORDINAL_RUNNO { get; set; }
        public string DEFATABLE { get; set; }
        public string DEFAVALUE { get; set; }
        public string IsDocCodeRunning { get; set; }
        public int SizeOfField { get; set; }
        public string Doc_Format { get; set; }
        public string Doc_LastNo { get; set; }
        public string UseZeroBeforeNo { get; set; }
        public int CompanyNo { get; set; }
    }
}
