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
    public class FriendRequestsController : BaseController
    {
        private readonly PrivacyPulseContext dataContext;

        public FriendRequestsController(PrivacyPulseContext dataContext)
        {
            this.dataContext = dataContext;
        }

        [HttpGet()]
        public async Task<ActionResult<FriendRequestModel>> Get()
        {
            var result = TryGetUserId(out var userId);

            if (!result) return Unauthorized();

            var user = await dataContext.Users.Include(x => x.Friends).FirstAsync(x => x.Id == userId);

            var friendRequests = await dataContext.FriendRequests
                .Include(x => x.FromUser)
                .ThenInclude(x => x.Friends)
                .Where(x => x.ToUserId == userId)
                .ToListAsync(); ;

            return Ok(friendRequests.Select(x => new FriendRequestModel
            {
                UserId = x.FromUserId,
                Username = x.FromUser.Username,
                MutualFriends = x.FromUser.Friends.Select(x => x.FriendUserId).Sum(x => user.Friends.Any(y => y.FriendUserId == x) ? 1 : 0)
            }));
        }

        [HttpPost()]
        public async Task<IActionResult> Post(int addedUserId)
        {
            var result = TryGetUserId(out var userId);

            if (!result) return Unauthorized();

            if (addedUserId == userId || addedUserId == null) return BadRequest();

            var userExists = await dataContext.Users.AnyAsync(x => x.Id == addedUserId);

            if (!userExists) return NotFound("User could not be found");

            var existingFriendRequest = await dataContext.FriendRequests.FirstOrDefaultAsync(x => x.FromUserId == (int)userId && x.ToUserId == addedUserId);

            if (existingFriendRequest != null) return BadRequest("User has already sent a friend request");

            var incomingFriendRequest = await dataContext.FriendRequests.FirstOrDefaultAsync(x => x.ToUserId == (int)userId && x.FromUserId == addedUserId);

            if (incomingFriendRequest != null)
            {
                var newFriend = new UserFriend
                {
                    UserId = (int)userId,
                    FriendUserId = addedUserId
                };

                var newFriend2 = new UserFriend
                {
                    UserId = addedUserId,
                    FriendUserId = (int)userId
                };

                dataContext.UserFriends.Add(newFriend);
                dataContext.UserFriends.Add(newFriend2);
                await dataContext.SaveChangesAsync();

                return Ok();
            }

            var newRequest = new FriendRequest
            {
                FromUserId = (int)userId,
                ToUserId = addedUserId,
            };

            dataContext.FriendRequests.Add(newRequest);
            await dataContext.SaveChangesAsync();

            return Ok();
        }
    }
}
