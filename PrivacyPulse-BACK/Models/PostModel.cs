namespace PrivacyPulse_BACK.Models
{
    public class PostModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Body { get; set; }
        public string Image { get; set; }
        public bool IsLiked { get; set; }
        public DateTime PostedAt { get; set; }
    }
}
