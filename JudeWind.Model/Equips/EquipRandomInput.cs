using GreenUtility.Equip;
using JudeWind.Model.Base;
using static GreenUtility.RPGSetting;

namespace JudeWind.Model.Equips
{
    #region <-- Equip Base -->
    /// <summary> Random Equip </summary>
    public class EquipRandomInput
    {
        /// <summary> Total Number </summary>
        public int Numbers { get; set; }

        public Weapon? Weapon { get; set; } = null;

        public Armor? Armor { get; set; } = null;
    }
    /// <summary> Equips </summary>
    public class EquipOutput
    {
        public BaseEquip Equip { get; set; } = null!;
        public string TypeName { get => Equip.GetType().Name; }
    }
    #endregion

    #region <-- Equip Decorate -->
    /// <summary> Decorate Equip </summary>
    public class DecoratorEquipInput
    {
        /// <summary> Decorate infos </summary>
        public List<DecoratorEquipBoxInfo> DecorateBox { get; set; } = [];
    }

    /// <summary> Decorate Equip </summary>
    public class DecoratorEquipOutput
    {
        public BaseEquip Equip { get; set; } = null!;
        public string TypeName { get => Equip.GetType().Name; }
        public List<UnhealthyStatus> UnhealthyStatuses = [];
        public List<Element> Elements = [];
        public List<GreatElement> GreatElements = [];
        public List<PhysicType> PhysicTypes = [];
    }
    #endregion
}
