using System.Net;

namespace JudeWind.Model.Base
{
    public class BaseResultHelper
    {
        public static BaseResult<bool> GetBaseResult(Enum ResponseCode)
        {
            BaseResult<bool> result = new()
            {
                Body = true,
                Code = Convert.ToInt32(ResponseCode)
            };
            return result;
        }
    }

    public class BaseResult<T>
    {
        public required T Body { get; set; }
        public bool IsSuccess => Code == (int)HttpStatusCode.OK;
        public string Message { get; set; } = string.Empty;
        public int Code { get; set; }
        public Exception? Exception { get; set; }
    }
}
