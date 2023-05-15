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
    public class FriendsController : BaseController
    {
        private readonly PrivacyPulseContext dataContext;

        public FriendsController(PrivacyPulseContext dataContext)
        {
            this.dataContext = dataContext;
        }

        [HttpGet()]
        public async Task<ActionResult<FriendModel>> Get()
        {
            var result = TryGetUserId(out var userId);

            if (!result) return Unauthorized();

            var user = await dataContext.Users.Include(x => x.Friends).ThenInclude(x => x.FriendUser).FirstAsync(x => x.Id == userId);

            return Ok(user.Friends.Select(x => new FriendModel
            {
                UserId = x.FriendUserId,
                Username = x.FriendUser.Username,
            }));
        }
    }
}
