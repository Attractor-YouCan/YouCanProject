using Microsoft.EntityFrameworkCore;
using YouCan.Entities;
namespace YouCan.Repository;

public class SubjectInitializer
{
    private readonly ModelBuilder _modelBuilder;

    public SubjectInitializer(ModelBuilder modelBuilder)
    {
        _modelBuilder = modelBuilder;
    }
    public void Seed()
    {
        _modelBuilder.Entity<Subject>().HasData(
            new Subject() {Id = 1, Name = "Mathematics", ImageUrl = "/topicImages/mathematics1icon.png", ParentId = null, SubjectType = SubjectType.Parent},
            new Subject() {Id = 2, Name = "Russian", ImageUrl = "/topicImages/grammarIcon.png", ParentId = null, SubjectType = SubjectType.Parent},
            new Subject(){Id = 3, Name = "Mathematics1", ImageUrl = "/topicImages/mathematics1icon.png", ParentId = 1, SubjectType = SubjectType.Child},
            new Subject(){Id = 4, Name = "Mathematics2", ImageUrl = "/topicImages/mathematics2icon.png", ParentId = 1, SubjectType = SubjectType.Child},
            new Subject(){Id = 5, Name = "Analogy", ImageUrl = "/topicImages/analogyIcon.png", ParentId = 2, SubjectType = SubjectType.Parent},
            new Subject(){Id = 6, Name = "Grammar", ImageUrl = "/topicImages/grammarIcon.png", ParentId = 2, SubjectType = SubjectType.Child},
            new Subject(){Id = 7, Name = "Reading and Understanding", ImageUrl = "/topicImages/readUnderstIcon.png", ParentId = 2, SubjectType = SubjectType.Child, UserTestType = Entites.Models.UserTestType.Test},
            new Subject(){Id = 8, Name = "Analogy", ImageUrl = "/topicImages/analogyIcon.png", ParentId = 5, SubjectType = SubjectType.Child},
            new Subject(){Id = 9, Name = "Addition of Offers", ImageUrl = "/topicImages/readUnderstIcon.png", ParentId = 5, SubjectType = SubjectType.Child}
        );
        
    }
}