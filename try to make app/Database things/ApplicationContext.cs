using System.Diagnostics;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace try_to_make_app.Database_things;

public class ApplicationContext : DbContext
{
    public DbSet<AppModel> Apps { get; set; } = null!;
    public DbSet<DayModel> Days { get; set; } = null!;
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=DataBase.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<DayModel>()
            .HasMany(d => d.AppModels)
            .WithMany(ap => ap.Days)
            .UsingEntity<AppDay>(
                j => j
                    .HasOne(ad => ad.AppM)
                    .WithMany(ap => ap.AppDays)
                    .HasForeignKey(ad => ad.AppId),
                j => j
                    .HasOne(ad => ad.Day)
                    .WithMany(d => d.AppDays)
                    .HasForeignKey(ad => ad.DayId),
                j =>
                {
                    j.Property(pt => pt.WorkTimeToDay).HasDefaultValue(0);
                    j.HasKey(t => new { t.DayId, t.AppId});
                    j.ToTable("AppDay");
                });
    }


}