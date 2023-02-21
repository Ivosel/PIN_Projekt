using System.ComponentModel.DataAnnotations;

namespace PIN_Projekt.Models
{
    public class ReservationCreateViewModel
    {
        public string VehicleType { get; set; }
        public List<string> VehicleTypes { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [Required]          
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
    }
}
