using JudeWind.Model.Items;
using JudeWind.Service.Items;
using Microsoft.AspNetCore.Mvc;

namespace JudeWindApp.Controllers
{
    /// <summary> Potion Box </summary>
    public class PotionController(PotionService potionService, PowerItemService powerItemService) : BaseApiController
    {
        /// <summary> 藥水箱 </summary>
        [HttpPost]
        public List<ClassicPotionOutput> RandomPotionBox(ClassicPotionRandomInput input) => potionService.RandomClassicPotionBox(input);

        /// <summary> 彩蛋藥水箱 </summary>
        [HttpPost]
        public List<DecoratorClassicPotionOutput> RandomDecPotionBox(DecoratorClassicPotionInput input) => potionService.RandomDecClassicPotionBox(input);

        /// <summary> 道具箱 </summary>
        [HttpPost]
        public List<PowerItemRandomOutput> RandomPowerItemBox(PowerItemRandomInput input) => powerItemService.RandomPowerItemBox(input);
        /// <summary> 小型道具箱 </summary>
        [HttpGet("{numbers}")]
        public List<PowerItemRandomOutput> RandomPowerItemSamllBox(int numbers) => powerItemService.RandomPowerItemSamllBox(numbers);
        /// <summary> 道具箱 </summary>
        [HttpPost]
        public List<PowerItemRandomOutput> PowerItemBox(PowerItemInput input) => powerItemService.PowerItemBox(input);
    }
}
