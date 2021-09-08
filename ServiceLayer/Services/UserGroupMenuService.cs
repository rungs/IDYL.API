using Domain.Interfaces;
using IdylAPI.Models.Master;
using IdylAPI.Models.Syst;
using IdylAPI.Services.Interfaces;
using IdylAPI.Services.Interfaces.Syst;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystUser = IdylAPI.Models.Syst.SystUser;

namespace SocialMedia.Core.Services
{
    public class UserGroupMenuService : IUserGroupMenuService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserGroupMenuService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<UserGroupMenu> GetByUserGroup(int userGroupId)
        {
            return _unitOfWork.UserGroupMenuRepository.GetByUserGroup(userGroupId);
        }
    
        public async Task Update(UserGroupMenu userGroupMenu)
        {
            UserGroupMenu userGroupMenu1 =  await _unitOfWork.UserGroupMenuRepository.GetByMenuUserGroup(userGroupMenu.UserGroupNo, userGroupMenu.MenuId);
            userGroupMenu1.UserGroupNo = userGroupMenu.UserGroupNo;
            userGroupMenu1.MenuId = userGroupMenu.MenuId;
            userGroupMenu1.IsView = userGroupMenu.IsView;
            _unitOfWork.UserGroupMenuRepository.Update(userGroupMenu1);
            await _unitOfWork.SaveChangesAsync();
        }
        //public async Task UnlockUser(int userGroupId, int noOfUser, int companyNo)
        //{
        //    try
        //    {
        //        _unitOfWork.BeginTransaction();
        //        _unitOfWork.SystUserRepository.GenCodeFormat("SYS9998", 0);
        //        Site site = await _unitOfWork.CompanyRepository.GetById(companyNo);
        //        DateTime updatedDate = DateTime.Now;
        //        for (int i = 0; i < noOfUser; i++)
        //        {
        //            string uDemo = $"Demo-{i}";

        //            await _unitOfWork.SysParameter1Repository.GenCodeFormat(0, "SYS9998");
        //            SysParameter1 sysParameter1 = await _unitOfWork.SysParameter1Repository.GetSysParameterByParaAndCompany(0, "SYS9998");
        //            Customer customer = new Customer()
        //            {
        //                CompanyNo = companyNo,
        //                Firstname = uDemo,
        //                Lastname = "Lastname",
        //                IsMaintainance = false,
        //                Rate = 300,
        //                CustomerCode = sysParameter1.Doc_LastNo,
        //                CreatedDate = updatedDate,
        //                UpdatedDate = updatedDate
        //            };
        //            await _unitOfWork.CustomerRepository.Add(customer);
        //            await _unitOfWork.SaveChangesAsync();

        //            _unitOfWork.SystUserRepository.GenCodeFormat("SYS9999", 0);
        //            SysParameter1 userParameter = await _unitOfWork.SysParameter1Repository.GetSysParameterByParaAndCompany(0, "SYS9999");

        //            SystUser user = new SystUser()
        //            {
        //                Password = "cfcd208495d565ef66e7dff9f98764da",
        //                CompanyNo = companyNo,
        //                PINCode = "cfcd208495d565ef66e7dff9f98764da",
        //                ExpiredDate = site.ExpiredDate.HasValue ? site.ExpiredDate.Value : updatedDate.AddDays(30),
        //                Username = userParameter.Doc_LastNo,
        //                UserFixed = userParameter.Doc_LastNo,
        //                UnlockCode = "1",
        //                IsLogin = true,
        //                IsDelete = false,
        //                IsActive = true,
        //                CreatedDate = updatedDate,
        //                UpdatedDate = updatedDate,
        //                IsSuperUser = false,
        //                UserGroupId = userGroupId
        //            };

        //            await _unitOfWork.SystUserRepository.Add(user);
        //            await _unitOfWork.SaveChangesAsync();

        //            SystUser systUser = await _unitOfWork.SystUserRepository.GetById(user.UserNo);
        //            systUser.UnlockCode = ((user.UserNo + Convert.ToInt32(userParameter.Doc_LastNo.Replace("U", ""))) * 3.5).ToString();
        //            _unitOfWork.SystUserRepository.Update(systUser);

        //            IEnumerable<Menu> menus = _unitOfWork.SystMenuRepository.GetAll();
        //            foreach (var item in menus)
        //            {
        //                FormPermission formPermission = new FormPermission()
        //                {
        //                    UserNo = user.UserNo,
        //                    MenuId = item.MenuID,
        //                    IsView = !item.FlagAdminOnly
        //                };
        //            }

        //            IEnumerable<SystAction> systActions = _unitOfWork.SystActionRepository.GetAll();
        //            foreach (var item in systActions)
        //            {
        //                FormPermissionAction formPermission = new FormPermissionAction()
        //                {
        //                    UserNo = user.UserNo,
        //                    MenuId = item.MenuID,
        //                    ActionId = item.ActionID,
        //                    IsActive = !item.FlagAdminOnly
        //                };
        //                await _unitOfWork.SystPermissionsActionRepository.Add(formPermission);
        //            }

        //            FormPermissionData formPermissionData = new FormPermissionData()
        //            {
        //                UserId = user.UserNo,
        //                MenuId = 13,
        //                Type = "A"
        //            };
        //            await _unitOfWork.SystPermissionsDataRepository.Add(formPermissionData);
        //            formPermissionData = new FormPermissionData()
        //            {
        //                UserId = user.UserNo,
        //                MenuId = 16,
        //                Type = "A"
        //            };
        //            await _unitOfWork.SystPermissionsDataRepository.Add(formPermissionData);
        //            formPermissionData = new FormPermissionData()
        //            {
        //                UserId = user.UserNo,
        //                MenuId = 22,
        //                Type = "A"
        //            };
        //            await _unitOfWork.SystPermissionsDataRepository.Add(formPermissionData);

        //            SystUserCustomer systUserCustomer = new SystUserCustomer()
        //            {
        //                UserNo = user.UserNo,
        //                CustomerNo = customer.CustomerNo
        //            };

        //            await _unitOfWork.SystUserCustomerRepository.Add(systUserCustomer);
        //            await _unitOfWork.SaveChangesAsync();
        //        }

        //        site.UserUnlock += noOfUser;
        //        _unitOfWork.CompanyRepository.Update(site);
        //        await _unitOfWork.SaveChangesAsync();
        //        _unitOfWork.CommitTransaction();
        //    }

        //    catch (Exception e)
        //    {
        //        _unitOfWork.RollbackTransaction();
        //        throw e;
        //    }
        //}

        //public async Task Update(int id, SystUser obj)
        //{
        //    SystUser systUser = await _unitOfWork.SystUserRepository.GetById(id);
        //    systUser.Username = obj.Username;
        //    systUser.ExpiredDate = obj.ExpiredDate;
        //    systUser.IsActive = obj.IsActive;
        //    systUser.IsDelete = obj.IsDelete;
        //    systUser.IsSuperUser = obj.IsSuperUser;
        //    systUser.UserGroupId = obj.UserGroupId;
        //    systUser.UpdatedDate = DateTime.Now;
        //    _unitOfWork.SystUserRepository.Update(systUser);
        //    await _unitOfWork.SaveChangesAsync();
        //}
    }
}
