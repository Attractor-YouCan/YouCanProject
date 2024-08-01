namespace YouCan.Areas.Train.ViewModels;

public class QuestionResultViewModel
{
    public int QuestionId { get; set; }
    public string QuestionContent { get; set; }
    public int SelectedAnswerId { get; set; }
    public int CorrectAnswerId { get; set; }
    public bool IsCorrect { get; set; }
    public List<AnswerViewModel> Answers { get; set; } = new List<AnswerViewModel>();
}