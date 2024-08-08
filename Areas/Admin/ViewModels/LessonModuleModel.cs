using System.Text.Json.Serialization;

namespace YouCan.Areas.Admin.ViewModels;

public class LessonModuleModel
{
    [JsonPropertyName("moduleTitle")]
    public string ModuleTitle { get; set; }
    [JsonPropertyName("moduleContent")]
    public string ModuleContent { get; set; }
    [JsonPropertyName("modulePhoto")]
    public IFormFile? ModulePhoto { get; set; }
    public string? ExistsPhotoUrl { get; set; }
}
