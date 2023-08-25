using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plant_Hub_Core.Managers.Account;
using Plant_Hub_ModelView;
using System.Security.Principal;

namespace Plant_Hub.Controllers
{
    [Authorize]
    [ApiController]
    public class AccontController : ControllerBase
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
            var result = await _account.SignUp(user);

            return Ok(result);
        }

        [AllowAnonymous]
        [Route("api/account/SignIn")]
        [HttpPost]
        public async Task<IActionResult> SignIn(LoginModelView user)
        {
            var result = await _account.SignIn(user);
            return Ok(result);
        }

        
        [Route("api/account/ConfirmationEmail")]
        [HttpPost]
        public IActionResult ConfirmationEmail( int confirmationCode, string email)
        {
            var result = _account.ConfirmationCode(confirmationCode, email);
            return Ok(result);
        }

        [AllowAnonymous]
        [Route("api/account/SendEmailToResetPassword")]
        [HttpPost]
        public async Task<IActionResult> SendEmailToResetPassword( string email)
        {
            var result = await _account.SendEmailToResetPassword( email);
            return Ok(result);
        }

        [AllowAnonymous]
        [Route("api/account/ConfirmCodeResetPassword")]
        [HttpPost]
        public IActionResult ConfirmCodeResetPassword(string email, int code)
        {
            var result = _account.ConfirmCodeResetPassword(email, code);
            return Ok(result);
        }

        [AllowAnonymous]
        [Route("api/account/ResetPassword")]
        [HttpPost]
        public IActionResult ResetPassword(string email, ResetPasswordMV resetPasswordMV)
        {
            var result = _account.ResetPassword(email, resetPasswordMV);
            return Ok(result);
        }

    }
}
