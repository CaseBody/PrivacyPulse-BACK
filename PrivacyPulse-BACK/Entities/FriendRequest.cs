namespace PrivacyPulse_BACK.Entities
{
    public class FriendRequest
    {
        public int Id { get; set; }
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public User FromUser { get; set; }
        public User ToUser { get; set; }
    }
}
