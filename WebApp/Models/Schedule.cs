using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApp.Data;

namespace WebApp.Models
{
    public class Schedule
    {
        [Key]
        public int ScheduleId { get; set; }

        [ForeignKey("Zone")]
        public int ZoneId { get; set; }
        public Zone? Zone { get; set; }

       // [Required]
        public byte DaysOfWeek { get; set; } // битовая маска: 1=пн,2=вт…

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        public bool IsEnabled { get; set; } = true;
    }
}
