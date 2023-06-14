using CampingAPI.DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrivacyPulse_BACK.Attributes;
using PrivacyPulse_BACK.Constants;
using PrivacyPulse_BACK.Models;

namespace PrivacyPulse_BACK.Controllers
{
    [Route("api/[controller]")]
    [JWTAuth]
    [ApiController]
    public class UsersController : BaseController
    {
        private readonly PrivacyPulseContext dataContext;

        public UsersController(PrivacyPulseContext dataContext)
        {
            this.dataContext = dataContext;
        }

        [HttpGet("{id}/profile")]
        public async Task<ActionResult<ProfileModel>> Get(int id)
        {
            var result = TryGetUserId(out var userId);

            if (!result) return Unauthorized();

            var user = await dataContext.Users.Include(x => x.Friends).FirstOrDefaultAsync(x => x.Id == id);

            if (user == null) return NotFound();

            if (id != userId && !user.Friends.Any(x  => x.FriendUserId == userId) && user.PrivateProfile)
            {
                return Unauthorized();
            }
            
            return Ok(new ProfileModel
            {
                Id = user.Id,
                Username = user.Username,
                Biography = user.Biography,
            });

        }

        [HttpPut("updateBio")]
        public async Task<IActionResult> Put(string bio)
        {
            var result = TryGetUserId(out var userId);

            if (!result) return Unauthorized();

            var user = await dataContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

            user.Biography = bio;

            await dataContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("uploadProfileImage")]
        public async Task<IActionResult> UploadProfileImage(IFormFile file)
        {
            var result = TryGetUserId(out var userId);

            if (!result) return Unauthorized();

            var memoryStream = new MemoryStream();
            file.CopyTo(memoryStream);

            System.IO.File.WriteAllBytes(Paths.GetProfilePicturePath((int)userId), memoryStream.ToArray());
            return Ok();
        }

        [HttpGet("feed")]
        public async Task<ActionResult<ProfileModel>> GetFeed()
        {
            var result = TryGetUserId(out var userId);

            if (!result) return Unauthorized();

            var user = await dataContext.Users.Include(x => x.Friends).FirstOrDefaultAsync(x => x.Id == userId);

            var friendUserIds = user.Friends.Select(f => f.FriendUserId);

            var posts = await dataContext.Posts
                .Include(x => x.User)
                .Where(x => friendUserIds.Contains(x.UserId))
                .OrderByDescending(x => x.PostedAt)
                .Take(10)
                .ToListAsync();

            return Ok(posts.Select(x => new PostModel
            {
                Id = x.Id,
                Body = x.Body,
                UserId = x.UserId,
                Username = x.User.Username,
                PostedAt = x.PostedAt,
                Image = new Func<string>(() =>
                {
                    var imageBytes = System.IO.File.ReadAllBytes(Paths.GetPostImagePath(x.Id));
                    return Convert.ToBase64String(imageBytes);
                })()
            }));
        }
    }
}
