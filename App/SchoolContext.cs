using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;

namespace App;

public class SchoolContext : DbContext
{
    private readonly string _connectionString;
    private readonly bool _useConsoleLogger;

    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }

    public SchoolContext(string connectionString, bool useConsoleLogger)
    {
        _connectionString = connectionString;
        _useConsoleLogger = useConsoleLogger;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
            builder
                .AddFilter((category, level) =>
                    category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information)
                .AddConsole());

        optionsBuilder
            .UseSqlServer(_connectionString)
            .UseLazyLoadingProxies();

        if (_useConsoleLogger)
            optionsBuilder
                .UseLoggerFactory(loggerFactory)
                .EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>(x =>
        {
            x.ToTable(nameof(Student)).HasKey(k => k.Id);
            x.Property(p => p.Id).HasColumnName(nameof(Student) + "ID");
            x.Property(p => p.Email)
                .HasConversion(p => p.Value, p => Email.Create(p).Value);
            x.Property(p => p.Name);
            x.HasOne(p => p.FavoriteCourse).WithMany();
            x.HasMany(p => p.Enrollments).WithOne(p => p.Student)
                // it isn't required as the Enrollments property isn't nullable
                // .OnDelete(DeleteBehavior.Cascade)
                .Metadata.PrincipalToDependent?.SetPropertyAccessMode(PropertyAccessMode.Field);
        });

        modelBuilder.Entity<Course>(x =>
        {
            x.ToTable(nameof(Course)).HasKey(k => k.Id);
            x.Property(p => p.Id).HasColumnName(nameof(Course) + "ID");
            x.Property(p => p.Name)
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
        });

        modelBuilder.Entity<Enrollment>(x =>
        {
            x.ToTable(nameof(Enrollment)).HasKey(k => k.Id);
            x.Property(p => p.Id).HasColumnName(nameof(Enrollment) + "ID");
            x.HasOne(p => p.Student).WithMany(p => p.Enrollments);
            x.HasOne(p => p.Course).WithMany();
            x.Property(p => p.Grade);
        });
    }

    //public override int SaveChanges()
    //{
    //    foreach (EntityEntry<Course> course in ChangeTracker.Entries<Course>())
    //        course.State = EntityState.Unchanged;

    //    return base.SaveChanges();
    //}
}
