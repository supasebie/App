using System.ComponentModel.DataAnnotations;

namespace AppApi.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [StringLength(8, MinimumLength=4, ErrorMessage="Maximum password length is 8 characters, minimum length is 4")]
        public string Password { get; set; }
    }
}