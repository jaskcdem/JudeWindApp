using DataAcxess.Extension;
using GreenUtility;
using GreenUtility.Interface;
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
        public IMagicSkill GetFullRanderMagic()
        {
            TargrtType _ttype = targetTypes[Utility.RandomInt(1, targetTypes.Length)].ToEnum<TargrtType>();
            return GetRanderMagic(_ttype);
        }
        public IMagicSkill GetRanderMagic(TargrtType ttype)
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

        public IMagicSkill GetElementMagic(Element mtype, TargrtType ttype) => CreateMagic(mtype, ttype);
        public IMagicSkill GetElementMagic(TargrtType ttype)
        {
            Element mtype = elementType[Utility.RandomInt(1, elementType.Length)].ToEnum<Element>();
            return GetElementMagic(mtype, ttype);
        }
        public IMagicSkill GetElementMagic(Element mtype)
        {
            TargrtType ttype = targetTypes[Utility.RandomInt(1, targetTypes.Length)].ToEnum<TargrtType>();
            return GetElementMagic(mtype, ttype);
        }

        public IMagicSkill GetGreateElementMagic(GreatElement mtype, TargrtType ttype) => CreateGreateMagic(mtype, ttype);
        public IMagicSkill GetGreateElementMagic(TargrtType ttype)
        {
            GreatElement mtype = greatElementType[Utility.RandomInt(1, greatElementType.Length)].ToEnum<GreatElement>();
            return GetGreateElementMagic(mtype, ttype);
        }
        public IMagicSkill GetGreateElementMagic(GreatElement mtype)
        {
            TargrtType ttype = targetTypes[Utility.RandomInt(1, targetTypes.Length)].ToEnum<TargrtType>();
            return GetGreateElementMagic(mtype, ttype);
        }
        #endregion

        #region <-- Factory -->
        IMagicSkill CreateMagic(Element mtype, TargrtType ttype)
        {
            short rank = this.GetRandomRank();
            IMagicSkill skill = mtype switch
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
                _ => throw new ArgumentNullException(message: "Undefind Element Type", paramName: "GetMagic")
            };
            skill.TargrtType = ttype;
            skill.ResetName();
            return skill;
        }
        IMagicSkill CreateGreateMagic(GreatElement mtype, TargrtType ttype)
        {
            short rank = this.GetRandomRank();
            IMagicSkill skill = mtype switch
            {
                GreatElement.Plant => new BasePlantMagic(rank),
                GreatElement.Earth => new BasePlantMagic(rank),
                GreatElement.Electric => new BasePlantMagic(rank),
                GreatElement.Decay => new BasePlantMagic(rank),
                _ => throw new ArgumentNullException(message: "Undefind Great Element Type", paramName: "GetMagic")
            };
            skill.TargrtType = ttype;
            skill.ResetName();
            return skill;
        }
        #endregion
    }
}
