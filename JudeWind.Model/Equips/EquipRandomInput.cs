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
    /// <summary> Equips info </summary>
    public class EquipInfo
    {
        public BaseEquip Equip { get; set; } = null!;
        public string TypeName { get => Equip.GetType().Name; }
    }
    /// <summary> Equips </summary>
    public class EquipOutput
    {
        /// <summary> equip list </summary>
        public List<EquipInfo> Equips { get; set; } = [];
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
        /// <summary> Decorate Equips info </summary>
        public List<DecoratorEquipInfo> Equips { get; set; } = [];
    }

    /// <summary> Decorate Equip info </summary>
    public class DecoratorEquipInfo
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
