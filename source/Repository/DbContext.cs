using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SocialWorkApi.Domain.Entities.Agencies;
using SocialWorkApi.Domain.Entities.PopulationTypes;
using SocialWorkApi.Domain.Entities.Services;
using SocialWorkApi.Domain.Entities.ServiceSubTypes;
using SocialWorkApi.Domain.Entities.ServiceTypes;
using SocialWorkApi.Domain.Entities.Users;

namespace SocialWorkApi.Services.Database;

public class ApplicationContext(DbContextOptions<ApplicationContext> options, IOptions<DbOptions> connectionOptions) : DbContext(options)
{
    private readonly DbOptions _dbConfig = connectionOptions.Value;
    public DbSet<User> Users { get; set; }
    public DbSet<PopulationType> PopulationTypes { get; set; }
    public DbSet<ServiceType> ServiceTypes { get; set; }
    public DbSet<ServiceSubType> ServiceSubTypes { get; set; }
    public DbSet<Agency> Agencies { get; set; }
    public DbSet<Service> Services { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseMySql(_dbConfig.ConnectionString, ServerVersion.AutoDetect(_dbConfig.ConnectionString))
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(builder =>
        {
            builder.Property(u => u.CreatedById).HasColumnName("CreatedBy");
            builder.Property(u => u.LastUpdatedById).HasColumnName("LastUpdatedBy");

            // .HasOne Creates the Navigation and the HasForeignKey is the connector. .WithMany() is just describing the relationship more. 
            // If it had something like .HasOne it would be a navigation thing
            builder.HasOne(u => u.LastUpdatedBy)
            .WithMany()
            .HasForeignKey(u => u.LastUpdatedById);

            builder.HasOne(u => u.CreatedBy)
            .WithMany()
            .HasForeignKey(u => u.CreatedById);
        });

        modelBuilder.Entity<PopulationType>(builder =>
        {
            builder.Property(p => p.CreatedById).HasColumnName("CreatedBy");
            builder.Property(p => p.LastUpdatedById).HasColumnName("LastUpdatedBy");

            builder.HasOne(p => p.LastUpdatedBy)
            .WithMany()
            .HasForeignKey(p => p.LastUpdatedById);

            builder.HasOne(p => p.CreatedBy)
            .WithMany()
            .HasForeignKey(p => p.CreatedById);

            builder.HasMany(p => p.Services)
            .WithOne(s => s.PopulationType)
            .HasForeignKey(s => s.PopulationTypeId);
        });

        modelBuilder.Entity<ServiceType>(builder =>
        {
            builder.Property(s => s.CreatedById).HasColumnName("CreatedBy");
            builder.Property(s => s.LastUpdatedById).HasColumnName("LastUpdatedBy");

            builder.HasOne(s => s.LastUpdatedBy)
            .WithMany()
            .HasForeignKey(s => s.LastUpdatedById);

            builder.HasOne(s => s.CreatedBy)
            .WithMany()
            .HasForeignKey(s => s.CreatedById);

            builder.HasMany(s => s.ServiceSubTypes)
            .WithOne()
            .HasForeignKey(s => s.ServiceTypeId);

            builder.HasMany(s => s.Services)
            .WithOne(ser => ser.ServiceType)
            .HasForeignKey(ser => ser.ServiceTypeId);
        });

        modelBuilder.Entity<ServiceSubType>(builder =>
        {
            builder.Property(s => s.CreatedById).HasColumnName("CreatedBy");
            builder.Property(s => s.LastUpdatedById).HasColumnName("LastUpdatedBy");

            builder.HasOne(s => s.LastUpdatedBy)
            .WithMany()
            .HasForeignKey(s => s.LastUpdatedById);

            builder.HasOne(s => s.CreatedBy)
            .WithMany()
            .HasForeignKey(s => s.CreatedById);

            builder.HasMany(s => s.Services)
            .WithOne(ser => ser.ServiceSubType)
            .HasForeignKey(ser => ser.ServiceSubTypeId);
        });

        modelBuilder.Entity<Agency>(builder =>
        {
            builder.Property(a => a.CreatedById).HasColumnName("CreatedBy");
            builder.Property(a => a.LastUpdatedById).HasColumnName("LastUpdatedBy");

            builder.HasOne(a => a.LastUpdatedBy)
            .WithMany()
            .HasForeignKey(a => a.LastUpdatedById);

            builder.HasOne(a => a.CreatedBy)
            .WithMany()
            .HasForeignKey(a => a.CreatedById);

            builder.HasMany(a => a.Services)
            .WithOne()
            .HasForeignKey(s => s.AgencyId);
        });

        modelBuilder.Entity<Service>(builder =>
        {
            builder.Property(s => s.CreatedById).HasColumnName("CreatedBy");
            builder.Property(s => s.LastUpdatedById).HasColumnName("LastUpdatedBy");

            builder.HasOne(s => s.LastUpdatedBy)
            .WithMany()
            .HasForeignKey(s => s.LastUpdatedById);

            builder.HasOne(s => s.CreatedBy)
            .WithMany()
            .HasForeignKey(s => s.CreatedById);

            builder.HasOne(s => s.Agency)
            .WithMany(a => a.Services)
            .HasForeignKey(s => s.AgencyId);

            builder.HasOne(s => s.ServiceType)
            .WithMany(st => st.Services)
            .HasForeignKey(s => s.ServiceTypeId);

            builder.HasOne(s => s.ServiceSubType)
            .WithMany(sst => sst.Services)
            .HasForeignKey(s => s.ServiceSubTypeId);

            builder.HasOne(s => s.PopulationType)
            .WithMany(pt => pt.Services)
            .HasForeignKey(s => s.PopulationTypeId);
        });
    }
}