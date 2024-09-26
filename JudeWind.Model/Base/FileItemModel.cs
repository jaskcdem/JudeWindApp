using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudeWind.Model.Base
{
    public class FileItemModel
    {
        /// <summary> 檔案名稱 (去除副檔名) </summary>
        public string FileName { get; set; } = string.Empty;
        /// <summary> 檔案轉Base64 </summary>
        public string FileBase64 { get; set; } = string.Empty;
        /// <summary> 檔案大小 </summary>
        public float FileSize { get; set; }
        /// <summary> 檔案副檔名 </summary>
        public string FileType { get; set; } = string.Empty;
    }
}
