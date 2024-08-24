using System.Text.Json.Serialization;

namespace YouCan.Mvc.Areas.Admin.ViewModels;

public class AnswerModel
{
    [JsonPropertyName("answerText")]
    public string? AnswerText { get; set; }
    [JsonPropertyName("answerImage")]
    public IFormFile? AnswerImage { get; set; }
    public bool IsCorrect { get; set; }
}