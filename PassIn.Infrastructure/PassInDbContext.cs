using Microsoft.EntityFrameworkCore;
using PassIn.Infrastructure.Entities;

namespace PassIn.Infrastructure;

public class PassInDbContext : DbContext
{
    public DbSet<Event> Events => Set<Event>();
    public DbSet<CheckIn> CheckIns => Set<CheckIn>();
    public DbSet<Attendee> Attendees => Set<Attendee>();

    public PassInDbContext(DbContextOptions options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region Event
        modelBuilder.Entity<Event>().ToTable("Events");
        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Title);
            entity.HasIndex(e => e.Slug).IsUnique();
        });
        #endregion

        #region CheckIn
        modelBuilder.Entity<CheckIn>().ToTable("CheckIns");
        modelBuilder.Entity<CheckIn>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.AttendeeId });
        });
        #endregion

        #region Attendee
        modelBuilder.Entity<Attendee>().ToTable("Attendees");
        modelBuilder.Entity<Attendee>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasIndex(e => new { e.Name, e.EventId });
        });
        #endregion
    }
}
