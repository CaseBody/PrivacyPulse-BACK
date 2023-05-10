using PrivacyPulse_BACK.Entities;
using Microsoft.EntityFrameworkCore;

namespace CampingAPI.DataBase
{
    public class PrivacyPulseContext : DbContext
    {
        public PrivacyPulseContext(DbContextOptions<PrivacyPulseContext> context) : base(context)
        {

        }

        public DbSet<User> Users { get; set; }
    }
}
