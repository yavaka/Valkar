namespace Infrastructure.Models
{
    using System;
    using Infrastructure.Common.Enums;

    public class Driver
    {
        public Guid Id { get; set; }
        public Titles Title { get; set; }
        public string FirstNames { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }
        public string Postcode { get; set; }
        public string PhoneNumber { get; set; }
        public string NationalInsuranceNumber { get; set; }
        public EmergencyContact EmergencyContact { get; set; }
        public LimitedCompany LimitedCompany { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}
