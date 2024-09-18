using Microsoft.EntityFrameworkCore;
using YouCan.Entities;

namespace YouCan.Repository.Services;

public class SubjectInitializer
{
    private readonly ModelBuilder _modelBuilder;

    public SubjectInitializer(ModelBuilder modelBuilder)
    {
        _modelBuilder = modelBuilder;
    }
    public void Seed()
    {
        // ������������� ������ ��� Subject
        _modelBuilder.Entity<Subject>().HasData(
            new Subject() {Id = 1, Name = "Математика", ImageUrl = "/topicImages/mathematics1icon.png", ParentId = null, SubjectType = SubjectType.Parent},
            new Subject() {Id = 2, Name = "Русский", ImageUrl = "/topicImages/grammarIcon.png", ParentId = null, SubjectType = SubjectType.Parent},
            new Subject(){Id = 3, Name = "Математика 1", ImageUrl = "/topicImages/mathematics1icon.png", ParentId = 1, SubjectType = SubjectType.Child},
            new Subject(){Id = 4, Name = "Математика 2", ImageUrl = "/topicImages/mathematics2icon.png", ParentId = 1, SubjectType = SubjectType.Child},
            new Subject(){Id = 5, Name = "Аналогия", ImageUrl = "/topicImages/analogyIcon.png", ParentId = 2, SubjectType = SubjectType.Parent},
            new Subject(){Id = 6, Name = "Грамматика", ImageUrl = "/topicImages/grammarIcon.png", ParentId = 2, SubjectType = SubjectType.Child},
            new Subject(){Id = 7, Name = "Чтение и понимание", ImageUrl = "/topicImages/readUnderstIcon.png", ParentId = 2, SubjectType = SubjectType.Child, UserTestType = YouCan.Entities.UserTestType.Test},
            new Subject(){Id = 8, Name = "Аналогия", ImageUrl = "/topicImages/analogyIcon.png", ParentId = 5, SubjectType = SubjectType.Child},
            new Subject(){Id = 9, Name = "Дополнение предложений", ImageUrl = "/topicImages/readUnderstIcon.png", ParentId = 5, SubjectType = SubjectType.Child}
        );

        _modelBuilder.Entity<SubjectLocalization>().HasData(
            new SubjectLocalization() { Id = 1, SubjectId = 1, Culture = "ru", Title = "Математика" },
            new SubjectLocalization() { Id = 2, SubjectId = 1, Culture = "ky", Title = "Математика" },
            new SubjectLocalization() { Id = 3, SubjectId = 2, Culture = "ru", Title = "Русский язык" },
            new SubjectLocalization() { Id = 4, SubjectId = 2, Culture = "ky", Title = "Орус тили" },
            new SubjectLocalization() { Id = 5, SubjectId = 3, Culture = "ru", Title = "Математика 1" },
            new SubjectLocalization() { Id = 6, SubjectId = 3, Culture = "ky", Title = "Математика 1" },
            new SubjectLocalization() { Id = 7, SubjectId = 4, Culture = "ru", Title = "Математика 2" },
            new SubjectLocalization() { Id = 8, SubjectId = 4, Culture = "ky", Title = "Математика 2" },
            new SubjectLocalization() { Id = 9, SubjectId = 5, Culture = "ru", Title = "Аналогия" },
            new SubjectLocalization() { Id = 10, SubjectId = 6, Culture = "ru", Title = "Грамматика" },
            new SubjectLocalization() { Id = 11, SubjectId = 7, Culture = "ru", Title = "Чтение и понимание" },
            new SubjectLocalization() { Id = 12, SubjectId = 7, Culture = "ky", Title = "Окуу жана тушунуу" },
            new SubjectLocalization() { Id = 13, SubjectId = 8, Culture = "ru", Title = "Аналогия" },
            new SubjectLocalization() { Id = 14, SubjectId = 9, Culture = "ru", Title = "Дополнение предложений" },
            new SubjectLocalization() { Id = 15, SubjectId = 9, Culture = "ky", Title = "Суйломго толуктоо" }
        );

    }
}