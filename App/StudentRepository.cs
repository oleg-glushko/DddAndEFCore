﻿namespace App;

public class StudentRepository
{
    private readonly SchoolContext _context;

    public StudentRepository(SchoolContext context)
    {
        _context = context;
    }

    public Student? GetById(long studentId)
    {
        Student? student = _context.Students.Find(studentId);

        if (student is null)
            return default;

        _context.Entry(student).Collection(x => x.Enrollments).Load();

        return student;
    }
}
