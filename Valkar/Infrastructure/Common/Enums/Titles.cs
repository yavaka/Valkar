namespace Infrastructure.Common.Enums
{
    using System.ComponentModel;

    public enum Titles
    {
        [Description("Please select")]
        None = 0,
        [Description("Mr")]
        Mr = 1,
        [Description("Ms")]
        Ms = 2,
        [Description("Mrs")]
        Mrs = 3,
        [Description("Miss")]
        Miss = 4
    }
}
