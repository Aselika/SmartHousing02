using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class Zone
    {
        [Key]
        public int ZoneId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, StringLength(50)]
        public string Type { get; set; }  // "Lighting", "Barrier", "Camera"

        public bool SupportsScheduling { get; set; }

        [Required, StringLength(50)]
        public string CommandOn { get; set; }

        [Required, StringLength(50)]
        public string CommandOff { get; set; }
    }
}
