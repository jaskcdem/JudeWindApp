using PdfSharp.Fonts;
using System.Reflection;

namespace JudeWindApp.Util;
/// <summary>  </summary>
public class KaiuResolverHelper : IFontResolver
{
    /// <summary>  </summary>
    public static readonly KaiuResolverHelper Instance = new();

    /// <summary> 字體名稱 </summary>
    public string DefaultFontName => "標楷體";

    /// <summary>  </summary>
    public FontResolverInfo? ResolveTypeface(string familyName, bool isBold, bool isItalic)
    {
        // 當請求的字體是 "標楷體" 時返回對應的字體文件
        if (familyName.Equals("標楷體", StringComparison.CurrentCultureIgnoreCase))
        {
            return new FontResolverInfo("KaiU");
        }
        return null;
    }

    /// <summary> 加載字體的字節數據 </summary>
    /// <remarks>請確保字體文件存在並路徑正確</remarks>
    public byte[]? GetFont(string faceName) => faceName switch
    {
        "KaiU" => LoadFontData("JudeWindApp.KaiU.ttf"),
        _ => null,
    };

    /// <summary> 載入內嵌字體 </summary>
    private static byte[] LoadFontData(string fontResourceName)
    {
        using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(fontResourceName)
            ?? throw new Exception($"Font {fontResourceName} not found.");
        byte[] fontData = new byte[stream.Length];
        stream.Read(fontData, 0, (int)stream.Length);
        return fontData;
    }
}
