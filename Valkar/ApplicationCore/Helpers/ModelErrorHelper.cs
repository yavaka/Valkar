namespace ApplicationCore.Helpers
{
    using System.Collections.Generic;
    
    public static class ModelErrorHelper
    {
        public static ICollection<string> ModelErrors { get; set; } = new List<string>();
    }
}
