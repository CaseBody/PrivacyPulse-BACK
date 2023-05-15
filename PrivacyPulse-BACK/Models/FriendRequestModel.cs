namespace PrivacyPulse_BACK.Models
{
    public class FriendRequestModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public int MutualFriends { get; set; }
    }
}
