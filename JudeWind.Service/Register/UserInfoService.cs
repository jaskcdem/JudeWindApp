using Common;
using DataAcxess.ProjectContext;
using GreenUtility.Extension;
using JudeWind.Model.DbSystem;

namespace JudeWind.Service.Register
{
    public class UserInfoService(ProjectContext projectContext) : BaseService(projectContext)
    {
        public async Task<string[]> GetUserPermission(string _userId)
        {
            var _query = _db.UserInfo.AsAsyncEnumerable()
                .Join(_db.RoleGroupUser.AsAsyncEnumerable(), u => u.ID, ru => ru.UserId, (u, ru) => (UserId: u.ID, RoleId: ru.RGId))
                .Join(_db.RoleGroupPermission.AsAsyncEnumerable(), ur => ur.RoleId, rp => rp.RGId, (ur, rp) => (ur.UserId, ur.RoleId, Permission: rp.MFFuncId))
                .Where(c => c.UserId == _userId);
            var _Permission = await _query.Select(x => x.Permission).ToListAsync();
            return [.. _Permission];
        }
        public async Task<bool> IsRole(string _userid, string _rgId) =>
            await _db.RoleGroup.AsAsyncEnumerable().Join(_db.RoleGroupUser.AsAsyncEnumerable(), rg => rg.Id, rgu => rgu.RGId, (rg, rgu) => new { rg, rgu })
                .AnyAsync(x => x.rgu.UserId == _userid && x.rg.Id == _rgId);

        /// <summary> 是否有資料 </summary>
        public async Task<bool> IsExist(string userId) => await _db.UserInfo.AsAsyncEnumerable().AnyAsync(u => u.ID == userId);

        #region << Base >>
        /// <summary> 新增使用者 </summary>
        public async Task NewUser(UserInfoInput input, string _ip)
        {
            if (await IsExist(input.UserId)) return;
            CryptographyHelper cryptography = new(input.Psw);
            _db.UserInfo.Add(new UserInfo
            {
                ID = input.UserId,
                Name = input.Name,
                Psw = cryptography.Encode(),
                Idno = input.Idno,
                Email = input.Email ?? string.Empty,
                LastLoginIP = _ip,
                Check = true,
                CheckDate = DateTime.Now,
            });
            await _db.SaveChangesAsync();
        }
        /// <summary> 編輯使用者 </summary>
        public async Task UpgradeUser(UserInfoInput input)
        {
            var entry = await _db.UserInfo.FindAsync(input.UserId);
            if (entry != null)
            {
                entry.Name = input.Name;
                entry.Idno = input.Idno;
                entry.Email = input.Email ?? string.Empty;
                entry.UpdateUser = input.ModifyUserId ?? input.UserId;
                entry.UpdateDataTime = DateTime.Now;
                _db.Update(entry);
                await _db.SaveChangesAsync();
            }
        }
        /// <summary> 取得使用者資料 </summary>
        public async Task<UserInfoOutput> GetUserInfo(string userId)
        {
            UserInfoOutput userInfo = new() { UserId = userId };
            var entry = await _db.UserInfo.FindAsync(userId);
            if (entry != null)
            {
                userInfo.Name = entry.Name;
                userInfo.Idno = entry.Idno;
                userInfo.Email = entry.Email;
            }
            return userInfo;
        }
        #endregion
        #region << Review >>
        /// <summary> 使用者啟用/關閉 </summary>
        public async Task CheckUserApply(UserApplyCheck applyCheck)
        {
            var entry = await _db.UserInfo.FindAsync(applyCheck.UserId);
            if (entry != null)
            {
                entry.Check = applyCheck.Check;
                entry.CheckDate = DateTime.Now;
                _db.Update(entry);
                await _db.SaveChangesAsync();
            }
        }
        #endregion
        #region << Passward >>
        /// <summary> 使用者變更密碼 </summary>
        public async Task<bool> ChangePsw(ChangePswInput input)
        {
            var entry = await _db.UserInfo.FindAsync(input.UserId);
            bool changed = false;
            if (entry != null)
            {
                CryptographyHelper cryptography = new(input.OldPassward);
                if (cryptography.Encode() == entry.Psw)
                {
                    entry.Psw = cryptography.Encode(input.NewPassward);
                    _db.Update(entry);
                    changed = await _db.SaveChangesAsync() > 0;
                }
            }
            return changed;
        }
        /// <summary> 使用者忘記密碼 </summary>
        public async Task<ForgetPswResult> ForgetPsw(string userId)
        {
            var entry = await _db.UserInfo.FindAsync(userId);
            ForgetPswResult result = new() { UserId = userId };
            if (entry != null)
            {
                CryptographyHelper cryptography = new($"{Guid.NewGuid()}".Replace("-", "")[..SysSetting.PasswardLength]);
                entry.Psw = cryptography.Encode();
                _db.Update(entry);
                await _db.SaveChangesAsync();
                result.NewPassward = cryptography.Decrypt();
            }
            return result;
        }
        #endregion
    }
}
