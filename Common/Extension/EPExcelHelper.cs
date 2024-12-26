using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace Common.Extension
{
    public class EPExcelHelper<T> where T : class
    {
        ExcelPackage? Package { get; set; }
        readonly List<ExcelSheetExportSetting<T>> sheetExportSettings = [];
        readonly List<ExcelSheetImportSetting<T>> sheetImportSettings = [];

        #region Export
        /// <summary> 匯出初始化 </summary>
        /// <param name="settings">資料表設定(表名, 資料清單, 欄位資訊: (標題,欄號,資料取得方法) )</param>
        public EPExcelHelper<T> InitExport(params (string sheetNames, IEnumerable<T> entries, (string header, int index, Func<T, object> selector)[] columns)[] settings)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            Package ??= new ExcelPackage();
            foreach (var (sheetNames, entries, columns) in settings)
            {
                ExcelSheetExportSetting<T> setting = new()
                {
                    Worksheet = Package.Workbook.Worksheets.Add(sheetNames),
                    Entries = entries,
                    Columns = columns.Select(x => new ExcelSheetColumnExportSetting<T>
                    {
                        Header = x.header,
                        Index = x.index,
                        Selector = x.selector
                    }).ToList()
                };
                sheetExportSettings.Add(setting);
            }
            return this;
        }

        /// <summary> 匯出 </summary>
        /// <returns>若無設定,回拋null</returns>
        public byte[]? Export()
        {
            if (sheetExportSettings == null || sheetExportSettings.Count == 0) return default;
            FillExcel();
            return Package?.GetAsByteArray();
        }
        /// <summary> 匯出 </summary>
        /// <returns>若無設定,回拋null</returns>
        public byte[]? ExportWithTitle(params (int index, ExcelCustomTitle[] titles)[] headerTitles)
        {
            if (sheetExportSettings == null || sheetExportSettings.Count == 0) return default;
            FillExcelWithTitle(headerTitles);
            return Package?.GetAsByteArray();
        }

        /// <summary> 填入資料 </summary>
        private void FillExcel()
        {
            for (int i = 0; i < sheetExportSettings.Count; i++)
            {
                ExcelSheetExportSetting<T>? elephant = sheetExportSettings[i];
                if (elephant == null) continue;
                var headers = elephant.Columns.OrderBy(c => c.Index).Select(c => c.Header).ToArray();
                // 設置標題行樣式
                for (int h = 0; h < headers.Length; h++)
                {
                    elephant.Worksheet.Cells[1, h + 1].Value = headers[h];
                    elephant.Worksheet.Cells[1, h + 1].Style.Font.Bold = true;
                    elephant.Worksheet.Cells[1, h + 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    elephant.Worksheet.Cells[1, h + 1].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                }

                // 填寫資料
                int row = 2;
                foreach (var entry in elephant.Entries)
                {
                    foreach (var col in elephant.Columns.OrderBy(c => c.Index))
                    {
                        var value = col.Selector(entry);
                        elephant.Worksheet.Cells[row, col.Index].Value = value;
                    }
                    row++;
                }

                // 自動調整欄位寬度
                elephant.Worksheet.Cells[elephant.Worksheet.Dimension.Address].AutoFitColumns();
            }
        }
        /// <summary> 填入資料 </summary>
        private void FillExcelWithTitle(params (int index, ExcelCustomTitle[] titles)[] headerTitles)
        {
            for (int i = 0; i < sheetExportSettings.Count; i++)
            {
                ExcelSheetExportSetting<T>? elephant = sheetExportSettings[i];
                if (elephant == null) continue;
                var (index, titles) = headerTitles.FirstOrDefault(x => x.index == i);
                int headerLast = 1;
                if (titles != null)
                {
                    headerLast = titles.Length + 1;
                    for (int t = 1; t < headerLast; t++)
                    {
                        var header = titles.FirstOrDefault(x => x.HeaderRow == t);
                        if (header == null || header.HeaderRow == default) continue;
                        for (int c = 0; c < header.HeaderColumn.Count; c++)
                        {
                            (string value, ExcelCustomStyle? style) = header.HeaderColumn[c];
                            elephant.Worksheet.Cells[header.HeaderRow, c + 1].Value = value;
                            if (style != null)
                            {
                                elephant.Worksheet.Cells[header.HeaderRow, c + 1].Style.Font.Bold = style.Bold;
                                elephant.Worksheet.Cells[header.HeaderRow, c + 1].Style.Font.Color.SetColor(style.FontColor);
                                elephant.Worksheet.Cells[header.HeaderRow, c + 1].Style.Fill.PatternType = style.FillStyle;
                                elephant.Worksheet.Cells[header.HeaderRow, c + 1].Style.Fill.BackgroundColor.SetColor(style.BackgroungColor);
                            }
                        }
                    }
                }

                var headers = elephant.Columns.OrderBy(c => c.Index).Select(c => c.Header).ToArray();
                for (int h = 0; h < headers.Length; h++)
                {
                    elephant.Worksheet.Cells[headerLast, h + 1].Value = headers[h];
                    elephant.Worksheet.Cells[headerLast, h + 1].Style.Font.Bold = true;
                    elephant.Worksheet.Cells[headerLast, h + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    elephant.Worksheet.Cells[headerLast, h + 1].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                }

                // 填寫資料
                int row = headerLast + 1;
                foreach (var entry in elephant.Entries)
                {
                    foreach (var col in elephant.Columns.OrderBy(c => c.Index))
                    {
                        var value = col.Selector(entry);
                        elephant.Worksheet.Cells[row, col.Index].Value = value;
                    }
                    row++;
                }

                // 自動調整欄位寬度
                elephant.Worksheet.Cells[elephant.Worksheet.Dimension.Address].AutoFitColumns();
            }
        }
        #endregion

        #region Import
        /// <summary> 匯入初始化 </summary>
        /// <param name="settings">資料表設定(表項次, 動態更新欄位, 標題起始列, 資料起始列)</param>
        public EPExcelHelper<T> InitImport(MemoryStream stream, params (int index, Dictionary<string, Action<string>> fieldMapps, int? header, int? data)[] sheetSettings)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            Package = new ExcelPackage(stream);
            foreach (var (index, fieldMapps, header, data) in sheetSettings)
            {
                if (Package.Workbook.Worksheets.Count <= index) continue;
                ExcelSheetImportSetting<T> setting = new()
                {
                    Worksheet = Package.Workbook.Worksheets[index],
                    FieldMapps = fieldMapps,
                    Index = index,
                    HeaderRow = header ?? 1,
                    DataRow = data ?? 2,
                };
                sheetImportSettings.Add(setting);
            }
            return this;
        }

        /// <summary> 匯入 </summary>
        /// <param name="headerVietifer">標題檢查(表項次, 檢查方法, 自訂訊息)</param>
        /// <param name="succActions">成功後作業(表項次, 執行方法)</param>
        /// <exception cref="InvalidDataException"/>
        /// <exception cref="Exception"/>
        public void Import((int index, Func<List<string>, bool> vertify, string failMessage)[] headerVietifer, (int index, Action succWork)[] succActions)
        {
            for (int i = 0; i < sheetImportSettings.Count; i++)
            {
                ExcelSheetImportSetting<T>? elephant = sheetImportSettings[i];
                if (elephant == null || !elephant.HadData) continue;
                if (elephant.DataRow <= elephant.HeaderRow) elephant.DataRow = elephant.HeaderRow + 1;

                int rowCount = elephant.Worksheet.Dimension?.Rows ?? 0;
                int colCount = elephant.Worksheet.Dimension?.Columns ?? 0;
                var headers = Enumerable.Range(1, colCount)
                    .Select(col => elephant.Worksheet.Cells[1, col].Value?.ToString()?.Trim() ?? string.Empty)
                    .ToList();
                if (headers.Any(string.IsNullOrWhiteSpace))
                    throw new InvalidDataException($"第{i + 1}張資料表的標題夾雜空白或無法辨識的資料,基於欄位對應原則,請將空白標題給移除");
                headers = headers.Where(c => !string.IsNullOrWhiteSpace(c)).ToList();

                List<string> errorMsg = [];
                foreach (var (index, vertify, failMessage) in headerVietifer.Where(x => x.index == i))
                    if (!vertify(headers)) errorMsg.Add($"{failMessage}");
                if (errorMsg.Count > 0)
                    throw new InvalidDataException($"第{i + 1}張資料表:{string.Join(',', errorMsg)}");

                errorMsg.Clear();
                for (int row = elephant.DataRow; row <= rowCount; row++)
                {
                    foreach (var fieldMapping in elephant.FieldMapps)
                    {
                        try
                        {
                            if (!headers.Contains(fieldMapping.Key)) continue;
                            var cellValue = elephant.Worksheet.Cells[row, headers.IndexOf(fieldMapping.Key) + 1].Value?.ToString()?.Trim() ?? string.Empty;
                            fieldMapping.Value(cellValue);
                        }
                        catch (Exception ex) { errorMsg.Add($"{fieldMapping.Key}->{ex.Message}"); }
                    }
                }
                if (errorMsg.Count > 0)
                    throw new Exception($"第{i + 1}張資料表:{string.Join(',', errorMsg)}");
                foreach (var (index, succWork) in succActions.Where(x => x.index == i)) succWork.Invoke();
            }
        }
        #endregion
    }

    /// <summary> 資料表設定 </summary>
    internal class ExcelSheetExportSetting<T> where T : class
    {
        /// <summary> 資料表 </summary>
        internal required ExcelWorksheet Worksheet { get; set; }
        internal IEnumerable<T> Entries { get; set; } = [];
        internal List<ExcelSheetColumnExportSetting<T>> Columns { get; set; } = [];
    }
    internal class ExcelSheetColumnExportSetting<T> where T : class
    {
        /// <summary> 標題 </summary>
        internal required string Header { get; set; }
        /// <summary> 欄號 </summary>
        internal int Index { get; set; } = 1;
        /// <summary> 資料取得方法 </summary>
        internal required Func<T, object> Selector { get; set; }
    }
    internal class ExcelSheetImportSetting<T> where T : class
    {
        /// <summary> 資料表 </summary>
        internal required ExcelWorksheet Worksheet { get; set; }
        internal bool HadData => Worksheet.Dimension?.Rows > HeaderRow;

        /// <summary> 資料表項次 </summary>
        internal int Index;
        /// <summary> 標題起始列 </summary>
        internal int HeaderRow = 1;
        /// <summary> 資料起始列 </summary>
        internal int DataRow = 2;
        /// <summary> 動態更新欄位 </summary>
        internal Dictionary<string, Action<string>> FieldMapps = [];
    }
    public class ExcelCustomTitle
    {
        /// <summary> 標題列 </summary>
        public int HeaderRow { get; set; }
        /// <summary> 標題欄清單(標題,設計) </summary>
        public List<(string value, ExcelCustomStyle? style)> HeaderColumn { get; set; } = [];
    }
    public class ExcelCustomStyle
    {
        public bool Bold { get; set; }
        public ExcelFillStyle FillStyle { get; set; } = ExcelFillStyle.Solid;
        public Color BackgroungColor { get; set; } = Color.White;
        public Color FontColor { get; set; } = Color.Black;
    }
}
