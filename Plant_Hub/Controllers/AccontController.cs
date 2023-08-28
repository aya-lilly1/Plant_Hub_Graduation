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

        [AllowAnonymous]
        [Route("api/account/ConfirmationEmail")]
        [HttpPost]
        public IActionResult ConfirmationEmail(ConfirmCodeResetPasswordMV confirmMV)
        {
            var result = _account.ConfirmationCode(confirmMV.code, confirmMV.Email);
            return Ok(result);
        }

        [AllowAnonymous]
        [Route("api/account/SendEmailToResetPassword")]
        [HttpPost]
        public async Task<IActionResult> SendEmailToResetPassword(EmailMV emailMV)
        {
            var result = await _account.SendEmailToResetPassword(emailMV.Email);
            return Ok(result);
        }

        [AllowAnonymous]
        [Route("api/account/ConfirmCodeResetPassword")]
        [HttpPost]
        public IActionResult ConfirmCodeResetPassword(ConfirmCodeResetPasswordMV confirmMV)
        {
            var result = _account.ConfirmCodeResetPassword(confirmMV.Email, confirmMV.code);
            return Ok(result);
        }

        [AllowAnonymous]
        [Route("api/account/ResetPassword")]
        [HttpPost]
        public IActionResult ResetPassword( ResetPasswordMV resetPasswordMV)
        {
            var result = _account.ResetPassword( resetPasswordMV);
            return Ok(result);
        }

    }
}
