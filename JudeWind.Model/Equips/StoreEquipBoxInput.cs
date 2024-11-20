using DataAcxess.DataEnums;
using GreenUtility.Equip;
using JudeWind.Model.Base;
using static DataAcxess.Repository.EquipRepository;
using static GreenUtility.RPGSetting;

namespace JudeWind.Model.Equips
{
    #region <-- Equip Base -->
    /// <summary> Store box </summary>
    public class StoreEquipBoxInput
    {
        /// <summary> box info </summary>
        public List<StoreBoxInfo> BoxInfos { get; set; } = [];
        /// <summary> Total Number </summary>
        public int Numbers { get; set; }

        public bool IsWeaponBox { get; set; } = false;
        public bool IsArmorBox { get; set; } = false;

        public Weapon? Weapon { get; set; } = null;
        public Armor? Armor { get; set; } = null;
        public SuitEquipType? SuitType { get; set; } = null;
    }

    /// <summary> Store box </summary>
    public class StoreEquipBoxOutput
    {
        /// <summary> Rare </summary>
        public ItemLevel Level { get; set; }
        public BaseEquip Equip { get; set; } = null!;
        public string TypeName { get => Equip.GetType().Name; }
    }
    #endregion

    /// <summary> Store Decorate box </summary>
    public class StoreDecEquipBoxInput
    {
        /// <summary> box info </summary>
        public List<StoreBoxInfo> BoxInfos { get; set; } = [];
        /// <summary> Decorate infos </summary>
        public List<DecoratorEquipBoxInfo> DecorateBox { get; set; } = [];

        public bool IsWeaponBox { get; set; } = false;
        public bool IsArmorBox { get; set; } = false;

        public Weapon? Weapon { get; set; } = null;
        public Armor? Armor { get; set; } = null;
        public SuitEquipType? SuitType { get; set; } = null;
    }

    /// <summary> Store Decorate box </summary>
    public class StoreDecEquipBoxOutput
    {
        /// <summary> Rare </summary>
        public ItemLevel Level { get; set; }
        public BaseEquip Equip { get; set; } = null!;
        public string TypeName { get => Equip.GetType().Name; }
        public List<UnhealthyStatus> UnhealthyStatuses = [];
        public List<Element> Elements = [];
        public List<GreatElement> GreatElements = [];
        public List<PhysicType> PhysicTypes = [];
    }
}
