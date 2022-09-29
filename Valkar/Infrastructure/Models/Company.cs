namespace Infrastructure.Models
{
    using System;
    
    public class Company
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string OfficeAddress { get; set; }
        public string OfficePostCode { get; set; }
        public string RegistrationNumber { get; set; }
    }
}
