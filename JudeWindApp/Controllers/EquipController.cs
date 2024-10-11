using JudeWind.Model.Equips;
using JudeWind.Service.Equips;
using Microsoft.AspNetCore.Mvc;

namespace JudeWindApp.Controllers
{
    /// <summary> Equip Box </summary>
    public class EquipController(EquipService equipService) : BaseApiController
    {
        readonly EquipService _equipService = equipService;
        /// <summary> 裝備箱 </summary>
        [HttpPost]
        public EquipOutput RandomEquipBox(EquipRandomInput input) => _equipService.RandomEquipBox(input);
        /// <summary> 武器箱 </summary>
        [HttpPost]
        public EquipOutput RandomWeaponBox(EquipRandomInput input) => _equipService.RandomWeaponBox(input);
        /// <summary> 防具箱 </summary>
        [HttpPost]
        public EquipOutput RandomArmorBox(EquipRandomInput input) => _equipService.RandomArmorBox(input);
        /// <summary> 彩蛋裝備箱 </summary>
        [HttpPost]
        public DecoratorEquipOutput RandomDecEquipBox(DecoratorEquipInput input) => _equipService.RandomDecEquipBox(input);
        /// <summary> 彩蛋武器箱 </summary>
        [HttpPost]
        public DecoratorEquipOutput RandomDecWeaponBox(DecoratorEquipInput input) => _equipService.RandomDecWeaponBox(input);
        /// <summary> 彩蛋防具箱 </summary>
        [HttpPost]
        public DecoratorEquipOutput RandomDecArmorBox(DecoratorEquipInput input) => _equipService.RandomDecArmorBox(input);
    }
}
