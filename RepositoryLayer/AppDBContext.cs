using Domain.Entities.PM;
using Domain.Entities.Syst;
using Domain.Interfaces;
using IdylAPI.Models;
using IdylAPI.Models.Img;
using IdylAPI.Models.Master;
using IdylAPI.Models.Specification;
using IdylAPI.Models.Syst;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using SystUser = IdylAPI.Models.Syst.SystUser;

namespace Persistence.Contexts
{
    public partial class AppDBContext : DbContext, IApplicationDbContext, IDisposable
    {
     
        public AppDBContext(DbContextOptions<AppDBContext> options): base(options)
        {
         
        }

        public virtual DbSet<Form> Form { get; set; }
        public virtual DbSet<UserGroup> UserGroup { get; set; }
        public virtual DbSet<UserGroupPermission> UserGroupPermission { get; set; }
        public virtual DbSet<Site> Site { get; set; }
        public virtual DbSet<SysParameter1> SysParameter1 { get; set; }
        public virtual DbSet<Section> Section { get; set; }
        public virtual DbSet<SystUser> SystUser { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<FormPermission> FormPermission { get; set; }
        public virtual DbSet<FormPermissionData> FormPermissionData { get; set; }
        public virtual DbSet<FormPermissionAction> FormPermissionAction { get; set; }
        public virtual DbSet<SysConfig> SysConfig { get; set; }
        public virtual DbSet<NotifyMsg> NotifyMsg { get; set; }
        public virtual DbSet<NotifyMsg_Role> NotifyMsg_Role { get; set; }
        public virtual DbSet<NotifyMsgDefault> NotifyMsgDefault { get; set; }
        public virtual DbSet<SystPermissionsActionCompany> SystPermissionsActionCompany { get; set; }
        public virtual DbSet<SystUserCustomer> SystUserCustomer { get; set; }
        public virtual DbSet<SystAction> SystAction { get; set; }
        public virtual DbSet<Menu> SystMenu { get; set; }
        public virtual DbSet<UserGroupMenu> UserGroupMenu { get; set; }
        public virtual DbSet<Spec> Specification { get; set; }
        public virtual DbSet<EqTypeSpec> EQTypeSpec { get; set; }
        public virtual DbSet<EQSpec> EQSpec { get; set; }
        public virtual DbSet<EQ> EQ { get; set; }
        public virtual DbSet<EQType> EQType { get; set; }
        public virtual DbSet<AttachFileObject> AttachFile { get; set; }
        public virtual DbSet<SystUserLocation> SystUserLocation { get; set; }
        public virtual DbSet<Location> Location { get; set; }
        public virtual DbSet<CraftType> CraftType { get; set; }
        public virtual DbSet<LogInHistory> LogInHistory { get; set; }
        public virtual DbSet<PM> PM { get; set; }
        public virtual DbSet<FreqUnit> FreqUnit { get; set; }
        public virtual DbSet<ProblemType> ProblemType { get; set; }
        public virtual DbSet<FailureObject> FailureObject { get; set; }
     
        public IDbConnection Connection => Database.GetDbConnection();

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer("Name=Dapper");
        //    }
        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserGroupPermission>(
                entity =>
                {
                    entity.HasKey(i => new { i.UserGroupNo, i.FormId });
                });
            modelBuilder.Entity<FormPermission>(
                entity =>
                {
                    entity.HasKey(i => new { i.MenuId, i.UserNo });
                });
            modelBuilder.Entity<FormPermissionData>(
              entity =>
              {
                  entity.HasKey(i => new { i.MenuId, i.UserId });
              });
            modelBuilder.Entity<FormPermissionAction>(
              entity =>
              {
                  entity.HasKey(i => new { i.UserNo, i.ActionId, i.MenuId });
              });
            modelBuilder.Entity<SysConfig>(
               entity =>
               {
                   entity.HasKey(i => new { i.ConfigType, i.ConfigName, i.CompanyNo });
               });
            modelBuilder.Entity<SystPermissionsActionCompany>(
             entity =>
             {
                 entity.HasKey(i => new { i.CompanyNo, i.ActionID });
             });
            modelBuilder.Entity<SystUserCustomer>(
             entity =>
             {
                 entity.HasKey(i => new { i.UserNo, i.CustomerNo });
             });
            modelBuilder.Entity<UserGroupMenu>(
             entity =>
             {
                 entity.HasKey(i => new { i.UserGroupNo, i.MenuId });
             });
            modelBuilder.Entity<EqTypeSpec>(
               entity =>
               {
                   entity.HasKey(i => new { i.EQTypeNo, i.SpecNo });
               });
            modelBuilder.Entity<EQSpec>(
             entity =>
             {
                 entity.HasKey(i => new { i.EQNo, i.SpecNo });
             });

            modelBuilder.Entity<SystUserLocation>(
            entity =>
            {
                entity.HasKey(i => new { i.LocationNo, i.UserNo });
            });
            modelBuilder.Entity<LogInHistory>(
            entity =>
            {
                entity.HasNoKey();
            });
            modelBuilder.Entity<NotifyMsg_Role>(
            entity =>
            {
                entity.HasKey(i => new { i.NotifyMsgNo, i.RoleNo });
            });
            modelBuilder.Entity<NotifyMsgDefault>(
            entity =>
            {
                entity.HasNoKey();
            });
        }

    }
  
}
