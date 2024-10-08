using DataAcxess.ProjectContext;
using JudeWind.Model.DbSystem;
using Org.BouncyCastle.Tsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudeWind.Service.DbSystem
{
    public class DbsysService(ProjectContext projectContext) : BaseService(projectContext)
    {
        #region << SysType >>
        /// <summary> 新增系統類別 </summary>
        public async Task AddSysType(SysTypeInput input)
        {
            if (IsTypeExist(input.Type)) return;
            _db.SysType.Add(new Sys_Type { TYPE = input.Type, DESC = input.Desc, EDITOR = input.Editor });
            await _db.SaveChangesAsync();
        }
        /// <summary> 編輯系統類別 </summary>
        public async Task EditSysType(SysTypeInput input)
        {
            var entry = await _db.SysType.AsAsyncEnumerable().FirstOrDefaultAsync(t => t.TYPE == input.Type);
            if (entry != null && entry.EDITOR)
            {
                entry.DESC = input.Desc;
                entry.EDITOR = input.Editor;
                _db.Update(entry);
                await _db.SaveChangesAsync();
            }
        }
        /// <summary> 取得系統類別 </summary>
        public async Task<List<SysTypeOutput>> GetTypes() => await _db.SysType.AsAsyncEnumerable().Select(t => new SysTypeOutput
        {
            Type = t.TYPE,
            Desc = t.DESC,
            Editor = t.EDITOR
        }).ToListAsync();
        /// <summary> 關閉系統類別 </summary>
        public async Task DisableType(string typeName)
        {
            var entry = await _db.SysType.AsAsyncEnumerable().FirstOrDefaultAsync(t => t.TYPE == typeName);
            if (entry != null && entry.EDITOR)
            {
                entry.EDITOR = false;
                _db.Update(entry);
                await _db.SaveChangesAsync();
            }
        }

        bool IsTypeExist(string typeName) => _db.SysType.Any(t => t.TYPE == typeName);
        #endregion

        #region << SysCode >>
        /// <summary> 新增系統代碼 </summary>
        public async Task AddCode(SysCodeInput input)
        {
            if (IsCodeExist(input.Type, input.Code, input.ParentPath, input.Level)) return;
            _db.SysCode.Add(new Sys_Code
            {
                TYPE = input.Type,
                CODE = input.Code,
                PARENT_PATH = input.ParentPath,
                LEVEL = input.Level,
                DESC = input.Desc,
                CreateUser = input.UserId,
            });
            await _db.SaveChangesAsync();
        }
        /// <summary> 編輯系統代碼 </summary>
        public async Task EditCode(SysCodeInput input)
        {
            var entry = await _db.SysCode.AsAsyncEnumerable()
                .FirstOrDefaultAsync(c => c.TYPE == input.Type && c.CODE == input.Code && c.PARENT_PATH == input.ParentPath && c.LEVEL == input.Level);
            if (entry != null && !entry.Status)
            {
                entry.DESC = input.Desc;
                entry.UpdateUser = input.UserId;
                entry.UpdateDatetime = DateTime.Now;
                _db.Update(entry);
                await _db.SaveChangesAsync();
            }
        }
        /// <summary> 取得系統代碼 </summary>
        public async Task<List<SysCodeOutput>> GetCodes(string typeName) => await _db.SysCode.AsAsyncEnumerable()
            .Where(c => c.TYPE == typeName && !c.Status)
            .Select(c => new SysCodeOutput
            {
                Type = c.TYPE,
                Code = c.CODE,
                ParentPath = c.PARENT_PATH,
                Level = c.LEVEL,
                Desc = c.DESC,
            }).ToListAsync();
        /// <summary> 關閉系統代碼 </summary>
        public async Task DisableCode(SysCodeInput input)
        {
            var entry = await _db.SysCode.AsAsyncEnumerable()
                .FirstOrDefaultAsync(c => c.TYPE == input.Type && c.CODE == input.Code && c.PARENT_PATH == input.ParentPath && c.LEVEL == input.Level);
            if (entry != null && !entry.Status)
            {
                entry.Status = true;
                entry.UpdateUser = input.UserId;
                entry.UpdateDatetime = DateTime.Now;
                _db.Update(entry);
                await _db.SaveChangesAsync();
            }
        }

        bool IsCodeExist(string typeName, int code, string parentPath, byte level)
            => _db.SysCode.Any(c => c.TYPE == typeName && c.CODE == code && c.PARENT_PATH == parentPath && c.LEVEL == level);
        #endregion

        #region << SysSetting >>
        /// <summary> 新增系統設定 </summary>
        public async Task AddSetting(SysSettingInput input)
        {
            if (IsSettingExist(input.PKey)) return;
            _db.SystemSetting.Add(new Sys_SystemSetting
            {
                Key = input.PKey.Key,
                Type = input.PKey.Type,
                Value = input.Value,
                Name = input.Name,
                ModifiedUser = input.UserId,
                ModifiedDateTime = DateTime.Now,
                Order = GetMaxOrder(input.PKey) + 1
            });
            await _db.SaveChangesAsync();
        }
        /// <summary> 編輯系統設定 </summary>
        public async Task EditSetting(SysSettingInput input)
        {
            var entry = await _db.SystemSetting.FindAsync(input.PKey); //.AsAsyncEnumerable().FirstOrDefaultAsync(c => c.Key == input.PKey.Key && c.Type == input.PKey.Type);
            if (entry != null)
            {
                entry.Value = input.Value;
                entry.Name = input.Name;
                entry.ModifiedUser = input.UserId;
                entry.ModifiedDateTime = DateTime.Now;
                _db.Update(entry);
                await _db.SaveChangesAsync();
            }
        }
        /// <summary> 取得系統設定 </summary>
        public async Task<List<SysSettingOutput>> GetSettings(SysSettingPKey pKey)
            => await _db.SystemSetting.AsAsyncEnumerable().Where(s => s.Key == pKey.Key && s.Type == pKey.Type).OrderBy(s => s.Order).ThenBy(s => s.Value)
            .Select(s => new SysSettingOutput
            {
                Key = s.Key,
                Type = s.Type,
                Name = s.Name,
                Value = s.Value,
            }).ToListAsync();
        /// <summary> 移除系統設定 </summary>
        public async Task RemoveSettings(SysSettingPKey pKey)
        {
            var entry = await _db.SystemSetting.FindAsync(pKey);
            if (entry != null)
            {
                _db.Remove(entry);
                await _db.SaveChangesAsync();
            }
        }

        bool IsSettingExist(SysSettingPKey pKey) => _db.SystemSetting.Any(s => s.Key == pKey.Key && s.Type == pKey.Type);
        int GetMaxOrder(SysSettingPKey pKey) => _db.SystemSetting.Where(s => s.Key == pKey.Key && s.Type == pKey.Type).Max(s => s.Order);
        #endregion
    }
}
