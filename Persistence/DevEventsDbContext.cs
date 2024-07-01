using Microsoft.EntityFrameworkCore;
using PortifolioTeste1.Models;

namespace PortifolioTeste1.Persistence
{
    public class DevEventsDbContext : DbContext
    {
        public DevEventsDbContext(DbContextOptions<DevEventsDbContext> options) : base(options)
        {
            //  Esse contrutor será utilizado pelo EF Core quando formos configurar nossa aplicação na classe Program.cs
        }

        public DbSet<DevEvent> DevEvents { get; set; }
        public DbSet<DevEventSpeaker> DevEventSpeakers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<DevEvent>(e =>
            {
                e.HasKey(e => e.Id);

                e.Property(de => de.Title).IsRequired(false);

                e.Property(de => de.Description)
                    .HasMaxLength(200)
                    .HasColumnType("varchar(200)");

                e.Property(de => de.StartDate)
                    .HasColumnName("Start_Date");

                e.Property(de => de.EndDate)
                    .HasColumnName("End_Date");

                e.HasMany(de => de.Speakers)
                    .WithOne()
                    .HasForeignKey(de => de.DevEventId);

            });

            builder.Entity<DevEvent>(e =>
            {

            });

        }
    }
}
