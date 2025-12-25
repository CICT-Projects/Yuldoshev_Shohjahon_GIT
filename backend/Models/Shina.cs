using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Shina
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Brand { get; set; } = string.Empty;
    }
}
