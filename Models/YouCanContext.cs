using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace YouCan.Models;

public class YouCanContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public DbSet<Statistic> Statistics { get; set; }
    public DbSet<Topic> Topics { get; set; }
    public DbSet<Subtopic> Subtopics { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<Test> Tests { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Answer> Answers { get; set; }
    public DbSet<UserLessons> UserLessons { get; set; }
    public DbSet<UserLevel> UserLevels { get; set; }
    public DbSet<UserORTTests> UserORTTests { get; set; }
    public YouCanContext(DbContextOptions<YouCanContext> options) : base(options){}

}