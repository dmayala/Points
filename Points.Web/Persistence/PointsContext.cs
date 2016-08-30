using Microsoft.EntityFrameworkCore;
using Points.Shared.Models;

namespace Points.Web.Persistence
{
    public class PointsContext : DbContext
    {
        public DbSet<Card> Cards { get; set; }
        public DbSet<Valuation> Valuations { get; set; }

        public PointsContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Card>().Ignore(t => t.Image);
        }
    }
}