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
        public async Task<ActionResult> Create(CommentModel commentData)
        {
            var result = TryGetUserId(out var userId);

            if (!result) return Unauthorized();

            var selectedPost = await dataContext.Posts.FirstOrDefaultAsync(p => p.Id == commentData.CommentId);

            if (selectedPost == null) return NotFound();

            var comment = new Comment
            {
                UserId = (int)userId,
                PostId = selectedPost.Id,
                Body = commentData.Body,
            };

            dataContext.Add(comment);
            await dataContext.SaveChangesAsync();

            return Ok(comment);
        }
        
        [Route("/api/comment/{id}/delete")]
        [HttpDelete()]
        public async Task<ActionResult> Create(int id)
        {
            var result = TryGetUserId(out var userId);

            if (!result) return Unauthorized();

            var selectedComment = await dataContext.Comments.FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

            if (selectedComment == null) return NotFound();

            dataContext.Remove(selectedComment);
            await dataContext.SaveChangesAsync();

            return Ok();
        }

        [Route("/api/comments/{id}/get")]
        [HttpGet()]
        public async Task<ActionResult> Get(int id)
        {
            var result = TryGetUserId(out var userId);

            if (!result) return Unauthorized();

            var selectedComment = await dataContext.Comments.Include(u => u.User).Where(c => c.PostId == id && c.UserId == userId).ToListAsync();

            if (selectedComment == null) return NotFound();

            return Ok(selectedComment.Select(x => new GetCommentModel
            {
                Id = x.User.Id,
                username = x.User.Username,
                body = x.Body,
            }));
        }
    }
}
