using YouCan.Areas.Study.ViewModels;
using YouCan.ViewModels.Account;

namespace YouCan.ViewModels;

public class TestSubmissionModel
{
    public List<TestAnswersModel> SelectedAnswers { get; set; }
    public List<OrtTestModel> TimeSpent { get; set; }
    public int? OrtTestId { get; set; }
}