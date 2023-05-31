using CampingAPI.DataBase;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PrivacyPulse_BACK.Entities;
using PrivacyPulse_BACK.Enums;
using PrivacyPulse_BACK.Services;

namespace PrivacyPulse_BACK.Hubs
{
    public class ChatHub : Hub
    {
        private readonly PrivacyPulseContext dataContext;
        private readonly JWTService jwtService;

        public ChatHub(PrivacyPulseContext dataContext, IConfiguration configuration)
        {
            this.dataContext = dataContext;
            jwtService = new JWTService(configuration.GetSection("AppSettings:Token").Value);
        }

        public async Task Connect(int chatId, string token)
        {
            var chat = await dataContext.Chats
                .Include(x => x.UserChats)
                .FirstOrDefaultAsync(x => x.Id == chatId);

            if (chat == null) return;

            if (jwtService.ValidateAndReadJWT(token, out var decodedToken))
            {
                var userId = int.Parse(decodedToken.Claims.First(x => x.Type == "user").Value);

                if (chat.UserChats.Any(x => x.UserId == userId)) await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
            }
        }

        public async Task SendMessage(int chatId, string incomingCipherText, string outgoingCipherText, string token)
        {
            var chat = await dataContext.Chats
                .Include(x => x.UserChats)
                .Include(x => x.Messages)
                .FirstOrDefaultAsync(x => x.Id == chatId);

            if (chat == null) return;

            if (jwtService.ValidateAndReadJWT(token, out var decodedToken))
            {
                var userId = int.Parse(decodedToken.Claims.First(x => x.Type == "user").Value);
                var forUserId = chat.UserChats.FirstOrDefault(x => x.UserId != userId).UserId;

                if (chat.UserChats.Any(x => x.UserId == userId))
                {
                    chat.Messages.Add(new Message
                    {
                        FromUserId = userId,
                        MessageType = MessageType.UserMessage,
                        SendDate = DateTime.UtcNow,
                        MessageContents = new List<MessageContent>
                        {
                            new MessageContent
                            {
                                ForUserId = userId,
                                CipherText = incomingCipherText,
                            },
                            new MessageContent
                            {
                                ForUserId = forUserId,
                                CipherText = outgoingCipherText,
                            }
                        }
                    });

                    await Clients.Group(chatId.ToString()).SendAsync("new", forUserId, outgoingCipherText, userId);
                    await Clients.Group(chatId.ToString()).SendAsync("new", userId, incomingCipherText, userId);

                    await dataContext.SaveChangesAsync();
                }
            }
        }
    }
}
