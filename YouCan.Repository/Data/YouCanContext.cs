using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using YouCan.Entities;
using YouCan.Services;

namespace YouCan.Repository;

public class YouCanContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public DbSet<User> Users { get; set; }
    public DbSet<Statistic> Statistics { get; set; }
    public DbSet<Topic> Topics { get; set; }
    public DbSet<Subtopic> Subtopics { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<Test> Tests { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Answer> Answers { get; set; }
    public DbSet<UserLessons> UserLessons { get; set; }
    public DbSet<UserLevel> UserLevels { get; set; }
    public DbSet<UserOrtTest> UserORTTests { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        new TopicInitializer(modelBuilder).Seed();
        new LessonInitializer(modelBuilder).Seed();
        new TestInitializer(modelBuilder).Seed();
    }
    public YouCanContext(DbContextOptions<YouCanContext> options) : base(options){}

}