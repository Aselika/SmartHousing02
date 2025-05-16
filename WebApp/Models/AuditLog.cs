using System;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class AuditLog
    {
        [Key]
        public int AuditLogId { get; set; }

        [Required, StringLength(50)]
        public string EntityName { get; set; }  // "Zone", "Schedule"...

        [Required, StringLength(50)]
        public string EntityId { get; set; }    // например, "3"

        [Required, StringLength(20)]
        public string Action { get; set; }      // "Create", "Update", "Delete"

        public string ChangedBy { get; set; }   // UserId из AspNetUsers

        public DateTimeOffset ChangedAt { get; set; } = DateTimeOffset.Now;

        public string Changes { get; set; }     // JSON: старое→новое
    }
}
