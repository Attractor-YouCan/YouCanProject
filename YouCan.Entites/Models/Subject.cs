
namespace YouCan.Entities;

public class Subject : EntityBase
{
    public string Name { get; set; }
    public string? ImageUrl { get; set; }
    public SubjectType SubjectType { get; set; }
    public int? ParentId { get; set; } 
    public Subject? Parent { get; set; }  
    public List<Subject>? SubSubjects { get; set; } 
    public List<Lesson>? Lessons { get; set; }
    public bool UserTestIsTest { get; set; } = false;
}