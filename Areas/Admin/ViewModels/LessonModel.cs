using System.Text.Json.Serialization;

namespace YouCan.Areas.Admin.ViewModels;

public class LessonModel
{
    [JsonPropertyName("lessonLevel")]
    public int LessonLevel { get; set; }
    [JsonPropertyName("lessonTitle")]
    public string LessonTitle { get; set; }
    [JsonPropertyName("lessonTitle2")]
    public string LessonTitle2 { get; set; }
    [JsonPropertyName("description")]
    public string Description { get; set; }
    [JsonPropertyName("lecture")]
    public string Lecture { get; set; }
    [JsonPropertyName("modules")]
    public List<LessonModuleModel>? Modules { get; set; }
}
