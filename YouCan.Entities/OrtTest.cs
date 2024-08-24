namespace YouCan.Entities;

public class OrtTest : EntityBase
{
    public List<Test> Tests { get; set; }
    public int? OrtLevel { get; set; }
    public int? TimeForTestInMin { get; set; }
    public string Language { get; set; }

}