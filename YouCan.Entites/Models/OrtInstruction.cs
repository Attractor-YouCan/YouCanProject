namespace YouCan.Models;

public class OrtInstruction
{
    public int Id { get; set; }
    public int? TestId { get; set; }
    public Test? Test { get; set; }
    public string? Description { get; set; }
    public int? QuestionsCount { get; set; }
    public int? TimeInMin { get; set; }
    
}