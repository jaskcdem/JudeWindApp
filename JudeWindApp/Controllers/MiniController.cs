using JudeWind.Model.MiniPlay;
using JudeWind.Service.MiniPlay;
using Microsoft.AspNetCore.Mvc;

namespace JudeWindApp.Controllers
{
    /// <summary> Mini game (maybe) </summary>
    public class MiniController(ClassesService classesService) : BaseApiController
    {
        /// <summary> 職業樹 </summary>
        [HttpPut]
        public ClassesTreeOutput GetTree(ClassesTreeInput input) => classesService.GetTree(input);

    }
}
