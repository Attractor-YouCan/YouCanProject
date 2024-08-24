namespace YouCan.Entities;

public class Question : EntityBase
{
    public string? Instruction { get; set; }
    public string? Type { get; set; }
    public string? Content { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsPublished { get; set; } = false;
    public int? TestId { get; set; }
    public Test? Test { get; set; }
    public int? Point { get; set; }
    public int? UserId { get; set; }
    public User? User { get; set; }
    public bool AnswersIsImage { get; set; }
    public int? SubjectId { get; set; }
    public Subject Subject { get; set; }
    public List<Answer> Answers { get; set; }
    public Question()
    {
        Answers = new List<Answer>();
    }
}
