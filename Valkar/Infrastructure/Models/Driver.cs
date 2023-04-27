namespace Infrastructure.Models
{
    using System;
    using System.Collections.Generic;
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
        public DateTime DateOfBirth { get; set; }
        public string NationalInsuranceNumber { get; set; }
        /// <summary>
        /// Shows whether or not the driver filled in all personal details
        /// </summary>
        public bool IsCompleted { get; set; }
        public string GoogleDriveFolderId { get; set; }

        #region Mappings

        public ICollection<LicenceCategory> LicenceCategories { get; set; }
        public ICollection<File> PersonalDocuments { get; set; }
        public EmergencyContact EmergencyContact { get; set; }
        public LimitedCompany LimitedCompany { get; set; }
        public ICollection<WorkingDay> WorkedDays { get; set; }
        public ICollection<TempDocument> ReceivedDocuments { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        
        #endregion
    }
}
