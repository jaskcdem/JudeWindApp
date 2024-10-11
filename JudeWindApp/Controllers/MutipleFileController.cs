using Common;
using GreenUtility.Extension;
using JudeWind.Model.Files;
using JudeWindApp.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace JudeWindApp.Controllers
{
    /// <summary>  </summary>
    public class MutipleFileController(IWebHostEnvironment env) : BaseApiController
    {
        private readonly IWebHostEnvironment _env = env;

        /// <summary>  </summary>
        [HttpPost]
        public async Task<UploadFileOutput> Upload(ICollection<IFormFile> files, UploadFileInput input)
        {
            UploadFileOutput _result = new();
            string _subPath = !string.IsNullOrWhiteSpace(input.SubDic) ? input.SubDic : DateTime.Now.ToString("yyyy/MM/dd"),
                vertiPath = $"\\{SysSetting.UploadPath}\\{_subPath}", rootPath = _env.ContentRootPath + vertiPath;
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var filePath = file.FileName;
                    using var stream = System.IO.File.Create(rootPath + filePath);
                    await file.CopyToAsync(stream);
                    _result.FilePaths.Add(vertiPath + filePath);
                }
            }
            return _result;
        }

        /// <summary>  </summary>
        [HttpPost]
        [BypassApiResult]
        public IActionResult DownloadDoc(string path)
        {
            string rootPath = _env.ContentRootPath + path, downloadPath = rootPath.Replace(SysSetting.UploadPath, SysSetting.DownloadPath);
            List<string> Message = [];
            FileUploader uploader = new(downloadPath, rootPath);
            uploader.UploadDoc(ref Message);
            return DownloadFile(downloadPath);
        }
        /// <summary>  </summary>
        [HttpPost]
        [BypassApiResult]
        public IActionResult DownloadImg(string path)
        {
            string rootPath = _env.ContentRootPath + path, downloadPath = rootPath.Replace(SysSetting.UploadPath, SysSetting.DownloadPath);
            List<string> Message = [];
            FileUploader uploader = new(downloadPath, rootPath);
            uploader.UploadImage(ref Message);
            return DownloadFile(downloadPath);
        }
    }
}
