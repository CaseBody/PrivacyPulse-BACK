namespace PrivacyPulse_BACK.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public string Body { get; set; }

        public User User { get; set; }
        public Post Post { get; set; }
    }
}
