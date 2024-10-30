using Microsoft.AspNetCore.Mvc;

namespace JudeWindApp.Controllers
{
    /// <summary> something note </summary>
    public class SelfResearchController : Controller
    {
    /// <summary> <see cref="Path.Combine(string, string)"/> </summary>
        public List<string> PathCombine()
        {
            List<string> _result = [];
            const string path1 = "c:\\temp\\", path2 = "\\upp", path3 = "\\upp\\", path4 = "\\upp\\123.txt", path5 = "upp\\123.txt"
              , path6 = "upp", path7 = "upp\\";
            _result.Add($"path1 + path2 = {Path.Combine(path1, path2)}");
            _result.Add($"path1 + path3 = {Path.Combine(path1, path3)}");
            _result.Add($"path1 + path4 = {Path.Combine(path1, path4)}");
            _result.Add($"path1 + path5 = {Path.Combine(path1, path5)}");
            _result.Add($"path1 + path6 = {Path.Combine(path1, path6)}");
            _result.Add($"path1 + path7 = {Path.Combine(path1, path7)}");
            return _result;
        }
    }
}
