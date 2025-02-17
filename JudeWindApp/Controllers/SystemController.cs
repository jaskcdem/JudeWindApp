using Common;
using GreenUtility;
using JudeWind.Model.Base;
using JudeWind.Model.DbSystem;
using JudeWind.Service.DbSystem;
using JudeWind.Service.Register;
using JudeWindApp.Util;
using Microsoft.AspNetCore.Mvc;

namespace JudeWindApp.Controllers
{
    /// <summary> System Info </summary>
    [ApiController]
    [Route("Sys")]
    public class SystemController(DbsysService dbsysService, UserInfoService userInfoService, RoleService roleService, LoginService loginService, ICodeValidator codeValidator) : BaseApiController
    {
        private readonly DbsysService _dbsysService = dbsysService;
        private readonly UserInfoService _userInfoService = userInfoService;
        private readonly RoleService _roleService = roleService;
        private readonly LoginService _loginService = loginService;
        private readonly ICodeValidator _codeValidator = codeValidator;

        #region <-- Dbsys -->
        /// <summary> 取得系統類別 </summary>
        [HttpGet]
        [Route("Types/List")]
        public async Task<List<SysTypeOutput>> GetTypes() => await _dbsysService.GetTypes();
        /// <summary> 新增系統類別 </summary>
        [HttpPost]
        [Route("Types")]
        public async Task AddSysType(SysTypeInput input) => await _dbsysService.AddSysType(input);
        /// <summary> 編輯系統類別 </summary>
        [HttpPut]
        [Route("Types")]
        public async Task EditSysType(SysTypeInput input) => await _dbsysService.EditSysType(input);
        /// <summary> 關閉系統類別 </summary>
        [HttpDelete]
        [Route("Types")]
        public async Task DisableType(string typeName) => await _dbsysService.DisableType(typeName);

        /// <summary> 取得系統代碼 </summary>
        [HttpGet]
        [Route("Codes/List")]
        public async Task<GreenTree<SysCodeOutput>> GetCodes(string typeName) => GetSysCodeTree(await _dbsysService.GetCodes(typeName));
        /// <summary> 新增系統代碼 </summary>
        [HttpPost]
        [Route("Codes")]
        public async Task AddCode(SysCodeInput input) => await _dbsysService.AddCode(input);
        /// <summary> 編輯系統代碼 </summary>
        [HttpPut]
        [Route("Codes")]
        public async Task EditCode(SysCodeInput input) => await _dbsysService.EditCode(input);
        /// <summary> 關閉系統代碼 </summary>
        [HttpDelete]
        [Route("Codes")]
        public async Task DisableCode(SysCodeInput input) => await _dbsysService.DisableCode(input);

        /// <summary> 取得系統設定 </summary>
        [HttpGet]
        [Route("Settings/List")]
        public async Task<List<SysSettingOutput>> GetSettings(SysSettingPKey pKey) => await _dbsysService.GetSettings(pKey);
        /// <summary> 新增系統設定 </summary>
        [HttpPost]
        [Route("Settings")]
        public async Task AddSetting(SysSettingInput input) => await _dbsysService.AddSetting(input);
        /// <summary> 編輯系統設定 </summary>
        [HttpPut]
        [Route("Settings")]
        public async Task EditSetting(SysSettingInput input) => await _dbsysService.EditSetting(input);
        /// <summary> 移除系統設定 </summary>
        [HttpDelete]
        [Route("Settings")]
        public async Task RemoveSettings(SysSettingPKey pKey) => await _dbsysService.RemoveSettings(pKey);
        #endregion

        #region <-- UserInfo -->
        /// <summary> 取得使用者資料 </summary>
        /// <param name="userId">使用者Id</param>
        [HttpGet]
        [Route("User/List")]
        public async Task<UserInfoOutput> GetUserInfo([FromBody] string userId) => await _userInfoService.GetUserInfo(userId);
        /// <summary> 新增使用者 </summary>
        [HttpPost]
        [Route("User")]
        public async Task NewUser(UserInfoInput input) => await _userInfoService.NewUser(input, IPAddress);
        /// <summary> 編輯使用者 </summary>
        [HttpPut]
        [Route("User")]
        public async Task UpgradeUser(UserInfoInput input) => await _userInfoService.UpgradeUser(input);
        /// <summary> 使用者啟用/關閉 </summary>
        [HttpPatch]
        [Route("User")]
        public async Task CheckUserApply(UserApplyCheck applyCheck) => await _userInfoService.CheckUserApply(applyCheck);
        /// <summary> 使用者變更密碼 </summary>
        [HttpPost]
        [Route("User/ChangePw")]
        public async Task<bool> ChangePsw(ChangePswInput input) => await _userInfoService.ChangePsw(input);
        /// <summary> 使用者忘記密碼 </summary>
        /// <param name="userId">使用者Id</param>
        [HttpPost]
        [Route("User/ForgetPw")]
        public async Task<ForgetPswResult> ForgetPsw([FromBody] string userId) => await _userInfoService.ForgetPsw(userId);
        #endregion

        #region <-- Role -->
        /// <summary> 權限群組清單 </summary>
        [HttpGet]
        [Route("Roles/List")]
        public async Task<List<RoleOutput>> GetRoles() => await _roleService.GetRoles();
        /// <summary> 新增權限群組 </summary>
        [HttpPost]
        [Route("Roles")]
        public async Task AddRole(SysRoleInput input) => await _roleService.AddRole(input);
        /// <summary> 編輯權限群組 </summary>
        [HttpPut]
        [Route("Roles")]
        public async Task EditRole(SysRoleInput input) => await _roleService.EditRole(input);
        /// <summary> 移除權限群組 </summary>
        /// <param name="rgid">角色id</param>
        [HttpDelete]
        [Route("Roles")]
        public async Task DeleteRole([FromBody] string rgid) => await _roleService.DeleteRole(rgid);
        /// <summary> 是否重名 </summary>
        /// <param name="roleName">角色名稱</param>
        [HttpHead]
        [Route("Roles")]
        public async Task<bool> HadSameRoleName([FromBody] string roleName) => await _roleService.HadSameName(roleName);
        /// <summary> 是否有資料 </summary>
        /// <param name="rgid">角色id</param>
        [HttpHead]
        [Route("Roles/Exist")]
        public async Task<bool> IsRoleExist([FromBody] string rgid) => await _roleService.IsExist(rgid);
        /// <summary> 是否有使用者有此權限 </summary>
        /// <param name="rgid">角色id</param>
        [HttpHead]
        [Route("Roles/User")]
        public async Task<bool> HadUserInRole([FromBody] string rgid) => await _roleService.HadUserInRole(rgid);

        /// <summary> 取得使用者權限 </summary>
        /// <param name="userId">使用者Id</param>
        [HttpGet]
        [Route("UserRole/List")]
        public async Task<RoleUserOutput> GetUserRole([FromBody] string userId) => await _roleService.GetUserRole(userId);
        /// <summary> 更新使用者權限 </summary>
        [HttpPost]
        [Route("UserRole")]
        public async Task UpgradeUserRole(SysRoleUserInput input) => await _roleService.UpgradeUserRole(input);
        /// <summary> 取得使用者授權 </summary>
        /// <param name="userId">使用者Id</param>
        [HttpGet]
        [Route("UserRole")]
        public async Task<UserPermissionOutput> GetUserPermissionInfo([FromBody] string userId) => await _roleService.GetUserPermissionInfo(userId);
        /// <summary> 更新授權 </summary>
        [HttpPost]
        [Route("UserRole/Permission")]
        public async Task UpgradePermission(SysRolePermissionInput input) => await _roleService.UpgradePermission(input);

        /// <summary> 取得全部使用者授權 </summary>
        [HttpGet]
        [Route("UserRole/Permission")]
        public async Task<BasePageViewModel<UserPermissionOutput>> SearchUserPermissionInfo(int page = SysSetting.PageDefult, int size = SysSetting.SizeDefult)
        {
            var _data = await _roleService.SearchUserPermissionInfo();
            BasePageViewModel<UserPermissionOutput> result = new() { Detail = _data, CurPage = page, Size = size };
            result.Pagging();
            return result;
        }
        /// <summary> 取得全部使用者權限 </summary>
        [HttpGet]
        [Route("UserRole/Full")]
        public async Task<BasePageViewModel<RoleUserOutput>> SearchUserRole(int page = SysSetting.PageDefult, int size = SysSetting.SizeDefult)
        {
            var _data = await _roleService.SearchUserRole();
            BasePageViewModel<RoleUserOutput> result = new() { Detail = _data, CurPage = page, Size = size };
            result.Pagging();
            return result;
        }
        #endregion

        #region <-- Login -->
        /// <summary> 使用者登入 </summary>
        [HttpPost]
        [Route("Login")]
        public async Task<LoginResult> UserLogin(UserLogin user)
        {
            user.Ip = IPAddress;
            return await _loginService.UserLogin(user);
        }
        #endregion

        /// <summary> 產生驗證碼 </summary>
        [HttpGet]
        [Route("Vcode/generate")]
        public ActionResult<string> Generate() => Ok(_codeValidator.Generate());

        /// <summary> 比對驗證碼 </summary>
        [HttpGet]
        [Route("Vcode/validate/{code}")]
        public ActionResult Validate(string code) => _codeValidator.Validate(code) ? Ok() : BadRequest();

        /// <summary> 取得系統代碼樹 </summary>
        static GreenTree<SysCodeOutput> GetSysCodeTree(List<SysCodeOutput> codes)
        {
            //JudeTree<SysCodeOutput> codeTree = new();
            List<(int level, List<GreenTreeNode<SysCodeOutput>> nodes)> codeTree = [];
            int _floor = 1, maxFloor = codes.Max(c => c.Level);
            while (_floor <= maxFloor)
            {
                var floorCodes = codes.Where(c => c.Level == _floor).OrderBy(c => c.ParentPath).ThenBy(c => c.Code);
                if (_floor == 1) codeTree.Add((_floor, floorCodes.Select(x => new GreenTreeNode<SysCodeOutput>(x)).ToList()));
                else
                {
                    var parentList = codeTree.Where(x => x.level == (_floor - 1)).SelectMany(x => x.nodes);
                    foreach (var parent in parentList)
                    {
                        string _parentsPath = $"{parent.Data.ParentPath}/{parent.Data.Code}";
                        var _childs = floorCodes.Where(c => c.ParentPath == $"{_parentsPath}/{c.Code}");
                        foreach (var child in _childs)
                            parent.AddLastChild(new GreenTreeNode<SysCodeOutput>(child) { Parent = parent });
                        codeTree.Add((_floor, _childs.Select(x => new GreenTreeNode<SysCodeOutput>(x)).ToList()));
                    }
                }
                _floor++;
            }
            return new() { Trees = codeTree.Where(x => x.level == 1).SelectMany(x => x.nodes).ToList() };
        }
    }
}
