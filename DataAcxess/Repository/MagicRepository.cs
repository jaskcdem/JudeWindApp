using DataAcxess.Extension;
using GreenUtility;
using GreenUtility.Magic;
using static GreenUtility.RPGSetting;

namespace DataAcxess.Repository
{
    public class MagicRepository : ISampleRepository
    {
        readonly string[] targetTypes = Enum.GetNames(typeof(TargrtType)),
            elementType = Enum.GetNames(typeof(Element)),
            greatElementType = Enum.GetNames(typeof(GreatElement));

        #region methods
        public BaseSkill GetFullRanderMagic()
        {
            TargrtType _ttype = targetTypes[Utility.RandomInt(1, targetTypes.Length)].ToEnum<TargrtType>();
            return GetRanderMagic(_ttype);
        }
        public BaseSkill GetRanderMagic(TargrtType ttype)
        {
            int _eleLevel = Utility.RandomInt(2);
            string[] elementTypes = _eleLevel switch
            {
                1 => greatElementType,
                _ => elementType,
            };
            return _eleLevel switch
            {
                1 => CreateGreateMagic(elementTypes[Utility.RandomInt(1, elementTypes.Length)].ToEnum<GreatElement>(), ttype),
                _ => CreateMagic(elementTypes[Utility.RandomInt(1, elementTypes.Length)].ToEnum<Element>(), ttype)
            };
        }

        public BaseSkill GetElementMagic(Element mtype, TargrtType ttype) => CreateMagic(mtype, ttype);
        public BaseSkill GetElementMagic(TargrtType ttype)
        {
            Element mtype = elementType[Utility.RandomInt(1, elementType.Length)].ToEnum<Element>();
            return GetElementMagic(mtype, ttype);
        }
        public BaseSkill GetElementMagic(Element mtype)
        {
            TargrtType ttype = targetTypes[Utility.RandomInt(1, targetTypes.Length)].ToEnum<TargrtType>();
            return GetElementMagic(mtype, ttype);
        }

        public BaseSkill GetGreateElementMagic(GreatElement mtype, TargrtType ttype) => CreateGreateMagic(mtype, ttype);
        public BaseSkill GetGreateElementMagic(TargrtType ttype)
        {
            GreatElement mtype = greatElementType[Utility.RandomInt(1, greatElementType.Length)].ToEnum<GreatElement>();
            return GetGreateElementMagic(mtype, ttype);
        }
        public BaseSkill GetGreateElementMagic(GreatElement mtype)
        {
            TargrtType ttype = targetTypes[Utility.RandomInt(1, targetTypes.Length)].ToEnum<TargrtType>();
            return GetGreateElementMagic(mtype, ttype);
        }
        #endregion

        #region <-- Factory -->
        BaseSkill CreateMagic(Element mtype, TargrtType ttype)
        {
            short rank = this.GetRandomRank();
            BaseSkill skill = mtype switch
            {
                Element.Water => new BaseWaterMagic(rank),
                Element.Fire => new BaseFireMagic(rank),
                Element.Wind => new BaseWindMagic(rank),
                Element.Ground => new BaseGroundMagic(rank),
                Element.Ice => new BaseThunderMagic(rank),
                Element.Thunder => new BaseIceMagic(rank),
                Element.Light => new BaseLightMagic(rank),
                Element.Dark => new BaseDarkMagic(rank),
                Element.Star => new BaseStarMagic(rank),
                _ => throw new ArgumentNullException(nameof(mtype), "Undefind Element Type")
            };
            skill.TargrtType = ttype;
            skill.ResetName();
            return skill;
        }
        BaseSkill CreateGreateMagic(GreatElement mtype, TargrtType ttype)
        {
            short rank = this.GetRandomRank();
            BaseSkill skill = mtype switch
            {
                GreatElement.Plant => new BasePlantMagic(rank),
                GreatElement.Earth => new BaseEarthMagic(rank),
                GreatElement.Electric => new BaseElectricMagic(rank),
                GreatElement.Decay => new BaseDecayMagic(rank),
                _ => throw new ArgumentNullException(nameof(mtype), "Undefind Great Element Type")
            };
            skill.TargrtType = ttype;
            skill.ResetName();
            return skill;
        }
        #endregion
    }
}
