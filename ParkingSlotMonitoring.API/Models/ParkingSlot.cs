using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingSlotMonitoring.API.Models
{
    [Table("ParkingSlot")]
    public class ParkingSlot
    {
        [Key]
        public int slot_id { get; set; }
        public int area_id { get; set; }
        public string slot_number { get; set; }
        public int row_number { get; set; }
        public int column_number { get; set; }
        public string status { get; set; }
    }
}