using System.ComponentModel.DataAnnotations;

namespace PIN_Projekt.Models
{
    public class ReservationViewModel
    {
        public int Id { get; set; }
        public string VehicleModel { get; set; }
        public string VehicleMake { get; set; }
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        public string Email { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
