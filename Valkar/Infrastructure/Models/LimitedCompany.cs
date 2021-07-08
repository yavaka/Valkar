namespace Infrastructure.Models
{
    using System;
    
    public class LimitedCompany
    {
        public Guid Id { get; set; }
        public string CompanyName { get; set; }
        public string CompanyRegistrationNumber { get; set; }
        public Guid OwnerId { get; set; }
        public Driver Owner { get; set; }
    }
}
