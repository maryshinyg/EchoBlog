using System.ComponentModel.DataAnnotations;

namespace EchoBlog.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        public string? ReturnUrl { get; set; }
    }
}