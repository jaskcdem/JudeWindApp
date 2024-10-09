using Common;
using DataAcxess.ProjectContext;
using GreenUtility.Extension;
using JudeWind.Model.DbSystem;
using JudeWind.Service.DbSystem;
using System.Net;

namespace JudeWind.Service.Register
{
    public class LoginService(ProjectContext projectContext, UserInfoService userInfoService, RoleService roleService) : BaseService(projectContext)
    {
        private UserInfoService _userInfoService = userInfoService;
        private RoleService _roleService = roleService;
        const string VertifyFailMessage = "帳號或密碼錯誤", FailThreeTimesMessage = "密碼錯誤超過3次";
        const int WaitingMinutes = 15;

        /// <summary> 使用者登入 </summary>
        public async Task<LoginResult> UserLogin(UserLogin user)
        {
            var entry = await _db.UserInfo.FindAsync(user.UserId);
            LoginResult result = new() { User = new() { UserId = user.UserId } };
            if (entry == null)
            {
                result.HttpCode = (int)HttpStatusCode.Unauthorized;
                result.Message = VertifyFailMessage;
                return result;
            }

            if (entry.LoginFailedCount >= 3 && entry.LastLoginTime.HasValue && DateTime.Now.Subtract(entry.LastLoginTime.Value).TotalMinutes < WaitingMinutes)
            {
                result.HttpCode = (int)HttpStatusCode.Unauthorized;
                result.Message = FailThreeTimesMessage;
                return result;
            }

            CryptographyHelper cryptography = new(user.Password);
            if (cryptography.Encode() != entry.Psw)
            {
                entry.LoginFailedCount++;
                _db.Update(entry);
                await _db.SaveChangesAsync();
                result.HttpCode = (int)HttpStatusCode.Unauthorized;
                result.Message = VertifyFailMessage;
                return result;
            }

            entry.LoginFailedCount = 0;
            entry.LastLoginIP = user.Ip ?? SysSetting.DefaultIp;
            entry.LastLoginTime = DateTime.Now;
            _db.Update(entry);
            await _db.SaveChangesAsync();

            result.User.Name = entry.Name;
            result.HttpCode = (int)HttpStatusCode.OK;
            var userRole = await _roleService.GetUserRole(user.UserId);
            var userPermission = await _roleService.GetUserPermissionInfo(user.UserId);
            result.User.Roles = [.. userRole.Roles.Distinct()];
            result.User.Permissions = [.. userPermission.Permissions.Select(p => new RolePermissionInput { MFSystem = p.MFSystem, MFFuncId = p.MFFuncId })
                .Distinct().OrderBy(m => m.MFFuncId)];
            return result;
        }

    }
}
