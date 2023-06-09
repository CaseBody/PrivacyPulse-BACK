namespace PrivacyPulse_BACK.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Body { get; set; }
        public DateTime PostedAt { get; set; }
    }
}
