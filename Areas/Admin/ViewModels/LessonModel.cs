using System.Text.Json.Serialization;

namespace YouCan.Areas.Admin.ViewModels;

public class LessonModel
{
    public int? Id { get; set; }
    [JsonPropertyName("lessonLevel")]
    public int LessonLevel { get; set; }
    [JsonPropertyName("lessonTitle")]
    public string LessonTitle { get; set; }
    [JsonPropertyName("lessonTitle2")]
    public string LessonTitle2 { get; set; }
    [JsonPropertyName("description")]
    public string Description { get; set; }
    public IFormFile? LessonVideo { get; set; }
    [JsonPropertyName("lecture")]
    public string Lecture { get; set; }
    [JsonPropertyName("modules")]
    public List<LessonModuleModel>? Modules { get; set; }
    
    public string ExistsVideoUrl { get; set; }
}
