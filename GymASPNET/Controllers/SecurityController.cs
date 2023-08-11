using GymASPNET.CustomClass;
using GymASPNET.Models;
using GymASPNET.Security;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GymASPNET.Controllers
{
    [RoutePrefix("api/security")]
    public class SecurityController : ApiController
    {
        private AuthRepository _repo;

        public SecurityController()
        {
            _repo = new AuthRepository();
        }

        [HttpPost]
        [Route("register")]
        public IHttpActionResult Register(User userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = _repo.RegisterUser(userModel);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Registration failed");
            }
        }

        [HttpPost]
        [Route("login")]
        public IHttpActionResult Login(LoginModel loginModel)
        {
            var user = _repo.FindUser(loginModel.UserName, loginModel.Password);

            if (user != null)
            {
                // Generate and return a JWT token
                string token = _repo.GenerateToken(user);
                return Ok(new { Token = token });
            }
            else
            {
                return BadRequest("Invalid login credentials");
            }
        }
    }
}
