﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YouCan.Areas.Study.Dto;
using YouCan.Entites.Models;
using YouCan.Entities;
using YouCan.Service.Service;

namespace YouCan.Areas.Study.Controllers;

[Area("Study")]
[Authorize]
public class LessonsController : Controller
{
    private ICRUDService<Lesson> _lessonService;
    private ICRUDService<UserLessons> _userLessonService;
    private UserManager<User> _userManager;
    private ICRUDService<LessonTime> _lessonTimeService;
    public LessonsController(ICRUDService<Lesson> lessonService,
        ICRUDService<UserLessons> userLessonService,
        ICRUDService<LessonTime> lessonTimeService,
        UserManager<User> userManager)
    {
        _lessonService = lessonService;
        _userLessonService = userLessonService;
        _lessonTimeService = lessonTimeService;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index(int subTopicId)
    {
        List<Lesson>? lessons = _lessonService.GetAll().Where(l => l.SubjectId == subTopicId).ToList();
        if (lessons == null)
            return NotFound("NO lessons in this topic !");
        User? currentUser = await _userManager.GetUserAsync(User);
        if (currentUser == null)
            return NotFound("No user");

        UserLessons? userLessons =  _userLessonService.GetAll()
            .FirstOrDefault(ul => ul.UserId == currentUser.Id
                                       && ul.SubjectId == subTopicId);
        if (userLessons == null)
        {
            userLessons = new UserLessons() { UserId = currentUser.Id, SubjectId = subTopicId, PassedLevel = 0, IsPassed = true, SubtopicId = null };
            await _userLessonService.Insert(userLessons);
        }
        ViewBag.CurrentLessonLevel = userLessons.PassedLevel;
        return View(lessons);
    }


    public async Task<IActionResult> Details(int lessonId)
    {
        User? currentUser = await _userManager.GetUserAsync(User);
        Lesson? lesson = await _lessonService.GetById(lessonId);
        if (lesson == null)
            return NotFound("No Lesson!");
        UserLessons? userLessons = _userLessonService.GetAll()
            .FirstOrDefault(ul => ul.UserId == currentUser.Id
            && ul.SubjectId == lesson.SubjectId);
        if (lesson.LessonLevel > userLessons.PassedLevel + 1)
            return RedirectToAction("Index", new { subTopicId = lesson.SubjectId });
        if (userLessons == null)
            return NotFound("NO UserLesson!");
        if (userLessons.PassedLevel >= lesson.LessonLevel || userLessons.PassedLevel + 1 == lesson.LessonLevel)
            return View(lesson);
        return NotFound("Пройдите предыдущий урок, чтобы открыть");
    }
    [HttpPost]
    public async Task<IActionResult> LogTime([FromBody] LessonTimeDto model)
    {
        if(ModelState.IsValid)
        {
            var lesson = _lessonService.GetById(model.LessonId);
            if(lesson is null)
            {
                return NotFound($"Lesson with id: {model.LessonId} not found");
            }
            var user = await _userManager.GetUserAsync(User);
            var lessonTime = new LessonTime()
            {
                LessonId = model.LessonId,
                UserId = user.Id,
                TimeSpent = TimeSpan.FromMilliseconds(model.TimeSpent),
                Date = DateOnly.FromDateTime(DateTime.Now),
            };
            await _lessonTimeService.Insert(lessonTime);
            return Ok();
        }
        return BadRequest();
    }
}
