﻿namespace YouCan.Models;

public class UserLessons
{
    public int Id { get; set; }
    public bool IsPassed { get; set; }

    public int UserId { get; set; }
    public User? User { get; set; }

    public int LessonId { get; set; }
    public Lesson? Lesson { get; set; }
}
