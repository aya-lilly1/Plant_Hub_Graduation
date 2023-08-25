using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Plant_Hub.Controllers
{
    public class BaseController : ControllerBase
    {
        public readonly string _UserId;
        public readonly IHttpContextAccessor _httpContextAccessor;
        public BaseController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _UserId = _httpContextAccessor.HttpContext.User.FindFirst("UserId")?.Value;

        }
    }
}
