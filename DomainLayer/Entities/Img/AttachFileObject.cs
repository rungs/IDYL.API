using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdylAPI.Models.Img
{
    [Table("AttachFile")]
    public class AttachFileObject : BaseEntity
    {
        [Key]
        public int AttachNo { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public string FileType { get; set; }
        public int LinkNo { get; set; }
        public int CompanyNo { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDelete { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool? IsUrl { get; set; }
        public bool? IsInspectionFile { get; set; }
        public string Extension { get; set; }
        public int? TaskNo { get; set; }

        [NotMapped]
        public string DocName { get; set; }
        [NotMapped]
        public string UidName { get; set; }
        [NotMapped]
        public string Extenion { get; set; }
        [NotMapped]
        public string WebAddress { get; set; }
        [NotMapped]
        public string LinkCode { get; set; }
    }
}
