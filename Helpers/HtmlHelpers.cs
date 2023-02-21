using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PIN_Projekt.Helpers
{
    public static class HtmlHelpers
    {
        public static IHtmlContent Currency(this IHtmlHelper htmlHelper, decimal value)
        {
            return new HtmlString(value.ToString("C"));
        }
    }
}