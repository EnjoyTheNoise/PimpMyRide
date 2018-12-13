using System.ComponentModel.DataAnnotations;

namespace PimpMyRide.Core.Data.Models
{
    public class EngineType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Type { get; set; }
    }
}
