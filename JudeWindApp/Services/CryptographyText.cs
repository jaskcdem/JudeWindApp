using GreenUtility;
using GreenUtility.Extension;
using System.Text;

namespace JudeWindApp.Services
{
    internal class CryptographyText
    {
        const string TextDefult = "Konyou";
        public string PlainText { get; set; } = TextDefult;
        StringBuilder Builder { get; set; } = new();

        internal string Run()
        {
            Builder.AppendLine("測試加密 ===>");
            CryptographyHelper helperFx = new(PlainText);

            Builder.AppendLine($"{CryptographyHelper.CryptographyType.Fx.GetEnumDescription()} : ");
            helperFx.SetCrypto(CryptographyHelper.CryptographyType.Fx);
            Builder.AppendLine($"Fx公鑰: {helperFx.GetPubKey()}");
            Builder.AppendLine($"Fx私鑰: {helperFx.GetPrivKey()}");

            var _plainData = helperFx.Encode();
            Builder.AppendLine($"加密: {_plainData}");
            Builder.AppendLine($"解密: {helperFx.Decrypt()}");
            var _plainSign = helperFx.Sign(PlainText);
            Builder.AppendLine($"簽章: {_plainSign}");
            Builder.AppendLine($"自驗證: {helperFx.Vertify(_plainSign)}");
            string _outFiled = Guid.NewGuid().ToString().Replace("-", "");
            Builder.AppendLine($"他驗證({_outFiled}): {helperFx.Vertify(_outFiled, _plainSign)}");
            PrintDivider();

            CryptographyHelper helperCore = new(PlainText);
            Builder.AppendLine($"{CryptographyHelper.CryptographyType.Core.GetEnumDescription()} : ");
            helperCore.SetCrypto(CryptographyHelper.CryptographyType.Core);
            Builder.AppendLine($"Core公鑰: {helperCore.GetPubKey()}");
            Builder.AppendLine($"Core私鑰: {helperCore.GetPrivKey()}");

            _plainData = helperCore.Encode();
            Builder.AppendLine($"加密: {_plainData}");
            Builder.AppendLine($"解密: {helperCore.Decrypt()}");
            _plainSign = helperCore.Sign(PlainText);
            Builder.AppendLine($"簽章: {_plainSign}");
            Builder.AppendLine($"自驗證: {helperCore.Vertify()}");
            _outFiled = Guid.NewGuid().ToString().Replace("-", "");
            Builder.AppendLine($"他驗證({_outFiled}): {helperCore.Vertify(_outFiled)}");
            PrintDivider();

            Builder.AppendLine($"【交叉測試】 {CryptographyHelper.CryptographyType.Fx.GetEnumDescription()}到{CryptographyHelper.CryptographyType.Core.GetEnumDescription()} :");
            _plainData = helperFx.Encode();
            Builder.AppendLine($"加密: {_plainData}");
            CryptographyHelper helperCoreCross = new(PlainText);
            helperCoreCross.SetCryptoFromPrivkey(CryptographyHelper.CryptographyType.Core, helperFx.GetPrivKey());
            Builder.AppendLine($"解密: {helperCoreCross.Decrypt(helperFx.EncodeData)}");

            _plainSign = helperFx.Sign(PlainText);
            Builder.AppendLine($"簽章: {_plainSign}");
            helperCoreCross.SetCryptoFromPubkey(CryptographyHelper.CryptographyType.Core, helperFx.GetPubKey());
            Builder.AppendLine($"自驗證: {helperCoreCross.Vertify(_plainSign)}");
            _outFiled = Guid.NewGuid().ToString().Replace("-", "");
            Builder.AppendLine($"他驗證({_outFiled}): {helperCoreCross.Vertify(_outFiled, _plainSign)}");
            PrintDivider();

            Builder.AppendLine($"【交叉測試】 {CryptographyHelper.CryptographyType.Core.GetEnumDescription()}到{CryptographyHelper.CryptographyType.Fx.GetEnumDescription()} :");
            _plainData = helperCore.Encode();
            Builder.AppendLine($"加密: {_plainData}");
            CryptographyHelper helperFxCross = new(PlainText);
            helperFxCross.SetCryptoFromPrivkey(CryptographyHelper.CryptographyType.Fx, helperCore.GetPrivKey());
            Builder.AppendLine($"解密: {helperFxCross.Decrypt(helperCore.EncodeData)}");
            _plainSign = helperCore.Sign(PlainText);
            Builder.AppendLine($"簽章: {_plainSign}");
            helperFxCross.SetCryptoFromPubkey(CryptographyHelper.CryptographyType.Fx, helperCore.GetPubKey());
            Builder.AppendLine($"自驗證: {helperFxCross.Vertify(_plainSign)}");
            _outFiled = Guid.NewGuid().ToString().Replace("-", "");
            Builder.AppendLine($"他驗證({_outFiled}): {helperFxCross.Vertify(_outFiled, _plainSign)}");
            PrintDivider();

            Builder.AppendLine("===> 測試加密結束");
            return Builder.ToString();
        }
        void PrintDivider() => Builder.AppendLine("================");
    }
}
