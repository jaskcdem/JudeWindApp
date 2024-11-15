using Aspose.Cells;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using MiniExcelLibs;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Reflection;
using Column = MigraDoc.DocumentObjectModel.Tables.Column;
using Cell = MigraDoc.DocumentObjectModel.Tables.Cell;
using Style = MigraDoc.DocumentObjectModel.Style;
using Font = MigraDoc.DocumentObjectModel.Font;

namespace Common.Extension
{
    public static class ExportHelper
    {
        #region 匯出
        public static byte[] ExportExcel(List<Dictionary<string, object>> sources)
        {
            using var stream = new MemoryStream();
            stream.SaveAs(sources);
            return stream.ToArray();
        }

        public static Stream ExportOds(params OdsBuildInfo[] sheets)
        {
            ArgumentNullException.ThrowIfNull(sheets, nameof(sheets));
            if (sheets.Length == 0) throw new InvalidDataException("BuildInfo must at least give one!");

            Workbook workbook = new(FileFormatType.Ods);
            if (sheets.Length > 1) for (int w = 1; w < sheets.Length; w++) workbook.Worksheets.Add(SheetType.Worksheet);
            for (int w = 0; w < sheets.Length; w++)
            {
                Worksheet sheet = workbook.Worksheets[w];
                OdsBuildInfo buildInfo = sheets[w];
                sheet.Name = buildInfo.SheetName;

                int row = 0;
                for (int i = 0; i < buildInfo.ColumnsSetting.Count; i++)
                {
                    var (_, name, width) = buildInfo.ColumnsSetting[i];
                    if (width <= 0) width = OdsBuildInfo.DefWidth;
                    sheet.Cells.SetColumnWidth(i, width);
                    sheet.Cells[row, i].PutValue(name.Replace(" ", ""));
                }
                row++;

                foreach (var element in buildInfo.Datas)
                {
                    foreach (var (index, value) in element)
                        sheet.Cells[row, index].PutValue(value);
                    row++;
                }
            }

            Stream fileStream = new MemoryStream();
            workbook.Save(fileStream, SaveFormat.Ods);
            fileStream.Position = 0;
            return fileStream;
        }

        public static byte[] ExportPdf(PdfBuildInfo buildInfo)
        {
            ArgumentNullException.ThrowIfNull(buildInfo, nameof(buildInfo));

            var document = new Document();
            var section = document.AddSection();
            section.PageSetup.Orientation = buildInfo.Orientation;
            if (buildInfo.NomalStyle != default)
            {
                var normalStyle = document.Styles.Normal;
                ImportStyle(buildInfo.NomalStyle, normalStyle);
            }

            foreach (var (_, content) in buildInfo.Tables.OrderBy(t => t.sort))
                PdfTable(ref document, ref section, (PdfTableBuildInfo)content);

            // 渲染並生成 PDF
            PdfDocumentRenderer renderer = new() { Document = document };
            renderer.RenderDocument();

            // 保存 PDF
            using var stream = new MemoryStream();
            renderer.PdfDocument.Save(stream, false);
            return stream.ToArray();
        }
        #endregion

        #region PDF建立表格
        private static void PdfTable(ref Document document, ref Section section, PdfTableBuildInfo tableBuildInfo)
        {
            //pick font info
            var normalStyle = document.Styles.Normal;
            var fSize = normalStyle.Font.Size;
            var fFamily = normalStyle.Font.Name;

            // 建立表格
            var table = section.AddTable();
            table.Borders.Width = 0.75;
            // 定義表格列寬度
            var columns = new Column[tableBuildInfo.ColumnsSetting.Count];
            for (int i = 0; i < tableBuildInfo.ColumnsSetting.Count; i++)
            {
                var cbi = tableBuildInfo.ColumnsSetting[i];
                Unit colWidth = Unit.FromCentimeter(cbi.Width);
                columns[i] = table.AddColumn(colWidth);
                columns[i].Format.Alignment = cbi.Alignment;
                columns[i].LeftPadding = colWidth * cbi.PaddingPrecent;
                columns[i].RightPadding = colWidth * cbi.PaddingPrecent;
            }
            // 添加表頭
            var headerRow = table.AddRow();
            headerRow.HeadingFormat = true;
            headerRow.Format.Font.Bold = true;
            headerRow.Shading.Color = Colors.LightGray;
            for (int i = 0; i < tableBuildInfo.ColumnsSetting.Count; i++)
            {
                var cbi = tableBuildInfo.ColumnsSetting[i];
                PdfAddTextToCell(cbi.Name, headerRow.Cells[i], fFamily, fSize);
                if (cbi.Format != default) cbi.Format.Invoke(headerRow.Cells[i].Format);
                else
                {
                    headerRow.Cells[i].Format.Font.Name = fFamily;
                    headerRow.Cells[i].Format.Font.Size = fSize;
                    headerRow.Cells[i].Format.Font.Color = normalStyle.Font.Color;
                    headerRow.Cells[i].Format.Alignment = normalStyle.ParagraphFormat.Alignment;
                }
            }

            // 添加資料行
            foreach (var element in tableBuildInfo.Datas)
            {
                var row = table.AddRow();
                foreach (var item in element)
                {
                    PdfAddTextToCell(Convert.ToString(item.Value) ?? string.Empty, row.Cells[item.Index], fFamily, fSize);
                    if (item.Format != default) item.Format.Invoke(row.Cells[item.Index].Format);
                    else
                    {
                        row.Cells[item.Index].Format.Font.Name = fFamily;
                        row.Cells[item.Index].Format.Font.Size = fSize;
                        row.Cells[item.Index].Format.Font.Color = normalStyle.Font.Color;
                        row.Cells[item.Index].Format.Alignment = normalStyle.ParagraphFormat.Alignment;
                    }
                }
            }
        }
        #endregion

        #region PDF幫手
        private static void ImportStyle(Style from, Style to)
        {
            foreach (var member in typeof(Style).GetMembers())
            {
                if (member != null)
                {
                    object? val;
                    switch (member.MemberType)
                    {
                        case MemberTypes.Field:
                            FieldInfo? _fi = typeof(Style).GetField(member.Name);
                            //Constants => IsLiteral = T and IsInitOnly = F
                            //readonly Variables => IsLiteral = F and IsInitOnly = T
                            if (_fi != null && !_fi.IsLiteral && !_fi.IsInitOnly)
                            {
                                val = _fi.GetValue(from);
                                if (val != null) _fi.SetValue(to, val);
                            }
                            break;
                        case MemberTypes.Property:
                            PropertyInfo? _pi = typeof(Style).GetProperty(member.Name);
                            if (_pi != null && _pi.CanWrite)
                            {
                                val = _pi.GetValue(from);
                                if (val != null)
                                {
                                    if (_pi.PropertyType == typeof(Font))
                                        _pi.SetValue(to, ((Font)val).Clone());
                                    else if (_pi.PropertyType == typeof(ParagraphFormat))
                                        _pi.SetValue(to, ((ParagraphFormat)val).Clone());
                                    else
                                        _pi.SetValue(to, val);
                                }
                            }
                            break;
                    }
                }
            }
        }

        /// <summary> 填入文字 </summary>
        /// <param name="instring">內容</param>
        /// <param name="cell">儲存格</param>
        /// <param name="fontFamily">字型</param>
        /// <param name="fontsize">字大小</param>
        /// <param name="paragraph"></param>
        private static Paragraph? PdfAddTextToCell(string instring, Cell cell, string fontFamily, Unit fontsize, Paragraph? paragraph = null)
        {
            instring ??= " ";
            using PdfDocument pdfd = new();
            PdfPage pg = pdfd.AddPage();

            using XGraphics oGFX = XGraphics.FromPdfPage(pg);
            //實際寬度 = 列寬 - padding寬
            Paragraph? par = paragraph;
            if (cell.Column == null) return par;
            Unit padding = cell.Column.LeftPadding + cell.Column.RightPadding;
            Unit maxWidth = cell.Column.Width - padding;

            //測量字體大小
            XFont font = new(fontFamily, fontsize.Centimeter);
            var fontSize = oGFX.MeasureString(instring, font);
            //當 字寬小於列寬或與列寬差小於留白處, 則不換行
            if (fontSize.Width < maxWidth.Value || fontSize.Width - maxWidth.Value < padding.Value)
            {
                if (par != null) par.AddText(instring);
                else par = cell.AddParagraph(instring);
            }
            else
            {
                //MigraDoc遇到空格會自動換行
                double width = 0;
                string str = "";
                var ds = instring.Split(' ');
                var inArr = instring.ToCharArray();
                if (ds.Length > 1 && !inArr.Any(c => !AplaNumberList().Contains(c)))
                {
                    //純英數字的不另外處裡
                    par ??= cell.AddParagraph();
                    par.AddText(instring);
                }
                else if (ds.Length > 1)
                {
                    //自訂換行處裡
                    par ??= cell.AddParagraph();
                    foreach (var s in ds)
                    {
                        foreach (var item in s.ToArray())
                            AutoWrap(oGFX, ref padding, ref maxWidth, par, font, ref width, ref str, item);
                    }
                    par.AddText(str);
                }
                else
                {
                    par ??= cell.AddParagraph();
                    foreach (var item in instring.ToArray())
                        AutoWrap(oGFX, ref padding, ref maxWidth, par, font, ref width, ref str, item);
                    par.AddText(str);
                }
            }
            return par;
        }

        private static void AutoWrap(XGraphics oGFX, ref Unit padding, ref Unit maxWidth, Paragraph par, XFont font, ref double width, ref string str, char item)
        {
            //測量字元寬並累加
            width += oGFX.MeasureString(item.ToString(), font).Width;
            //如果長度差超過留白處, 則插入換行以實現自動換行
            if (width > maxWidth.Value - padding.Value)
            {
                par.AddText(str.Trim());
                par.AddFormattedText("\r");
                str = "";
                width = 0;
            }
            str += item.ToString();
        }
        static List<char> AplaNumberList()
        {
            char[] lowerApla = "abcdefghijklmnopqrtsuvwxyz".ToCharArray(),
                upperApla = "ABCDEFGHIJKLMNOPQRTSUVWXYZ".ToCharArray(),
                numbers = "0123456789".ToCharArray();
            List<char> box = [.. lowerApla, .. upperApla, .. numbers];
            return box;
        }
        #endregion
    }
}
