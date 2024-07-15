using Microsoft.EntityFrameworkCore;
using YouCan.Models;

namespace YouCan.Services;

public class TrainTestInitializer
{
    private ModelBuilder _modelBuilder;

    public TrainTestInitializer(ModelBuilder modelBuilder)
    {
        _modelBuilder = modelBuilder;
    }

    public void Seed()
    {
        _modelBuilder.Entity<Test>().HasData(
            new Test(){Id = 3, SubtopicId = 4}
        );
        
        _modelBuilder.Entity<Question>().HasData(
            new Question(){Id = 7, TestId = 3, Instruction = "Выберите правильный вариант", Content = "В каком слове вместо точек следует вставить букву с?"},
            new Question(){Id = 8, TestId = 3, Instruction = "Выберите правильный вариант", Content = "В каком предложении подчеркнутое слово можно заменить словом (высохший (-ая, -ее))?"},
            new Question(){Id = 9, TestId = 3, Instruction = "Выберите правильный вариант", Content = "В каком слове вместо точек следует вставить букву з?"},
            new Question(){Id = 10, TestId = 3, Instruction = "Выберите правильный вариант", Content = "В каком предложении подчеркнутое слово употреблено в правильной форме?"},
            new Question(){Id = 11, TestId = 3, Instruction = "Какое слово следует вставить вместо точек в предложение?", Content = "Задание, ... нами, не вызывает особых затруднений."}
        );
        
        _modelBuilder.Entity<Answer>().HasData(
            new Answer(){Id = 13, QuestionId = 7, IsCorrect = true, Text = "ве...ти концерт"},
            new Answer(){Id = 14, QuestionId = 7, IsCorrect = false, Text = "ве...ти груз"},
            new Answer(){Id = 15, QuestionId = 7, IsCorrect = false, Text = "ни...кая температура"},
            new Answer(){Id = 16, QuestionId = 7, IsCorrect = false, Text = "передвигались пол...ком"},
            
            new Answer(){Id = 17, QuestionId = 8, IsCorrect = false, Text = "Лето будет жаркое и (сухое)."},
            new Answer(){Id = 18, QuestionId = 8, IsCorrect = false, Text = "Надо купить пакет (сухого) молока."},
            new Answer(){Id = 19, QuestionId = 8, IsCorrect = false, Text = "Матч закончился с (сухим) счетом."},
            new Answer(){Id = 20, QuestionId = 8, IsCorrect = true, Text = "В ванной висит (сухое) полотенце"},
            
            new Answer(){Id = 21, QuestionId = 9, IsCorrect = true, Text = "в...рыхлённый граблями"},
            new Answer(){Id = 22, QuestionId = 9, IsCorrect = false, Text = "в...копанный огород"},
            new Answer(){Id = 23, QuestionId = 9, IsCorrect = false, Text = "в...порхнувший с ветки"},
            new Answer(){Id = 24, QuestionId = 9, IsCorrect = false, Text = "в...тавленный в текст"},
            
            new Answer(){Id = 25, QuestionId = 10, IsCorrect = false, Text = "Слева стояли такие высокие здания, что мимо (их) проплывали облака."},
            new Answer(){Id = 26, QuestionId = 10, IsCorrect = false, Text = "Впереди шла стройная женщина, а позади (её) бежал малыш."},
            new Answer(){Id = 27, QuestionId = 10, IsCorrect = false, Text = "На углу улицы я увидел мальчика, возле (его) стояла корзина с цветами."},
            new Answer(){Id = 28, QuestionId = 10, IsCorrect = true, Text = "Когда Мурат свернул на шоссе, то увидел, что навстречу (ему) медленно движется колонна машин."},
           
            new Answer(){Id = 29, QuestionId = 11, IsCorrect = false, Text = "выполняющееся"},
            new Answer(){Id = 30, QuestionId = 11, IsCorrect = true, Text = "выполняемое"},
            new Answer(){Id = 31, QuestionId = 11, IsCorrect = false, Text = "выполненное"},
            new Answer(){Id = 32, QuestionId = 11, IsCorrect = false, Text = "выполнявшееся"}
        );
    }
}