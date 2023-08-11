using GymASPNET.Models;
using System;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;

namespace GymASPNET.Controllers
{
    public class UserController : ApiController
    {
        //DbContext instace 
        private GYMEntitiesGlobal dbContext = new GYMEntitiesGlobal();


        [HttpGet]
        public IHttpActionResult GetUser(int userId)
        {
            var user = dbContext.Users.FirstOrDefault(x => x.Id == userId);
            if (user != null)
            {
                return Content(HttpStatusCode.OK, new { obj = user, request = "Process completed success" });
            }
            else
            {
                return Content(HttpStatusCode.NotFound, new { obj = user, request = "No Found" });
            }
        }

        [HttpGet]
        public IHttpActionResult GetUserList()
        {
            try
            {
                var result = dbContext.Users.ToList();
                return Content(HttpStatusCode.OK, new { obj = result, request = "Process completed success" });
            }catch(Exception ex)
            {
                return Content(HttpStatusCode.NotFound, new { obj = "", request = ex.Message });
            }
        }

        [HttpPost]
        public IHttpActionResult CreateUser(User user)
        {
            try
            {
                byte[] salt = GenerateSalt();
                // Hash the password with the salt
                user.Password = HashPassword(user.Password, salt);
                dbContext.Users.Add(user);
                dbContext.SaveChanges();
                return Content(HttpStatusCode.OK, new { obj = user, request = "Process completed success" });
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.NotFound, new { obj = "", request = ex.Message });
            }
        }

        static byte[] GenerateSalt()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] salt = new byte[16]; // You can adjust the salt length as needed
                rng.GetBytes(salt);
                return salt;
            }
        }

        static string HashPassword(string password, byte[] salt)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] combinedBytes = new byte[passwordBytes.Length + salt.Length];

                Array.Copy(passwordBytes, 0, combinedBytes, 0, passwordBytes.Length);
                Array.Copy(salt, 0, combinedBytes, passwordBytes.Length, salt.Length);

                byte[] hashBytes = sha256.ComputeHash(combinedBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}