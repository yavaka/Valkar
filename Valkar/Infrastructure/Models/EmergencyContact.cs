namespace Infrastructure.Models
{
    using System;
    using Infrastructure.Common.Enums;

    public class EmergencyContact
    {
        public Guid Id { get; set; }
        public Titles Title { get; set; }
        public string FirstNames { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }
        public string Postcode { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Relationship { get; set; }

        public Guid DriverId { get; set; }
        public Driver Driver{ get; set; }
    }
}
