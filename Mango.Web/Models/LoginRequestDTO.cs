using System.ComponentModel.DataAnnotations;

namespace Mango.Web.Models
{
    public class LoginRequestDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
