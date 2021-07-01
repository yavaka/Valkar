namespace Valkar.Infrastructure.Identity
{
    using System;
    using Microsoft.AspNetCore.Identity;
    using Valkar.Application.Interfaces.Identity;

    public class User : IdentityUser, IUser
    {
        public DateTime RegisterOn { get; set; }
    }
}
