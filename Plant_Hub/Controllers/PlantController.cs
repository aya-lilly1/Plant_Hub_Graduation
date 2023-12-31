﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plant_Hub_Core.Managers.Categories;
using Plant_Hub_Core.Managers.Plants;
using Plant_Hub_Core.Managers.Users;
using Plant_Hub_Models.Models;
using Plant_Hub_ModelView;

namespace Plant_Hub.Controllers
{
    [Authorize]
    [ApiController]
    public class PlantController : BaseController
    {
        private IPlant _plant;
        private readonly IHttpContextAccessor __httpContextAccessor;
        public PlantController(IPlant plant, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _plant = plant ;
            __httpContextAccessor = httpContextAccessor;
        }

        [Route("api/Plant/CreatePlant")]
        [HttpPost]
        public IActionResult CreatePlant([FromForm] CreatePlantMV plant)
        {
            var res = _plant.CreatePlant(plant);
            return Ok(res);
        }

        [AllowAnonymous]
        [Route("api/Plant/GetAllPlants")]
        [HttpGet]
        public IActionResult GetAllPlants(int langId)
        {
            var res = _plant.GetAllPlants(_UserId,langId);
            return Ok(res);
        }

        [AllowAnonymous]
        [Route("api/Plant/GetAllPlantsWithOutLang")]
        [HttpGet]
        public IActionResult GetAllPlantsWithOutLang()
        {
            var res = _plant.GetAllPlants();
            return Ok(res);
        }

        [AllowAnonymous]
        [Route("api/Plant/GetPlantById")]
        [HttpGet]
        public IActionResult GetPlantById(int plantId, int langId)
        {
            var res = _plant.GetPlantById(plantId, langId);
            return Ok(res);
        }

        [AllowAnonymous]
        [Route("api/Plant/GetPlanstByCategoryId")]
        [HttpGet]
        public IActionResult GetPlantsByCategoryId(int categoryId, int langId)
        {
            var res = _plant.GetPlantByCategoryId(categoryId, langId);
            return Ok(res);
        }
        [Route("api/Plant/SearchForPlantsByCategory")]
        [HttpGet]
        public IActionResult SearchForPlantsByCategory(String PlantName, int CategoryId, int langId)
        {
            var result = _plant.SearchForPlantsByCategory(PlantName, CategoryId,langId, _UserId);
            return Ok(result);
        }
        [Route("api/Plant/SearchForPlants")]
        [HttpGet]
        public IActionResult SearchForPlants(String PlantName, int langId)
        {
            var result = _plant.SearchForPlants(PlantName, langId, _UserId);
            return Ok(result);
        }
        [Route("api/Plant/OrderToAddNewPlant")]
		[HttpPost]
		public IActionResult OrderToAddNewPlant([FromBody] OrderAddNewPlantMV plant)
		{
			var res = _plant.OrderToAddNewPlant(_UserId,plant);
			return Ok(res);
		}
		[Route("api/Plant/GetAllOrderToAddNewPlant")]
		[HttpGet]
		public IActionResult GetAllOrderToAddNewPlant()
		{
			var result = _plant.GetAllOrderToAddNewPlant();
			return Ok(result);
		}
		[Route("api/Plant/DeleteOrderToAddNewPlant")]
		[HttpDelete]
		public IActionResult DeleteOrderToAddNewPlant(int id)
		{
			var res = _plant.DeleteOrderToAddNewPlant(id);
			return Ok(res);
		}

		[Route("api/Plant/UpdatePlant")]
        [HttpPut]
        public IActionResult UpdatePlantById([FromForm] UpdatePlantMV plant)
        {
            var res = _plant.UpdatePlantById( plant);
            return Ok(res);
        }

        [Route("api/Plant/DeletePlant")]
        [HttpDelete]
        public IActionResult DeletePlantById(int plantId)
        {
            var res = _plant.DeletePlantById(plantId);
            return Ok(res);
        }
    }
}
