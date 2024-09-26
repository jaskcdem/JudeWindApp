using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudeWind.Service.Register
{
    public class UserInfoService : BaseService
    {
        public UserInfoService() { }

        public async Task<string[]> GetUserPermission(string _userId) => [];
        public async Task<bool> IsRole(string _userid, string _rgId) => true;
        //public async Task<string[]> GetUserPermission(string _userId)
        //{
        //    var _Permission = await _db.View_UserPermission.AsNoTracking().AsAsyncEnumerable().Where(c => c.UserId == _userId).FirstOrDefaultAsync();
        //    return _Permission.Permission.Split(',');
        //}
        //public async Task<bool> IsRole(string _userid, string _rgId) =>
        //  await _db.SYS_RoleGroup.LeftJoin(_db.SYS_RoleGroupUser, rg => rg.RG_Id, rgu => rgu.RG_Id, (rg, rgu) => new { rg, rgu })
        //        .AnyAsync(x => x.rgu.U_UserId == _userid && x.rg.RG_Id == _rgId);
    }
}
