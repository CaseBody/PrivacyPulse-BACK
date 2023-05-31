using PrivacyPulse_BACK.Enums;

namespace PrivacyPulse_BACK.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public int? FromUserId { get; set; }
        public int ChatId { get; set; }
        public DateTime SendDate { get; set; }
        public MessageType MessageType { get; set; }

        public User? FromUser { get; set; }
        public Chat Chat { get; set; }
        public List<MessageContent> MessageContents { get; set; }
    }
}
