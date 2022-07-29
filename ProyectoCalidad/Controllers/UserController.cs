using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using ProyectoCalidad.Services;
using ProyectoCalidad.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProyectoCalidad.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private UserService userService;

        public UserController()
        {
            userService = new UserService();
        }

        [HttpPost]
        [Route("ValidateCredentials")]
        [ValidateParam(typeof(UserObject))]
        public IActionResult ValidateCredentials(UserObject requestBody)
        {
            return userService.ValidateCredentials(requestBody);
        }

        [HttpPost]
        [Route("SignUp")]
        [ValidateParam(typeof(UserObject))]
        public IActionResult SignUp(UserObject userObject)
        {
            return userService.SignUp(userObject);
        }

        [HttpPost]
        [Route("FailedLogin")]
        [ValidateParam(typeof(UserObject))]
        public IActionResult FailedLogin(UserObject userObject)
        {
            return userService.FailedLogin(userObject);
        }

        [HttpPost]
        [Route("LockUser")]
        [ValidateParam(typeof(LockUserObject))]
        public IActionResult LockUser(LockUserObject userObject)
        {
            return userService.LockUser(userObject);
        }

        [HttpPost]
        [Route("UnlockUser")]
        [ValidateParam(typeof(LockUserObject))]
        public IActionResult UnlockUser(LockUserObject userObject)
        {
            return userService.UnlockUser(userObject);
        }
    }
}
