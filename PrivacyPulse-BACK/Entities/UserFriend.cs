namespace PrivacyPulse_BACK.Entities
{
    public class UserFriend
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FriendUserId { get; set; }
        public User User { get; set; }
        public User FriendUser { get; set; }
    }
}
