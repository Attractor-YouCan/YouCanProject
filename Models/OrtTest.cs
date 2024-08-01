namespace YouCan.Models;

public class OrtTest
{
    public int Id { get; set; }
    public List<Test> Tests { get; set; }
    public int? OrtLevel { get; set; }
    public int? TimeForTestInMin { get; set; }
    public string Language { get; set; }
    
}