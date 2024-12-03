using Common;
using System.Net;
using System.Text.Json.Serialization;

namespace JudeWind.Model.Base
{
    public class BaseResultHelper<T>
    {
        public static BaseResult<T> GetBaseResult(Enum ResponseCode, T Data)
        {
            BaseResult<T> result = new()
            {
                Body = Data,
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

    public class BaseInput
    {
        /// <summary> 頁次 </summary>
        public int Page { get; set; } = SysSetting.PageDefult;
        /// <summary> 每頁筆數 </summary>
        public int Size { get; set; } = SysSetting.SizeDefult;
        /// <summary> 使用分頁模式 </summary>
        [JsonIgnore]
        public bool UsingPaging { get; set; } = true;
    }

    public class BaseOutput<TData> : BaseInput
    {
        #region members
        /// <summary> 資料明細 </summary>
        public List<TData> Detail { get; set; } = [];

        /// <summary> 總筆數 </summary>
        public int Count => Detail.Count;

        int _totalPage = 0;
        public int TotalPage
        {
            get
            {
                _totalPage = SysSetting.PageDefult;
                try
                {
                    if (UsingPaging && Size > 0 && Count > 0)
                    {
                        if (Count > Size)
                        {
                            _totalPage = Count / Size;
                            if (Count % Size > 0) _totalPage++;
                        }
                        Page = Math.Clamp(Page, SysSetting.PageDefult, _totalPage);
                    }
                }
                catch (Exception) { _totalPage = SysSetting.PageDefult; UsingPaging = false; }
                return _totalPage;
            }
            set { _totalPage = value; }
        }
        #endregion

        #region methods
        public void InitPagging<TInfo>(TInfo info) where TInfo : BaseInput
        {
            if (info != default && UsingPaging)
            {
                Page = Math.Clamp(Page, SysSetting.PageDefult, int.MaxValue);
                Size = Math.Clamp(Size, SysSetting.SizeDefult, int.MaxValue);
            }
        }

        /// <summary> Page <see cref="Detail"/> </summary>
        public void Pagging()
        {
            if (UsingPaging && Detail != null && Count > 0)
            {
                int _ingore = (Page - 1) * Size;
                Detail = Detail.Skip(_ingore).Take(Size).ToList();
            }
        }
        /// <summary> Page <paramref name="data"/> and add into <see cref="Detail"/> </summary>
        public void Pagging(IEnumerable<TData> data)
        {
            if (UsingPaging && Detail != null && Count > 0)
            {
                int _ingore = (Page - 1) * Size;
                Detail.AddRange(data.Skip(_ingore).Take(Size));
            }
        }
        /// <summary> Page <paramref name="data"/> and return </summary>
        public IEnumerable<TRes> Pagging<TRes>(IEnumerable<TRes> data) where TRes : class
        {
            if (UsingPaging && Detail != null && Count > 0)
            {
                int _ingore = (Page - 1) * Size;
                return data.Skip(_ingore).Take(Size);
            }
            else return [];
        }
        /// <summary> Page <paramref name="data"/> and add into <see cref="Detail"/> </summary>
        public void PaggingCast<TRes>(IEnumerable<TRes> data) where TRes : class
        {
            if (data.GetType().BaseType != Detail.GetType().BaseType)
                throw new TypeAccessException($"{data.GetType().Name} and {Detail.GetType()} must under same BaseType");
            if (UsingPaging && Detail != null && Count > 0)
            {
                int _ingore = (Page - 1) * Size;
                Detail.AddRange(data.Skip(_ingore).Take(Size).Cast<TData>());
            }
        }
        #endregion
    }
}
