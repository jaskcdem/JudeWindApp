using Common;
using JudeWind.Model.Base;
using JudeWind.Model.DbSystem;
using JudeWind.Service.DbSystem;
using JudeWind.Service.Register;
using Microsoft.AspNetCore.Mvc;

namespace JudeWindApp.Controllers
{
    /// <summary> System Info </summary>
    public class SystemController(DbsysService dbsysService, UserInfoService userInfoService, RoleService roleService, LoginService loginService) : BaseApiController
    {
        private readonly DbsysService _dbsysService = dbsysService;
        private readonly UserInfoService _userInfoService = userInfoService;
        private readonly RoleService _roleService = roleService;
        private readonly LoginService _loginService = loginService;

        #region <-- Dbsys -->
        /// <summary> 取得系統類別 </summary>
        [HttpGet]
        public async Task<List<SysTypeOutput>> GetTypes() => await _dbsysService.GetTypes();
        /// <summary> 新增系統類別 </summary>
        [HttpPost]
        public async Task AddSysType(SysTypeInput input) => await _dbsysService.AddSysType(input);
        /// <summary> 編輯系統類別 </summary>
        [HttpPut]
        public async Task EditSysType(SysTypeInput input) => await _dbsysService.EditSysType(input);
        /// <summary> 關閉系統類別 </summary>
        [HttpDelete]
        public async Task DisableType(string typeName) => await _dbsysService.DisableType(typeName);

        /// <summary> 取得系統代碼 </summary>
        [HttpGet]
        public async Task<JudeTree<SysCodeOutput>> GetCodes(string typeName) => GetSysCodeTree(await _dbsysService.GetCodes(typeName));
        /// <summary> 新增系統代碼 </summary>
        [HttpPost]
        public async Task AddCode(SysCodeInput input) => await _dbsysService.AddCode(input);
        /// <summary> 編輯系統代碼 </summary>
        [HttpPut]
        public async Task EditCode(SysCodeInput input) => await _dbsysService.EditCode(input);
        /// <summary> 關閉系統代碼 </summary>
        [HttpDelete]
        public async Task DisableCode(SysCodeInput input) => await _dbsysService.DisableCode(input);

        /// <summary> 取得系統設定 </summary>
        [HttpGet]
        public async Task<List<SysSettingOutput>> GetSettings(SysSettingPKey pKey) => await _dbsysService.GetSettings(pKey);
        /// <summary> 新增系統設定 </summary>
        [HttpPost]
        public async Task AddSetting(SysSettingInput input) => await _dbsysService.AddSetting(input);
        /// <summary> 編輯系統設定 </summary>
        [HttpPut]
        public async Task EditSetting(SysSettingInput input) => await _dbsysService.EditSetting(input);
        /// <summary> 移除系統設定 </summary>
        [HttpDelete]
        public async Task RemoveSettings(SysSettingPKey pKey) => await _dbsysService.RemoveSettings(pKey);
        #endregion

        #region <-- UserInfo -->
        /// <summary> 取得使用者資料 </summary>
        /// <param name="userId">使用者Id</param>
        [HttpGet]
        public async Task<UserInfoOutput> GetUserInfo([FromBody] string userId) => await _userInfoService.GetUserInfo(userId);
        /// <summary> 新增使用者 </summary>
        [HttpPost]
        public async Task NewUser(UserInfoInput input) => await _userInfoService.NewUser(input, IPAddress);
        /// <summary> 編輯使用者 </summary>
        [HttpPut]
        public async Task UpgradeUser(UserInfoInput input) => await _userInfoService.UpgradeUser(input);
        /// <summary> 使用者啟用/關閉 </summary>
        [HttpPatch]
        public async Task CheckUserApply(UserApplyCheck applyCheck) => await _userInfoService.CheckUserApply(applyCheck);
        /// <summary> 使用者變更密碼 </summary>
        [HttpPost]
        public async Task<bool> ChangePsw(ChangePswInput input) => await _userInfoService.ChangePsw(input);
        /// <summary> 使用者忘記密碼 </summary>
        /// <param name="userId">使用者Id</param>
        [HttpPost]
        public async Task<ForgetPswResult> ForgetPsw([FromBody] string userId) => await _userInfoService.ForgetPsw(userId);
        #endregion

        #region <-- Role -->
        /// <summary> 權限群組清單 </summary>
        [HttpGet]
        public async Task<List<RoleOutput>> GetRoles() => await _roleService.GetRoles();
        /// <summary> 新增權限群組 </summary>
        [HttpPost]
        public async Task AddRole(SysRoleInput input) => await _roleService.AddRole(input);
        /// <summary> 編輯權限群組 </summary>
        [HttpPut]
        public async Task EditRole(SysRoleInput input) => await _roleService.EditRole(input);
        /// <summary> 移除權限群組 </summary>
        /// <param name="rgid">角色id</param>
        [HttpDelete]
        public async Task DeleteRole([FromBody] string rgid) => await _roleService.DeleteRole(rgid);
        /// <summary> 是否重名 </summary>
        /// <param name="roleName">角色名稱</param>
        [HttpHead]
        public async Task<bool> HadSameRoleName([FromBody] string roleName) => await _roleService.HadSameName(roleName);
        /// <summary> 是否有資料 </summary>
        /// <param name="rgid">角色id</param>
        [HttpHead]
        public async Task<bool> IsRoleExist([FromBody] string rgid) => await _roleService.IsExist(rgid);
        /// <summary> 是否有使用者有此權限 </summary>
        /// <param name="rgid">角色id</param>
        [HttpHead]
        public async Task<bool> HadUserInRole([FromBody] string rgid) => await _roleService.HadUserInRole(rgid);

        /// <summary> 取得使用者權限 </summary>
        /// <param name="userId">使用者Id</param>
        [HttpGet]
        public async Task<RoleUserOutput> GetUserRole([FromBody] string userId) => await _roleService.GetUserRole(userId);
        /// <summary> 更新使用者權限 </summary>
        [HttpPost]
        public async Task UpgradeUserRole(SysRoleUserInput input) => await _roleService.UpgradeUserRole(input);
        /// <summary> 取得使用者授權 </summary>
        /// <param name="userId">使用者Id</param>
        [HttpGet]
        public async Task<UserPermissionOutput> GetUserPermissionInfo([FromBody] string userId) => await _roleService.GetUserPermissionInfo(userId);
        /// <summary> 更新授權 </summary>
        [HttpPost]
        public async Task UpgradePermission(SysRolePermissionInput input) => await _roleService.UpgradePermission(input);

        /// <summary> 取得全部使用者授權 </summary>
        [HttpGet]
        public async Task<BaseOutput<UserPermissionOutput>> SearchUserPermissionInfo(int page = SysSetting.PageDefult, int size = SysSetting.SizeDefult)
        {
            var _data = await _roleService.SearchUserPermissionInfo();
            BaseOutput<UserPermissionOutput> result = new() { Detail = _data, Page = page, Size = size };
            result.Pagging();
            return result;
        }
        /// <summary> 取得全部使用者權限 </summary>
        [HttpGet]
        public async Task<BaseOutput<RoleUserOutput>> SearchUserRole(int page = SysSetting.PageDefult,int size = SysSetting.SizeDefult)
        {
            var _data = await _roleService.SearchUserRole();
            BaseOutput<RoleUserOutput> result = new() { Detail = _data, Page = page, Size = size };
            result.Pagging();
            return result;
        }
        #endregion

        #region <-- Login -->
        /// <summary> 使用者登入 </summary>
        [HttpPost]
        public async Task<LoginResult> UserLogin(UserLogin user)
        {
            user.Ip = IPAddress;
            return await _loginService.UserLogin(user);
        }
        #endregion

        /// <summary> 取得系統代碼樹 </summary>
        static JudeTree<SysCodeOutput> GetSysCodeTree(List<SysCodeOutput> codes)
        {
            //JudeTree<SysCodeOutput> codeTree = new();
            List<(int level, List<JudeTreeNode<SysCodeOutput>> nodes)> codeTree = [];
            int _floor = 1, maxFloor = codes.Max(c => c.Level);
            while (_floor <= maxFloor)
            {
                var floorCodes = codes.Where(c => c.Level == _floor).OrderBy(c => c.ParentPath).ThenBy(c => c.Code);
                if (_floor == 1) codeTree.Add((_floor, floorCodes.Select(x => new JudeTreeNode<SysCodeOutput>(x)).ToList()));
                else
                {
                    var parentList = codeTree.Where(x => x.level == (_floor - 1)).SelectMany(x => x.nodes);
                    foreach (var parent in parentList)
                    {
                        string _parentsPath = $"{parent.Data.ParentPath}/{parent.Data.Code}";
                        var _childs = floorCodes.Where(c => c.ParentPath == $"{_parentsPath}/{c.Code}");
                        foreach (var child in _childs)
                            parent.AddLastChild(new JudeTreeNode<SysCodeOutput>(child) { Parent = parent });
                        codeTree.Add((_floor, _childs.Select(x => new JudeTreeNode<SysCodeOutput>(x)).ToList()));
                    }
                }
                _floor++;
            }
            return new() { Trees = codeTree.Where(x => x.level == 1).SelectMany(x => x.nodes).ToList() };
        }
    }
}
