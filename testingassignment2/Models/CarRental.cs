using System;
using System.ComponentModel.DataAnnotations;

namespace testingassignment2.Models
{
    public class CarRental
    {
        [Display(Name = "Car")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a car.")]
        public int CarId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Full Name")]
        public string CustomerName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string CustomerEmail { get; set; }

        [Phone]
        [Display(Name = "Contact Number")]
        public string CustomerPhone { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Pickup Date")]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Return Date")]
        public DateTime EndDate { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Pickup Location")]
        public string PickupLocation { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Drop-off Location")]
        public string DropoffLocation { get; set; }

        [StringLength(500)]
        [Display(Name = "Additional Notes")]
        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }
    }
}
