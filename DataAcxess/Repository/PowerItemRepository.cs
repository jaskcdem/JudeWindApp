using DataAcxess.DataEnums;
using DataAcxess.Extension;
using GreenUtility.Item;
using GreenUtility.Scroll;
using GreenUtility.Potion;

namespace DataAcxess.Repository
{
    public class PowerItemRepository : ISampleRepository
    {
        #region private members
        //const int purpleDivisor = 2, yellowDivisor = 4;
        readonly string[] parameterTypes = Enum.GetNames(typeof(ParameterType));
        //readonly List<(PotionType potion, string defName, (int basePoint, int growPoint, int basePercent, int growPercent)[] values)> PotionList
        //    = [(PotionType.Red, string.Empty, [(50, 2, 0, 0)]), (PotionType.Red, "回復劑", [(20, 20, 0, 0)])
        //        , (PotionType.Red, "蓬萊赤草", [(0, 0, 10, 5)]), (PotionType.Red, "赭紅之泉", [(100, 25, 20, 0)])
        //        , (PotionType.Blue, string.Empty, [(25, 1, 0, 0)]), (PotionType.Blue, "魔法藥水", [(10, 10, 0, 0)])
        //        , (PotionType.Blue, "蓬萊青草", [(0, 0, 10, 5)]), (PotionType.Blue, "海藍之泉", [(50, 25, 20, 0)])
        //        , (PotionType.Purple, string.Empty, [(50, 6, 0, 0)]), (PotionType.Purple, "戰鬥乾糧", [(80, 20, 0, 0)])
        //        , (PotionType.Purple, "蓬萊葡萄草", [(0, 0, 10, 5)]), (PotionType.Purple, "紫藤之泉", [(100, 25, 20, 0)])
        //        , (PotionType.Yellow, string.Empty, [(50, 2, 0, 0)]), (PotionType.Yellow, "神聖之水", [(20, 20, 0, 0)])
        //        , (PotionType.Yellow, "蓬萊還魂草", [(0, 0, 10, 5)]), (PotionType.Yellow, "亡者之泉", [(100, 25, 20, 0)])
        //        ];
        #endregion


    }
}
