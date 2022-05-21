using System.Text.RegularExpressions;

namespace EmailSender.Extensions
{
    public static class StringExtensions
    {
        public static string StripHtml(this string model)
        {
            return Regex.Replace(model, "<.*?>", string.Empty);
        }
    }
}