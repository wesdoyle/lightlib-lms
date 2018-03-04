using System.ComponentModel.DataAnnotations;

namespace Library.Data.Models
{
    public class Status
    {
        public int Id { get; set; }

        [Required] public string Name { get; set; }

        [Required] public string Description { get; set; }
    }
}