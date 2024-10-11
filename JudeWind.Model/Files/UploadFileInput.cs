using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudeWind.Model.Files
{
    /// <summary>  </summary>
    public class UploadFileInput
    {
        public string SubDic {  get; set; } = string.Empty;
    }

    public class UploadFileOutput
    {
        public List<string> FilePaths { get; set; } = [];
    }
}
