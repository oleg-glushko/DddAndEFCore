using App;
using Microsoft.Extensions.Configuration;

string result2 = Execute(x => x.AddEnrollment(1, 2, Grade.A));
string result = Execute(x => x.CheckStudentFavoriteCourse(1, 2));

; // no-op for a breakpoint

static string Execute(Func<StudentController, string> func)
{
    string connectionString = GetConnectionString();

    using (var context = new SchoolContext(connectionString, true))
    {
        var controller = new StudentController(context);
        return func(controller);
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