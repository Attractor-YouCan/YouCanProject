using System.Text.Json.Serialization;

namespace YouCan.Areas.Admin.ViewModels;

public class QuestionModel
{
    [JsonPropertyName("instruction")]
    public string Instruction { get; set; }
    [JsonPropertyName("text")]
    public string? Text { get; set; }
    [JsonPropertyName("image")]
    public IFormFile? Image { get; set; }
    public List<AnswerModel> Answers { get; set; }
   
}