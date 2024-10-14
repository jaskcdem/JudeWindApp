using DataAcxess.ProjectContext;
using DataAcxess.Repository;
using GreenUtility.Interface;
using JudeWind.Model.Enums;
using JudeWind.Model.Skill;
using static GreenUtility.RPGSetting;

namespace JudeWind.Service.Skill
{
    public class MagicService(ProjectContext projectContext, MagicRepository magic) : BaseService(projectContext)
    {
        MagicRepository _magicRepository = magic;

        #region methods
        /// <summary> 魔法箱 </summary>
        /// <remarks>Not use: <see cref="MagicRandomInput.Level"/></remarks>
        public MagicOutput RandomMagic(MagicRandomInput input) => IsNullTarget(input.TargrtType)
          ? MagicBoxing(input.Numbers, _magicRepository.GetFullRanderMagic)
          : MagicBoxing(input.Numbers, () => _magicRepository.GetRanderMagic(input.TargrtType));
        /// <summary> 魔法箱 </summary>
        public MagicOutput RanderElementMagic(MagicRandomInput input) => IsNullTarget(input.TargrtType)
            ? RandomMagic(input)
            : input.Level switch
            {
                ElementLevel.Great => MagicBoxing(input.Numbers, () => _magicRepository.GetGreateElementMagic(input.TargrtType)),
                _ => MagicBoxing(input.Numbers, () => _magicRepository.GetElementMagic(input.TargrtType))
            };
        /// <summary> 魔法箱 </summary>
        public MagicOutput ElementMagic(MagicBaseInput input) => IsNullTarget(input.TargrtType)
            ? MagicBoxing(input.Numbers, () => _magicRepository.GetElementMagic(input.Element))
            : MagicBoxing(input.Numbers, () => _magicRepository.GetElementMagic(input.Element, input.TargrtType));
        /// <summary> 魔法箱 </summary>
        public MagicOutput GreatElementMagic(MagicGreateInput input) => IsNullTarget(input.TargrtType)
            ? MagicBoxing(input.Numbers, () => _magicRepository.GetGreateElementMagic(input.Element))
            : MagicBoxing(input.Numbers, () => _magicRepository.GetGreateElementMagic(input.Element, input.TargrtType));
        #endregion

        #region private method
        static MagicOutput MagicBoxing(int number, Func<IMagicSkill> box)
        {
            MagicOutput _result = new();
            for (int i = 1; i <= number; i++)
                _result.Skills.Add(new() { Skill = box.Invoke() });
            return _result;
        }
        static bool IsNullTarget(TargrtType targrt) => targrt == TargrtType.None;
        #endregion
    }
}
