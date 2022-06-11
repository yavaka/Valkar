namespace ApplicationCore.Helpers
{
    using System.Text.RegularExpressions;
    
    public static class ValidationHelper
    {
        public static bool RegexValidation(string fieldValue, string regex)
        {
            var match = Regex.Match(fieldValue, regex);
            if (match.Success)
            {
                return true;
            }
            return false;
        }
    }
}
