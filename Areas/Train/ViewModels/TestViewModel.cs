using YouCan.Models;

namespace YouCan.Areas.Train.ViewModels;

public class TestViewModel
{
    public PageViewModel PageViewModel { get; set; }
    public Question? CurrentQuestion { get; set; }
    public int SubtopicId { get; set; }
}