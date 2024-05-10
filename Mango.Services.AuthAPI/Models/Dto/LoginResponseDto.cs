using System.Security;

namespace Mango.Services.AuthAPI.Models.Dto
{
    public class LoginResponseDto
    {
        public UserDTO User { get; set; }
        public string Token {  get; set; } 
    }
}
