﻿namespace App;

public class StudentController
{
    private SchoolContext _context;

    public StudentController(SchoolContext context)
    {
        _context = context;
    }

    public string CheckStudentFavoriteCourse(long studentId, long courseId)
    {
        Student? student = _context.Students.Find(studentId);
        if (student is null)
            return "Student not found";

        Course? course = Course.FromId(courseId);
        if (course is null)
            return "Course not found";

        return student.FavoriteCourse == course ? "Yes" : "No";
    }

    public string AddEnrollment(long studentId, long courseId, Grade grade)
    {
        Student? student = _context.Students.Find(studentId);
        if (student is null)
            return "Student not found";

        Course? course = Course.FromId(courseId);
        if (course is null)
            return "Course not found";

        student.Enrollments.Add(new Enrollment(course, student, grade));

        _context.SaveChanges();

        return "OK";
    }
}
