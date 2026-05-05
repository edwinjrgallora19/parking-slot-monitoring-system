using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingSlotMonitoring.API.Models
{
    [Table("Reservation")]
    public class Reservation
    {
        [Key]
        public int reservation_id { get; set; }
        public int user_id { get; set; }
        public int slot_id { get; set; }
        public DateTime start_time { get; set; }
        public DateTime end_time { get; set; }
        public decimal total_amount { get; set; }
        public string status { get; set; }
    }
}
