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
        public IActionResult CreateCategoty([FromForm]CategoryMV category)
        {
            var res = _catregory.CreateCategoty(category);
            return Ok(res);
        }

        [AllowAnonymous]
        [Route("api/Category/GetAllCategories")]
        [HttpGet]
        public IActionResult GetAllCategories()
        {
            var res = _catregory.GetAllCategories();
            return Ok(res);
        }

        [AllowAnonymous]
        [Route("api/Category/GetCategoryById")]
        [HttpGet]
        public IActionResult GetCategoryById(int categoryId)
        {
            var res = _catregory.GetCategoryById(categoryId);
            return Ok(res);
        }

        [Route("api/Category/UpdateCategoty")]
        [HttpPost]
        public IActionResult UpdateCategoty( int categoryId,[FromForm] CategoryMV category)
        {
            var res = _catregory.UpdateCategoryById(categoryId, category);
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
