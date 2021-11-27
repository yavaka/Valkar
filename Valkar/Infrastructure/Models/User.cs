namespace Infrastructure.Models
{
    using System;  
    using Microsoft.AspNetCore.Identity;

    public class User : IdentityUser
    {
        public User()
        {
            this.RegisteredOn = DateTime.Now;
            this.IsCompleted = false;
        }

        public DateTime RegisteredOn { get; set; }
        public bool IsCompleted { get; set; }
        public Driver Driver { get; set; } = new Driver();
    }
}
