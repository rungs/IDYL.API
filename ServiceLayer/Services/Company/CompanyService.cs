using Domain.Entities.Syst;
using Domain.Interfaces;
using IdylAPI.Models;
using IdylAPI.Models.Authorize;
using IdylAPI.Models.Img;
using IdylAPI.Models.Master;
using IdylAPI.Models.Syst;
using IdylAPI.Services.Interfaces.Company;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using PAUtility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SystUser = IdylAPI.Models.Syst.SystUser;

namespace SocialMedia.Core.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _host;

        public CompanyService(IUnitOfWork unitOfWork, IConfiguration configuration, IHostingEnvironment host)
        {
            _unitOfWork = unitOfWork;
            _host = host;
            _configuration = configuration;
        }

        public async Task<Site> GetCompanyById(int id)
        {
            return await _unitOfWork.CompanyRepository.GetById(id);
        }

        public IEnumerable<Site> GetCompanyByUserId(int userId)
        {

            IEnumerable<Site> sites = _unitOfWork.CompanyRepository.GetCompanyByUserId(userId);

            long size = 0;
            double allReserve = 0;
            double allUsed = 0;
            double allSpace = 0;

            string pPath = _host.ContentRootPath;
            int pathLevel = InputVal.ToInt(_configuration["PathLevel"]);
            for (int j = 0; j <= pathLevel; j++)
            {
                pPath = Directory.GetParent(pPath).ToString();
            }

            pPath += _configuration["AttPath"];
            pPath = Directory.GetParent(pPath).ToString();
            //result.Data = sites;
            foreach (Site item in sites)
            {
                size = 0;
                allReserve += item.UploadSize.Value;
                allSpace += item.AllSpace.HasValue ? item.AllSpace.Value : 0;
                if (item.AttachFiles.Count > 0)
                {
                    foreach (AttachFileObject attach in item.AttachFiles)
                    {
                        try
                        {
                            string filePath = $"{pPath}/{ attach.Path}";
                            FileInfo fi = new FileInfo(filePath.Replace("/", @"\"));
                            size += fi.Length;
                        }
                        catch (Exception)
                        {
                            size += 0;
                        }
                    }
                }
                item.StorageUsed = ConvertBytesToMegabytes(size);
                allUsed += item.StorageUsed;
                item.AttachFiles = null;
            }
            if (sites.ToList().Count > 0)
            {
                sites.ToList()[0].StorageUsedAll = allUsed;
                sites.ToList()[0].ReserveSpaceAll = allReserve;
                sites.ToList()[0].FreeSpaceAll = allSpace - allReserve;
            }

            return sites;



        }

        private double ConvertBytesToMegabytes(long bytes)
        {
            return (bytes / 1024f) / 1024f;
        }

        public IEnumerable<Site> GetCompanyies()
        {
            return _unitOfWork.CompanyRepository.GetAll();
        }

        public async Task<Site> InsertCompany(Site site)
        {
            try
            {
                DateTime updatedDate = DateTime.Now;
                int updatedBy = -99;
                _unitOfWork.BeginTransaction();
                int companyNo = 57;
                Site demoSite = await _unitOfWork.CompanyRepository.GetById(companyNo);

                site.CompanyName_EN = site.CompanyName_TH;
                site.IsDelete = false;
                site.CreatedBy = updatedBy;
                site.CreatedDate = updatedDate;
                site.FlagCooperative = false;
                site.Version = demoSite.Version;
                site.Package = site.Package.HasValue ? site.Package.Value : 0;
                site.IsUpdateExpired = site.IsUpdateExpired.HasValue ? site.IsUpdateExpired.Value : false;
                site.UploadSize = site.UploadSize.HasValue ? site.UploadSize.Value : 500;
                site.SubsiteCode = string.IsNullOrEmpty(site.SubsiteCode) ? "Main" : site.SubsiteCode;
                site.SubsiteName = string.IsNullOrEmpty(site.SubsiteName) ? "Main" : site.SubsiteName;
                site.LimitUser = site.LimitUser.HasValue ? site.LimitUser.Value : site.UserUnlock;
                site.AllSpace = site.AllSpace.HasValue ? site.AllSpace.Value : site.UploadSize;
                site.IsMainSite = true;


                Random generator = new Random();
                string validKey = generator.Next(0, 100000).ToString("D5");

                site.ValidKey = validKey;
                await _unitOfWork.CompanyRepository.Add(site);
                await _unitOfWork.SaveChangesAsync();

                int newCompanyNo = site.CompanyNo;

                //int demoSection = 227;


                IEnumerable<SysParameter1> sysParameter1List = await _unitOfWork.SysParameter1Repository.GetSysParameterByCompany(companyNo);
                foreach (var sysParameter1 in sysParameter1List)
                {
                    sysParameter1.CompanyNo = newCompanyNo;
                    sysParameter1.YearNo = ((DateTime.Now.Year % 100) + 1).ToString();
                    sysParameter1.LAST_RUNNO = 0;
                    sysParameter1.Doc_LastNo = $"{sysParameter1.PREFIX}-000000";

                    await _unitOfWork.SysParameter1Repository.Add(sysParameter1);
                }
                await _unitOfWork.SaveChangesAsync();

                Section section = new Section()
                {
                    SectionCode = "MNT",
                    SectionName = "ซ่อมบำรุง",
                    CompanyNo = newCompanyNo,
                    IsDelete = false,
                    CreateDate = updatedDate,
                    CreatedBy = updatedBy
                };
                await _unitOfWork.SectionRepository.Add(section);
                await _unitOfWork.SaveChangesAsync();
                int newSectionNo = section.SectionNo;

                for (int i = 0; i < site.UserUnlock; i++)
                {

                    await _unitOfWork.SysParameter1Repository.GenCodeFormat(0, "SYS9998");
                    SysParameter1 sysParameter1 = await _unitOfWork.SysParameter1Repository.GetSysParameterByParaAndCompany(0, "SYS9998");
                    string uDemo = sysParameter1.Doc_LastNo;

                    Customer customer = new Customer()
                    {
                        CompanyNo = newCompanyNo,
                        Firstname = uDemo,
                        Lastname = "Lastname",
                        IsMaintainance = false,
                        CompanyName = site.CompanyName_TH,
                        SectionNo = newSectionNo,
                        Rate = 300,
                        CustomerCode = sysParameter1.Doc_LastNo,
                        CreatedDate = updatedDate,
                        UpdatedDate = updatedDate,
                        IsActive = true,
                        IsDelete = false,
                    };
                    await _unitOfWork.CustomerRepository.Add(customer);
                    await _unitOfWork.SaveChangesAsync();

                    await _unitOfWork.SysParameter1Repository.GenCodeFormat(0, "SYS9999");
                    SysParameter1 userParameter = await _unitOfWork.SysParameter1Repository.GetSysParameterByParaAndCompany(0, "SYS9999");
                    SystUser user = new SystUser()
                    {
                        Password = "cfcd208495d565ef66e7dff9f98764da",
                        CompanyNo = newCompanyNo,
                        PINCode = "cfcd208495d565ef66e7dff9f98764da",
                        ExpiredDate = site.ExpiredDate ?? updatedDate.AddDays(30),
                        Username = userParameter.Doc_LastNo,
                        UserFixed = userParameter.Doc_LastNo,
                        UnlockCode = "1",
                        IsLogin = false,
                        IsDelete = false,
                        IsActive = true,
                        CreatedDate = updatedDate,
                        UpdatedDate = updatedDate,
                        IsSuperUser = site.FlagSubsite.HasValue && site.FlagSubsite.Value && i == 0,
                        UserGroupId = 1,
                        NotSeeAuto_AuthenLocation = false,
                        IsActivate = i == 0,
                        ActivateDate = updatedDate
                    };

                    await _unitOfWork.SystUserRepository.Add(user);
                    await _unitOfWork.SaveChangesAsync();

                    SystUser systUser = await _unitOfWork.SystUserRepository.GetById(user.UserNo);
                    systUser.UnlockCode = (user.UserNo * (user.UserNo + Convert.ToInt32(userParameter.Doc_LastNo.Replace("U", ""))) * 3.5).ToString();
                    _unitOfWork.SystUserRepository.Update(systUser);

                    IEnumerable<Menu> menus = _unitOfWork.SystMenuRepository.GetAll();
                    foreach (var item in menus)
                    {
                        FormPermission formPermission = new FormPermission()
                        {
                            UserNo = user.UserNo,
                            MenuId = item.MenuID,
                            IsView = i == 0 ? true : !item.FlagAdminOnly
                        };
                        await _unitOfWork.SystPermissionsRepository.Add(formPermission);
                    }

                    IEnumerable<SystAction> systActions = _unitOfWork.SystActionRepository.GetAll();
                    foreach (var item in systActions)
                    {
                        FormPermissionAction formPermission = new FormPermissionAction()
                        {
                            UserNo = user.UserNo,
                            MenuId = item.MenuID,
                            ActionId = item.ActionID,
                            IsActive = i == 0 ? true : !item.FlagAdminOnly
                        };
                        await _unitOfWork.SystPermissionsActionRepository.Add(formPermission);
                    }

                    FormPermissionData formPermissionData = new FormPermissionData()
                    {
                        UserId = user.UserNo,
                        MenuId = 13,
                        Type = "A"
                    };
                    await _unitOfWork.SystPermissionsDataRepository.Add(formPermissionData);
                    formPermissionData = new FormPermissionData()
                    {
                        UserId = user.UserNo,
                        MenuId = 16,
                        Type = "A"
                    };
                    await _unitOfWork.SystPermissionsDataRepository.Add(formPermissionData);
                    formPermissionData = new FormPermissionData()
                    {
                        UserId = user.UserNo,
                        MenuId = 22,
                        Type = "A"
                    };
                    await _unitOfWork.SystPermissionsDataRepository.Add(formPermissionData);

                    SystUserCustomer systUserCustomer = new SystUserCustomer()
                    {
                        UserNo = user.UserNo,
                        CustomerNo = customer.CustomerNo
                    };

                    await _unitOfWork.SystUserCustomerRepository.Add(systUserCustomer);
                    await _unitOfWork.SaveChangesAsync();

                    #region Create User To Demo Subsite PA
                    bool IsAddDemo = InputVal.ToBool(_configuration["IsAddDemo"]);
                    if (IsAddDemo)
                    {
                        SystUserCustomer systUserCustomerDemo = new SystUserCustomer()
                        {
                            UserNo = user.UserNo,
                            CustomerNo = 6033
                        };

                        await _unitOfWork.SystUserCustomerRepository.Add(systUserCustomerDemo);
                        await _unitOfWork.SaveChangesAsync();

                        IEnumerable<Location> locations = _unitOfWork.LocationRepository.GetByCompany(companyNo);
                        foreach (var item in locations)
                        {
                            SystUserLocation systUserLocation = new SystUserLocation()
                            {
                                UserNo = user.UserNo,
                                LocationNo = item.LocationNo,
                            };
                            await _unitOfWork.SystUserLocationRepository.Add(systUserLocation);
                        }
                        await _unitOfWork.SaveChangesAsync();
                    }

                    #endregion
                }


                SysConfig sysConfig = new SysConfig()
                {
                    ConfigName = "DIGIT",
                    ConfigType = "SRRUNNING",
                    ConfigValue = "3",
                    CompanyNo = newCompanyNo
                };
                await _unitOfWork.SystConfigRepository.Add(sysConfig);
                sysConfig = new SysConfig()
                {
                    ConfigName = "SYSTEM",
                    ConfigType = "TABLECAPTION",
                    ConfigValue = "System",
                    CompanyNo = newCompanyNo
                };
                await _unitOfWork.SystConfigRepository.Add(sysConfig);
                sysConfig = new SysConfig()
                {
                    ConfigName = "ACTIVE",
                    ConfigType = "GENPMAUTO",
                    ConfigValue = "Y",
                    CompanyNo = newCompanyNo
                };
                await _unitOfWork.SystConfigRepository.Add(sysConfig);
                sysConfig = new SysConfig()
                {
                    ConfigName = "PREVDAYS",
                    ConfigType = "GENPMAUTO",
                    ConfigValue = "1",
                    CompanyNo = newCompanyNo
                };
                await _unitOfWork.SystConfigRepository.Add(sysConfig);
                SystPermissionsActionCompany systPermissionsActionCompany = new SystPermissionsActionCompany()
                {
                    CompanyNo = newCompanyNo,
                    ActionID = 20,
                    MenuID = 16,
                    IsActive = false
                };
                await _unitOfWork.SystPermissionsActionCompanyRepository.Add(systPermissionsActionCompany);
                await _unitOfWork.SaveChangesAsync();

                //************** Insert customer system , section system เพื่อ generate pm auto*************
                Section sectionSys = new Section()
                {
                    SectionCode = "SYS",
                    SectionName = "SYSTEM",
                    CompanyNo = newCompanyNo,
                    IsDelete = false,
                    CreateDate = DateTime.Now,
                    CreatedBy = -99
                };
                await _unitOfWork.SectionRepository.Add(sectionSys);
                await _unitOfWork.SaveChangesAsync();
                Customer customerSys = new Customer()
                {
                    CompanyNo = newCompanyNo,
                    Firstname = "SYSTEM",
                    SectionNo = sectionSys.SectionNo,
                    CustomerCode = "SYS",
                    CreatedDate = updatedDate,
                    UpdatedDate = updatedDate
                };
                await _unitOfWork.CustomerRepository.Add(customerSys);
                await _unitOfWork.SaveChangesAsync();

                //************** Insert NotifyMsg*************//
                IEnumerable<NotifyMsg> notifyMsgs = await _unitOfWork.NotifyMsgRepository.GetNotifyMsgByCompany(-99);
                foreach (var item in notifyMsgs)
                {
                    NotifyMsg notifyMsg = new NotifyMsg()
                    {
                        Message = item.Message,
                        WOStatusNo = item.WOStatusNo,
                        CompanyNo = newCompanyNo,
                        IsActive = item.IsActive,
                        DefaultTo = item.DefaultTo,
                        Action = item.Action,
                        Title = item.Title,
                        ActionName = item.ActionName,
                        IsDelete = item.IsDelete,
                        IndexNo = item.IndexNo
                    };
                    await _unitOfWork.NotifyMsgRepository.Add(notifyMsg);
                }
                await _unitOfWork.SaveChangesAsync();

                IEnumerable<NotifyMsg> notifyMsgsCurrentSite = await _unitOfWork.NotifyMsgRepository.GetNotifyMsgByCompany(newCompanyNo);
                foreach (NotifyMsg notifyMsgCurrent in notifyMsgsCurrentSite)
                {
                    IEnumerable<NotifyMsgDefault> notifyMsgDefaults = await _unitOfWork.NotifyMsgDefaultRepository.GetNotifyMsgDefaultByIndex(notifyMsgCurrent.IndexNo);
                    foreach (NotifyMsgDefault notifyMsgDefault in notifyMsgDefaults)
                    {
                        await _unitOfWork.NotifyMsgRoleRepository.Add(new NotifyMsg_Role() { NotifyMsgNo = notifyMsgCurrent.NotifyNo, RoleNo = notifyMsgDefault.RoleNo, IsSystem = notifyMsgDefault.IsSystem });
                    }
                }
                await _unitOfWork.SaveChangesAsync();
                _unitOfWork.CommitTransaction();

                return await _unitOfWork.CompanyRepository.GetById(newCompanyNo);
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTransaction();
                throw;
            }
        }

        public async Task UpdateCompany(Site site, int id)
        {
            Site oldSite = await _unitOfWork.CompanyRepository.GetById(id);
            oldSite.CompanyName_TH = site.CompanyName_TH;
            oldSite.CompanyName_EN = site.CompanyName_TH;
            oldSite.IsDelete = site.IsDelete;
            oldSite.IsActive = site.IsActive;
            oldSite.FlagSubsite = site.FlagSubsite;
            oldSite.SubsiteCode = site.SubsiteCode;
            oldSite.SubsiteName = site.SubsiteName;
            oldSite.ExpiredDate = site.ExpiredDate;
            oldSite.UploadSize = site.UploadSize;
            oldSite.Package = site.Package;
            oldSite.Platform = site.Platform;
            oldSite.CustomerType = site.CustomerType;
            oldSite.ContactName = site.ContactName;
            oldSite.Email = site.Email;
            oldSite.Phone = site.Phone;
            oldSite.UpdatedDate = DateTime.Now;
            oldSite.AllSpace = site.AllSpace;
            oldSite.LimitUser = site.LimitUser;
            oldSite.CreatedDate = site.CreatedDate;
            oldSite.LimitRow = site.LimitRow;
            oldSite.Package = site.Package;
            oldSite.PeriodUse = site.PeriodUse;

            _unitOfWork.CompanyRepository.Update(oldSite);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteCompany(int id, User user)
        {
            Site oldSite = await _unitOfWork.CompanyRepository.GetById(id);
            oldSite.IsDelete = true;
            _unitOfWork.CompanyRepository.Update(oldSite);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateSubsite(Site site, User user)
        {
            Site oldSite = await _unitOfWork.CompanyRepository.GetById(site.CompanyNo);
            oldSite.IsActive = site.IsActive;
            oldSite.SubsiteCode = site.SubsiteCode;
            oldSite.SubsiteName = site.SubsiteName;
            oldSite.UploadSize = site.UploadSize;
            oldSite.UpdatedDate = DateTime.Now;
            _unitOfWork.CompanyRepository.Update(oldSite);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<Site> InsertSubsite(Site site, User user)
        {
            try
            {
                if (user == null)
                {
                    user = new User()
                    {
                        UserNo = -99,
                        CustomerNo = -99
                    };
                }
                _unitOfWork.BeginTransaction();
                int oldCompanyNo = 0;
                Site demoSite = _unitOfWork.CompanyRepository.GetCompanyByProductKey(site.ProductKey);
                oldCompanyNo = demoSite.CompanyNo;
                demoSite.IsActive = site.IsActive;
                demoSite.CreatedBy = user.UserNo;
                demoSite.CreatedDate = DateTime.Now;
                demoSite.UploadSize = site.UploadSize.Value;
                demoSite.SubsiteCode = site.SubsiteCode;
                demoSite.SubsiteName = site.SubsiteName;
                demoSite.CompanyNo = 0;
                demoSite.IsMainSite = false;
                demoSite.AllSpace = null;

                await _unitOfWork.CompanyRepository.Add(demoSite);
                await _unitOfWork.SaveChangesAsync();

                int newCompanyNo = demoSite.CompanyNo;

                DateTime updatedDate = DateTime.Now;
                int updatedBy = user.UserNo;
                IEnumerable<SysParameter1> sysParameter1List = await _unitOfWork.SysParameter1Repository.GetSysParameterByCompany(oldCompanyNo);
                foreach (var sysParameter1 in sysParameter1List)
                {
                    sysParameter1.CompanyNo = newCompanyNo;
                    sysParameter1.YearNo = ((DateTime.Now.Year % 100) + 1).ToString();
                    sysParameter1.LAST_RUNNO = 0;
                    sysParameter1.Doc_LastNo = $"{sysParameter1.PREFIX}-000000";

                    await _unitOfWork.SysParameter1Repository.Add(sysParameter1);
                }
                await _unitOfWork.SaveChangesAsync();


                if (user.UserNo == 99)
                {
                    Customer customer = _unitOfWork.CustomerRepository.GetCustomerByUserCompany(user.UserNo, oldCompanyNo);
                    Customer newCustomer = new Customer()
                    {
                        CompanyNo = newCompanyNo,
                        Firstname = customer.Firstname,
                        Lastname = customer.Lastname,
                        IsMaintainance = false,
                        CompanyName = site.CompanyName_TH,
                        CustomerCode = customer.CustomerCode,
                        CreateBy = user.CustomerNo,
                        CreatedDate = updatedDate,
                        UpdatedDate = updatedDate,
                        IsActive = true,
                        IsDelete = false,
                    };

                    await _unitOfWork.CustomerRepository.Add(newCustomer);
                    await _unitOfWork.SaveChangesAsync();

                    SystUserCustomer systUserCustomerDemo = new SystUserCustomer()
                    {
                        UserNo = user.UserNo,
                        CustomerNo = newCustomer.CustomerNo
                    };
                    await _unitOfWork.SystUserCustomerRepository.Add(systUserCustomerDemo);
                    await _unitOfWork.SaveChangesAsync();
                }

                SysConfig sysConfig = new SysConfig()
                {
                    ConfigName = "DIGIT",
                    ConfigType = "SRRUNNING",
                    ConfigValue = "3",
                    CompanyNo = newCompanyNo
                };
                await _unitOfWork.SystConfigRepository.Add(sysConfig);
                sysConfig = new SysConfig()
                {
                    ConfigName = "SYSTEM",
                    ConfigType = "TABLECAPTION",
                    ConfigValue = "System",
                    CompanyNo = newCompanyNo
                };
                await _unitOfWork.SystConfigRepository.Add(sysConfig);
                sysConfig = new SysConfig()
                {
                    ConfigName = "ACTIVE",
                    ConfigType = "GENPMAUTO",
                    ConfigValue = "Y",
                    CompanyNo = newCompanyNo
                };
                await _unitOfWork.SystConfigRepository.Add(sysConfig);
                sysConfig = new SysConfig()
                {
                    ConfigName = "PREVDAYS",
                    ConfigType = "GENPMAUTO",
                    ConfigValue = "1",
                    CompanyNo = newCompanyNo
                };
                await _unitOfWork.SystConfigRepository.Add(sysConfig);
                SystPermissionsActionCompany systPermissionsActionCompany = new SystPermissionsActionCompany()
                {
                    CompanyNo = newCompanyNo,
                    ActionID = 20,
                    MenuID = 16,
                    IsActive = false
                };
                await _unitOfWork.SystPermissionsActionCompanyRepository.Add(systPermissionsActionCompany);
                await _unitOfWork.SaveChangesAsync();

                Section sectionSys = new Section()
                {
                    SectionCode = "SYS",
                    SectionName = "SYSTEM",
                    CompanyNo = newCompanyNo,
                    IsDelete = false,
                    CreateDate = DateTime.Now,
                    CreatedBy = -99
                };
                await _unitOfWork.SectionRepository.Add(sectionSys);
                await _unitOfWork.SaveChangesAsync();
                Customer customerSys = new Customer()
                {
                    CompanyNo = newCompanyNo,
                    Firstname = "SYSTEM",
                    SectionNo = sectionSys.SectionNo,
                    CustomerCode = "SYS",
                    CreatedDate = updatedDate,
                    UpdatedDate = updatedDate
                };
                await _unitOfWork.CustomerRepository.Add(customerSys);
                await _unitOfWork.SaveChangesAsync();


                if (site.AddDefaultData)
                {
                    string pPath = _host.ContentRootPath;
                    FileInfo template = new FileInfo($"{pPath}\\InitData.xlsx");
                    using (var package = new ExcelPackage(template))
                    {
                        var workbook = package.Workbook;

                        foreach (ExcelWorksheet worksheet in workbook.Worksheets)
                        {
                            if (worksheet.Name == "PROBLEM")
                            {
                                List<FailureObject> problems = new List<FailureObject>();
                                for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                                {
                                    problems.Add(new FailureObject()
                                    {
                                        ObjectNo = 0,
                                        ObjectType = "PRO",
                                        ObjectCode = worksheet.Cells[i, 1].Value.ToString(),
                                        ObjectName = worksheet.Cells[i, 2].Value.ToString(),
                                        CompanyNo = newCompanyNo,
                                        Description = worksheet.Cells[i, 3].Value.ToString(),
                                        IsDelete = false
                                    });
                                }
                                await _unitOfWork.FailureObjectRepository.InsertBulk(problems);
                            }
                            else if (worksheet.Name == "ACTION")
                            {
                                List<FailureObject> actions = new List<FailureObject>();
                                for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                                {
                                    actions.Add(new FailureObject()
                                    {
                                        ObjectNo = 0,
                                        ObjectType = "ACT",
                                        ObjectCode = worksheet.Cells[i, 1].Value.ToString(),
                                        ObjectName = worksheet.Cells[i, 2].Value.ToString(),
                                        CompanyNo = newCompanyNo,
                                        Description = worksheet.Cells[i, 3].Value.ToString(),
                                        IsDelete = false
                                    });
                                }
                                await _unitOfWork.FailureObjectRepository.InsertBulk(actions);
                            }
                            else if (worksheet.Name == "CAUSE")
                            {
                                List<FailureObject> cause = new List<FailureObject>();
                                for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                                {
                                    cause.Add(new FailureObject()
                                    {
                                        ObjectNo = 0,
                                        ObjectType = "CAU",
                                        ObjectCode = worksheet.Cells[i, 2].Value.ToString(),
                                        ObjectName = worksheet.Cells[i, 3].Value.ToString(),
                                        CompanyNo = newCompanyNo,
                                        Description = worksheet.Cells[i, 4].Value.ToString(),
                                        CauseOfTrouble = worksheet.Cells[i, 1].Value?.ToString(),
                                        IsDelete = false
                                    });
                                }
                                await _unitOfWork.FailureObjectRepository.InsertBulk(cause);
                            }
                            else if (worksheet.Name == "SECTION")
                            {
                                int i = worksheet.Dimension.Start.Row + 1;
                                Section section = new Section()
                                {
                                    SectionCode = worksheet.Cells[i, 1].Value.ToString(),
                                    SectionName = worksheet.Cells[i, 2].Value.ToString(),
                                    CompanyNo = newCompanyNo,
                                    IsDelete = false,
                                };
                                await _unitOfWork.SectionRepository.Add(section);
                                await _unitOfWork.SaveChangesAsync();
                            }
                            else if (worksheet.Name == "PROBLEM_TYPE")
                            {
                                int i = worksheet.Dimension.Start.Row + 1;
                                Section section = _unitOfWork.SectionRepository.GetSectionByCode("MNT", companyNo: newCompanyNo);
                                ProblemType problemType = new ProblemType()
                                {
                                    ProblemTypeCode = worksheet.Cells[i, 1].Value.ToString(),
                                    ProblemTypeName = worksheet.Cells[i, 2].Value.ToString(),
                                    CompanyNo = newCompanyNo,
                                    SectionNo = section.SectionNo,
                                    IsDelete = false,
                                };
                                await _unitOfWork.ProblemTypeRepository.Add(problemType);
                            }
                        }
                    }
                }
                //************** Insert NotifyMsg*************//
                IEnumerable<NotifyMsg> notifyMsgs = await _unitOfWork.NotifyMsgRepository.GetNotifyMsgByCompany(-99);
                foreach (var item in notifyMsgs)
                {
                    NotifyMsg notifyMsg = new NotifyMsg()
                    {
                        Message = item.Message,
                        WOStatusNo = item.WOStatusNo,
                        CompanyNo = newCompanyNo,
                        IsActive = item.IsActive,
                        DefaultTo = item.DefaultTo,
                        Action = item.Action,
                        Title = item.Title,
                        ActionName = item.ActionName,
                        IsDelete = item.IsDelete,
                        IndexNo = item.IndexNo
                    };
                    await _unitOfWork.NotifyMsgRepository.Add(notifyMsg);
                }
                await _unitOfWork.SaveChangesAsync();

                IEnumerable<NotifyMsg> notifyMsgsCurrentSite = await _unitOfWork.NotifyMsgRepository.GetNotifyMsgByCompany(newCompanyNo);
                foreach (NotifyMsg notifyMsgCurrent in notifyMsgsCurrentSite)
                {
                    IEnumerable<NotifyMsgDefault> notifyMsgDefaults = await _unitOfWork.NotifyMsgDefaultRepository.GetNotifyMsgDefaultByIndex(notifyMsgCurrent.IndexNo);
                    foreach (NotifyMsgDefault notifyMsgDefault in notifyMsgDefaults)
                    {
                        await _unitOfWork.NotifyMsgRoleRepository.Add(new NotifyMsg_Role() { NotifyMsgNo = notifyMsgCurrent.NotifyNo, RoleNo = notifyMsgDefault.RoleNo, IsSystem = notifyMsgDefault.IsSystem });
                    }
                }
                await _unitOfWork.SaveChangesAsync();
                _unitOfWork.CommitTransaction();

                return await _unitOfWork.CompanyRepository.GetById(newCompanyNo);
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTransaction();
                throw;
            }
        }

        public IEnumerable<Site> GetCompanyProductKeyUser(string productkey, int userid)
        {
            return _unitOfWork.CompanyRepository.GetCompanyProductKeyUser(productkey, userid);
        }


    }
}
