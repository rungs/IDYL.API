using IdylAPI.Models.Img;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdylAPI.Models.Master
{

    [Table("Company")]
    public class Site : BaseEntity
    {
        [Key]
        public int CompanyNo { get; set; }
        public string CompanyName_TH { get; set; }
        public string CompanyName_EN { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDelete { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        [NotMapped]
        public string CompanyName { get; set; }
        [NotMapped]
        public string Subsite { get; set; }
        public string SubsiteCode { get; set; }
        public string SubsiteName { get; set; }
        public string Platform { get; set; }
        public string CustomerType { get; set; }
        [NotMapped]
        public string ServerAddress { get; set; }
       
        public string PathLogo { get; set; }
        [NotMapped]
        public int LogoWidth { get; set; }
        [NotMapped]
        public int LogoHeight { get; set; }
        [NotMapped]
        public int LogoTop { get; set; }
        [NotMapped]
        public int LogoLeft { get; set; }
        public string Phone { get; set; }
        public string RegisterKey { get; set; }
        public string ProductKey { get; set; }
        public int? UserUnlock { get; set; }
        public string ContactName { get; set; }
        public string Email { get; set; }
        public string SerialHDD { get; set; }
        public string AuthorizeCode { get; set; }
        public string Version { get; set; }
        public double? UploadSize { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public DateTime? InstalledDate { get; set; }
        public bool? FlagSubsite { get; set; }
        public bool? FlagCooperative { get; set; }
        public int? Package { get; set; }
        public string ValidKey { get; set; }
        public string Type { get; set; }
        public string Partner { get; set; }
        public bool? IsUpdateExpired { get; set; }
        public int? LimitUser { get; set; }
        public double? AllSpace { get; set; }
        [NotMapped]
        public double FreeSpaceAll { get; set; }
        [NotMapped]
        public double? StorageUsedAll { get; set; }
        [NotMapped]
        public double ReserveSpaceAll { get; set; }
        [NotMapped]
        public double StorageUsed { get; set; }

        [NotMapped]
        public List<AttachFileObject> AttachFiles { get; set; }
        public bool? IsMainSite { get; set; }
        public int? LimitRow { get; set; }
        public string PeriodUse { get; set; }
        [NotMapped]
        public bool AddDefaultData { get; set; }


    }
}
