namespace Infrastructure.Models
{
    using System;
    
    public class WorkingDay
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan TimeIn { get; set; }
        public TimeSpan Break { get; set; }
        public TimeSpan TimeOut { get; set; }
        public TimeSpan TotalHours { get; set; }
        public Guid DriverId { get; set; }
        public Driver Driver { get; set; }
    }
}
