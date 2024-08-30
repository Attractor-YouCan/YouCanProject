using System.ComponentModel.DataAnnotations.Schema;
namespace YouCan.Entities;

public class Test : EntityBase
{
    public int GainingExperience { get; set; }
    public int? TimeForTestInMin { get; set; }
    
    public string? Text { get; set; }
    public bool IsPublished { get; set; }

    //Альтернатива сабтопику
    public int? SubjectId { get; set; }
    public Subject? Subject { get; set; }

    public int? LessonId { get; set; }
    public Lesson? Lesson { get; set; }

    public int? OrtTestId { get; set; }
    public OrtTest? OrtTest { get; set; }

    public int? OrtInstructionId { get; set; }
    public OrtInstruction? OrtInstruction { get; set; }

    public List<Question> Questions { get; set; }

    public int? UserId { get; set; }
    public User? User { get; set; }

    public Test()
    {
        Questions = new List<Question>();
    }
}
