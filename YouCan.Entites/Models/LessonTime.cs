using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YouCan.Entities;

namespace YouCan.Entites.Models;

public class LessonTime : EntityBase
{
    public int UserId { get; set; }
    public User? User { get; set; }
    public int LessonId { get; set; }
    public Lesson? Lesson { get; set; }
    public TimeSpan TimeSpent { get; set; }
    public DateOnly Date { get; set; }
}
