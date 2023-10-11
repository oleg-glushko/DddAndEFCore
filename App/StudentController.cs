using CSharpFunctionalExtensions;

namespace App;

public class StudentController
{
    private readonly SchoolContext _context;
    private readonly StudentRepository _repository;

    public StudentController(SchoolContext context)
    {
        _context = context;
        _repository = new StudentRepository(context);
    }

    public string CheckStudentFavoriteCourse(long studentId, long courseId)
    {
        Student? student = _repository.GetById(studentId);
        if (student is null)
            return "Student not found";

        Course? course = Course.FromId(courseId);
        if (course is null)
            return "Course not found";

        return student.FavoriteCourse == course ? "Yes" : "No";
    }

    public string EnrollStudent(long studentId, long courseId, Grade grade)
    {
        Student? student = _repository.GetById(studentId);
        if (student is null)
            return "Student not found";

        Course? course = Course.FromId(courseId);
        if (course is null)
            return "Course not found";

        string result = student.EnrollIn(course, grade);

        _context.SaveChanges();

        return result;
    }

    public string DisenrollStudent(long studentId, long courseId)
    {
        Student? student = _repository.GetById(studentId);
        if (student is null)
            return "Student not found";

        Course? course = Course.FromId(courseId);
        if (course is null)
            return "Course not found";

        student.Disenroll(course);

        _context.SaveChanges();

        return "OK";
    }

    public string RegisterStudent(string firstName, string lastName, string email,
        long favoriteCourseId, Grade favoriteCourseGrade)
    {

        Course? favoriteCourse = Course.FromId(favoriteCourseId);
        if (favoriteCourse is null)
            return "Course not found";

        Result<Email> emailResult = Email.Create(email);
        if (emailResult.IsFailure)
            return emailResult.Error;

        Result<Name> nameResult = Name.Create(firstName, lastName);
        if (nameResult.IsFailure)
            return nameResult.Error;

        var student = new Student(nameResult.Value, emailResult.Value, favoriteCourse, favoriteCourseGrade);
        _repository.Save(student);
        
        _context.SaveChanges();

        return "OK";
    }

    public string EditPersonalInfo(long studentId, string firstName, string lastName, string email, long favoriteCourseId)
    {
        Student? student = _repository.GetById(studentId);
        if (student is null)
            return "Student not found";

        Course? favoriteCourse = Course.FromId(favoriteCourseId);
        if (favoriteCourse is null)
            return "Course not found";

        Result<Email> emailResult = Email.Create(email);
        if (emailResult.IsFailure)
            return emailResult.Error;

        Result<Name> nameResult = Name.Create(firstName, lastName);
        if (nameResult.IsFailure)
            return nameResult.Error;

        // The next code works well for a ComplexProperty without the resort to .Copy()
        //Student? bob = _repository.GetById(2L);
        //student.Name = bob?.Name ?? student.Name;
        //student.Email = bob?.Email ?? student.Email;

        student.EditPersonalInfo(nameResult.Value, emailResult.Value, favoriteCourse);

        _context.SaveChanges();

        return "OK";
    }
}
