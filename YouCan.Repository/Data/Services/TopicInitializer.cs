using Microsoft.EntityFrameworkCore;
using YouCan.Entities;

namespace YouCan.Services;

public class TopicInitializer
{
    private readonly ModelBuilder _modelBuilder;

    public TopicInitializer(ModelBuilder modelBuilder)
    {
        _modelBuilder = modelBuilder;
    }
    public void Seed()
    {
        _modelBuilder.Entity<Topic>().HasData(
            new Topic(){Id = 1, Name = "Mathematics"},
            new Topic(){Id = 2, Name = "Russian"}
        );
        _modelBuilder.Entity<Subtopic>().HasData(
            new Subtopic() { Id = 1, Name = "Mathematics1", ImageUrl = "/topicImages/mathematics1icon.png", TopicId = 1 },
            new Subtopic() { Id = 2, Name = "Mathematics2", ImageUrl = "/topicImages/mathematics2icon.png", TopicId = 1 },
            new Subtopic() { Id = 3, Name = "Analogy", ImageUrl = "/topicImages/analogyIcon.png", TopicId = 2},
            new Subtopic() { Id = 4, Name = "Grammar", ImageUrl = "/topicImages/grammarIcon.png", TopicId = 2 },
            new Subtopic() { Id = 5, Name = "Reading and Understanding", ImageUrl = "/topicImages/readUnderstIcon.png", TopicId = 2 }
            );
        
    }
}