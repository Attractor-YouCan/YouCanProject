using YouCan.Entites.Models;

namespace YouCan.Areas.Admin.ViewModels;

public class StatViewModel
{
    public List<GroupQueryType> Tests { get; set; }
    public List<GroupQueryType> Users { get; set; }

    public int UsersCount { get; set; }

    public Tariff Start { get; set; }
    public Tariff Pro { get; set; }
    public Tariff Premium { get; set; }

    public double AllNewUsers { get; set; }
    public double NewStartUsers { get; set; }
    public double NewProUsers { get; set; }
    public double NewPremiumUsers { get; set; }
}
