using CampingAPI.DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrivacyPulse_BACK.Attributes;
using PrivacyPulse_BACK.Constants;
using PrivacyPulse_BACK.Entities;
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
                PostedAt = DateTime.UtcNow,
            };

            dataContext.Posts.Add(post);
            await dataContext.SaveChangesAsync();

            var memoryStream = new MemoryStream();
            postData.Image.CopyTo(memoryStream);

            System.IO.File.WriteAllBytes(Paths.GetPostImagePath(post.Id), memoryStream.ToArray());

            return Ok(post);
        }

        [Route("/api/posts/user")]
        [HttpGet()]
        public async Task<ActionResult> Get()
        {
            var result = TryGetUserId(out var userId);

            if (!result) return Unauthorized();

            var posts = await dataContext.Posts.Where(x => x.UserId == userId).ToListAsync();

            if (posts == null) return NotFound();

            return Ok(posts);
        }
    }
}
