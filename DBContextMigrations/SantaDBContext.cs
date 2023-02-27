using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using RequestsForSanta;

namespace DBContextMigrations
{
    public class SantaDBContext : DbContext
    {
        public DbSet<Gift> Gifts { get; set; }
        public DbSet<ChildGiftRequest> GiftRequests { get; set; }

        public DbSet<GiftOfChild> GiftsOfChild { get; set; }

        public IConfiguration Configuration { get; }
        public SantaDBContext(IConfiguration config)
        {
            Configuration = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            }

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GiftOfChild>()
                .HasOne(c => c.request)
                .WithMany(x=>x.childGifts)
                .HasForeignKey(c=>c.requestID);

            modelBuilder.Entity<ChildGiftRequest>()
            .HasCheckConstraint("CK_ChildGiftRequest_TotalPrice", "[TotalPrice] <= 50");
        }

      
    }

}
