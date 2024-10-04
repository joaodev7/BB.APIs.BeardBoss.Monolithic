using BB.APIs.BeardBoss.Monolithic.Models.Enums;

namespace BB.APIs.BeardBoss.Monolithic.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public ServiceType ServiceType { get; set; }
        public DateTime AppointmentDate { get; set; }
        public bool IsConfirmed { get; set; }
    }
}
