using Microsoft.EntityFrameworkCore;
using YouCan.Models;

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
            new Subtopic() { Id = 1, Name = "Mathematics1", TopicId = 1 },
            new Subtopic() { Id = 2, Name = "Mathematics2", TopicId = 1 },
            new Subtopic() { Id = 3, Name = "Analogy", TopicId = 2},
            new Subtopic() { Id = 4, Name = "Grammar", TopicId = 2 },
            new Subtopic() { Id = 5, Name = "Reading and Understanding", TopicId = 2 }
            );
        
    }
}