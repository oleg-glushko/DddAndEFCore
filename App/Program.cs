using App;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

string connectionString = GetConnectionString();

using (var context = new SchoolContext(connectionString, true))
{
    Student? student = context.Students
        .Include(x => x.FavoriteCourse)
        .SingleOrDefault(x => x.Id == 1);
}


static string GetConnectionString()
{
    IConfigurationRoot configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();

    return configuration["ConnectionString"] ??
        throw new NullReferenceException();
}