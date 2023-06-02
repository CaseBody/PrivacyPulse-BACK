using CampingAPI.DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrivacyPulse_BACK.Attributes;
using PrivacyPulse_BACK.Entities;
using PrivacyPulse_BACK.Models;
using PrivacyPulse_BACK.Services;
using System.Security.Cryptography;

namespace PrivacyPulse_BACK.Controllers
{
    [Route("api/[controller]")]
    [JWTAuth]
    [ApiController]
    public class SettingsController : BaseController
    {
        private readonly PrivacyPulseContext dataContext;

        public SettingsController(PrivacyPulseContext dataContext)
        {
            this.dataContext = dataContext;
        }

        [HttpGet()]
        public async Task<ActionResult<SettingsModel>> Get()
        {
            var result = TryGetUserId(out var userId);

            if (!result) return Unauthorized();

            var user = await dataContext.Users.FirstAsync(x => x.Id == userId);

            return Ok(new SettingsModel
            {
                PrivateProfile = user.PrivateProfile,
            });
        }

        [HttpPost()]
        public async Task<IActionResult> Post(SettingsModel settings)
        {
            var result = TryGetUserId(out var userId);  

            if (!result) return Unauthorized();

            var user = await dataContext.Users.FirstAsync(x => x.Id == userId);
            
            user.PrivateProfile = settings.PrivateProfile;

            await dataContext.SaveChangesAsync();

            return Ok();
        }
    }
}
