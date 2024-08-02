using System.Text.Json.Serialization;
using YouCan.Models;

namespace YouCan.ViewModels;

public class OrtTestResultModel
{
    [JsonPropertyName("rightsCount")]
    public int? RightsCount { get; set; }

    [JsonPropertyName("questionCount")]
    public int? QuestionCount { get; set; }

    [JsonPropertyName("testId")]
    public int? TestId { get; set; }

    [JsonPropertyName("spentTimeInMin")]
    public double? SpentTimeInMin { get; set; }
    [JsonPropertyName("testPoints")]
    public int? TestPoints { get; set; }
    [JsonPropertyName("pointsSum")]
    public int? PointsSum { get; set; }
}