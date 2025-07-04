using Common.Extension;
using Microsoft.AspNetCore.Mvc;

namespace JudeWindApp.Controllers
{
    /// <summary> something note </summary>
    [Route("Research")]
    public class SelfResearchController(IWebHostEnvironment env) : BaseApiController
    {
        /// <summary> <see cref="Path.Combine(string, string)"/> </summary>
        [HttpGet("Path")]
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
        /// <summary></summary>
        [HttpGet("Build")]
        public List<string> BuildStrArr()
        {
            List<string> _strArr = [];
            List<int> _index = [];
            string ConvertToIndArray(List<int> _list) => $"['{string.Join("','", _index.Select(x => x.ToString()))}']";

            _strArr.Add("能力強化0");
            for (int i = 199; i <= 206; i++) _index.Add(i);
            for (int i = 215; i <= 222; i++) _index.Add(i);
            for (int i = 232; i <= 238; i++) _index.Add(i);
            for (int i = 246; i <= 252; i++) _index.Add(i);
            _strArr.Add(ConvertToIndArray(_index));
            _index.Clear();

            _strArr.Add("能力強化1");
            for (int i = 207; i <= 214; i++) _index.Add(i);
            for (int i = 223; i <= 230; i++) _index.Add(i);
            for (int i = 239; i <= 245; i++) _index.Add(i);
            for (int i = 253; i <= 259; i++) _index.Add(i);
            for (int i = 260; i <= 263; i++) _index.Add(i);
            _strArr.Add(ConvertToIndArray(_index));
            _index.Clear();

            _strArr.Add("技能書");
            for (int i = 351; i <= 369; i++) _index.Add(i);
            for (int i = 399; i <= 408; i++) _index.Add(i);
            _strArr.Add(ConvertToIndArray(_index));
            _index.Clear();

            return _strArr;
        }
        /// <summary> doc temple export </summary>
        [HttpGet("Doc")]
        public IActionResult Doc()
        {
            var dotxPath = env.ContentRootPath + "\\Temple\\PrisonPrincess.dotx";
            Dictionary<string, string> placeholders = new()
            {
                {"Char1", "亞莉亞" }, {"Job1", "槌手"}, {"Elem1", "鬼"}, {"Weap1", "汀普拉"}, {"Magic1", "梅西亞"},
                {"Char2", "賽娜" }, {"Job2", "大劍師"}, {"Elem2", "月"}, {"Weap2", "薩巴拉加穆瓦"}, {"Magic2", "修多"},
                {"Char3", "茱麗葉" }, {"Job3", "槍兵"}, {"Elem3", "電"}, {"Weap3", "努沃勒埃利耶"}, {"Magic3", "萊伊"},
                {"Char4", "夏洛特" }, {"Job4", "劍士"}, {"Elem4", "火"}, {"Weap4", "大吉嶺"}, {"Magic4", "卡查"},
                {"Char5", "費南雪" }, {"Job5", "法師"}, {"Elem5", "水"}, {"Weap5", "烏巴"}, {"Magic5", "嘯海"},
                {"Char6", "休凱特" }, {"Job6", "盜賊"}, {"Elem6", "風"}, {"Weap6", "盧哈娜"}, {"Magic6", "比拂"},
                {"Char7", "甘納許" }, {"Job7", "斧戰士"}, {"Elem7", "冰"}, {"Weap7", "錫金邦"}, {"Magic7", "吹雪"},
                {"Char8", "舒服蕾" }, {"Job8", "拳手"}, {"Elem8", "地"}, {"Weap8", "阿薩姆"}, {"Magic8", "克拉洛克"},
                {"Char9", "克拉芙緹" }, {"Job9", "武士"}, {"Elem9", "木"}, {"Weap9", "尼爾吉里"}, {"Magic9", "葛利夫"},
                {"Char10", "尤彌爾" }, {"Job10", "忍者"}, {"Elem10", "影"}, {"Weap10", "童子切安岡"}, {"Magic10", "黑水"},
                {"Char11", "蕭叕邦" }, {"Job11", "獵人"}, {"Elem11", "毒"}, {"Weap11", "紫雨"}, {"Magic11", "提勒無"},
                {"Char12", "柴金" }, {"Job12", "召喚師"}, {"Elem12", "龍"}, {"Weap12", "聖典之音"}, {"Magic12", "嘯猴"}
            };
            return DownloadFileContent(ExportHelper.ExportDoc(dotxPath, placeholders), "PrisonPrincess.docx");
        }
        /// <summary> doc temple export </summary>
        [HttpGet("Doc/Html")]
        public IActionResult DocHtml()
        {
            string html = @"<h1>冰冰秀人員專業經歷表</h1>
    <h2>基本資料</h2>
    <table>
      <tr><td>中文姓名</td><td>張三</td></tr>
      <tr><td>國籍</td><td>台灣</td></tr>
      <tr><td>最高學歷</td><td>大學</td></tr>
      <tr><td>iT相關工作年資</td><td>5年</td></tr>
      <tr><td>職務經驗</td><td>
        <p>PM: 1 yr.</p>
        <p>SA: 1 yr.</p>
        <p>SD: 1 yr.</p>
        <p>PG: 1 yr.</p>
        <p>QA/Testing: 1 yr.</p>
      </td></tr>
      <tr><td>證照</td><td>TOEIC 800分</td></tr>
      <tr><td>訓練課程</td><td>資策會JAVA班...</td></tr>
      <tr><td>工作性格</td><td>工作態度積極...</td></tr>
    </table>
    <h2>專案經歷</h2>
      <p>公司名稱: 快樂人壽</p>
      <p>專案名稱: 快樂人壽數位服務系統</p>
      <p>專案期間: 2024/01~2025/01</p>
      <p>主要職務: 系統改版</p>
      <p>專案詳述: 撰寫人壽服務系統的功能API...</p>
    <h2>技術純熟度</h2>
    <table>
        <tr>
          <td>技術名稱: Java</td>
          <td>熟練度: A</td>
        </tr>
        <tr>
          <td>技術名稱: Spring Boot</td>
          <td>熟練度: B</td>
        </tr>
    </table>";
            return DownloadFileContent(ExportHelper.ExportDoc(html), "ResumeDemo.docx");
        }
    }
}
