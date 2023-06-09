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
    }
}
