using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingSlotMonitoring.API.Models
{
    [Table("User")]
    public class User
    {
        [Key]
        public int user_id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }

        [ForeignKey("Role")]
        public int role_id { get; set; }

        public Role Role { get; set; }
    }
}
