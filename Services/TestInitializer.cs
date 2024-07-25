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
        
        _modelBuilder.Entity<OrtInstruction>().HasData(
            new OrtInstruction()
            {
                Id = 1, Description = "Добро пожаловать на предметный тест по Математике. Тест состоит из 40 заданий, на выполнение которых отводится 80 минут. Во время выполнения заданий Вы можете изменить выбранный вариант ответа, но сделать это можно только один раз для каждого задания. При исправлении новый (правильный) ответ будет представлен квадратиком. После прохождения теста Вы получите результат, в котором будет указано количество данных Вами правильных ответов и общий процент правильно и неправильно выполненных заданий. Желаем удачи!"
                , QuestionsCount = 40, TimeInMin = 80
            },
            new OrtInstruction()
            {
                Id = 2,  Description = "Добро пожаловать на предметный тест по Аналогиям и дополнениям . Тест состоит из 30 заданий, на выполнение которых отводится 30 минут. Во время выполнения заданий Вы можете изменить выбранный вариант ответа, но сделать это можно только один раз для каждого задания. При исправлении новый (правильный) ответ будет представлен квадратиком. После прохождения теста Вы получите результат, в котором будет указано количество данных Вами правильных ответов и общий процент правильно и неправильно выполненных заданий. Желаем удачи!"
                , QuestionsCount = 30, TimeInMin = 70
            },
            new OrtInstruction()
            {
                Id = 3,  Description = "Добро пожаловать на предметный тест по Чтению и пониманию. Тест состоит из 30 заданий, на выполнение которых отводится 35 минут. Во время выполнения заданий Вы можете изменить выбранный вариант ответа, но сделать это можно только один раз для каждого задания. При исправлении новый (правильный) ответ будет представлен квадратиком. После прохождения теста Вы получите результат, в котором будет указано количество данных Вами правильных ответов и общий процент правильно и неправильно выполненных заданий. Желаем удачи!"
                , QuestionsCount = 30, TimeInMin = 60
            },
            new OrtInstruction()
            {
                Id = 4,  Description = "Добро пожаловать на предметный тест по Практической грамматике. Тест состоит из 30 заданий, на выполнение которых отводится 35 минут. Во время выполнения заданий Вы можете изменить выбранный вариант ответа, но сделать это можно только один раз для каждого задания. При исправлении новый (правильный) ответ будет представлен квадратиком. После прохождения теста Вы получите результат, в котором будет указано количество данных Вами правильных ответов и общий процент правильно и неправильно выполненных заданий. Желаем удачи!"
                , QuestionsCount = 30, TimeInMin = 50
            }
            );
        
        _modelBuilder.Entity<Test>().HasData(
            new Test(){Id = 1, LessonId = 12, OrtTestId = 1, SubjectId = 1, OrtInstructionId = 1, TimeForTestInMin = 40},//Альтернатива LessonId = 1
            new Test(){Id = 2, LessonId = 13, OrtTestId = 1, SubjectId = 5, OrtInstructionId = 2, TimeForTestInMin = 40}, //Альтернатива LessonId = 2
            new Test(){Id = 4,  OrtTestId = 1, SubjectId = 7, OrtInstructionId = 3, TimeForTestInMin = 30},
            new Test(){Id = 5,  OrtTestId = 1, SubjectId = 2, OrtInstructionId = 4, TimeForTestInMin = 30} 
        );

        
        
        _modelBuilder.Entity<Question>().HasData(
            new Question(){Id = 1, TestId = 1, Point = 2, Type = "analogy", Instruction = "Что больше?", Content = "23 : 34"},
            new Question(){Id = 2, TestId = 1, Point = 2, Type = "general", Instruction = "Отвечайте на следующие вопросы", Content = "Что такое интеграл?"},
            new Question(){Id = 3, TestId = 1, Point = 3, Type = "general", Instruction = "Отметьте вариант, наиболее близкий к контрольной паре", Content = "Грамматика это___"},
            new Question(){Id = 4, TestId = 2, Point = 3, Type = "analogy", Instruction = "Отметьте вариант, наиболее близкий к контрольной паре", Content = "Птица : Гнездо"},
            new Question(){Id = 5, TestId = 2, Point = 2, Type = "general", Instruction = "Отвечайте на следующие вопросы", Content = "Что такое интеграл?"},
            new Question(){Id = 6, TestId = 2, Point = 2, Type = "general", Instruction = "Отвечайте на следующие вопросы", Content = "Lorem Ipsum - это текст-\"рыба\", часто используемый в печати и вэб-дизайне. Lorem Ipsum является стандартной \"рыбой\" для текстов на латинице с начала XVI века. В то время некий безымянный печатник создал большую коллекцию размеров и форм шрифтов, используя Lorem Ipsum для распечатки образцов. Lorem Ipsum не только успешно пережил без заметных изменений пять веков, но и перешагнул в электронный дизайн."},
            new Question(){Id = 12, TestId = 4, Point = 3, Type = "analogy", Instruction = "Что больше?", Content = "23 : 34"},
            new Question(){Id = 13, TestId = 4, Point = 3, Type = "general", Instruction = "Отвечайте на следующие вопросы", Content = "Что такое интеграл?"},
            new Question(){Id = 14, TestId = 4, Point = 2, Type = "general", Instruction = "Отметьте вариант, наиболее близкий к контрольной паре", Content = "Грамматика это___"},
            new Question(){Id = 15, TestId = 5, Point = 2, Type = "analogy", Instruction = "Отметьте вариант, наиболее близкий к контрольной паре", Content = "Птица : Гнездо"},
            new Question(){Id = 16, TestId = 5, Point = 3, Type = "general", Instruction = "Отвечайте на следующие вопросы", Content = "Что такое интеграл?"},
            new Question(){Id = 17, TestId = 5, Point = 3, Type = "general", Instruction = "Отвечайте на следующие вопросы", Content = "Lorem Ipsum - это текст-\"рыба\", часто используемый в печати и вэб-дизайне. Lorem Ipsum является стандартной \"рыбой\" для текстов на латинице с начала XVI века. В то время некий безымянный печатник создал большую коллекцию размеров и форм шрифтов, используя Lorem Ipsum для распечатки образцов. Lorem Ipsum не только успешно пережил без заметных изменений пять веков, но и перешагнул в электронный дизайн."}
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
            new Answer(){Id = 12, QuestionId = 3, IsCorrect = true, Text = "Грамматика это 5 по русскому"},
            new Answer(){Id = 33, QuestionId = 14, IsCorrect = false, Text = "Грамматика это чудо"},
            new Answer(){Id = 34, QuestionId = 14, IsCorrect = false, Text = "Грамматика это таска"},
            new Answer(){Id = 35, QuestionId = 14, IsCorrect = false, Text = "Грамматика это пипец"},
            new Answer(){Id = 36, QuestionId = 14, IsCorrect = true, Text = "Грамматика это 5 по русскому"},
            new Answer(){Id = 37, QuestionId = 15, IsCorrect = false, Text = "A больше"},
            new Answer(){Id = 38, QuestionId = 15, IsCorrect = true, Text = "B больше"},
            new Answer(){Id = 39, QuestionId = 15, IsCorrect = false, Text = " оба равны"},
            new Answer(){Id = 40, QuestionId = 15, IsCorrect = false, Text = "нельзя сравнить"},
            new Answer(){Id = 41, QuestionId = 16, IsCorrect = false, Text = "Площадь под функции f(x)"},
            new Answer(){Id = 42, QuestionId = 16, IsCorrect = false, Text = "интеграл от f(x) от 0 до x"},
            new Answer(){Id = 43, QuestionId = 16, IsCorrect = true, Text = "Функция, производная которой равна f(x)"},
            new Answer(){Id = 44, QuestionId = 16, IsCorrect = false, Text = "Функция, производная которой равна Y(f-k)"},
            new Answer(){Id = 45, QuestionId = 17, IsCorrect = false, Text = "Грамматика это чудо"},
            new Answer(){Id = 46, QuestionId = 17, IsCorrect = false, Text = "Грамматика это таска"},
            new Answer(){Id = 47, QuestionId = 17, IsCorrect = false, Text = "Грамматика это пипец"},
            new Answer(){Id = 48, QuestionId = 17, IsCorrect = true, Text = "Грамматика это 5 по русскому"},
            new Answer(){Id = 49, QuestionId = 4, IsCorrect = false, Text = "A больше"},
            new Answer(){Id = 50, QuestionId = 4, IsCorrect = true, Text = "B больше"},
            new Answer(){Id = 51, QuestionId = 4, IsCorrect = false, Text = " оба равны"},
            new Answer(){Id = 52, QuestionId = 4, IsCorrect = false, Text = "нельзя сравнить"},
            new Answer(){Id = 53, QuestionId = 5, IsCorrect = false, Text = "Площадь под функции f(x)"},
            new Answer(){Id = 54, QuestionId = 5, IsCorrect = false, Text = "интеграл от f(x) от 0 до x"},
            new Answer(){Id = 55, QuestionId = 5, IsCorrect = true, Text = "Функция, производная которой равна f(x)"},
            new Answer(){Id = 56, QuestionId = 5, IsCorrect = false, Text = "Функция, производная которой равна Y(f-k)"},
            new Answer(){Id = 57, QuestionId = 6, IsCorrect = false, Text = "Грамматика это чудо"},
            new Answer(){Id = 58, QuestionId = 6, IsCorrect = false, Text = "Грамматика это таска"},
            new Answer(){Id = 59, QuestionId = 6, IsCorrect = false, Text = "Грамматика это пипец"},
            new Answer(){Id = 60, QuestionId = 6, IsCorrect = true, Text = "Грамматика это 5 по русскому"},
            new Answer(){Id = 61, QuestionId = 12, IsCorrect = false, Text = "A больше"},
            new Answer(){Id = 62, QuestionId = 12, IsCorrect = true, Text = "B больше"},
            new Answer(){Id = 63, QuestionId = 12, IsCorrect = false, Text = " оба равны"},
            new Answer(){Id = 64, QuestionId = 12, IsCorrect = false, Text = "нельзя сравнить"},
            new Answer(){Id = 65, QuestionId = 13, IsCorrect = false, Text = "Площадь под функции f(x)"},
            new Answer(){Id = 66, QuestionId = 13, IsCorrect = false, Text = "интеграл от f(x) от 0 до x"},
            new Answer(){Id = 67, QuestionId = 13, IsCorrect = true, Text = "Функция, производная которой равна f(x)"},
            new Answer(){Id = 68, QuestionId = 13, IsCorrect = false, Text = "Функция, производная которой равна Y(f-k)"}
            );
    }
}