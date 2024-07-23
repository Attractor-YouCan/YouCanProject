using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using YouCan.Services;

namespace YouCan.Models;

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
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<QuestionReport> QuestionReports { get; set; }
    public DbSet<OrtTest> OrtTests { get; set; }
    public DbSet<OrtInstruction> OrtInstructions { get; set; }

protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Subject>()
            .HasMany(t => t.SubSubjects)
            .WithOne(t => t.Parent)
            .HasForeignKey(t => t.ParentId);
        modelBuilder.Entity<OrtInstruction>()
            .HasOne(t => t.Test)
            .WithOne(o => o.OrtInstruction)
            .HasForeignKey<Test>(o => o.OrtInstructionId); 
        
        base.OnModelCreating(modelBuilder);
        new OrtTestInitializer(modelBuilder).Seed();
        new TopicInitializer(modelBuilder).Seed();
        new LessonInitializer(modelBuilder).Seed();
        new TestInitializer(modelBuilder).Seed();
        new TrainTestInitializer(modelBuilder).Seed();
    }
    public YouCanContext(DbContextOptions<YouCanContext> options) : base(options){}

}