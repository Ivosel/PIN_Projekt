using System.ComponentModel.DataAnnotations;

namespace PIN_Projekt.Models
{
    public class Vehicle
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Model { get; set; }

        [Required]
        public string Make { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Plate { get; set; }
        [Required]
        public decimal DailyRate { get; set; }

    }
}
