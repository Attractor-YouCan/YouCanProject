namespace YouCan.Entities;

public class OrtTest : EntityBase
{
    public int? OrtLevel { get; set; }
    public int? TimeForTestInMin { get; set; } = 0;
    public string Language { get; set; }
    
    public List<Test> Tests { get; set; }
    public OrtTest()
    {
        Tests = new List<Test>();   
    }
}