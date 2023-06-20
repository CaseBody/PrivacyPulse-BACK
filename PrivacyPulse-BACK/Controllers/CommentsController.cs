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
    public class CommentsController : BaseController
    {
        private readonly PrivacyPulseContext dataContext;

        public CommentsController(PrivacyPulseContext dataContext)
        {
            this.dataContext = dataContext;
        }

        [Route("/api/comment/add")]
        [HttpPost()]
        public async Task<ActionResult> Create([FromForm] CommentModel commentData)
        {
            var result = TryGetUserId(out var userId);

            if (!result) return Unauthorized();

            var selectedPost = await dataContext.Posts.FirstOrDefaultAsync(p => p.Id == commentData.Id);

            if (selectedPost == null) return NotFound();

            var comment = new Comment
            {
                UserId = (int)userId,
                PostId = selectedPost.Id,
                Body = commentData.Body,
            };

            return Ok(comment);
        }
    }
}
