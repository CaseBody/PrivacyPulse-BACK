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
    public class ChatsController : BaseController
    {
        private readonly PrivacyPulseContext dataContext;

        public ChatsController(PrivacyPulseContext dataContext)
        {
            this.dataContext = dataContext;
        }

        [Route("/api/chats/create")]
        [HttpPost()]
        public async Task<ActionResult<int>> Create(int id)
        {
            var result = TryGetUserId(out var userId);

            if (!result) return Unauthorized();

            var user = await dataContext.Users
                .Include(x => x.UserChats)
                .ThenInclude(x => x.Chat)
                .Include(x => x.Friends)
                .FirstOrDefaultAsync(x => x.Id == userId);

            var newChatUser = await dataContext.Users
                .Include(x => x.UserChats)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (newChatUser == null) return NotFound();

            if (!user.Friends.Any(x => x.FriendUserId == id)) return BadRequest();

            if (user.UserChats.Any(x => x.Chat.UserChats.Any(x => x.UserId == id))) return BadRequest();

            var chat = new Chat
            {
                UserChats = new List<UserChat>
                {
                    new UserChat { UserId = id },
                    new UserChat { UserId = user.Id }
                }
            };

            dataContext.Chats.Add(chat);

            await dataContext.SaveChangesAsync();

            return Ok(chat.Id);
        }

        [HttpGet("find")]
        public async Task<ActionResult<FriendModel>> FindChat(string? name)
        {
            var result = TryGetUserId(out var userId);

            if (!result) return Unauthorized();

            var user = await dataContext.Users
                .Include(x => x.Friends)
                .ThenInclude(x => x.FriendUser)
                .ThenInclude(x => x.UserChats)
                .ThenInclude(x => x.Chat)
                .ThenInclude(x => x.UserChats)
                .FirstAsync(x => x.Id == userId);

            var query = user.Friends.Where(x => !x.FriendUser.UserChats.Any(c => c.Chat.UserChats.Any(x => x.UserId == userId)));

            if (name != "" && name != null)
            {
                query = query.Where(x => x.FriendUser.Username.ToLower().Contains(name.ToLower()));
            }

            var users = query.ToList().Select(x => x.FriendUser);

            return Ok(users.Select(x => new
            {
                x.Id,
                x.Username
            }));
        }

        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            var result = TryGetUserId(out var userId);

            if (!result) return Unauthorized();

            var chats = await dataContext.Chats
                .Include(x => x.UserChats)
                .ThenInclude(x => x.User)
                .Where(x => x.UserChats.Any(x => x.UserId == userId)).ToListAsync();

            return Ok(chats.Select(x => new
            {
                x.Id,
                UserId = x.UserChats.First(x => x.UserId != userId).UserId,
                Username = x.UserChats.First(x => x.UserId != userId).User.Username,
                publicKey = x.UserChats.First(x => x.UserId != userId).User.PublicKey,
            }));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = TryGetUserId(out var userId);

            if (!result) return Unauthorized();

            var chat = await dataContext.Chats
                .Include(x => x.UserChats)
                .ThenInclude(x => x.User)
                .Include(x => x.Messages)
                .ThenInclude(x => x.MessageContents)
                .Where(x => x.Id == id).FirstOrDefaultAsync();

            if (chat == null) return NotFound();

            if (!chat.UserChats.Any(x => x.UserId == userId)) return Unauthorized();

            return Ok(chat.Messages.Select(x => new
            {
                x.FromUserId,
                x.SendDate,
                message = x.MessageContents.First(x => x.ForUserId == userId).CipherText
            }));
        }

    }
}
