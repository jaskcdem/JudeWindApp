using DataAcxess.Repository;
using GreenUtility.Potion;
using JudeWind.Model.Base;
using static GreenUtility.RPGSetting;

namespace JudeWind.Model.Items
{
    #region  <-- Potion Base -->
    /// <summary> Random Classic Potion </summary>
    public class ClassicPotionRandomInput
    {
        /// <summary> Total Number </summary>
        public int Numbers { get; set; }

        public PotionRepository.PotionType? Potion { get; set; } = null;
    }
    /// <summary> Classic Potions </summary>
    public class ClassicPotionOutput
    {
        public BasePotion Potion { get; set; } = null!;
        public string TypeName { get => Potion.GetType().Name; }
    }
    #endregion

    #region <-- Classic Potion Decorate -->
    /// <summary> Decorate Potion </summary>
    public class DecoratorClassicPotionInput
    {
        /// <summary> Decorate infos </summary>
        public List<DecoratorClassicPotionBoxInfo> DecorateBox { get; set; } = [];
    }

    /// <summary> Decorate Classic Potion </summary>
    public class DecoratorClassicPotionOutput
    {
        public BasePotion Potion { get; set; } = null!;
        public string TypeName { get => Potion.GetType().Name; }
        public List<UnhealthyStatus> UnhealthyStatuses = [];
        public List<Element> Elements = [];
        public List<GreatElement> GreatElements = [];
        public List<PhysicType> PhysicTypes = [];
    }
    #endregion
}
