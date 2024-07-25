namespace YouCan.Areas.Train.Dto;

public class QuestionDto
{
    public int SubjectId { get; set; }
    public string? Text { get; set; }
    public IFormFile? Image { get; set; }
    public List<AnswerDto> Answers { get; set; }
    public int CorrectAnswerId { get; set; }
    public bool AnswerIsImage { get; set; } = false;
}
