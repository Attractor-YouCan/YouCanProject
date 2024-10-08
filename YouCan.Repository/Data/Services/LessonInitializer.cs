using Microsoft.EntityFrameworkCore;
using YouCan.Entities;

namespace YouCan.Repository.Services;

public class LessonInitializer
{
    private ModelBuilder _modelBuilder;

    public LessonInitializer(ModelBuilder modelBuilder)
    {
        _modelBuilder = modelBuilder;
    }

    public void Seed()
    {
        _modelBuilder.Entity<Lesson>().HasMany(l => l.LessonModules).WithOne(lm => lm.Lesson).HasForeignKey(lm => lm.LessonId);
        
        _modelBuilder.Entity<Lesson>().HasData(
            new Lesson() { Id = 1, Title = "Integrals", SubTitle = "Интеграл — одно из важнейших понятий математического анализа, которое возникает при решении задач", VideoUrl = "/lessonResources/lessonVideo.mp4", Description = "понятие интеграла;\nзамена переменной при интегрировании и интегрирование по частям;\nприменение определённых интегралов в геометрии и физике;\nдифференциальные уравнения.", Lecture = "Lorem Ipsum - это текст-\"рыба\", часто используемый в печати и вэб-дизайне. Lorem Ipsum является стандартной \"рыбой\" для текстов на латинице с начала XVI века. В то время некий безымянный печатник создал большую коллекцию размеров и форм шрифтов, используя Lorem Ipsum для распечатки образцов. Lorem Ipsum не только успешно пережил без заметных изменений пять веков, но и перешагнул в электронный дизайн.", SubjectId = 3, RequiredLevel = 0, LessonLevel = 1 },
            new Lesson() { Id = 2, Title = "Integrals", SubTitle = "Интеграл — одно из важнейших понятий математического анализа, которое возникает при решении задач", VideoUrl = "/lessonResources/lessonVideo.mp4", Description = "понятие интеграла;\nзамена переменной при интегрировании и интегрирование по частям;\nприменение определённых интегралов в геометрии и физике;\nдифференциальные уравнения.", Lecture = "Lorem Ipsum - это текст-\"рыба\", часто используемый в печати и вэб-дизайне. Lorem Ipsum является стандартной \"рыбой\" для текстов на латинице с начала XVI века. В то время некий безымянный печатник создал большую коллекцию размеров и форм шрифтов, используя Lorem Ipsum для распечатки образцов. Lorem Ipsum не только успешно пережил без заметных изменений пять веков, но и перешагнул в электронный дизайн.", SubjectId = 3, RequiredLevel = 1, LessonLevel = 2 },
            new Lesson() { Id = 3, Title = "Integrals", SubTitle = "Интеграл — одно из важнейших понятий математического анализа, которое возникает при решении задач", VideoUrl = "/lessonResources/lessonVideo.mp4", Description = "понятие интеграла;\nзамена переменной при интегрировании и интегрирование по частям;\nприменение определённых интегралов в геометрии и физике;\nдифференциальные уравнения.", Lecture = "Lorem Ipsum - это текст-\"рыба\", часто используемый в печати и вэб-дизайне. Lorem Ipsum является стандартной \"рыбой\" для текстов на латинице с начала XVI века. В то время некий безымянный печатник создал большую коллекцию размеров и форм шрифтов, используя Lorem Ipsum для распечатки образцов. Lorem Ipsum не только успешно пережил без заметных изменений пять веков, но и перешагнул в электронный дизайн.", SubjectId = 3, RequiredLevel = 2, LessonLevel = 3 },
            new Lesson() { Id = 4, Title = "Integrals", SubTitle = "Интеграл — одно из важнейших понятий математического анализа, которое возникает при решении задач", VideoUrl = "/lessonResources/lessonVideo.mp4", Description = "понятие интеграла;\nзамена переменной при интегрировании и интегрирование по частям;\nприменение определённых интегралов в геометрии и физике;\nдифференциальные уравнения.", Lecture = "Lorem Ipsum - это текст-\"рыба\", часто используемый в печати и вэб-дизайне. Lorem Ipsum является стандартной \"рыбой\" для текстов на латинице с начала XVI века. В то время некий безымянный печатник создал большую коллекцию размеров и форм шрифтов, используя Lorem Ipsum для распечатки образцов. Lorem Ipsum не только успешно пережил без заметных изменений пять веков, но и перешагнул в электронный дизайн.", SubjectId = 3, RequiredLevel = 3, LessonLevel = 4 },
            new Lesson() { Id = 5, Title = "Integrals", SubTitle = "Интеграл — одно из важнейших понятий математического анализа, которое возникает при решении задач", VideoUrl = "/lessonResources/lessonVideo.mp4", Description = "понятие интеграла;\nзамена переменной при интегрировании и интегрирование по частям;\nприменение определённых интегралов в геометрии и физике;\nдифференциальные уравнения.", Lecture = "Lorem Ipsum - это текст-\"рыба\", часто используемый в печати и вэб-дизайне. Lorem Ipsum является стандартной \"рыбой\" для текстов на латинице с начала XVI века. В то время некий безымянный печатник создал большую коллекцию размеров и форм шрифтов, используя Lorem Ipsum для распечатки образцов. Lorem Ipsum не только успешно пережил без заметных изменений пять веков, но и перешагнул в электронный дизайн.", SubjectId = 3, RequiredLevel = 4, LessonLevel = 5 },
            new Lesson() { Id = 6, Title = "Integrals", SubTitle = "Интеграл — одно из важнейших понятий математического анализа, которое возникает при решении задач", VideoUrl = "/lessonResources/lessonVideo.mp4", Description = "понятие интеграла;\nзамена переменной при интегрировании и интегрирование по частям;\nприменение определённых интегралов в геометрии и физике;\nдифференциальные уравнения.", Lecture = "Lorem Ipsum - это текст-\"рыба\", часто используемый в печати и вэб-дизайне. Lorem Ipsum является стандартной \"рыбой\" для текстов на латинице с начала XVI века. В то время некий безымянный печатник создал большую коллекцию размеров и форм шрифтов, используя Lorem Ipsum для распечатки образцов. Lorem Ipsum не только успешно пережил без заметных изменений пять веков, но и перешагнул в электронный дизайн.", SubjectId = 3, RequiredLevel = 5, LessonLevel = 6 },
            new Lesson() { Id = 7, Title = "Integrals", SubTitle = "Интеграл — одно из важнейших понятий математического анализа, которое возникает при решении задач", VideoUrl = "/lessonResources/lessonVideo.mp4", Description = "понятие интеграла;\nзамена переменной при интегрировании и интегрирование по частям;\nприменение определённых интегралов в геометрии и физике;\nдифференциальные уравнения.", Lecture = "Lorem Ipsum - это текст-\"рыба\", часто используемый в печати и вэб-дизайне. Lorem Ipsum является стандартной \"рыбой\" для текстов на латинице с начала XVI века. В то время некий безымянный печатник создал большую коллекцию размеров и форм шрифтов, используя Lorem Ipsum для распечатки образцов. Lorem Ipsum не только успешно пережил без заметных изменений пять веков, но и перешагнул в электронный дизайн.", SubjectId = 3, RequiredLevel = 6, LessonLevel = 7 },
            new Lesson() { Id = 8, Title = "Integrals", SubTitle = "Интеграл — одно из важнейших понятий математического анализа, которое возникает при решении задач", VideoUrl = "/lessonResources/lessonVideo.mp4", Description = "понятие интеграла;\nзамена переменной при интегрировании и интегрирование по частям;\nприменение определённых интегралов в геометрии и физике;\nдифференциальные уравнения.", Lecture = "Lorem Ipsum - это текст-\"рыба\", часто используемый в печати и вэб-дизайне. Lorem Ipsum является стандартной \"рыбой\" для текстов на латинице с начала XVI века. В то время некий безымянный печатник создал большую коллекцию размеров и форм шрифтов, используя Lorem Ipsum для распечатки образцов. Lorem Ipsum не только успешно пережил без заметных изменений пять веков, но и перешагнул в электронный дизайн.", SubjectId = 3, RequiredLevel = 7, LessonLevel = 8 },
            new Lesson() { Id = 9, Title = "Integrals", SubTitle = "Интеграл — одно из важнейших понятий математического анализа, которое возникает при решении задач", VideoUrl = "/lessonResources/lessonVideo.mp4", Description = "понятие интеграла;\nзамена переменной при интегрировании и интегрирование по частям;\nприменение определённых интегралов в геометрии и физике;\nдифференциальные уравнения.", Lecture = "Lorem Ipsum - это текст-\"рыба\", часто используемый в печати и вэб-дизайне. Lorem Ipsum является стандартной \"рыбой\" для текстов на латинице с начала XVI века. В то время некий безымянный печатник создал большую коллекцию размеров и форм шрифтов, используя Lorem Ipsum для распечатки образцов. Lorem Ipsum не только успешно пережил без заметных изменений пять веков, но и перешагнул в электронный дизайн.", SubjectId = 3, RequiredLevel = 8, LessonLevel = 9 },
            new Lesson() { Id = 10, Title = "Integrals", SubTitle = "Интеграл — одно из важнейших понятий математического анализа, которое возникает при решении задач", VideoUrl = "/lessonResources/lessonVideo.mp4", Description = "понятие интеграла;\nзамена переменной при интегрировании и интегрирование по частям;\nприменение определённых интегралов в геометрии и физике;\nдифференциальные уравнения.", Lecture = "Lorem Ipsum - это текст-\"рыба\", часто используемый в печати и вэб-дизайне. Lorem Ipsum является стандартной \"рыбой\" для текстов на латинице с начала XVI века. В то время некий безымянный печатник создал большую коллекцию размеров и форм шрифтов, используя Lorem Ipsum для распечатки образцов. Lorem Ipsum не только успешно пережил без заметных изменений пять веков, но и перешагнул в электронный дизайн.", SubjectId = 3, RequiredLevel = 9, LessonLevel = 10 },
            new Lesson() { Id = 11, Title = "Integrals", SubTitle = "Интеграл — одно из важнейших понятий математического анализа, которое возникает при решении задач", VideoUrl = "/lessonResources/lessonVideo.mp4", Description = "понятие интеграла;\nзамена переменной при интегрировании и интегрирование по частям;\nприменение определённых интегралов в геометрии и физике;\nдифференциальные уравнения.", Lecture = "Lorem Ipsum - это текст-\"рыба\", часто используемый в печати и вэб-дизайне. Lorem Ipsum является стандартной \"рыбой\" для текстов на латинице с начала XVI века. В то время некий безымянный печатник создал большую коллекцию размеров и форм шрифтов, используя Lorem Ipsum для распечатки образцов. Lorem Ipsum не только успешно пережил без заметных изменений пять веков, но и перешагнул в электронный дизайн.", SubjectId = 3, RequiredLevel = 10, LessonLevel = 11 }
            );
        _modelBuilder.Entity<LessonModule>().HasData(
        new LessonModule
        {
            Id = 1,
            Title = "Основные формулы",
            Content = """
            1. ∫a dx = ax + C 2. ∫x^n dx = (x^(n+1))/(n+1) + C

            3. ∫e^x dx = e^x + C

            4. ∫sin(x) dx = -cos(x) + C

            5. ∫cos(x) dx = sin(x) + C
            """,
            LessonId = 1
        },
        new LessonModule
        {
            Id = 2,
            Title = "Основные формулы",
            Content = "<p>1. ∫a dx = ax + C</p><p>2. ∫x^n dx = (x^(n+1))/(n+1) + C</p><p>3. ∫e^x dx = e^x + C</p><p>4. ∫sin(x) dx = -cos(x) + C</p><p>5. ∫cos(x) dx = sin(x) + C</p>",
            LessonId = 1
        },
        new LessonModule
        {
            Id = 3,
            Title = "Основные формулы",
            Content = "<p>1. ∫a dx = ax + C</p><p>2. ∫x^n dx = (x^(n+1))/(n+1) + C</p><p>3. ∫e^x dx = e^x + C</p><p>4. ∫sin(x) dx = -cos(x) + C</p><p>5. ∫cos(x) dx = sin(x) + C</p>",
            PhotoUrl = "/studyImages/Screenshot_1.png",
            LessonId = 1
        }
    );
    }
}