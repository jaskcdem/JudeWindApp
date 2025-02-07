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
        public static BasePageViewModel<T> GetPageResult(Enum ResponseCode, List<T> Data)
        {
            BasePageViewModel<T> result = new()
            {
                Detail = Data,
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
        /// <summary> Clone to new result </summary>
        public BaseResult<TRust> ToNewResult<TRust>(TRust newData) => new()
        {
            Body = newData,
            Message = Message,
            Code = Code,
            Exception = Exception
        };
        /// <summary> Clone to new viewModel </summary>
        public BasePageViewModel<TRust> ToNewModel<TRust>(List<TRust> newData) => new()
        {
            Detail = newData,
            Message = Message,
            Code = Code,
            Exception = Exception,
        };
    }

    public class BasePageCondition
    {
        /// <summary> 頁次 </summary>
        /// <remarks>page為系統保留字,不能用</remarks>
        public int CurPage { get; set; } = SysSetting.PageDefult;
        /// <summary> 每頁筆數 </summary>
        public int Size { get; set; } = SysSetting.SizeDefult;
        /// <summary> 使用分頁模式 </summary>
        [JsonIgnore]
        public bool UsingPaging { get; set; } = true;
    }

    public class BasePageViewModel<TData> : BasePageCondition
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
                if (_totalPage <= 0)
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
                            CurPage = Math.Clamp(CurPage, SysSetting.PageDefult, _totalPage);
                        }
                    }
                    catch (Exception) { _totalPage = SysSetting.PageDefult; UsingPaging = false; }
                }
                return _totalPage;
            }
            set { _totalPage = value; }
        }

        public bool IsSuccess => Code == (int)HttpStatusCode.OK;
        public string Message { get; set; } = string.Empty;
        public int Code { get; set; }
        public Exception? Exception { get; set; }
        #endregion

        #region methods
        public void InitPagging<TInfo>(TInfo info) where TInfo : BasePageCondition
        {
            if (info != default && UsingPaging)
            {
                CurPage = Math.Clamp(info.CurPage, SysSetting.PageDefult, int.MaxValue);
                Size = Math.Clamp(info.Size, SysSetting.SizeDefult, int.MaxValue);
            }
        }

        /// <summary> Page <see cref="Detail"/> </summary>
        public void Pagging()
        {
            if (UsingPaging && Detail != null && Count > 0)
            {
                int total = TotalPage, _ingore = (CurPage - 1) * Size;
                Detail = Detail.Skip(_ingore).Take(Size).ToList();
                TotalPage = total;
            }
        }
        /// <summary> Page <paramref name="data"/> and add into <see cref="Detail"/> </summary>
        public void Pagging(IEnumerable<TData> data, int page, int size)
        {
            if (Detail != null && data.Any())
            {
                int _ingore = (page - 1) * size;
                Detail.AddRange(data.Skip(_ingore).Take(size));
            }
        }
        /// <summary> Page <paramref name="data"/> and return </summary>
        public static IEnumerable<TRes> Pagging<TRes>(IEnumerable<TRes> data, int page, int size) where TRes : class
        {
            if (data.Any())
            {
                int _ingore = (page - 1) * size;
                return data.Skip(_ingore).Take(size);
            }
            else return [];
        }
        /// <summary> Page <paramref name="data"/> and add into <see cref="Detail"/> </summary>
        public void PaggingCast<TRes>(IEnumerable<TRes> data, int page, int size) where TRes : class
        {
            if (data.GetType().BaseType != Detail.GetType().BaseType)
                throw new TypeAccessException($"{data.GetType().Name} and {Detail.GetType()} must under same BaseType");
            if (Detail != null && data.Any())
            {
                int _ingore = (page - 1) * size;
                Detail.AddRange(data.Skip(_ingore).Take(size).Cast<TData>());
            }
        }
        /// <summary> Clone to new result </summary>
        public BaseResult<TRust> ToNewResult<TRust>(TRust newData) => new()
        {
            Body = newData,
            Message = Message,
            Code = Code,
            Exception = Exception
        };
        /// <summary> Clone to new viewModel </summary>
        public BasePageViewModel<TRust> ToNewModel<TRust>(List<TRust> newData) => new()
        {
            Detail = newData,
            Message = Message,
            Code = Code,
            Exception = Exception,
            CurPage = CurPage,
            Size = Size,
            TotalPage = TotalPage,
            UsingPaging = UsingPaging,
        };
        #endregion
    }
}
