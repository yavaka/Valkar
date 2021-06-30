namespace Valkar.Domain.Models
{
    using System;
    using Microsoft.AspNetCore.Identity;

    public class User : IdentityUser
    {
        public User()
        {
            this.FirstName = default!;
            this.LastName = default!;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{this.FirstName} {this.LastName}"; 
        public DateTime DateOfBirth { get; set; }
        public DateTime RegisteredOn{ get; set; }
    }
}
