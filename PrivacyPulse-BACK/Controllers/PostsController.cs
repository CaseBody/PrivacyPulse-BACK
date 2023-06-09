using CampingAPI.DataBase;
using Microsoft.AspNetCore.Mvc;
using PrivacyPulse_BACK.Attributes;

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
        public async Task<ActionResult<int>> Create(int id)
        {
            var result = TryGetUserId(out var userId);

            if (!result) return Unauthorized();

            return Ok();
        }
    }
}
