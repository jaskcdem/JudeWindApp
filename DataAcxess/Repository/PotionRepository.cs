using DataAcxess.Extension;
using GreenUtility;
using GreenUtility.Interface;
using GreenUtility.Potion;
using static GreenUtility.RPGSetting;

namespace DataAcxess.Repository
{
    public class PotionRepository : ISampleRepository
    {
        public enum PotionType { Red, Blue, Purple, Yellow }

        #region private members
        const int purpleDivisor = 2, yellowDivisor = 4;
        readonly string[] potionTypes = Enum.GetNames(typeof(PotionType));
        readonly List<(PotionType potion, string defName, (int basePoint, int growPoint, int basePercent, int growPercent)[] values)> PotionList
            = [(PotionType.Red, string.Empty, [(50, 2, 0, 0)]), (PotionType.Red, "回復藥", [(20, 20, 0, 0)])
                , (PotionType.Red, "蓬萊赤草", [(0, 0, 10, 5)]), (PotionType.Red, "赭紅之泉", [(100, 25, 20, 0)])
                , (PotionType.Blue, string.Empty, [(25, 1, 0, 0)]), (PotionType.Blue, "魔法藥水", [(10, 10, 0, 0)])
                , (PotionType.Blue, "蓬萊青草", [(0, 0, 10, 5)]), (PotionType.Blue, "海藍之泉", [(50, 25, 20, 0)])
                , (PotionType.Purple, string.Empty, [(50, 6, 0, 0)]), (PotionType.Purple, "戰鬥乾糧", [(80, 20, 0, 0)])
                , (PotionType.Purple, "蓬萊葡萄草", [(0, 0, 10, 5)]), (PotionType.Purple, "紫藤之泉", [(100, 25, 20, 0)])
                , (PotionType.Yellow, string.Empty, [(50, 2, 0, 0)]), (PotionType.Yellow, "神聖之水", [(20, 20, 0, 0)])
                , (PotionType.Yellow, "蓬萊還魂草", [(0, 0, 10, 5)]), (PotionType.Yellow, "亡者之泉", [(100, 25, 20, 0)])
                ];
        #endregion

        #region methods
        public IPotion GetFullRandomPotion()
        {
            PotionType _ptype = potionTypes[Utility.RandomInt(potionTypes.Length)].ToEnum<PotionType>();
            return GetRandomPotion(_ptype);
        }
        public IPotion GetRandomPotion(PotionType ptype) => CreatePotion(ptype);
        #endregion

        #region <-- Factory -->
        IPotion CreatePotion(PotionType ptype)
        {
            short rank = this.GetRandomRank();
            var query = PotionList.Where(p => p.potion == ptype);
            var (potion, defName, values) = query.ElementAt(Utility.RandomInt(query.Count()));
            IPotion item = ptype switch
            {
                PotionType.Red => new RedCow(rank, defName),
                PotionType.Blue => new BlueBird(rank, defName),
                PotionType.Purple => new PurpleVne(rank, defName),
                PotionType.Yellow => new GoldApple(rank, defName),
                _ => throw new ArgumentNullException(nameof(ptype), "Undefind Potion Type"),
            };
            SetPotion(item, rank, values);
            item.Note = defName;
            return item;
        }
        static void SetPotion(IPotion item, short rank, (int basePoint, int growPoint, int basePercent, int growPercent)[] values)
        {
            if (item == null) return;
            foreach (var (basePoint, growPoint, basePercent, growPercent) in values)
            {
                var t = item.GetType();
                if (t == typeof(RedCow))
                {
                    item.HealingPoint = basePoint + rank * growPoint;
                    item.HealingPrecent = basePercent + rank * growPercent;
                }
                else if (t == typeof(BlueBird))
                {
                    item.RecoverPoint = basePoint + rank * growPoint;
                    item.RecoverPrecent = basePercent + rank * growPercent;
                }
                else if (t == typeof(PurpleVne))
                {
                    item.HealingPoint = basePoint + rank * growPoint;
                    item.HealingPrecent = basePercent + rank * growPercent;
                    item.RecoverPoint = basePoint / purpleDivisor + rank * growPoint;
                    item.RecoverPrecent = basePercent / purpleDivisor + rank * growPercent;
                }
                else if (t == typeof(GoldApple))
                {
                    item.HealingPoint = basePoint + rank * growPoint;
                    item.HealingPrecent = basePercent + rank * growPercent;
                    item.RecoverPoint = RepositoryExtension.RoundInt(basePoint, yellowDivisor) + rank * growPoint;
                    item.RecoverPrecent = RepositoryExtension.RoundInt(basePoint, yellowDivisor) + rank * growPercent;
                }
            }
        }
        #endregion
    }
}
