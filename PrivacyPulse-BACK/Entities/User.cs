
namespace PrivacyPulse_BACK.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Biography { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string PublicKey { get; set; }
        public string EncryptedPrivateKey { get; set; }

        public List<UserFriend> Friends { get; set; }
    }
}
