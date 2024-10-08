﻿namespace YouCan.Entities;

public class Answer : EntityBase
{
    public string Content { get; set; }
    public bool IsCorrect { get; set; }

    public int QuestionId { get; set; }
    public Question? Question { get; set; }
}
