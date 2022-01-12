using System;

namespace CalibrationApp.Models
{
    public class Calibration
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string ContactType { get; set; }
        public string ContactId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
