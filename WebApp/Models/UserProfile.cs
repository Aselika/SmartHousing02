using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApp.Data;


namespace WebApp.Models
{
    public class UserProfile
    {
        [Key]
        public int ProfileId { get; set; }

        [Required, ForeignKey("User"), StringLength(450)]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [StringLength(100)]
        public string FirstName { get; set; }

        [StringLength(100)]
        public string LastName { get; set; }

        [Phone, StringLength(20)]
        public string PhoneNumber { get; set; }
    }
}
