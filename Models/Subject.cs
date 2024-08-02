using System.ComponentModel.DataAnnotations.Schema;
using YouCan.Services;

namespace YouCan.Models;

public class Subject
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? ImageUrl { get; set; }
    public SubjectType? SubjectType { get; set; }
    public int? ParentId { get; set; } 
    public Subject? Parent { get; set; }  
    [NotMapped]
    public IFormFile? ImageFile { get; set; }
    public List<Subject>? SubSubjects { get; set; } 
    public List<Lesson>? Lessons { get; set; }

    
}