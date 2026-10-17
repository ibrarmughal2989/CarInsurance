
using System;
using System.ComponentModel.DataAnnotations;

namespace CarInsurance.Models
{
    public class Insuree
    {
        // Unique ID for each customer
        public int Id { get; set; }

        // Customer's first name
        [Required]
        public string FirstName { get; set; } = "";

        // Customer's last name
        [Required]
        public string LastName { get; set; } = "";

        // Customer's email address
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; } = "";

        // Customer's date of birth
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        // Year of the car
        public int CarYear { get; set; }

        // Car company name
        [Required]
        public string CarMake { get; set; } = "";

        // Car model name
        [Required]
        public string CarModel { get; set; } = "";

        // Has the customer had a DUI?
        public bool DUI { get; set; }

        // Number of speeding tickets
        public int SpeedingTickets { get; set; }

        // True means full coverage
        public bool CoverageType { get; set; }

        // Monthly insurance price
        public decimal Quote { get; set; }
    }
}
