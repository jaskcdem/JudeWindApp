using MigraDoc.DocumentObjectModel;

namespace Common.Extension
{
    /// <summary> Pdf匯出資訊 </summary>
    public class PdfBuildInfo
    {
        /// <summary> 方向 </summary>
        public Orientation Orientation { get; set; } = Orientation.Portrait;

        /// <summary> 通版格式 </summary>
        public Style? NomalStyle { get; set; }

        /// <summary> 表格匯出設定(排序,資訊) </summary>
        public List<(int sort, PdfContent content)> Tables { get; set; } = [];
    }

    /// <summary> Pdf內容匯出資訊 </summary>
    public class PdfContent { }

    #region << Table >>
    /// <summary> Pdf表格匯出資訊 </summary>
    public class PdfTableBuildInfo : PdfContent
    {
        /// <summary> 欄位設定 </summary>
        public List<PdfTableColumnBuildInfo> ColumnsSetting = [];

        /// <summary> 資料清單 </summary>
        public IEnumerable<List<PdfTableCellBuildInfo>> Datas = [];
    }

    /// <summary> Pdf表格欄位匯出資訊 </summary>
    public class PdfTableColumnBuildInfo
    {
        /// <summary> 欄序 </summary>
        public int Index { get; set; }
        /// <summary> 欄名 </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary> 欄寬(cm) </summary>
        public double Width { get; set; } = 2.3;
        /// <summary> 邊界比例 </summary>
        public double PaddingPrecent { get; set; } = 0.025;
        /// <summary> 對齊 </summary>
        public ParagraphAlignment Alignment { get; set; } = ParagraphAlignment.Center;
        /// <summary> 額外設定 </summary>
        public Action<ParagraphFormat>? Format { get; set; }
    }

    /// <summary> Pdf儲存格匯出資訊 </summary>
    public class PdfTableCellBuildInfo
    {
        /// <summary> 欄序 </summary>
        public int Index { get; set; }
        /// <summary> 資料 </summary>
        public object Value { get; set; } = string.Empty;
        /// <summary> 額外設定 </summary>
        public Action<ParagraphFormat>? Format { get; set; }
    }
    #endregion
}
