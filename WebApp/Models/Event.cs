using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApp.Data;


namespace WebApp.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }

        [ForeignKey("Zone")]
        public int ZoneId { get; set; }
        public Zone Zone { get; set; }

        [Required, StringLength(50)]
        public string EventType { get; set; } // "On", "Off", "Schedule"

        public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.Now;

        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; } // ApplicationUser — ваш класс, унаследованный от IdentityUser
    }
}
