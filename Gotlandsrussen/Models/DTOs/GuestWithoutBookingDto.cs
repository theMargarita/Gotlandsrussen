using System.ComponentModel.DataAnnotations;

namespace Gotlandsrussen.Models.DTOs
{
    public class GuestWithoutBookingDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string Phone { get; set; }
    }
}
