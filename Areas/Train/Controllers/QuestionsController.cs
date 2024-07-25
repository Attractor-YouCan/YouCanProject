using Microsoft.AspNetCore.Mvc;
using YouCan.Areas.Train.Dto;

namespace YouCan.Areas.Train.Controllers;

[Area("Train")]
public class QuestionsController : Controller
{
    [HttpGet]
    public IActionResult Create(int subSubjectId)
    {
        var model = new QuestionDto()
        {
            Answers = new List<AnswerDto>(),
            SubjectId = subSubjectId
        };
        for(int i=0; i<5;i++)
        {
            model.Answers.Add(new AnswerDto());
        }
        return View(model);
    }
}
