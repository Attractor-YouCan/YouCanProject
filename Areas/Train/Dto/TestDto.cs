namespace YouCan.Areas.Train.Dto;

public class TestDto
{
    public class TestQuestionDto
    {
        public string Text { get; set; }
        public List<string> Answers { get; set; }
        public int CorrectAnswerId { get; set; }
    }
    public int SubjectId { get; set; }
    public string Text { get; set; }
    public List<TestQuestionDto> Questions { get; set; }
}
