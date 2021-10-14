namespace ApplicationCore.ServiceModels.WorkingDay
{
    using System;
    using System.ComponentModel;

    public class WorkingDayServiceModel
    {
        public string DriverId { get; set; }
        
        public DateTime Date { get; set; } = DateTime.Now.Date;
        
        [DisplayName("Time In")]
        public TimeSpan TimeIn { get; set; }
        
        public TimeSpan Break { get; set; }
        
        [DisplayName("Time Out")]
        public TimeSpan TimeOut { get; set; }
        
        [DisplayName("Total Hours")]
        public TimeSpan TotalHours { get; set; }

        /// <summary>
        /// Calculate total hours and set the result to TotalHours prop
        /// </summary>
        public void CalculateTotlaHours() 
        {
            if (this.TimeIn != default && this.TimeOut != default)
            {
                this.TotalHours = this.TimeOut.Subtract(this.TimeIn).Subtract(this.Break);
            }
            else
            {
                throw new Exception("Time in and out are with default values!");
            }
        }
    }
}
