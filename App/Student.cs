namespace App;

public class Student : Entity
{
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public virtual Course FavoriteCourse { get; private set; } = null!;
    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    protected Student()
    {
    }

    public Student(string name, string email, Course favoriteCourse) : this()
    {
        Name = name;
        Email = email;
        FavoriteCourse = favoriteCourse;
    }
}
