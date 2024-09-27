using Common;
using DataAcxess.ProjectContext;
using JudeWind.Model.Base;
using System.Data;

namespace JudeWind.Service
{
    /// <summary>  </summary>
    public class BaseService(ProjectContext projectContext)
    {
        #region Protected Members
        /// <summary> 配合Entity使用的DbContext </summary>
        protected readonly ProjectContext _db = projectContext;

        /// <summary> 用於資料庫識別系統 </summary>
        protected readonly string SYSTEM_NAME = SysSetting.SysName;
        #endregion

        #region Protected Methods
        /// <summary> 處理字串參數
        /// <list type="number">
        /// <item>null則回傳空白</item>
        /// <item>有挾雜空白則予以清除</item>
        /// </list>
        /// </summary>
        /// <param name="parameter"></param>
        protected string ProcessStringParameterFormat(string parameter) => (parameter ?? string.Empty).Trim();

        /// <summary> 儲存檔案 (base64) </summary>
        /// <param name="fileItem"></param>
        /// <param name="savePath">存放實體路徑</param>
        /// <param name="useFileName">是否引用原檔名 (預設否)</param>
        /// <returns></returns>
        public string SaveFile_Base64(FileItemModel fileItem, string savePath, bool useFileName = false)
        {
            string fileFullName = "";
            if (!string.IsNullOrEmpty(fileItem.FileName) && !string.IsNullOrEmpty(fileItem.FileType) && !string.IsNullOrEmpty(savePath))
            {
                string fileExt = $".{fileItem.FileType.Replace(".", "")}";
                string fileNewName = useFileName ? fileItem.FileName : Path.GetRandomFileName().Replace(".", "");
                fileFullName = fileNewName + fileExt;

                if (!Directory.Exists(savePath)) { Directory.CreateDirectory(savePath); }

                if (!string.IsNullOrEmpty(fileItem.FileBase64))
                {
                    File.WriteAllBytes(Path.Combine(savePath, fileFullName), Convert.FromBase64String(fileItem.FileBase64));
                }
            }
            return fileFullName;
        }
        #endregion
    }

    public abstract class BaseDapperService(ProjectContext projectContext, IDbConnection dbConnection) : BaseService(projectContext)
    {
        protected readonly IDbConnection _dbConnection = dbConnection;
    }
}
