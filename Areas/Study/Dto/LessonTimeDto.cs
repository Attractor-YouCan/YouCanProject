using System.ComponentModel.DataAnnotations;

namespace YouCan.Areas.Study.Dto;

public class LessonTimeDto
{
    public int LessonId { get; set; }
    [Range(1000, int.MaxValue)]
    public int TimeSpent { get; set; }
}
