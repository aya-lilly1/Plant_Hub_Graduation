using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plant_Hub_Core.Managers.Account;
using Plant_Hub_Core.Managers.Categories;
using Plant_Hub_ModelView;

namespace Plant_Hub.Controllers
{
    [Authorize]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private ICatregory _catregory;
        public CategoryController(ICatregory catregory)
        {
            _catregory = catregory;
        }
       
        [Route("api/Category/CreateCategory")]
        [HttpPost]
        public async Task<IActionResult> CreateCategoty([FromForm] CreateCategoryMV category)
        {
            var res =await _catregory.CreateCategoty(category);
            return Ok(res);
        }

        [AllowAnonymous]
        [Route("api/Category/GetAllCategories")]
        [HttpGet]
        public IActionResult GetAllCategories(int langId)
        {
            var res = _catregory.GetAllCategories( langId);
            return Ok(res);
        }

        [AllowAnonymous]
        [Route("api/Category/GetCategoryById")]
        [HttpGet]
        public IActionResult GetCategoryById(int categoryId, int langId)
        {
            var res = _catregory.GetCategoryById(categoryId, langId);
            return Ok(res);
        }
        [Route("api/Category/SearchForCategory")]
        [HttpGet]
        public IActionResult SearchForCategory(string categoryName, int langId)
        {
            var result = _catregory.SearchForCategory(categoryName, langId);
            return Ok(result);
        }

        [Route("api/Category/UpdateCategoty")]
        [HttpPost]
        public IActionResult UpdateCategoty( [FromForm] CategoryMV category)
        {
            var res = _catregory.UpdateCategoryById( category);
            return Ok(res);
        }

        [Route("api/Category/DeleteCategoty")]
        [HttpDelete]
        public IActionResult DeleteCategoty(int categoryId)
        {
            var res = _catregory.DeleteCategoryById(categoryId);
            return Ok(res);
        }

    }
}
