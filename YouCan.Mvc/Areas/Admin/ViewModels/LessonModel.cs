using System.Text.Json.Serialization;

namespace YouCan.Mvc.Areas.Admin.ViewModels;

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
    public int SubjectId { get; set; }
    public string? LessonVideo { get; set; }
    [JsonPropertyName("lecture")]
    public string Lecture { get; set; }
    [JsonPropertyName("modules")]
    public List<LessonModuleModel>? Modules { get; set; }
    public List<QuestionModel>? Questions { get; set; }
    
    public string ExistsVideoUrl { get; set; }
}
