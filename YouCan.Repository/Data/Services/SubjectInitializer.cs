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
            new Subject() {Id = 1, Name = "Математика", ImageUrl = "/topicImages/mathematics1icon.png", ParentId = null, SubjectType = SubjectType.Parent},
            new Subject() {Id = 2, Name = "Русский", ImageUrl = "/topicImages/grammarIcon.png", ParentId = null, SubjectType = SubjectType.Parent},
            new Subject(){Id = 3, Name = "Математика 1", ImageUrl = "/topicImages/mathematics1icon.png", ParentId = 1, SubjectType = SubjectType.Child},
            new Subject(){Id = 4, Name = "Математика 2", ImageUrl = "/topicImages/mathematics2icon.png", ParentId = 1, SubjectType = SubjectType.Child},
            new Subject(){Id = 5, Name = "Аналогия", ImageUrl = "/topicImages/analogyIcon.png", ParentId = 2, SubjectType = SubjectType.Parent},
            new Subject(){Id = 6, Name = "Грамматика", ImageUrl = "/topicImages/grammarIcon.png", ParentId = 2, SubjectType = SubjectType.Child},
            new Subject(){Id = 7, Name = "Чтение и понимание", ImageUrl = "/topicImages/readUnderstIcon.png", ParentId = 2, SubjectType = SubjectType.Child, UserTestType = Entites.Models.UserTestType.Test},
            new Subject(){Id = 8, Name = "Аналогия", ImageUrl = "/topicImages/analogyIcon.png", ParentId = 5, SubjectType = SubjectType.Child},
            new Subject(){Id = 9, Name = "Дополнение предложений", ImageUrl = "/topicImages/readUnderstIcon.png", ParentId = 5, SubjectType = SubjectType.Child}
        );
        
    }
}