using GreenUtility;
using GreenUtility.Equip;
using JudeWind.Model.Equips;
using JudeWind.Service.Equips;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace JudeWindApp.Controllers
{
    /// <summary> Equip Box </summary>
    public class EquipController(EquipService equipService) : BaseApiController
    {
        /// <summary> 裝備箱 </summary>
        [HttpPost]
        public List<EquipOutput> RandomEquipBox(EquipRandomInput input) => equipService.RandomEquipBox(input);
        /// <summary> 武器箱 </summary>
        [HttpPost]
        public List<EquipOutput> RandomWeaponBox(EquipRandomInput input) => equipService.RandomWeaponBox(input);
        /// <summary> 防具箱 </summary>
        [HttpPost]
        public List<EquipOutput> RandomArmorBox(EquipRandomInput input) => equipService.RandomArmorBox(input);
        /// <summary> 彩蛋裝備箱 </summary>
        [HttpPost]
        public List<DecoratorEquipOutput> RandomDecEquipBox(DecoratorEquipInput input) => equipService.RandomDecEquipBox(input);
        /// <summary> 彩蛋武器箱 </summary>
        /// <remarks>not use: Weapo, Armor</remarks>
        [HttpPost]
        public List<DecoratorEquipOutput> RandomDecWeaponBox(DecoratorEquipInput input) => equipService.RandomDecWeaponBox(input);
        /// <summary> 彩蛋防具箱 </summary>
        /// <remarks>not use: Weapon, Armor</remarks>
        [HttpPost]
        public List<DecoratorEquipOutput> RandomDecArmorBox(DecoratorEquipInput input) => equipService.RandomDecArmorBox(input);

        /// <summary> 抽獎箱 </summary>
        [HttpPost]
        public List<StoreEquipBoxOutput> StoreEquipBoxes(StoreEquipBoxInput input) => equipService.StoreEquipBoxes(input);
        /// <summary> 彩蛋抽獎箱 </summary>
        [HttpPost]
        public List<StoreDecEquipBoxOutput> StoreDecEquipBoxes(StoreDecEquipBoxInput input) => equipService.StoreDecEquipBoxes(input);

        /// <summary> 裝備套組 </summary>
        [HttpPost]
        public List<BaseEquip> SuitEquipBag(SuitEquipInput input) => equipService.SuitEquipBag(input);
        /// <summary> 匯出裝備套組 </summary>
        /// <remarks><list type="table">fileType:
        /// <item><term>1</term><description> xlsx </description></item>,
        /// <item><term>2</term><description> ods </description></item>,
        /// <item><term>3</term><description> pdf </description></item>
        /// </list></remarks>
        [HttpPost]
        public IActionResult ExportSuitEquip(SuitEquipInput input, int fileType) => fileType switch
        {
            1 => DownloadFileContent(equipService.ExportSuitExcel(input), $"{input.SuitType.GetEnumDescription()}套組-{DateTime.Now:yyyyMMddHHmm}.xlsx"),
            2 => DownloadFileStream(equipService.ExportSuitOds(input), $"{input.SuitType.GetEnumDescription()}套組-{DateTime.Now:yyyyMMddHHmm}.ods"),
            3 => DownloadFileContent(equipService.ExportSuitPdf(input), $"{input.SuitType.GetEnumDescription()}套組-{DateTime.Now:yyyyMMddHHmm}.pdf"),
            _ => StatusCode((int)HttpStatusCode.BadRequest),
        };
        /// <summary> 匯出裝備套組 </summary>
        /// <remarks><list type="table">fileType:
        /// <item><term>1</term><description> xlsx </description></item>,
        /// <item><term>2</term><description> ods </description></item>,
        /// <item><term>3</term><description> pdf </description></item>,
        /// <item><term>4</term><description> ods spirt </description></item>
        /// </list></remarks>
        [HttpGet]
        public IActionResult ExportAllSuitEquip(int fileType) => fileType switch
        {
            1 => DownloadFileContent(equipService.ExportAllSuitExcel(), $"究極套組-{DateTime.Now:yyyyMMddHHmm}.xlsx"),
            2 => DownloadFileStream(equipService.ExportAllSuitOds(), $"究極套組-{DateTime.Now:yyyyMMddHHmm}.ods"),
            3 => DownloadFileContent(equipService.ExportAllSuitPdf(), $"究極套組-{DateTime.Now:yyyyMMddHHmm}.pdf"),
            4 => DownloadFileStream(equipService.ExportAllSuitOds(true), $"究極套組-{DateTime.Now:yyyyMMddHHmm}.ods"),
            _ => StatusCode((int)HttpStatusCode.BadRequest),
        };
        /// <summary> 匯出裝備套組Log </summary>
        [HttpGet]
        public IActionResult ExportSuitLogExcel() => DownloadFileContent(equipService.ExportSuitLogExcel(), $"套組Log-{DateTime.Now:yyyyMMddHHmm}.xlsx");
        /// <summary> 匯出裝備套組Log </summary>
        [HttpGet]
        public IActionResult ExportSuitLogEpExcel() => DownloadFileContent(equipService.ExportSuitLogExcelEp(), $"套組Log-{DateTime.Now:yyyyMMddHHmm}.xlsx");
        /// <summary> 匯出裝備套組Log </summary>
        [HttpGet]
        public IActionResult ExportSuitLogEpTitleExcel() => DownloadFileContent(equipService.ExportSuitLogExcelEpWithTitle(), $"套組Log-{DateTime.Now:yyyyMMddHHmm}.xlsx");
    }
}
