using System.Text.RegularExpressions;

namespace AutomationQaAssignment.Helpers
{
    public class TextHelper
    {
        public static string Normalize(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            var normalized = text.ToLowerInvariant();
            normalized = Regex.Replace(normalized, @"\[[^\]]*\]", " ");
            normalized = Regex.Replace(normalized, "[^a-z0-9\\s]", " ");
            normalized = Regex.Replace(normalized, "\\s+", " ").Trim();

            return normalized;
        }

        public static int CountUniqWords(string text)
        {
            var normalizes = Normalize(text);

            if (string.IsNullOrWhiteSpace(normalizes)) return 0;

            return normalizes.Split(' ', StringSplitOptions.RemoveEmptyEntries).Distinct().Count();
        }
    }
}