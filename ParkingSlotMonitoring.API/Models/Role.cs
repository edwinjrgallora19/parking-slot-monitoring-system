using System.ComponentModel.DataAnnotations;

namespace ParkingSlotMonitoring.API.Models
{
    public class Role
    {
        [Key]
        public int role_id { get; set; }
        public string role_name { get; set; }
    }
}
