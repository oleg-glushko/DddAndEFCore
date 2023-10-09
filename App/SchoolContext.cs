using Microsoft.EntityFrameworkCore;
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

        optionsBuilder.UseSqlServer(_connectionString);

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
            x.Property(p => p.Email);
            x.Property(p => p.Name);
            x.Property(p => p.FavoriteCourseId);
        });

        modelBuilder.Entity<Course>(x =>
        {
            x.ToTable(nameof(Course)).HasKey(k => k.Id);
            x.Property(p => p.Id).HasColumnName(nameof(Course) + "ID");
            x.Property(p => p.Name);
        });
    }
}
