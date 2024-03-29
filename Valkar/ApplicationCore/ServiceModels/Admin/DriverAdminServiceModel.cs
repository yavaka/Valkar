﻿namespace ApplicationCore.ServiceModels.Admin
{
    using ApplicationCore.ServiceModels.Driver;
    using ApplicationCore.ServiceModels.GoogleDriveAPI;
    using ApplicationCore.ServiceModels.WorkingDay;
    using Infrastructure.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class DriverAdminServiceModel
    {
        /// <summary>
        /// The account id of the driver
        /// </summary>
        public string UserId { get; set; }
        public string Email { get; set; }
        public string DriverId { get; set; }
        public string FirstNames { get; set; }
        public string Surname { get; set; }
        public string FullName => $"{this.FirstNames.Split(' ')[0]} {this.Surname}";
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string Postcode { get; set; }
        public DateTime CreatedOn { get; set; }
        public List<string> DrivingLicenceCategories { get; private set; }
        public string NiNo { get; set; }
        public string LimitedCompany { get; set; }
        public string CompanyRegistrationNumber { get; set; }
        public string GoogleDriveFolderId { get; set; }

        public EmergencyContactServiceModel EmergencyContact { get; set; }
        public ICollection<WorkingDayServiceModel> WorkingDays { get; set; }
        public IEnumerable<GoogleDriveFileServiceModel> Documents { get; set; }

        /// <summary>
        /// Converts driver`s licence category entities to list of categories names
        /// and set DrivingLicenceCategories prop
        /// </summary>
        public void ConvertDrivingLicenceEntitiesToCategoriesName(IEnumerable<LicenceCategory> licenceCategories)
            => this.DrivingLicenceCategories = licenceCategories
                .Select(c => c.Category.ToString())
                .ToList();
    }
}
