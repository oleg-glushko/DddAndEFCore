using App;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

string connectionString = GetConnectionString();
ILoggerFactory loggerFactory = CreateLoggerFactory();

var optionsBuilder = new DbContextOptionsBuilder<SchoolContext>();
optionsBuilder
    .UseSqlServer(connectionString)
    .UseLoggerFactory(loggerFactory)
    .EnableSensitiveDataLogging();

using (var context = new SchoolContext(optionsBuilder.Options))
{
    Student? student = context.Students.Find(1L);

    if (student != null)
    {
        student.Name += "2";
        student.Email += "2";
        context.SaveChanges();
    }
}

static ILoggerFactory CreateLoggerFactory() => LoggerFactory.Create(builder =>
    builder
       .AddFilter((category, level) =>
            category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information)
        .AddConsole());

static string GetConnectionString()
{
    IConfigurationRoot configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();

    return configuration["ConnectionString"] ??
        throw new NullReferenceException();
}