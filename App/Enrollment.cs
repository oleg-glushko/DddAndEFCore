namespace App;

public class Enrollment : Entity
{
    public Grade Grade { get; }
    public virtual Course Course { get; } = null!;
    public virtual Student Student { get; } = null!;

    protected Enrollment()
    {
    }

    public Enrollment(Course course, Student student, Grade grade)
    {
        Course = course;
        Student = student;
        Grade = grade;
    }
}

public enum Grade
{
    A = 0,
    B = 1,
    C = 2,
    D = 3,
    F = 4
}
