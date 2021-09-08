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
    public class UserGroupService : IUserGroupService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserGroupService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

      

        public async Task Update(UserGroup userGroupMenu)
        {
            UserGroup userGroupMenu1 =  await _unitOfWork.UserGroupRepository.GetByUserGroup(userGroupMenu.UserGroupNo);
            userGroupMenu1.UserGroupNo = userGroupMenu.UserGroupNo;
            userGroupMenu1.UseMobile = userGroupMenu.UseMobile;
            userGroupMenu1.UseWeb = userGroupMenu.UseWeb;
            _unitOfWork.UserGroupRepository.Update(userGroupMenu1);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
