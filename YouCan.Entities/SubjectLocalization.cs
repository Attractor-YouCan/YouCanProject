namespace YouCan.Entities;

public class SubjectLocalization
{
    public int Id { get; set; }
    public int SubjectId { get; set; }
    public Subject Subject { get; set; }
    public string Culture { get; set; }
    public string Title { get; set; }
    public string? Subtitle { get; set; }
    public string? Description { get; set; }
}
