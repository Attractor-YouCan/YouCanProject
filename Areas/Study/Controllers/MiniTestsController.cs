﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using YouCan.Areas.Study.ViewModels;
using YouCan.Entities;
using YouCan.Repository;
using YouCan.Service.Service;

namespace YouCan.Areas.Study.Controllers;

[Area("Study")]
[Authorize]
public class MiniTestsController : Controller
{
    private ICRUDService<Lesson> _lessonService;
    private ICRUDService<UserLessons> _userLessonService;
    private ICRUDService<Test> _testService;
    private ICRUDService<UserLevel> _userLevel;
    private UserManager<User> _userManager;

    public MiniTestsController(ICRUDService<Lesson> lessonService,
        ICRUDService<UserLessons> userLessonService,
        ICRUDService<Test> testService,ICRUDService<UserLevel> userLevel,
        UserManager<User> userManager)
    {
        _testService = testService;
        _lessonService = lessonService;
        _userLessonService = userLessonService;
        _userManager = userManager;
        _userLevel = userLevel;
    }

    public async Task<IActionResult> Index(int lessonId)
    {
        Test? test = _testService.GetAll()
            .FirstOrDefault(t => t.LessonId == lessonId);
        if (test == null)
            return NotFound("No Test!");
        return View(test);
    }

    [HttpPost]
    public async Task<IActionResult> Index([FromBody] List<TestAnswersModel> selectedAnswers)
    {
        User? currentUser = await _userManager.GetUserAsync(User);
        Test? test = await _testService.GetById(selectedAnswers[0].TestId);
        Lesson? lesson = await _lessonService.GetById((int)test.LessonId!);
        UserLevel? userLevels = _userLevel.GetAll()
            .FirstOrDefault(ul => ul.UserId == currentUser.Id && ul.SubjectId == lesson.SubjectId);

        UserLessons? userLesson = _userLessonService.GetAll()
            .FirstOrDefault(ul => ul.UserId == currentUser.Id && 
                ul.SubjectId == lesson.SubjectId && 
                ul.LessonId == lesson.Id);

        int passingCount = (
            from question in test.Questions
            from answer in question.Answers.Where(a => a.IsCorrect)
            join selectedAnswer in selectedAnswers on answer.Id equals selectedAnswer.AnswerId
            select selectedAnswer
        ).Count();
        if (passingCount >= 2)
        {
            userLevels.Level = lesson.LessonLevel;

            userLesson.IsPassed = true;

            await _userLevel.Update(userLevels);
            // await _userManager.UpdateAsync(currentUser);
            await _userLessonService.Update(userLesson);
        }
        var testResult = new
        {
            isPassed = passingCount>=2,
            lessonId = lesson.Id,
            subtopicId = lesson.SubjectId
        };
        return Ok(testResult);
    }

    [HttpGet]
    public IActionResult Result(bool isPassed, int lessonId, int subtopicId)
    {
        ViewBag.LessonId = lessonId;
        ViewBag.SubtopicId = subtopicId;
        return View(isPassed);
    }
}
