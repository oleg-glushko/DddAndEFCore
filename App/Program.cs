using App;
using Microsoft.Extensions.Configuration;

string connectionString = GetConnectionString();

using (var context = new SchoolContext(connectionString, true))
{
    Student? student = context.Students.Find(1L);

    if (student != null)
    {
        student.Name += "2";
        student.Email += "2";
        context.SaveChanges();
    }
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