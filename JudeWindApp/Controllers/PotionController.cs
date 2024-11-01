using JudeWind.Model.Items;
using JudeWind.Service.Items;
using Microsoft.AspNetCore.Mvc;

namespace JudeWindApp.Controllers
{
    /// <summary> Potion Box </summary>
    public class PotionController(PotionService potionService) : BaseApiController
    {
        readonly PotionService _potionService = potionService;

        /// <summary> 藥水箱 </summary>
        [HttpPost]
        public ClassicPotionOutput RandomPotionBox(ClassicPotionRandomInput input) => _potionService.RandomClassicPotionBox(input);

        /// <summary> 彩蛋藥水箱 </summary>
        [HttpPost]
        public DecoratorClassicPotionOutput RandomDecPotionBox(DecoratorClassicPotionInput input) => _potionService.RandomDecClassicPotionBox(input);
    }
}
