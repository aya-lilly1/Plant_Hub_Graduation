using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plant_Hub_Core.Managers.Account;
using Plant_Hub_Core.Managers.Users;
using Plant_Hub_ModelView;
using System.Security.Principal;

namespace Plant_Hub.Controllers
{
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUser _user;
        public UserController(IUser user)
        {
            _user = user;    
        }
        [Route("api/user/CreateUser")]
        [HttpPost]
        public  IActionResult CreateUser([FromForm] SignupUser user)
        {
            var result =  _user.CreateUser(user);

            return Ok(result);
        }
        [Route("api/user/GetAllUser")]
        [HttpGet]
        public IActionResult GetAllUser()
        {
            var result =  _user.GetAllUser();
            return Ok(result);
        }

        [Route("api/user/SearchForUser")]
        [HttpGet]
        public IActionResult SearchForUser( string name)
        {
            var result = _user.SearchForUser(name);
            return Ok(result);
        }

        [Route("api/user/UpdateTypeOfUser")]
        [HttpPost]
        public IActionResult UpdateTypeOfUser(UpdateUserType updateUser)
        {
            var result = _user.UpdateTypeOfUser(updateUser);
            return Ok(result);
        }

        [Route("api/user/LockUserAccount")]
        [HttpPost]
        public IActionResult LockUserAccount([FromBody] LockAccountMV lockAccount)
        {
            var result = _user.LockUserAccount( lockAccount);
            return Ok(result);
        }
        [Route("api/user/UnlockUserAccount")]
        [HttpPost]
        public IActionResult UnlockUserAccount([FromBody] LockAccountMV lockAccount)
        {
            var result = _user.UnlockUserAccount( lockAccount);
            return Ok(result);
        }
    }
}
