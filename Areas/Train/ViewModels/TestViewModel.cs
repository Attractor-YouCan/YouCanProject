using YouCan.Models;

namespace YouCan.Areas.Train.ViewModels;

public class TestViewModel
{
    public PageViewModel PageViewModel { get; set; }
    public Question? CurrentQuestion { get; set; }
    public int TestId { get; set; }
    public string? SubjectName { get; set; }
}