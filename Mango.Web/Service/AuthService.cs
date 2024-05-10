using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using static Mango.Web.Utility.SD;

namespace Mango.Web.Service
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService _baseService;
        public AuthService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDto?> AssignRoleAsync(RegistrationRequestDTO regRequestDTOs)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,

                Url = SD.AuthAPIBase + "/api/auth/AssignRole",

                Data = regRequestDTOs
            });
        }

        public async Task<ResponseDto?> LoginAsync(LoginRequestDTO loginRequestDTO)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,

                Url = SD.AuthAPIBase + "/api/auth/login",

                Data = loginRequestDTO
            }, withBearer: false);
        }

        public async Task<ResponseDto?> RegisterAsync(RegistrationRequestDTO regRequestDTO)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,

                Url = SD.AuthAPIBase + "/api/auth/register",

                Data = regRequestDTO
            }, withBearer: false);
        }
    }
}
