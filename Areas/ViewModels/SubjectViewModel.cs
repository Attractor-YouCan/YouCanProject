using System.ComponentModel.DataAnnotations.Schema;
using YouCan.Entities;

namespace YouCan.Areas.ViewModels;

public class SubjectViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? ImageUrl { get; set; }
    public SubjectType SubjectType { get; set; }
    public int? ParentId { get; set; } 
    public Subject? Parent { get; set; }  
    [NotMapped]
    public IFormFile? ImageFile { get; set; }
}