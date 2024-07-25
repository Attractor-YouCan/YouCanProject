namespace YouCan.Areas.Train.Dto;

public class TestDto
{
    public class TestQuestionDto
    {
        public string Content { get; set; }
        public List<string> Answers { get; set; }
    }
    public int SubjectId { get; set; }
    public string Content { get; set; }
    public List<TestQuestionDto> Questions { get; set; }
}
