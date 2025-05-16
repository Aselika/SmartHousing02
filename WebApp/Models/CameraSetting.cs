using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class CameraSetting
    {
        [Key]
        public int CameraId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required]
        public int DeviceIndex { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
