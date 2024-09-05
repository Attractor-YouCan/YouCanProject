using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using YouCan.Entites.Models;
using YouCan.Entities;
using YouCan.Services;

namespace YouCan.Repository;

public class YouCanContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public DbSet<User> Users { get; set; }
    public DbSet<Statistic> Statistics { get; set; }
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
    public DbSet<PassedQuestion> PassedQuestions { get; set; }
    public DbSet<Tariff> Tariffs { get; set; }
    public DbSet<League> Leagues { get; set; }
    public DbSet<AdminAction> AdminActions { get; set; }
    public DbSet<LessonTime> LessonTimes { get; set; }
    
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
        new SubjectInitializer(modelBuilder).Seed();
        //new OrtTestInitializer(modelBuilder).Seed();
        //new LessonInitializer(modelBuilder).Seed();
        //new TestInitializer(modelBuilder).Seed();
        
        modelBuilder.Entity<IdentityRole<int>>()
            .HasData(
                new IdentityRole<int> { Id = 1, Name = "user", NormalizedName = "USER" },
                new IdentityRole<int> { Id = 2, Name = "manager", NormalizedName = "MANAGER" },
                new IdentityRole<int> { Id = 3, Name = "admin", NormalizedName = "ADMIN" },
                new IdentityRole<int> { Id = 4, Name = "prouser", NormalizedName = "PROUSER"}
            );
        modelBuilder.Entity<Tariff>()
            .HasData(
                new Tariff { Id = 1, Name = "Start", Duration = null, Price = 0 },
                new Tariff { Id = 2, Name = "Pro", Duration = 1, Price = 1 },
                new Tariff { Id = 3, Name = "Premium", Duration = 3, Price = 2 }
            );
        
        modelBuilder.Entity<League>().HasData(
            new League
            {
                Id = 1,
                LeagueName = "Bronze",
                MinPoints = 0,
                MaxPoints = 999
            },
            new League
            {
                Id = 2,
                LeagueName = "Silver",
                MinPoints = 1000,
                MaxPoints = 1999
            },
            new League
            {
                Id = 3,
                LeagueName = "Gold",
                MinPoints = 2000,
                MaxPoints = 2999
            },
            new League
            {
                Id = 4,
                LeagueName = "Platinum",
                MinPoints = 3000,
                MaxPoints = 3999
            },
            new League
            {
                Id = 5,
                LeagueName = "Diamond",
                MinPoints = 4000,
                MaxPoints = int.MaxValue
            }
        );
        
        new TrainTestInitializer(modelBuilder).Seed();
    }
    public YouCanContext(DbContextOptions<YouCanContext> options) : base(options){}

}