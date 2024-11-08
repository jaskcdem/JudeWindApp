using GreenUtility.Magic;
using JudeWind.Model.Enums;
using static GreenUtility.RPGSetting;

namespace JudeWind.Model.Skill
{
    /// <summary> Random Magic </summary>
    public class MagicRandomInput
    {
        /// <summary> Total Number </summary>
        public int Numbers { get; set; }

        /// <summary> Element Level </summary>
        public ElementLevel Level { get; set; }

        /// <summary> Targrt Type </summary>
        public TargrtType TargrtType { get; set; }
    }

    /// <summary> Random Base Magic </summary>
    public class MagicBaseInput
    {
        /// <summary> Total Number </summary>
        public int Numbers { get; set; }

        /// <summary> Element Type </summary>
        public Element Element { get; set; }

        /// <summary> Targrt Type </summary>
        public TargrtType TargrtType { get; set; }
    }
    /// <summary> Random Greate Magic </summary>
    public class MagicGreateInput
    {
        /// <summary> Total Number </summary>
        public int Numbers { get; set; }

        /// <summary> Great Element Type </summary>
        public GreatElement Element { get; set; }

        /// <summary> Targrt Type </summary>
        public TargrtType TargrtType { get; set; }
    }

    /// <summary> Magic Skill info </summary>
    public class MagicInfo
    {
        public BaseSkill Skill { get; set; } = null!;
        public string TypeName { get => Skill.GetType().Name; }
    }
    /// <summary> Magic Skill </summary>
    public class MagicOutput
    {
        /// <summary> Skill List </summary>
        public List<MagicInfo> Skills { get; set; } = [];
    }
}
