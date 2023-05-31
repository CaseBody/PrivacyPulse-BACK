namespace PrivacyPulse_BACK.Entities
{
    public class Chat
    {
        public int Id { get; set; }
        public List<UserChat> UserChats { get; set; }
        public List<Message> Messages { get; set; }
    }
}
