using CampingAPI.DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrivacyPulse_BACK.Attributes;
using PrivacyPulse_BACK.Constants;
using PrivacyPulse_BACK.Entities;
using PrivacyPulse_BACK.Enums;
using PrivacyPulse_BACK.Models;

namespace PrivacyPulse_BACK.Controllers
{
    [Route("api/[controller]")]
    [JWTAuth]
    [ApiController]
    public class PostsController : BaseController
    {
        private readonly PrivacyPulseContext dataContext;

        public PostsController(PrivacyPulseContext dataContext)
        {
            this.dataContext = dataContext;
        }

        [Route("/api/posts/create")]
        [HttpPost()]
        public async Task<ActionResult> Create([FromForm] PostModal postData)
        {
            var result = TryGetUserId(out var userId);

            if (!result) return Unauthorized();

            var user = await dataContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

            var post = new Post
            {
                UserId = (int)userId,
                Body = postData.Body,
                PostedAt = DateTime.UtcNow.AddHours(2),
            };

            dataContext.Posts.Add(post);
            await dataContext.SaveChangesAsync();

            var memoryStream = new MemoryStream();
            postData.Image.CopyTo(memoryStream);

            System.IO.File.WriteAllBytes(Paths.GetPostImagePath(post.Id), memoryStream.ToArray());

            return Ok(post);
        }

        [Route("/api/users/{id}/posts")]
        [HttpGet()]
        public async Task<ActionResult<List<PostModel>>> GetUserPosts(int id)
        {
            var result = TryGetUserId(out var userId);

            if (!result) return Unauthorized();

            var user = await dataContext.Users.Include(x => x.Friends).FirstOrDefaultAsync(x => x.Id == id);

            if (user == null) return NotFound();

            if (id != userId && !user.Friends.Any(x => x.FriendUserId == userId) && user.PrivateProfile)
            {
                return Unauthorized();
            }

            var posts = await dataContext.Posts.Where(x => x.UserId == id).ToListAsync();

            return Ok(posts.Select(x => new PostModel
            {
                Id = x.Id,
                Body = x.Body,
                UserId = x.UserId,
                Username = user.Username,
                PostedAt = x.PostedAt,
                Image = new Func<string>(() =>
                {
                    var imageBytes = System.IO.File.ReadAllBytes(Paths.GetPostImagePath(x.Id));
                    return Convert.ToBase64String(imageBytes);
                })()
            }));
        }

        [HttpPost("{id}/share")]
        public async Task<IActionResult> Share(int id, int userChatId)
        {
            var result = TryGetUserId(out var userId);

            if (!result) return Unauthorized();

            var userChat = await dataContext.UserChats.Include(x => x.Chat).ThenInclude(x => x.Messages).FirstOrDefaultAsync(x => x.Id == userChatId);

            if (userChat == null) return NotFound();

            var post = await dataContext.Posts.FirstOrDefaultAsync(x => x.Id == id);

            if (post == null) return NotFound();

            userChat.Chat.Messages.Add(new Message
            {
                FromUserId = userId,
                MessageType = MessageType.SharedPost,
                PostId = id,
                SendDate = DateTime.UtcNow.AddHours(2),
            });

            await dataContext.SaveChangesAsync();

            return Ok();
        }
    }
}
