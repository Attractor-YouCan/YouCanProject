//
//
// using Microsoft.AspNetCore.Mvc.Rendering;
// using Microsoft.AspNetCore.Mvc.ViewFeatures;
// using Microsoft.AspNetCore.Razor.TagHelpers;
//
// [HtmlTargetElement("a", Attributes = "asp-area, asp-controller, asp-action")]
// public class ActiveTagHelper : TagHelper
// {
//     public string AspArea { get; set; }
//     public string AspController { get; set; }
//     public string AspAction { get; set; }
//
//     [ViewContext]
//     public ViewContext ViewContext { get; set; }
//
//     public override void Process(TagHelperContext context, TagHelperOutput output)
//     {
//         var area = ViewContext.RouteData.Values["area"]?.ToString();
//         var controller = ViewContext.RouteData.Values["controller"].ToString();
//         var action = ViewContext.RouteData.Values["action"].ToString();
//
//         if (string.Equals(AspArea, area, StringComparison.OrdinalIgnoreCase) &&
//             string.Equals(AspController, controller, StringComparison.OrdinalIgnoreCase) &&
//             string.Equals(AspAction, action, StringComparison.OrdinalIgnoreCase))
//         {
//             var existingClass = output.Attributes["class"]?.Value?.ToString();
//             output.Attributes.SetAttribute("class", $"{existingClass} active");
//         }
//     }
// }

using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

[HtmlTargetElement("a", Attributes = "asp-area, asp-controller, asp-action")]
public class ActiveTagHelper : TagHelper
{
    public string AspArea { get; set; }
    public string AspController { get; set; }
    public string AspAction { get; set; }

    [ViewContext]
    public ViewContext ViewContext { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var area = ViewContext.RouteData.Values["area"]?.ToString();
        var controller = ViewContext.RouteData.Values["controller"].ToString();
        var action = ViewContext.RouteData.Values["action"].ToString();

        if (string.IsNullOrEmpty(AspArea))
        {
            AspArea = null;
        }

        if (string.Equals(AspArea, area, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(AspController, controller, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(AspAction, action, StringComparison.OrdinalIgnoreCase))
        {
            var existingClass = output.Attributes["class"]?.Value?.ToString();
            output.Attributes.SetAttribute("class", $"{existingClass} active");
        }
    }
}
