using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface IAuthService
    {
        Task<ResponseDto?> LoginAsync(LoginRequestDTO loginRequestDTO);
        Task<ResponseDto?> RegisterAsync(RegistrationRequestDTO regRequestDTO);
        Task<ResponseDto?> AssignRoleAsync(RegistrationRequestDTO regRequestDTOs);
    }
}
