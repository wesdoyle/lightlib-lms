using System.ComponentModel.DataAnnotations;

namespace LightLib.Data.Models.Assets {
    public class Tag {
        [Key] public int Id { get; set; }
        public string Name { get; set; }
    }
}