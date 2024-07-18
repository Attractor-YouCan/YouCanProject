namespace YouCan.Areas.Train.ViewModels;

public class TestResultViewModel
{
    public int CorrectAnswers { get; set; }
    public List<QuestionResultViewModel> Questions { get; set; } = new List<QuestionResultViewModel>();
}