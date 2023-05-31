namespace PrivacyPulse_BACK.Entities
{
    public class MessageContent
    {
        public int Id { get; set; }
        public int MessageId { get; set; }
        public string CipherText { get; set; }
        public int ForUserId { get; set; }

        public User ForUser { get; set; }
        public Message Message { get; set; }
    }
}
