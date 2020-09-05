using System.ComponentModel.DataAnnotations;

namespace SalonAPI.Contracts.V1.Requests
{
    public class AuthenticateRequest
    {
        [Required]
        public string Username { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}