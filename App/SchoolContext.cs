using Microsoft.EntityFrameworkCore;

namespace App;

public class SchoolContext : DbContext
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }

    public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)
    {
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
