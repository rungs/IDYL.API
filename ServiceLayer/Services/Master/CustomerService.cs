using Domain.Entities.Syst;
using Domain.Interfaces;
using IdylAPI.Helper;
using IdylAPI.Models;
using IdylAPI.Models.Master;
using IdylAPI.Models.Syst;
using IdylAPI.Services.Interfaces.Syst;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SocialMedia.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace IdylAPI.Services.Master
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly ILogger<SystUserService> _logger;

        public CustomerService(IUnitOfWork unitOfWork, IConfiguration configuration, ILogger<SystUserService> logger)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _logger = logger;
        }
        public IEnumerable<Customer> GetByUser(int userNo)
        {
            return _unitOfWork.CustomerRepository.GetByUser(userNo);
        }
        public IEnumerable<Customer> GetAllExclude(int userNo)
        {
            return _unitOfWork.CustomerRepository.GetAllExclude(userNo);
        }

        [Obsolete]
        public async Task<Result> Register(Customer customer)
        {
            Result result = new Result();
            try
            {
                _unitOfWork.BeginTransaction();

                DateTime updatedDate = DateTime.Now;
                Site site = _unitOfWork.CompanyRepository.GetCompanyByProductKey("FREE");
                int UserGroupId = 5;
                await _unitOfWork.SysParameter1Repository.GenCodeFormat(0, "SYS9999");
                SysParameter1 userParameter = await _unitOfWork.SysParameter1Repository.GetSysParameterByParaAndCompany(0, "SYS9999");

                Models.Syst.SystUser user = new Models.Syst.SystUser()
                {
                    Username = customer.Email,
                    Password = new Cryptography().GetMd5Hash(System.Security.Cryptography.MD5.Create(), customer.Mobile),
                    UserGroupId = UserGroupId,
                    CompanyNo = site.CompanyNo,
                    UserFixed = userParameter.Doc_LastNo,
                    UnlockCode = "1",
                    IsLogin = false,
                    IsDelete = false,
                    IsActive = true,
                    CreatedDate = updatedDate,
                    UpdatedDate = updatedDate,
                    IsSuperUser = false,
                    NotSeeAuto_AuthenLocation = false,
                    ActivateCode = Guid.NewGuid().ToString(),
                    IsActivate = true,
                    ExpiredDate = site.ExpiredDate,
                    ActivateDate = updatedDate,
                    Firstname = customer.Firstname,
                    Lastname = customer.Lastname,
                    Mobile = customer.Mobile,
                    Email = customer.Email,
                };

                await _unitOfWork.SystUserRepository.Add(user);
                await _unitOfWork.SaveChangesAsync();

                Models.Syst.SystUser systUser = await _unitOfWork.SystUserRepository.GetById(user.UserNo);
                systUser.UnlockCode = (user.UserNo * (user.UserNo + Convert.ToInt32(userParameter.Doc_LastNo.Replace("U", ""))) * 3.5).ToString();
                _unitOfWork.SystUserRepository.Update(systUser);
                await _unitOfWork.SaveChangesAsync();

                #region Set Permission Menu
                IEnumerable<UserGroupMenu> menus = _unitOfWork.UserGroupMenuRepository.GetByUserGroup(UserGroupId);
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
                #endregion

                #region Set Permission Action
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

                #endregion

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

                //await _unitOfWork.SysParameter1Repository.GenCodeFormat(0, "SYS2770");
                //userParameter = await _unitOfWork.SysParameter1Repository.GetSysParameterByParaAndCompany(0, "SYS2770");
                Location location = new Location()
                {
                    LocationCode = systUser.UserFixed,
                    LocationName = customer.Firstname,
                    CompanyNo = site.CompanyNo,
                };
                await _unitOfWork.LocationRepository.Add(location);
                await _unitOfWork.SaveChangesAsync();

                SystUserLocation systUserLocation = new SystUserLocation()
                {
                    LocationNo = location.LocationNo,
                    UserNo = user.UserNo
                };
                await _unitOfWork.SystUserLocationRepository.Add(systUserLocation);
                await _unitOfWork.SaveChangesAsync();

                //Fix Section Free
                await _unitOfWork.SysParameter1Repository.GenCodeFormat(0, "SYS2760");
                userParameter = await _unitOfWork.SysParameter1Repository.GetSysParameterByParaAndCompany(0, "SYS2760");
                Section section = new Section()
                {
                    SectionCode = userParameter.Doc_LastNo,
                    SectionName = customer.Firstname,
                    CompanyNo = site.CompanyNo
                };
                await _unitOfWork.SectionRepository.Add(section);
                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.SysParameter1Repository.GenCodeFormat(0, "SYS2790");
                userParameter = await _unitOfWork.SysParameter1Repository.GetSysParameterByParaAndCompany(0, "SYS2790");
                ProblemType problemType = new ProblemType()
                {
                    ProblemTypeCode = systUser.UserFixed,
                    ProblemTypeName = userParameter.Doc_LastNo,
                    SectionNo = section.SectionNo,
                    CompanyNo = site.CompanyNo
                };
                await _unitOfWork.ProblemTypeRepository.Add(problemType);
                await _unitOfWork.SaveChangesAsync();

                //Section section = _unitOfWork.SectionRepository.GetByCompany(site.CompanyNo).First();

                await _unitOfWork.SysParameter1Repository.GenCodeFormat(0, "SYS9998");
                userParameter = await _unitOfWork.SysParameter1Repository.GetSysParameterByParaAndCompany(0, "SYS9998");
                customer.CustomerCode = userParameter.Doc_LastNo;
                customer.CompanyNo = site.CompanyNo;
                customer.IsMaintainance = true;
                customer.IsActive = true;
                customer.IsDelete = false;
                customer.IsHeadCraft = false;
                customer.IsHeadSection = true;
                customer.Email = user.Email;
                customer.SectionNo = section.SectionNo;
                await _unitOfWork.CustomerRepository.Add(customer);
                await _unitOfWork.SaveChangesAsync();

                site.UserUnlock += 1;
                _unitOfWork.CompanyRepository.Update(site);

                SystUserCustomer systUserCustomer = new SystUserCustomer()
                {
                    CustomerNo = customer.CustomerNo,
                    UserNo = user.UserNo
                };
                await _unitOfWork.SystUserCustomerRepository.Add(systUserCustomer);
                await _unitOfWork.SaveChangesAsync();

                result.ErrMsg = "ลงทะเบียนเรียบร้อยแล้ว";
                Mail.SendEmail(_configuration, user, _logger, "IDYL Account Register");
                _unitOfWork.CommitTransaction();
            }
            catch (Exception e)
            {
                result.ErrMsg = e.Message;
                _unitOfWork.RollbackTransaction();
            }
            return result;
        }
    }
}
