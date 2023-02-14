namespace ApplicationCore.Helpers
{
    using System.Text.RegularExpressions;
    
    public static class ValidationHelper
    {
        public static bool RegexValidation(string fieldValue, string regex) 
            => Regex.Match(fieldValue, regex).Success;
    }
}
