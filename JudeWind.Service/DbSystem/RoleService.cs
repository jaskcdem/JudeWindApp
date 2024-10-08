using Common;
using DataAcxess.ProjectContext;
using JudeWind.Model.DbSystem;
using JudeWind.Service.Register;
using Microsoft.EntityFrameworkCore;

namespace JudeWind.Service.DbSystem
{
    public class RoleService(ProjectContext projectContext, UserInfoService userInfoService) : BaseService(projectContext)
    {
        private UserInfoService _userInfoService = userInfoService;

        #region << Role >>
        /// <summary> 新增權限群組 </summary>
        public async Task AddRole(SysRoleInput input)
        {
            if (!await HadSameName(input.Name)) return;
            _db.RoleGroup.Add(new Sys_RoleGroup
            {
                Id = Guid.NewGuid().ToString(),
                Name = input.Name,
                System = SysSetting.SysName,
                CreateUserId = input.UserId,
            });
            await _db.SaveChangesAsync();
        }

        /// <summary> 編輯權限群組 </summary>
        public async Task EditRole(SysRoleInput input)
        {
            var _role = await _db.RoleGroup.AsAsyncEnumerable().FirstOrDefaultAsync(r => r.Id == input.RGId);
            if (_role != null)
            {
                _role.Name = input.Name;
                _role.UpdateUserId = input.UserId;
                _role.UpdateDateTime = DateTime.Now;
                _db.Entry(_role).State = EntityState.Modified;
                await _db.SaveChangesAsync();
            }
        }

        /// <summary> 權限群組清單 </summary>
        public async Task<List<RoleOutput>> GetRoles() => await _db.RoleGroup.AsAsyncEnumerable()
            .Select(x => new RoleOutput { Name = x.Name, RGId = x.Id }).ToListAsync();

        /// <summary> 移除權限群組 </summary>
        public async Task DeleteRole(string rgid)
        {
            if (await IsExist(rgid) && !await HadUserInRole(rgid))
            {
                var _permission = await _db.RoleGroupPermission.AsAsyncEnumerable().Where(x => x.RGId == rgid).ToListAsync();
                _db.RemoveRange(_permission);
                await _db.SaveChangesAsync();

                var _role = await GetRoleById(rgid);
                _db.Remove(_role);
                await _db.SaveChangesAsync();
            }
        }

        /// <summary> 是否重名 </summary>
        public async Task<bool> HadSameName(string roleName) => await _db.RoleGroup.AsAsyncEnumerable().AnyAsync(r => r.Name == roleName);
        /// <summary> 是否有資料 </summary>
        public async Task<bool> IsExist(string rgid) => await _db.RoleGroup.AsAsyncEnumerable().AnyAsync(r => r.Id == rgid);
        /// <summary> 是否有使用者有此權限 </summary>
        public async Task<bool> HadUserInRole(string rgid) => await _db.RoleGroupUser.AsAsyncEnumerable().AnyAsync(r => r.RGId == rgid);
        #endregion

        #region << UserRole >>
        /// <summary> 更新使用者權限 </summary>
        public async Task UpgradeUserRole(SysRoleUserInput input)
        {
            if (await _userInfoService.IsExist(input.UserId))
            {
                List<Sys_RoleGroupUser> _news = [];
                var userRoles = await GetUserRole(input.UserId);
                var _removeIDs = userRoles.Roles.Where(x => !input.RGId.Contains(x.RGId)).Select(x => x.RGId);
                var _removes = await _db.RoleGroupUser.AsAsyncEnumerable().Where(x => _removeIDs.Contains(x.RGId)).ToListAsync();
                foreach (string rgid in input.RGId)
                {
                    if (!await _userInfoService.IsRole(input.UserId, rgid))
                        _news.Add(new Sys_RoleGroupUser
                        {
                            UserId = input.UserId,
                            RGId = rgid,
                            RG_ = await GetRoleById(rgid)
                        });
                }
                _db.AddRange(_news);
                _db.RemoveRange(_removes);
                await _db.SaveChangesAsync();
            }
        }
        /// <summary> 取得使用者權限 </summary>
        public async Task<RoleUserOutput> GetUserRole(string userId)
        {
            var _result = new RoleUserOutput { UserId = userId };
            if (await _userInfoService.IsExist(userId))
            {
                _result.Roles = await _db.RoleGroup.AsAsyncEnumerable()
                    .Join(_db.RoleGroupUser.AsAsyncEnumerable(), r => r.Id, ru => ru.RGId, (r, ru) => (RGId: r.Id, r.Name, User: ru.UserId))
                    .Where(x => x.User == userId).Select(x => new RoleOutput { RGId = x.RGId, Name = x.Name }).ToListAsync();
            }
            return _result;
        }
        #endregion

        #region << Permission >>
        /// <summary> 更新授權 </summary>
        public async Task UpgradePermission(SysRolePermissionInput input)
        {
            if (await IsExist(input.RGId))
            {
                List<Sys_RoleGroupPermission> _news = [];
                var _removes = await _db.RoleGroupPermission.AsAsyncEnumerable()
                    .Where(z => !input.Permissions.Select(p => p.MFFuncId).Contains(z.MFFuncId)).ToListAsync();
                foreach (var mf in input.Permissions)
                {
                    if (!await _db.RoleGroupPermission.AsAsyncEnumerable().AnyAsync(z => z.MFFuncId == mf.MFFuncId))
                        _news.Add(new Sys_RoleGroupPermission
                        {
                            RGId = input.RGId,
                            MFSystem = mf.MFSystem,
                            MFFuncId = mf.MFFuncId,
                            Sys_RoleGroup = await GetRoleById(input.RGId)
                        });
                }
                _db.AddRange(_news);
                _db.RemoveRange(_removes);
                await _db.SaveChangesAsync();
            }
        }
        /// <summary> 取得使用者授權 </summary>
        public async Task<UserPermissionOutput> GetUserPermissionInfo(string userId)
        {
            UserPermissionOutput userPermission = new() { UserId = userId };
            if (await _userInfoService.IsExist(userId))
            {
                var _query = _db.RoleGroup.AsAsyncEnumerable()
                    .Join(_db.RoleGroupUser.AsAsyncEnumerable(), r => r.Id, ru => ru.UserId, (r, ru) => (Role: r, RoleUser: ru))
                    .Join(_db.RoleGroupPermission.AsAsyncEnumerable(), ur => ur.Role.Id, rp => rp.RGId, (ur, rp) => (ur.Role, ur.RoleUser, Permission: rp))
                    .Where(c => c.RoleUser.UserId == userId);
                userPermission.Permissions = await _query.Select(p => new RolePermissionOutput
                {
                    RGId = p.Role.Id,
                    Name = p.Role.Name,
                    MFSystem = p.Permission.MFSystem,
                    MFFuncId = p.Permission.MFSystem,
                }).ToListAsync();
            }
            return userPermission;
        }
        #endregion

        /// <summary> 取得全部使用者授權 </summary>
        public async Task<List<UserPermissionOutput>> SearchUserPermissionInfo()
            => await _db.UserInfo.AsAsyncEnumerable().Select(u => GetUserPermissionInfo(u.ID).GetAwaiter().GetResult()).ToListAsync();
        /// <summary> 取得全部使用者權限 </summary>
        public async Task<List<RoleUserOutput>> SearchUserRole()
            => await _db.UserInfo.AsAsyncEnumerable().Select(u => GetUserRole(u.ID).GetAwaiter().GetResult()).ToListAsync();

        async Task<Sys_RoleGroup> GetRoleById(string rgid) => await _db.RoleGroup.AsAsyncEnumerable().FirstAsync(r => r.Id == rgid);
    }
}
