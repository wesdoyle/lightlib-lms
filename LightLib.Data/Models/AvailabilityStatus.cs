using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LightLib.Data.Models {
    [Table("availability_statuses")]
    public class AvailabilityStatus {
        public int Id { get; set; }

        [Required] 
        public string Name { get; set; }

        [Required] 
        public string Description { get; set; }
    }
}