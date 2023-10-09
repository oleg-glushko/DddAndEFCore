namespace App;

public class Student
{
    public long Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public virtual Course FavoriteCourse { get; private set; } = null!;

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
