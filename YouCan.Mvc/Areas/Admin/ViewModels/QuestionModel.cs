using System.Text.Json.Serialization;

namespace YouCan.Mvc.Areas.Admin.ViewModels;

public class QuestionModel
{
    public int? QuestionId { get; set; }
    [JsonPropertyName("instruction")]
    public string Instruction { get; set; }
    [JsonPropertyName("text")]
    public string? Text { get; set; }
    [JsonPropertyName("image")]
    public IFormFile? Image { get; set; }
    [JsonPropertyName("questionExistsPhotoUrlElement")]
    public string? QuestionExistsPhotoUrlElement { get; set; }
    public int? Point { get; set; }
    public List<AnswerModel> Answers { get; set; }

    public QuestionModel()
    {
        Answers = new List<AnswerModel>();
    }

}