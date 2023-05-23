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

        [HttpGet("find")]
        public async Task<ActionResult<FriendModel>> FindFriends(string? name)
        {
            var result = TryGetUserId(out var userId);  

            if (!result) return Unauthorized();

            var user = await dataContext.Users.Include(x => x.Friends).FirstAsync(x => x.Id == userId);

            var query = dataContext.Users.Where(x => x.Id != user.Id);

            if (name != "" && name != null)
            {
                query = query.Where(x => x.Username.ToLower().Contains(name.ToLower()));
            }

            var outgoingRequests = await dataContext.FriendRequests.Where(x => x.FromUserId == user.Id).ToListAsync();

            var users = await query.Take(10).ToListAsync();

            users = users.Where(x => !user.Friends.Any(y => y.FriendUserId == x.Id) && !outgoingRequests.Any(y => y.ToUserId == x.Id)).ToList();

            return Ok(users.Select(x => new
            {
                Id = x.Id,
                Username = x.Username
            }));
        }
    }
}
