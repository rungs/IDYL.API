using IdylAPI.Models.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdylAPI.Models.Syst
{
    [Table("_systUser")]
    public class SystUser : BaseEntity
    {
        [Key]
        public int UserNo { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int? CompanyNo { get; set; }
        public string PINCode { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public string Username { get; set; }
        public string UserFixed { get; set; }
        public string UnlockCode { get; set; }
        public bool? IsLogin { get; set; }
        public bool? IsDelete { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsSuperUser { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int UserGroupId { get; set; }
        [ForeignKey("UserGroupId")]
        public UserGroup UserGroup { get; set; }
        public bool? NotSeeAuto_AuthenLocation { get; set; }
        public DateTime? ActivateDate { get; set; }
        public bool? IsActivate { get; set; }
        [NotMapped]
        public int? PackageAmt { get; set; }
        [NotMapped]
        public string PackageUnit { get; set; }
        [NotMapped]
        public string ActivateCode { get; set; }
        [NotMapped]
        public int? No { get; set; }

        [NotMapped]
        public Customer Customer { get; set; }
        [NotMapped]
        public int DaysRemaining { get; set; }
    }

    public class CreateUser
    {
        public int NoOfUser { get; set; }
        public int UserGroupId { get; set; }
        public string ProductKey { get; set; }
        public string FromApp { get; set; }
    }

    public class ActivateUser
    {
        public DateTime? ActivateDate { get; set; }
        public int CompanyNo { get; set; }
        public int? CraftTypeNo { get; set; }
        public int? DaysRemaining { get; set; }
        public string Email { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public string FirstName { get; set; }
        public bool? IsActivate { get; set; }
        public bool? IsHeadCraft { get; set; }
        public bool? IsHeadSection { get; set; }
        public bool? IsMaintainance { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public int? SectionNo { get; set; }
        public string UserGroupName { get; set; }
        public int UserNo { get; set; }
        public string Username { get; set; }
        public int CustomerNo { get; set; }
        public string Password { get; set; }
    }

    public class UserView
    {
        public string Username { get; set; }
        public string CompanyName { get; set; }
        public string SubsiteCode { get; set; }
        public string SubsiteName { get; set; }
        public DateTime? CreatedDate { get; set; }
    }

}
