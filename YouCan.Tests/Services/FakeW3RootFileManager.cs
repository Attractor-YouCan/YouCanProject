using Microsoft.AspNetCore.Http;
using YouCan.Services;

namespace YouCan.Tests.Services;

public class FakeW3RootFileManager : W3RootFileManager
{
    public FakeW3RootFileManager() : base(null) { }

    public  Task<string> SaveFormFileAsync(string localDirectory, IFormFile file)
    {
        // Возвращаем фиктивный путь для тестов
        return Task.FromResult("fake_path");
    }

    public  bool DeleteFile(string localPath)
    {
        // Возвращаем фиктивный результат
        return true;
    }
}