using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace App;

public class SchoolContext : DbContext
{
    private static readonly Type[] EnumerationTypes = { typeof(Course), typeof(Suffix) };

    private readonly string _connectionString;
    private readonly bool _useConsoleLogger;
    private readonly EventDispatcher _eventDispatcher;

    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }

    public SchoolContext(string connectionString, bool useConsoleLogger, EventDispatcher eventDispatcher)
    {
        _connectionString = connectionString;
        _useConsoleLogger = useConsoleLogger;
        _eventDispatcher = eventDispatcher;
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
            x.OwnsOne(p => p.Name, p =>
            {
                p.Property<long?>("NameSuffixID").HasColumnName("NameSuffixID");
                p.Property(pp => pp.First).HasColumnName(nameof(Name.First) + "Name");
                p.Property(pp => pp.Last).HasColumnName(nameof(Name.Last) + "Name");
                p.HasOne(pp => pp.Suffix).WithMany().HasForeignKey("NameSuffixID").IsRequired(false);
            });
            x.HasOne(p => p.FavoriteCourse).WithMany();
            x.HasMany(p => p.Enrollments).WithOne(p => p.Student)
                // it isn't required as the Enrollments property isn't nullable
                // .OnDelete(DeleteBehavior.Cascade)
                .Metadata.PrincipalToDependent?.SetPropertyAccessMode(PropertyAccessMode.Field);
        });

        modelBuilder.Entity<Suffix>(x =>
        {
            x.ToTable(nameof(Suffix)).HasKey(k => k.Id);
            x.Property(p => p.Id).HasColumnName(nameof(Suffix) + "ID");
            x.Property(p => p.Name);
        });

        modelBuilder.Entity<Course>(x =>
        {
            x.ToTable(nameof(Course)).HasKey(k => k.Id);
            x.Property(p => p.Id).HasColumnName(nameof(Course) + "ID");
            x.Property(p => p.Name);
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

    public override int SaveChanges()
    {
        IEnumerable<EntityEntry> enumerationEntries = ChangeTracker.Entries()
            .Where(x => EnumerationTypes.Contains(x.Entity.GetType()));

        foreach (EntityEntry enumerationEntry in enumerationEntries)
            enumerationEntry.State = EntityState.Unchanged;

        List<Entity> entities = ChangeTracker.Entries()
            .Where(x => x.Entity is Entity)
            .Select(x => (Entity)x.Entity)
            .ToList();

        int result = base.SaveChanges();

        foreach (Entity entity in entities)
        {
            _eventDispatcher.Dispatch(entity.DomainEvents);
            entity.ClearDomainEvents();
        }

        return result;
    }
}
