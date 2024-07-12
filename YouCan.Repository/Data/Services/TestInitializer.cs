using Microsoft.EntityFrameworkCore;
using YouCan.Entities;

namespace YouCan.Services;

public class TestInitializer
{
    private ModelBuilder _modelBuilder;

    public TestInitializer(ModelBuilder modelBuilder)
    {
        _modelBuilder = modelBuilder;
    }

    public void Seed()
    {
        _modelBuilder.Entity<Test>().HasData(
            new Test(){Id = 1, LessonId = 1},
            new Test(){Id = 2, LessonId = 2}
        );
        _modelBuilder.Entity<Question>().HasData(
            new Question(){Id = 1, TestId = 1, Type = "analogy", Instruction = "Что больше?", Content = "23 : 34"},
            new Question(){Id = 2, TestId = 1, Type = "general", Instruction = "Отвечайте на следующие вопросы", Content = "Что такое интеграл?"},
            new Question(){Id = 3, TestId = 1, Type = "general", Instruction = "Отметьте вариант, наиболее близкий к контрольной паре", Content = "Грамматика это___"},
            new Question(){Id = 4, TestId = 2, Type = "analogy", Instruction = "Отметьте вариант, наиболее близкий к контрольной паре", Content = "Птица : Гнездо"},
            new Question(){Id = 5, TestId = 2, Type = "general", Instruction = "Отвечайте на следующие вопросы", Content = "Что такое интеграл?"},
            new Question(){Id = 6, TestId = 2, Type = "general", Instruction = "Отвечайте на следующие вопросы", Content = "Lorem Ipsum - это текст-\"рыба\", часто используемый в печати и вэб-дизайне. Lorem Ipsum является стандартной \"рыбой\" для текстов на латинице с начала XVI века. В то время некий безымянный печатник создал большую коллекцию размеров и форм шрифтов, используя Lorem Ipsum для распечатки образцов. Lorem Ipsum не только успешно пережил без заметных изменений пять веков, но и перешагнул в электронный дизайн."}
        );

        _modelBuilder.Entity<Answer>().HasData(
            new Answer(){Id = 1, QuestionId = 1, IsCorrect = false, Text = "A больше"},
            new Answer(){Id = 2, QuestionId = 1, IsCorrect = true, Text = "B больше"},
            new Answer(){Id = 3, QuestionId = 1, IsCorrect = false, Text = " оба равны"},
            new Answer(){Id = 4, QuestionId = 1, IsCorrect = false, Text = "нельзя сравнить"},
            new Answer(){Id = 5, QuestionId = 2, IsCorrect = false, Text = "Площадь под функции f(x)"},
            new Answer(){Id = 6, QuestionId = 2, IsCorrect = false, Text = "интеграл от f(x) от 0 до x"},
            new Answer(){Id = 7, QuestionId = 2, IsCorrect = true, Text = "Функция, производная которой равна f(x)"},
            new Answer(){Id = 8, QuestionId = 2, IsCorrect = false, Text = "Функция, производная которой равна Y(f-k)"},
            new Answer(){Id = 9, QuestionId = 3, IsCorrect = false, Text = "Грамматика это чудо"},
            new Answer(){Id = 10, QuestionId = 3, IsCorrect = false, Text = "Грамматика это таска"},
            new Answer(){Id = 11, QuestionId = 3, IsCorrect = false, Text = "Грамматика это пипец"},
            new Answer(){Id = 12, QuestionId = 3, IsCorrect = true, Text = "Грамматика это 5 по русскому"}
            );
    }
}