using DataAcxess.Repository;
using static GreenUtility.RPGSetting;

namespace JudeWind.Model.Base
{
    /// <summary> Decorate info </summary>
    public class DecoratorBoxInfo
    {
        /// <summary> 隨機數量 </summary>
        public int Numbers { get; set; }

        /// <summary> 附加狀態數 </summary>
        /// <remarks>Max = 1</remarks>
        public int StatusCount { get; set; }

        /// <summary> 附加屬性數 </summary>
        /// <remarks>Max = 1</remarks>
        public int ElementCount { get; set; }

        /// <summary> 附加上位屬性數 </summary>
        /// <remarks>Max = 1, can't bind if <see cref="ElementCount"/> not zero</remarks>
        public int GreatElementCount { get; set; }

        /// <summary> 附加物理屬性數 </summary>
        /// <remarks>Max = 1</remarks>
        public int PhysicCount { get; set; }
    }

    /// <inheritdoc/>
    public class DecoratorEquipBoxInfo : DecoratorBoxInfo
    {
        public Weapon? Weapon { get; set; } = null;

        public Armor? Armor { get; set; } = null;
    }

    /// <inheritdoc/>
    public class DecoratorClassicPotionBoxInfo : DecoratorBoxInfo
    {
        public PotionRepository.PotionType? Potion { get; set; } = null;
    }
}
