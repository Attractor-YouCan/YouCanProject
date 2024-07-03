using Microsoft.AspNetCore.Mvc.Razor;

namespace YouCan.Services;

// Сервис который указывает путь до вьюшек для контроллеров админки
public class AdminViewLocationExpander : IViewLocationExpander
{
    public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
    {
        if (context.ControllerName.StartsWith("Admin"))
        {
            return new[]
            {
                "/Views/Admin/{1}/{0}.cshtml",
                "/Views/Shared/{0}.cshtml"
            };
        }
        return viewLocations;
    }

    public void PopulateValues(ViewLocationExpanderContext context)
    {
        
    }
}
