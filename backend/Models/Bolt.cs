using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Bolt
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Brand { get; set; } = string.Empty;
    }
}
