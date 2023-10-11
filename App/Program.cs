using App;
using Microsoft.Extensions.Configuration;

string result5 = Execute(x => x.EditPersonalInfo(4, "Carl 3", "Carlson 3", 2, "carl2@gmail.com", 1));
//string result4 = Execute(x => x.RegisterStudent("Carl", "carl@gmail.com", 2, Grade.B));
//string result3 = Execute(x => x.DisenrollStudent(1, 2));
//string result = Execute(x => x.CheckStudentFavoriteCourse(1, 2));
//string result2 = Execute(x => x.EnrollStudent(1, 2, Grade.A));

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