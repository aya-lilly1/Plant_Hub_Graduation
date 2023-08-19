using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plant_Hub_Core.Managers.Account;
using Plant_Hub_ModelView;
using System.Security.Principal;

namespace Plant_Hub.Controllers
{
    [Authorize]
    [ApiController]
    public class AccontController : Controller
    {
        private IAccount _account;
        public AccontController(IAccount account)
        {
           _account = account;
        }
        [AllowAnonymous]
        [Route("api/account/SignUp")]
        [HttpPost]
        public async Task<IActionResult> SignUpAsync([FromForm] SignupUser user)
        {
            var res = await _account.SignUp(user);

            return Ok(res);
        }

        [AllowAnonymous]
        [Route("api/account/SignIn")]
        [HttpPost]
        public async Task<IActionResult> SignIn([FromForm] LoginModelView user)
        {
            var res = await _account.SignIn(user);
            return Ok(res);
        }
    }
}
