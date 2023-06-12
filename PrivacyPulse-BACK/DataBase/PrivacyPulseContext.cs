using PrivacyPulse_BACK.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace CampingAPI.DataBase
{
    public class PrivacyPulseContext : DbContext
    {
        public PrivacyPulseContext(DbContextOptions<PrivacyPulseContext> context) : base(context)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(x => x.Friends)
                .WithOne(x => x.User);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserFriend> UserFriends { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<UserChat> UserChats { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<MessageContent> MessageContents { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Like> Like { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}
