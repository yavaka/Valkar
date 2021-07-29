namespace ApplicationCore.Helpers.CheckBox
{
    using Infrastructure.Common.Enums;
    using Infrastructure.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class Converter
    {
        public static CheckBoxModel[] GetDrivingLicenceCategoriesAsCheckBoxModels(List<LicenceCategory> licenceCategories = null)
        {
            var drivingLicenseCategories = new List<CheckBoxModel>();

            // Iterate all driving licence enum items
            var categoryValue = 0;
            foreach (var categoryName in Enum.GetNames(typeof(DrivingLicenceCategories)))
            {
                drivingLicenseCategories.Add(new CheckBoxModel
                {
                    Text = categoryName,
                    Value = categoryValue
                });
                categoryValue++;
            }

            if (licenceCategories is not null)
            {
                foreach (var category in licenceCategories)
                {
                    drivingLicenseCategories.FirstOrDefault(n => n.Text == category.Category.ToString()).IsChecked = true;
                }
            }

            return drivingLicenseCategories.ToArray();
        }
    }
}
