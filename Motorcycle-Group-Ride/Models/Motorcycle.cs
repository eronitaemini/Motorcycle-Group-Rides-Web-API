using System.ComponentModel.DataAnnotations;

namespace Motorcycle_Group_Ride.Models
{
    public class Motorcycle
    {
        [Key]
        public int MotorcycleId { get; set; } // Primary key

        [Required]
        [MaxLength(50)]
        public string Make { get; set; } // Manufacturer, e.g., Honda, Yamaha

        [Required]
        [MaxLength(50)]
        public string Model { get; set; } // Model, e.g., CBR600RR

        [Required]
        [Range(1900, 2100)]
        public int Year { get; set; } // Year of manufacture

        [Required]
        [MaxLength(50)]
        public string EngineSize { get; set; } // Engine size, e.g., 600cc

        [Required]
        [MaxLength(50)]
        public string RegistrationNumber { get; set; } // Vehicle registration number

        [Required]
        [Range(1, 10)]
        public int SafetyRating { get; set; } // Safety rating, e.g., 1 to 10

        [MaxLength(100)]
        public string Color { get; set; } // Color of the motorcycle

        [MaxLength(200)]
        public string OwnerComments { get; set; } // Additional comments from the owner

        [Required]
        public int UserId { get; set; } // Foreign key to the user who owns the motorcycle

        //   [ForeignKey("UserId")]
        //    public virtual UserProfile UserProfile { get; set; } // Navigation property to the user profile

    }
}
