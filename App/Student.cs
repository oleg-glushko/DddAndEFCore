namespace App;

public class Student : Entity
{
    public Name Name { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public virtual Course FavoriteCourse { get; private set; } = null!;
    private readonly List<Enrollment> _enrollments = new List<Enrollment>();
    public virtual IReadOnlyList<Enrollment> Enrollments => _enrollments.ToList();

    protected Student()
    {
    }

    public Student(Name name, Email email, Course favoriteCourse, Grade favoriteCourseGrade) : this()
    {
        Name = name;
        Email = email;
        FavoriteCourse = favoriteCourse;

        EnrollIn(favoriteCourse, favoriteCourseGrade);
    }

    public string EnrollIn(Course course, Grade grade)
    {
        if (_enrollments.Any(x => x.Course == course))
            return $"Already enrolled in course '{course.Name}'";

        var enrollment = new Enrollment(course, this, grade);
        _enrollments.Add(enrollment);
        
        return "OK";
    }

    public void Disenroll(Course course)
    {
        var enrollment = _enrollments.FirstOrDefault(x => x.Course == course);

        if (enrollment == null)
            return;

        _enrollments.Remove(enrollment);
    }

    public void EditPersonalInfo(Name name, Email email, Course favoriteCourse)
    {
        Name = name;
        Email = email;
        FavoriteCourse = favoriteCourse;
    }
}
