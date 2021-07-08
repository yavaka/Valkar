namespace Infrastructure.Models
{
    using System;  
    using Microsoft.AspNetCore.Identity;

    public class User : IdentityUser
    {
        public DateTime RegisteredOn { get; set; }
        public Driver Driver{ get; set; }
    }
}
