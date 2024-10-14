using JudeWind.Model.Skill;
using JudeWind.Service.Skill;
using Microsoft.AspNetCore.Mvc;
using static GreenUtility.RPGSetting;

namespace JudeWindApp.Controllers
{
    /// <summary> Magic Box </summary>
    public class MagicController(MagicService magicService) : BaseApiController
    {
        MagicService _magicService = magicService;

        /// <summary> 魔法箱 </summary>
        /// <remarks>Not use: <see cref="MagicRandomInput.Level"/></remarks>
        [HttpPost]
        public MagicOutput RandomMagic(MagicRandomInput input) => _magicService.RandomMagic(input);
        /// <summary> 魔法箱 </summary>
        [HttpPost]
        public MagicOutput RanderElementMagic(MagicRandomInput input) => _magicService.RanderElementMagic(input);
        /// <summary> 魔法箱 </summary>
        [HttpPost]
        public MagicOutput ElementMagic(MagicBaseInput input) => input.Element != Element.None ? _magicService.ElementMagic(input) : new MagicOutput();
        /// <summary> 魔法箱 </summary>
        [HttpPost]
        public MagicOutput GreatElementMagic(MagicGreateInput input) => _magicService.GreatElementMagic(input);
    }
}
