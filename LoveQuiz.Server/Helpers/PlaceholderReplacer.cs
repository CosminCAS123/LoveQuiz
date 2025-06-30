namespace LoveQuiz.Server.Helpers
{
    public static class PlaceholderReplacer
    {
        public static string ApplyGenderPlaceholders(string text, string gender)
        {
            if (string.IsNullOrWhiteSpace(text)) return text;

            return gender.ToLower() switch
            {
                "male" => text
                    .Replace("{{n}}", "ul")
                    .Replace("{{g}}", "l")
                    .Replace("{{a}}", "t")
                    .Replace("{{p}}", ""),

                "female" => text
                    .Replace("{{n}}", "a")
                    .Replace("{{g}}", "a")
                    .Replace("{{a}}", "ț")
                    .Replace("{{p}}", "ă"),

                _ => text
            };
        }
    }
}
