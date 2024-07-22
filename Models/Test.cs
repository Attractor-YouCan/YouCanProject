using System.ComponentModel.DataAnnotations.Schema;

namespace YouCan.Models;

public class Test
{
    public int Id { get; set; }
    public int GainingExperience { get; set; }
    public TimeSpan? TimeForTest { get; set; }
    [NotMapped]
    public int? MinutesForTest { get; set; }

    public int? SubtopicId { get; set; }
    public Subtopic? Subtopic { get; set; }
    //Альтернатива сабтопику
    public int? SubjectId { get; set; }
    public Subject? Subject { get; set; }
    public int? LessonId { get; set; }
    public Lesson? Lesson { get; set; }
    
    public int? OrtTestId { get; set; }
    public OrtTest? OrtTest { get; set; }
    
    public int? CreatorUserId { get; set; }
    public User? CreatorUser { get; set; }

    public List<Question> Questions { get; set; }
    public Test()
    {
        Questions = new List<Question>();
    }
}
