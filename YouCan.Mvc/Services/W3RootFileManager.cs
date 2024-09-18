namespace YouCan.Mvc.Services;

public class W3RootFileManager(IWebHostEnvironment webHostEnvironment)
{
    private readonly IWebHostEnvironment _env = webHostEnvironment;

    public async Task<string> SaveFormFileAsync(string localDirectory, IFormFile file)
    {
        var fullDirectory = Path.Combine(_env.WebRootPath, localDirectory);
        if (!Directory.Exists(fullDirectory))
        {
            Directory.CreateDirectory(fullDirectory);
        }
        var localPath = Path.Combine(localDirectory, $"{Guid.NewGuid()}.{file.ContentType.Split('/')[1]}");
        var fullPath = Path.Combine(_env.WebRootPath, localPath);
        using (FileStream stream = new(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
        return "/"+localPath;
    }
    public bool DeleteFile(string localPath)
    {
        var fullPath = Path.Combine(_env.WebRootPath, localPath);
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
            return true;
        }
        return false;
    }
}
