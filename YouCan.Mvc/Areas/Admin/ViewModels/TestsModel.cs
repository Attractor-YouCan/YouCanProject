namespace YouCan.Mvc.Areas.Admin.ViewModels;

public class TestsModel
{
    public int TestId { get; set; } 
    public int? SubjectId { get; set; }
    public string? Text { get; set; }
    public int? TimeForTestInMin { get; set; }
    public int? QuestionsCount { get; set; }
    public List<QuestionModel> Questions { get; set; } = new();
}
