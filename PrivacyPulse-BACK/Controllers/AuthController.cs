using PrivacyPulse_BACK.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using CampingAPI.DataBase;
using PrivacyPulse_BACK.Entities;
using PrivacyPulse_BACK.Models;

namespace PrivacyPulse_BACK.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly PrivacyPulseContext dataContext;
        private readonly IConfiguration configuration;
        private readonly JWTService jwtService;


        public AuthController(PrivacyPulseContext dataContext, IConfiguration configuration)
        {
            this.dataContext = dataContext;
            this.configuration = configuration;
            jwtService = new JWTService(configuration.GetSection("AppSettings:Token").Value);
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserCreationModel model)
        {
            var user = await dataContext.Users.Where(g => g.Username == model.Username).FirstOrDefaultAsync();

            if (user != null)
            {
                return BadRequest("Username is already in use.");
            }

            var newUser = new User();
            CreatePasswordHash(model.Password, out byte[] passwordHash, out byte[] passwordSalt);

            newUser.Username = model.Username;
            newUser.Biography = "";
            newUser.PasswordHash = passwordHash;
            newUser.PasswordSalt = passwordSalt;

            dataContext.Users.Add(newUser);
            dataContext.SaveChanges();

            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserLoginModel model)
        {
            var user = await dataContext.Users.Where(g => g.Username == model.Username).FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound("User was not found");
            }

            if (!VerifyPasswordHash(model.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Username or Password does not match");
            }

            return Ok(jwtService.CreateJWT(user));
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
