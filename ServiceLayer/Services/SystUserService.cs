using Domain.Entities.Syst;
using Domain.Interfaces;
using IdylAPI.Helper;
using IdylAPI.Models;
using IdylAPI.Models.Master;
using IdylAPI.Models.Syst;
using IdylAPI.Services.Interfaces.Syst;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PAUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystUser = IdylAPI.Models.Syst.SystUser;

namespace SocialMedia.Core.Services
{
    public class SystUserService : ISystUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly ILogger<SystUserService> _logger;
        private readonly IHostingEnvironment _hostingEnvironment;
        public SystUserService(IUnitOfWork unitOfWork, IConfiguration configuration, ILogger<SystUserService> logger, IHostingEnvironment env)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _logger = logger;
            _hostingEnvironment = env;
        }

        public async Task<Result> ActivateUser(ActivateUser systUser)
        {
            Result result = new Result();
            try
            {
                _unitOfWork.BeginTransaction();
                DateTime updatedDate = DateTime.Now;
                SystUserCustomer systUserCustomer = _unitOfWork.SystUserCustomerRepository.GetByUserCustomer(systUser.UserNo, systUser.CustomerNo);
                if (systUserCustomer == null)
                {
                    IEnumerable<Customer> customers = _unitOfWork.SystUserRepository.CheckEmailDupicate(systUser.Email);
                    if (customers.Count() > 0)
                    {
                        result.StatusCode = 500;
                        result.ErrMsg = "ไม่สามารถ activate ได้เนื่องจากมี email ซ้ำในระบบ";
                    }
                    else
                    {
                        string newUsername = "";
                        if (string.IsNullOrEmpty(systUser.Username))
                        {
                            int indexOf = systUser.Email.IndexOf("@");
                            newUsername =  systUser.Email.Substring(0, indexOf);
                            IEnumerable<SystUser> systs = _unitOfWork.SystUserRepository.CheckUsernameDupicate(newUsername);
                            if (systs.Count() > 0)
                            {
                                Random rnd = new Random();
                                newUsername = newUsername + rnd.Next(99);
                            }
                        }
                        else
                        {
                            IEnumerable<SystUser> systs = _unitOfWork.SystUserRepository.CheckUsernameDupicate(systUser.Username);
                            if (systs.Count() > 0)
                            {
                                Random rnd = new Random();
                                newUsername = systUser.Username + rnd.Next(99);
                            }
                        }

                        int updatedBy = systUser.UserNo;
                        await _unitOfWork.SysParameter1Repository.GenCodeFormat(0, "SYS9998");
                        SysParameter1 sysParameter1 = await _unitOfWork.SysParameter1Repository.GetSysParameterByParaAndCompany(0, "SYS9998");
                        string uDemo = sysParameter1.Doc_LastNo;

                        Customer newCustomer = new Customer()
                        {
                            CompanyNo = systUser.CompanyNo,
                            Firstname = systUser.FirstName,
                            Lastname = systUser.LastName,
                            IsMaintainance = systUser.IsMaintainance,
                            CustomerCode = uDemo,
                            CreateBy = updatedBy,
                            CreatedDate = updatedDate,
                            UpdatedDate = updatedDate,
                            IsActive = true,
                            IsDelete = false,
                            IsHeadCraft = systUser.IsHeadCraft,
                            IsHeadSection = systUser.IsHeadSection,
                            SectionNo = systUser.SectionNo,
                            CraftTypeNo = systUser.CraftTypeNo,
                            Mobile = systUser.Mobile,
                            Email = systUser.Email
                        };

                        await _unitOfWork.CustomerRepository.Add(newCustomer);
                        await _unitOfWork.SaveChangesAsync();

                        SystUser syst = await _unitOfWork.SystUserRepository.GetById(systUser.UserNo);
                        syst.Username = newUsername;
                        syst.IsActivate = true;
                        syst.ActivateDate = DateTime.Now;
                        syst.ExpiredDate = systUser.ExpiredDate;
                        syst.Password = systUser.Password;
                        syst.Email = systUser.Email;
                        syst.Firstname = systUser.FirstName;
                        syst.Lastname = systUser.LastName;
                        syst.Mobile = systUser.Mobile;

                        _unitOfWork.SystUserRepository.Update(syst);
                        await _unitOfWork.SaveChangesAsync();

                        SystUserCustomer userCustomer = new SystUserCustomer()
                        {
                            UserNo = systUser.UserNo,
                            CustomerNo = newCustomer.CustomerNo
                        };
                        await _unitOfWork.SystUserCustomerRepository.Add(userCustomer);
                        await _unitOfWork.SaveChangesAsync();
                    }

                    _unitOfWork.CommitTransaction();
                }
            }
            catch (Exception e)
            {
                result.ErrMsg = e.Message;
                _unitOfWork.RollbackTransaction();
            }

            return result;

        }

        public IEnumerable<Customer> CheckEmailDupicate(string email)
        {
            return _unitOfWork.SystUserRepository.CheckEmailDupicate(email);
        }

        public IEnumerable<SystUser> CheckUsernameDupicate(string username)
        {
            return _unitOfWork.SystUserRepository.CheckUsernameDupicate(username);
        }

        public IEnumerable<SystUser> GetByCompany(int companyNo)
        {
            return _unitOfWork.SystUserRepository.GetByCompany(companyNo);
        }

        public async Task<SystUser> GetByid(int id)
        {
            return await _unitOfWork.SystUserRepository.GetById(id);
        }

        public SystUser GetByUserId(int userId, int companyNo)
        {
            return _unitOfWork.SystUserRepository.GetByUserId(userId, companyNo);
        }

        public ActivateUser GetUserCustomerById(int userId, int companyNo)
        {
            return _unitOfWork.SystUserRepository.GetUserCustomerById(userId, companyNo);
        }

        public IEnumerable<SystUser> GetUserByProductKey(string productKey)
        {
            return _unitOfWork.SystUserRepository.GetUserByProductKey(productKey);
        }

        public async Task<Result> UnlockUser(int userGroupId, int noOfUser, int companyNo)
        {
            Result result = new Result();
            try
            {

                _unitOfWork.BeginTransaction();
                Site site = await _unitOfWork.CompanyRepository.GetById(companyNo);

                int masterCompany = 57;

                Site demoSite = await _unitOfWork.CompanyRepository.GetById(masterCompany);

                DateTime updatedDate = DateTime.Now;
                for (int i = 0; i < noOfUser; i++)
                {
                    await _unitOfWork.SysParameter1Repository.GenCodeFormat(0, "SYS9998");

                    SysParameter1 sysParameter1 = await _unitOfWork.SysParameter1Repository.GetSysParameterByParaAndCompany(0, "SYS9998");
                    Customer customer = new Customer()
                    {
                        CompanyNo = companyNo,
                        Firstname = sysParameter1.Doc_LastNo,
                        Lastname = "Lastname",
                        IsMaintainance = false,
                        Rate = 300,
                        CustomerCode = sysParameter1.Doc_LastNo,
                        CreatedDate = updatedDate,
                        UpdatedDate = updatedDate,
                        IsActive = true,
                        IsDelete = false
                    };
                    await _unitOfWork.CustomerRepository.Add(customer);
                    await _unitOfWork.SaveChangesAsync();

                    await _unitOfWork.SysParameter1Repository.GenCodeFormat(0, "SYS9999");
                    SysParameter1 userParameter = await _unitOfWork.SysParameter1Repository.GetSysParameterByParaAndCompany(0, "SYS9999");

                    SystUser user = new SystUser()
                    {
                        Password = "cfcd208495d565ef66e7dff9f98764da",
                        CompanyNo = companyNo,
                        PINCode = "cfcd208495d565ef66e7dff9f98764da",
                        //ExpiredDate = site.ExpiredDate.HasValue ? site.ExpiredDate.Value : updatedDate.AddDays(30),
                        Username = userParameter.Doc_LastNo,
                        UserFixed = userParameter.Doc_LastNo,
                        UnlockCode = "1",
                        IsLogin = false,
                        IsDelete = false,
                        IsActive = true,
                        CreatedDate = updatedDate,
                        UpdatedDate = updatedDate,
                        IsSuperUser = false,
                        UserGroupId = userGroupId,
                        NotSeeAuto_AuthenLocation = false,
                        ActivateCode = Guid.NewGuid().ToString(),
                        IsActivate = false
                    };

                    await _unitOfWork.SystUserRepository.Add(user);
                    await _unitOfWork.SaveChangesAsync();

                    SystUser systUser = await _unitOfWork.SystUserRepository.GetById(user.UserNo);
                    systUser.UnlockCode = (user.UserNo * (user.UserNo + Convert.ToInt32(userParameter.Doc_LastNo.Replace("U", ""))) * 3.5).ToString();
                    _unitOfWork.SystUserRepository.Update(systUser);
                    await _unitOfWork.SaveChangesAsync();

                    IEnumerable<UserGroupMenu> menus = _unitOfWork.UserGroupMenuRepository.GetByUserGroup(userGroupId);
                    foreach (var item in menus)
                    {
                        FormPermission formPermission = new FormPermission()
                        {
                            UserNo = user.UserNo,
                            MenuId = item.MenuId,
                            IsView = item.IsView,
                        };
                        await _unitOfWork.SystPermissionsRepository.Add(formPermission);
                    }
                    await _unitOfWork.SaveChangesAsync();
                    IEnumerable<SystAction> systActions = _unitOfWork.SystActionRepository.GetAll();
                    foreach (var item in systActions)
                    {
                        FormPermissionAction formPermission = new FormPermissionAction()
                        {
                            UserNo = user.UserNo,
                            MenuId = item.MenuID,
                            ActionId = item.ActionID,
                            IsActive = !item.FlagAdminOnly
                        };
                        await _unitOfWork.SystPermissionsActionRepository.Add(formPermission);
                    }

                    await _unitOfWork.SaveChangesAsync();

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
                        await _unitOfWork.SysParameter1Repository.GenCodeFormat(0, "SYS9998");
                        SysParameter1 sysParameter1Demo = await _unitOfWork.SysParameter1Repository.GetSysParameterByParaAndCompany(0, "SYS9998");
                        string uDemoSubsite = sysParameter1Demo.Doc_LastNo;

                        SystUserCustomer systUserCustomerDemo = new SystUserCustomer()
                        {
                            UserNo = user.UserNo,
                            CustomerNo = 6033
                        };

                        await _unitOfWork.SystUserCustomerRepository.Add(systUserCustomerDemo);
                        await _unitOfWork.SaveChangesAsync();

                        IEnumerable<Location> locations = _unitOfWork.LocationRepository.GetByCompany(masterCompany);
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

                site.UserUnlock += noOfUser;
                _unitOfWork.CompanyRepository.Update(site);
                await _unitOfWork.SaveChangesAsync();
                _unitOfWork.CommitTransaction();
            }
            catch (Exception e)
            {
                result.ErrMsg = e.Message;
                _unitOfWork.RollbackTransaction();

            }
            return result;
        }

        public async Task<Result> UnlockUser(CreateUser userObj)
        {
            Result result = new Result();
            try
            {
                _unitOfWork.BeginTransaction();
                Site site = _unitOfWork.CompanyRepository.GetCompanyByProductKey(userObj.ProductKey);

                int masterCompany = 57;
                Site demoSite = await _unitOfWork.CompanyRepository.GetById(masterCompany);

                DateTime updatedDate = DateTime.Now;
                for (int i = 0; i < userObj.NoOfUser; i++)
                {
                    await _unitOfWork.SysParameter1Repository.GenCodeFormat(0, "SYS9999");
                    SysParameter1 userParameter = await _unitOfWork.SysParameter1Repository.GetSysParameterByParaAndCompany(0, "SYS9999");

                    SystUser user = new SystUser()
                    {
                        Password = "cfcd208495d565ef66e7dff9f98764da",
                        CompanyNo = site.CompanyNo,
                        PINCode = "cfcd208495d565ef66e7dff9f98764da",
                        Username = "",
                        UserFixed = userParameter.Doc_LastNo,
                        UnlockCode = "1",
                        IsLogin = false,
                        IsDelete = false,
                        IsActive = true,
                        CreatedDate = updatedDate,
                        UpdatedDate = updatedDate,
                        IsSuperUser = false,
                        UserGroupId = userObj.UserGroupId,
                        NotSeeAuto_AuthenLocation = false,
                        ActivateCode = Guid.NewGuid().ToString(),
                        IsActivate = false,
                    };

                    await _unitOfWork.SystUserRepository.Add(user);
                    await _unitOfWork.SaveChangesAsync();

                    SystUser systUser = await _unitOfWork.SystUserRepository.GetById(user.UserNo);
                    systUser.UnlockCode = (user.UserNo * (user.UserNo + Convert.ToInt32(userParameter.Doc_LastNo.Replace("U", ""))) * 3.5).ToString();
                    _unitOfWork.SystUserRepository.Update(systUser);
                    await _unitOfWork.SaveChangesAsync();

                    IEnumerable<UserGroupMenu> menus = _unitOfWork.UserGroupMenuRepository.GetByUserGroup(userObj.UserGroupId);
                    foreach (var item in menus)
                    {
                        FormPermission formPermission = new FormPermission()
                        {
                            UserNo = user.UserNo,
                            MenuId = item.MenuId,
                            IsView = item.IsView,
                        };
                        await _unitOfWork.SystPermissionsRepository.Add(formPermission);
                    }
                    await _unitOfWork.SaveChangesAsync();
                    IEnumerable<SystAction> systActions = _unitOfWork.SystActionRepository.GetAll();
                    foreach (var item in systActions)
                    {
                        FormPermissionAction formPermission = new FormPermissionAction()
                        {
                            UserNo = user.UserNo,
                            MenuId = item.MenuID,
                            ActionId = item.ActionID,
                            IsActive = !item.FlagAdminOnly
                        };
                        await _unitOfWork.SystPermissionsActionRepository.Add(formPermission);
                    }

                    await _unitOfWork.SaveChangesAsync();

                    #region Set Permission Data
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
                    #endregion

                    //SystUserCustomer systUserCustomer = new SystUserCustomer()
                    //{
                    //    UserNo = user.UserNo,
                    //    CustomerNo = customer.CustomerNo
                    //};

                    //await _unitOfWork.SystUserCustomerRepository.Add(systUserCustomer);
                    //await _unitOfWork.SaveChangesAsync();

                    #region Create User To Demo Subsite PA
                    bool IsAddDemo = InputVal.ToBool(_configuration["IsAddDemo"]);
                    if (IsAddDemo)
                    {
                        await _unitOfWork.SysParameter1Repository.GenCodeFormat(0, "SYS9998");
                        SysParameter1 sysParameter1Demo = await _unitOfWork.SysParameter1Repository.GetSysParameterByParaAndCompany(0, "SYS9998");
                        string uDemoSubsite = sysParameter1Demo.Doc_LastNo;

                        SystUserCustomer systUserCustomerDemo = new SystUserCustomer()
                        {
                            UserNo = user.UserNo,
                            CustomerNo = 6033
                        };

                        await _unitOfWork.SystUserCustomerRepository.Add(systUserCustomerDemo);
                        await _unitOfWork.SaveChangesAsync();

                        IEnumerable<Location> locations = _unitOfWork.LocationRepository.GetByCompany(masterCompany);
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

                site.UserUnlock += userObj.NoOfUser;
                _unitOfWork.CompanyRepository.Update(site);
                await _unitOfWork.SaveChangesAsync();
                _unitOfWork.CommitTransaction();
            }
            catch (Exception e)
            {
                result.ErrMsg = e.Message;
                _unitOfWork.RollbackTransaction();

            }
            return result;
        }

        public async Task Update(int id, SystUser obj)
        {
            SystUser systUser = await _unitOfWork.SystUserRepository.GetById(id);
            systUser.Username = obj.Username;
            systUser.ExpiredDate = obj.ExpiredDate;
            systUser.IsActive = obj.IsActive;
            systUser.IsDelete = obj.IsDelete;
            systUser.IsSuperUser = obj.IsSuperUser;
            systUser.UserGroupId = obj.UserGroupId;
            systUser.UpdatedDate = DateTime.Now;
            systUser.IsActivate = obj.IsActivate;
            systUser.ActivateDate = obj.ActivateDate;
            _unitOfWork.SystUserRepository.Update(systUser);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateUserActiveSite(int CompanyNo, bool IsActive, int UserNo)
        {
            await _unitOfWork.SystUserRepository.UpdateUserActiveSite(CompanyNo, IsActive, UserNo);
        }

        public async Task UpdateNotSeeAutoAuthenLocation(int id, string isNotSeeAuto_AuthenLocation)
        {
            SystUser systUser = await _unitOfWork.SystUserRepository.GetById(id);
            systUser.NotSeeAuto_AuthenLocation = isNotSeeAuto_AuthenLocation == "T" ? true : false;
            _unitOfWork.SystUserRepository.Update(systUser);
            await _unitOfWork.SaveChangesAsync();
        }

        [Obsolete]
        public async Task<Result> SendEmail(int userId)
        {
            SystUser user = await _unitOfWork.SystUserRepository.GetById(userId);
            return Mail.SendEmail(_configuration, user, _logger, "IDYL Activate user");
            //Result result = new Result();
            //try
            //{


            //    var contentRoot = Path.Combine(_configuration.GetValue<string>(WebHostDefaults.ContentRootKey), "MailTemplate.html");

            //    string mailServer = _configuration["MailServer"];
            //    string senderMail = _configuration["SenderMail"];
            //    string password = _configuration["Password"];
            //    bool enableSSL = InputVal.ToBool(_configuration["EnableSSL"]);
            //    int port = InputVal.ToInt(_configuration["Port"]);

            //    MimeMessage message = new MimeMessage();

            //    MailboxAddress from = new MailboxAddress(senderMail);
            //    message.From.Add(from);

            //    MailboxAddress to = new MailboxAddress(user.Firstname + " " + user.Lastname, user.Email);
            //    message.To.Add(to);

            //    message.Subject = "IDYL Activate user";
            //    BodyBuilder bodyBuilder = new BodyBuilder();
            //    using (StreamReader SourceReader = System.IO.File.OpenText(contentRoot))
            //    {
            //        bodyBuilder.HtmlBody = SourceReader.ReadToEnd();
            //        bodyBuilder.HtmlBody = bodyBuilder.HtmlBody.Replace("{name}", user.Firstname);
            //        bodyBuilder.HtmlBody = bodyBuilder.HtmlBody.Replace("{username}", user.Username);
            //        bodyBuilder.HtmlBody = bodyBuilder.HtmlBody.Replace("{password}", user.Mobile);
            //    }

            //    message.Body = bodyBuilder.ToMessageBody();

            //    SmtpClient client = new SmtpClient();
            //    client.Connect(mailServer, port, enableSSL);
            //    client.Authenticate(senderMail, password);

            //    client.Send(message);
            //    client.Disconnect(true);
            //    client.Dispose();

            //    result.StatusCode = 200;
            //    _logger.LogDebug($"send email : @{user.Firstname + " " + user.Lastname} - @{user.Email}");
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex.Message);
            //    result.StatusCode = 500;
            //    result.ErrMsg = ex.Message;
            //}

            //return result;
        }

        public IEnumerable<UserView> GetUserView()
        {
            return _unitOfWork.SystUserRepository.GetUserView();
        }
    }
}
