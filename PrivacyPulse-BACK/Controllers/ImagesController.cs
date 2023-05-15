using CampingAPI.DataBase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrivacyPulse_BACK.Attributes;
using PrivacyPulse_BACK.Constants;
using PrivacyPulse_BACK.Entities;
using PrivacyPulse_BACK.Models;
using PrivacyPulse_BACK.Services;
using System.Security.Cryptography;

namespace PrivacyPulse_BACK.Controllers
{
    [Route("api/[controller]")]
    [JWTAuth]
    [ApiController]
    public class ImagesController : BaseController
    {
        private readonly PrivacyPulseContext dataContext;

        public ImagesController(PrivacyPulseContext dataContext)
        {
            this.dataContext = dataContext;
        }

        [Route("/api/users/{id}/profilePicture")]
        [AllowAnonymous]
        [HttpGet()]
        public async Task<ActionResult<FriendRequestModel>> GetProfile(int id)
        {
            if (System.IO.File.Exists(Paths.GetProfilePicturePath(id)))
            {
                return File(System.IO.File.ReadAllBytes(Paths.GetProfilePicturePath(id)), "image/png");
            }
            else
            {
                return NotFound();
            }
        }

    }
}
