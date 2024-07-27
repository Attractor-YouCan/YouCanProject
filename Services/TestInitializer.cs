using Microsoft.EntityFrameworkCore;
using YouCan.Models;

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
            new Test(){Id = 1, LessonId = 12},//Альтернатива LessonId = 1
            new Test(){Id = 2, LessonId = 13}//Альтернатива LessonId = 2
        );
        _modelBuilder.Entity<Question>().HasData(
            new Question(){Id = 1, TestId = 1, Type = "analogy", Instruction = "Что больше?", Text = "23 : 34"},
            new Question(){Id = 2, TestId = 1, Type = "general", Instruction = "Отвечайте на следующие вопросы", Text = "Что такое интеграл?"},
            new Question(){Id = 3, TestId = 1, Type = "general", Instruction = "Отметьте вариант, наиболее близкий к контрольной паре", Text = "Грамматика это___"},
            new Question(){Id = 4, TestId = 2, Type = "analogy", Instruction = "Отметьте вариант, наиболее близкий к контрольной паре", Text = "Птица : Гнездо"},
            new Question(){Id = 5, TestId = 2, Type = "general", Instruction = "Отвечайте на следующие вопросы", Text = "Что такое интеграл?"},
            new Question(){Id = 6, TestId = 2, Type = "general", Instruction = "Отвечайте на следующие вопросы", Text = "Lorem Ipsum - это текст-\"рыба\", часто используемый в печати и вэб-дизайне. Lorem Ipsum является стандартной \"рыбой\" для текстов на латинице с начала XVI века. В то время некий безымянный печатник создал большую коллекцию размеров и форм шрифтов, используя Lorem Ipsum для распечатки образцов. Lorem Ipsum не только успешно пережил без заметных изменений пять веков, но и перешагнул в электронный дизайн."}
        );

        _modelBuilder.Entity<Answer>().HasData(
            new Answer(){Id = 1, QuestionId = 1, IsCorrect = false, Content = "A больше"},
            new Answer(){Id = 2, QuestionId = 1, IsCorrect = true, Content = "B больше"},
            new Answer(){Id = 3, QuestionId = 1, IsCorrect = false, Content = " оба равны"},
            new Answer(){Id = 4, QuestionId = 1, IsCorrect = false, Content = "нельзя сравнить"},
            new Answer(){Id = 5, QuestionId = 2, IsCorrect = false, Content = "Площадь под функции f(x)"},
            new Answer(){Id = 6, QuestionId = 2, IsCorrect = false, Content = "интеграл от f(x) от 0 до x"},
            new Answer(){Id = 7, QuestionId = 2, IsCorrect = true, Content = "Функция, производная которой равна f(x)"},
            new Answer(){Id = 8, QuestionId = 2, IsCorrect = false, Content = "Функция, производная которой равна Y(f-k)"},
            new Answer(){Id = 9, QuestionId = 3, IsCorrect = false, Content = "Грамматика это чудо"},
            new Answer(){Id = 10, QuestionId = 3, IsCorrect = false, Content = "Грамматика это таска"},
            new Answer(){Id = 11, QuestionId = 3, IsCorrect = false, Content = "Грамматика это пипец"},
            new Answer(){Id = 12, QuestionId = 3, IsCorrect = true, Content = "Грамматика это 5 по русскому"}
            );
    }
}