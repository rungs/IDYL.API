using IdylAPI.Models.Master;
using IdylAPI.Models.Syst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IdylAPI.Models.Authorize
{
    public class Login
    {

    }
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponse
    {
        public LoginResponse()
        {
            Token = "";
            Permission = null;
            PermissionData = null;
            PermissionAction = null;
            UserGroupPermissions = null;
            Lang = "";
            ServerAddress = "";
            UserGroupDefaultInfo = new UserGroupDefaultInfo();
            SectionNo = 0;
            CanRegister = "F";
            CanReset = "F";
            UserInfo = new User();
        }

        public string Token { get; set; }
        public string Lang { get; set; }
        public string ServerAddress { get; set; }
        public int SectionNo { get; set; }
        public string UserLogin { get; set; }
        public string UserGroupCode { get; set; }
        public bool SuperUser { get; set; }
        public string CanRegister { get; set; }
        public string CanReset { get; set; }

        public IEnumerable<FormPermission> Permission { get; set; }
        public IEnumerable<FormPermissionData> PermissionData { get; set; }
        public IEnumerable<FormPermissionAction> PermissionAction { get; set; }
        public IEnumerable<UserGroupPermission> UserGroupPermissions{ get; set; }
        public UserGroupDefaultInfo UserGroupDefaultInfo { get; set; }
        public User UserInfo { get; set; }
    }

    public class UserGroupDefaultInfo
    {
        public Location Location { get; set; }
        public Section Section { get; set; }
        public ProblemType ProblemType { get; set; }
        public Customer Customer { get; set; }
    }
}
