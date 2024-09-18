using YouCan.Entities;

namespace YouCan.ViewModels;

public class CombinedRatingViewModel
{
    public IEnumerable<League> Leagues { get; set; }
    public IEnumerable<User> Users { get; set; }

}