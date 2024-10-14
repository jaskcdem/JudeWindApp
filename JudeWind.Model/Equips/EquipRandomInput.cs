using GreenUtility.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GreenUtility.RPGSetting;

namespace JudeWind.Model.Equips
{
    #region <-- Equip Base -->
    /// <summary> Random Equip </summary>
    public class EquipRandomInput
    {
        /// <summary> Total Number </summary>
        public int Numbers { get; set; }

        public Weapon? Weapon { get; set; }

        public Armor? Armor { get; set; }
    }
    /// <summary> Equips info </summary>
    public class EquipInfo
    {
        public IEquipItem Equip { get; set; } = null!;
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
        public List<DecoratorInfo> DecorateBox { get; set; } = [];
    }

    /// <summary> Decorate info </summary>
    public class DecoratorInfo
    {
        /// <summary> 隨機數量 </summary>
        public int Numbers { get; set; }

        public Weapon? Weapon { get; set; }

        public Armor? Armor { get; set; }

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

    /// <summary> Decorate Equip </summary>
    public class DecoratorEquipOutput
    {
        /// <summary> Decorate Equips info </summary>
        public List<DecoratorEquipInfo> Equips { get; set; } = [];
    }

    /// <summary> Decorate Equip info </summary>
    public class DecoratorEquipInfo
    {
        public IEquipItem Equip { get; set; } = null!;
        public string TypeName { get => Equip.GetType().Name; }
        public List<UnhealthyStatus> UnhealthyStatuses = [];
        public List<Element> Elements = [];
        public List<GreatElement> GreatElements = [];
        public List<PhysicType> PhysicTypes = [];
    }
    #endregion
}
