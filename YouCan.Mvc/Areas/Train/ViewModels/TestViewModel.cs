using YouCan.Entities;

namespace YouCan.Mvc.Areas.Train.ViewModels;

public class TestViewModel
{
    public PageViewModel PageViewModel { get; set; }
    public Question? CurrentQuestion { get; set; }
    public int SubjectId { get; set; }
    public string? SubjectName { get; set; }
}