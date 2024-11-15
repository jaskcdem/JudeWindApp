namespace Common.Extension
{
    /// <summary> Ods匯出資訊 </summary>
    public class OdsBuildInfo
    {
        /// <summary> 資料表名稱 </summary>
        public string SheetName { get; set; } = "Sheet1";

        /// <summary> 欄位設定(欄序,欄名,欄寬) </summary>
        public List<(int index, string name, double width)> ColumnsSetting = [];

        /// <summary> 資料清單(欄序,資料) </summary>
        public IEnumerable<List<(int index, object value)>> Datas = [];

        internal const double DefWidth = 10;
    }
}
