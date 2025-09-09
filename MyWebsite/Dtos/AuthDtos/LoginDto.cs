using MyWebsite.Entities;
using System.ComponentModel.DataAnnotations;

namespace MyWebsite.Dtos.AuthDtos
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Kullanıcı adı boş olamaz.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Parola boş olamaz.")]
        public string Password { get; set; }
    }
} 
