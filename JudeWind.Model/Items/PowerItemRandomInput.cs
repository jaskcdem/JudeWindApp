using DataAcxess.DataEnums;
using GreenUtility.Item;
using static GreenUtility.RPGSetting;

namespace JudeWind.Model.Items
{
    #region  <-- Potion Base -->
    /// <summary> Random Power Item </summary>
    public class PowerItemRandomInput
    {
        /// <summary> Total Number </summary>
        public int Numbers { get; set; }
        /// <summary> defult event </summary>
        public bool IsItem { get; set; }
        /// <summary> Custom event </summary>
        public bool IsCustom { get; set; }
    }
    /// <summary> Random Power Item </summary>
    public class PowerItemInput : PowerItemRandomInput
    {
        /// <summary>  </summary>
        public ParameterType? ParameterType { get; set; }
        /// <summary>  </summary>
        public ItemEffect? ItemEffect { get; set; }
        /// <summary>  </summary>
        public ItemEffectParameter? EffectParameter { get; set; }
    }
    /// <summary> Power Items </summary>
    public class PowerItemRandomOutput
    {
        public BasePowerPotion Item { get; set; } = null!;
        public BasePowerScroll Custom { get; set; } = null!;
        public string ItemTypeName { get => Item == null ? string.Empty : Item.GetType().Name; }
        public string CustomTypeName { get => Custom == null ? string.Empty : Custom.GetType().Name; }
    }
    #endregion
}
