﻿namespace YouCan.Models;

public class Lesson
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? SubTitle { get; set; }
    public string? VideoUrl { get; set; }
    public string? Description { get; set; }
    public string? Lecture { get; set; }
    public List<LessonModule>? LessonModules { get; set; }
    
    public int RequiredLevel { get; set; }

    public int SubtopicId { get; set; }
    public Subtopic? Subtopic { get; set; }

    public List<Test> Tests { get; set; }
    public Lesson()
    {
        Tests = new List<Test>();
        LessonModules = new List<LessonModule>();
    }
}
