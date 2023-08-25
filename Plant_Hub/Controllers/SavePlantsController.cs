using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plant_Hub_Core.Managers.Plants;
using Plant_Hub_ModelView;

namespace Plant_Hub.Controllers
{
    [Authorize]
    [ApiController]
    public class SavePlantsController : BaseController
    {
        private readonly IPlant _plant;
        private readonly IHttpContextAccessor __httpContextAccessor;
        public SavePlantsController(IPlant plant, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _plant = plant;
            __httpContextAccessor = httpContextAccessor;
        }

        [Route("api/SavePlant/SaveNewPlant")]
        [HttpPost]
        public IActionResult SavePlant(int plantId)
        {
            var res = _plant.SavePlant(_UserId, plantId);
            return Ok(res);
        }

        [Route("api/SavePlant/GetPreservedPlants")]
        [HttpGet]
        public IActionResult GetPreservedPlants()
        {
            var res = _plant.GetPreservedPlants(_UserId);
            return Ok(res);
        }
        [Route("api/SavePlant/DeletePreservedPlant")]
        [HttpDelete]
        public IActionResult DeletePreservedPlant(int idToControl)
        {
            var res = _plant.DeletePreservedPlant( idToControl);
            return Ok(res);
        }

        [Route("api/SavePlant/RemovePreservedPlant")]
        [HttpDelete]
        public IActionResult RemovePreservedPlant(int PlantId)
        {
            var res = _plant.RemovePreservedPlant(_UserId, PlantId);
            return Ok(res);
        }

    }
}
