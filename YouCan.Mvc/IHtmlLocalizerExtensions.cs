using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Localization;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;

namespace Microsoft.AspNetCore.Mvc.Localization;

public static class IHtmlLocalizerExtensions
{
    public static LocalizedHtmlString Links(this IHtmlLocalizer localizer, string text, params Func<string, IHtmlContent>[] links)
    {
        var index = 0;
        var localized = Regex.Replace(localizer[text].Value, "<(.+?)>", match =>
        {
            var linkText = match.Groups[1].Value;
            var link = links[index++](linkText);
            return GetHtml(link);
        });
        return new LocalizedHtmlString(text, localized);
    }

    private static string GetHtml(IHtmlContent content)
    {
        using var writer = new System.IO.StringWriter();
        content.WriteTo(writer, HtmlEncoder.Default);
        return writer.ToString();
    }
}