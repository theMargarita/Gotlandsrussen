using System.ComponentModel.DataAnnotations;

namespace Gotlandsrussen.Models
{
    public class Guest
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string Phone { get; set; }

        // Navigation properties
        public ICollection<Booking> Bookings { get; set; }
    }
}