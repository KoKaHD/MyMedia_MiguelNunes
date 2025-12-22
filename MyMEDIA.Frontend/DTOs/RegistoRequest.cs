using System.ComponentModel.DataAnnotations;

namespace MyMEDIA.Frontend.DTOs
{
    public class RegistoRequest
    {
        [Required]
        public string NomeCompleto { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare(nameof(Password))]
        public string ConfirmaPassword { get; set; } = string.Empty;
    }
}