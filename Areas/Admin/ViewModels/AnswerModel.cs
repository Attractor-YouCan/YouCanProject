using System.Text.Json.Serialization;

namespace YouCan.Areas.Admin.ViewModels;

public class AnswerModel
{
    public int? AnswerId { get; set; }
    [JsonPropertyName("answerText")]
    public string? AnswerText { get; set; }
    [JsonPropertyName("answerImage")]
    public IFormFile? AnswerImage { get; set; }
    public bool IsCorrect { get; set; }
}