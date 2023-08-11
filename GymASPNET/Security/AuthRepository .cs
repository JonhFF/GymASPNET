
using GymASPNET.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace GymASPNET.Security
{
    public class AuthRepository
    {
        private UserManager<IdentityUser> _userManager;

        public AuthRepository()
        {
            _userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(new GYMEntitiesGlobal()));
        }

        public IdentityResult RegisterUser(User userModel)
        {
            var user = new IdentityUser
            {
                UserName = userModel.UserName
            };

            return _userManager.Create(user, userModel.Password);
        }

        public IdentityUser FindUser(string userName, string password)
        {
            return _userManager.Find(userName, password);
        }

        public string GenerateToken(IdentityUser user)
        {
            var key = Encoding.UTF8.GetBytes("your_secret_key_here");
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName)
        };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}