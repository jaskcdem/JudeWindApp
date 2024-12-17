using System.Text;

namespace JudeWindApp.Util
{
    /// <summary> 驗證器介面 </summary>
    public interface ICodeValidator
    {
        /// <summary> 驗證 </summary>
        /// <param name="code"></param>
        bool Validate(string code);

        /// <summary> 產生驗證碼 </summary>
        string Generate();
    }
    /// <summary> 驗證器 </summary>
    public class CodeValidator(IHttpContextAccessor httpContextAccessor) : ICodeValidator
    {
        private const string KEY = "ValidationCode";
        private HttpContext HttpContext { get; set; } = httpContextAccessor.HttpContext!;

        /// <inheritdoc/>
        public string Generate()
        {
            string code = CreateRandomWord();

            // session只能儲存byte[]，將字串轉為byte[]
            byte[] codeBytes = Encoding.ASCII.GetBytes(code);

            HttpContext.Session.Set(KEY, codeBytes);
            return code;
        }

        /// <inheritdoc/>
        public bool Validate(string code)
        {
            bool isOk = false;
            if (HttpContext.Session.TryGetValue(KEY, out byte[]? codeBytes) && codeBytes != null)
            {
                // 從Session取出來的byte[] 轉成字串
                string serverCode = Encoding.ASCII.GetString(codeBytes);

                // 忽略大小寫比對
                if (serverCode.Equals(code, StringComparison.InvariantCultureIgnoreCase))
                {
                    isOk = true;
                }
            }

            // 無論成功失敗，都清除Session。(依情境，非必要)
            HttpContext.Session.Remove(KEY);
            return isOk;
        }

        /// <summary> 產生隨機字串 </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        private static string CreateRandomWord(int length = 6)
        {
            string code = "";
            var letters = "ABCDEFGHJKMPQRSTUVWXYZ23456789abcdefghjkmpqrstuvwxyz".ToArray();

            Random r = new();
            for (int i = 0; i < length; i++)
            {
                int index = r.Next(0, letters.Length);
                code += letters[index];
            }

            return code;
        }
    }
}
