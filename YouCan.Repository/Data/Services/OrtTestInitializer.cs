using Microsoft.EntityFrameworkCore;
using YouCan.Entities;

namespace YouCan.Services;

public class OrtTestInitializer
{
    private ModelBuilder _modelBuilder;

    public OrtTestInitializer(ModelBuilder modelBuilder)
    {
        _modelBuilder = modelBuilder;
    }

    public void Seed()
    {
        _modelBuilder.Entity<OrtTest>().HasData(
            new OrtTest(){Id = 1, OrtLevel = 1, Language = "ru"},
            new OrtTest(){Id = 2, OrtLevel = 2, Language = "ru"},
            new OrtTest(){Id = 3, OrtLevel = 3, Language = "ru"},
            new OrtTest(){Id = 4, OrtLevel = 4, Language = "ru"},
            new OrtTest(){Id = 5, OrtLevel = 5, Language = "ru"},
            new OrtTest(){Id = 6, OrtLevel = 6, Language = "ru"},
            new OrtTest(){Id = 7, OrtLevel = 7, Language = "ru"},
            new OrtTest(){Id = 8, OrtLevel = 8, Language = "ru"},
            new OrtTest(){Id = 9, OrtLevel = 9, Language = "ru"}
        );
        
    }
}