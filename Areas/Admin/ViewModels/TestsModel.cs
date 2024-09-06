using YouCan.Entities;

namespace YouCan.Areas.Admin.ViewModels;

public class TestsModel
{
    public int TestId { get; set; } 
    public List<QuestionModel> Questions { get; set; } 
}
