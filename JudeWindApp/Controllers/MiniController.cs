using JudeWind.Model.MiniPlay;
using JudeWind.Service.MiniPlay;
using Microsoft.AspNetCore.Mvc;

namespace JudeWindApp.Controllers
{
    /// <summary> Mini game (maybe) </summary>
    public class MiniController(ClassesService classesService) : BaseApiController
    {
        private ClassesService _classesService = classesService;

        /// <summary> 職業樹 </summary>
        [HttpPut]
        public ClassesTreeOutput GetTree(ClassesTreeInput input) => _classesService.GetTree(input);

    }
}
