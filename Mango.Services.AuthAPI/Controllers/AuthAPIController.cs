using Mango.Services.AuthAPI.Models.Dto;
using Mango.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService _authService;
        protected ResponseDto _response;

        public AuthAPIController(IAuthService authService)
        {
            _authService=authService;
            _response = new();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDTO registrationRequestDTO)
        {
            var errorMessage = await _authService.Register(registrationRequestDTO);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                _response.IsSuccess = false;
                _response.Message = errorMessage;

                return BadRequest(_response);
            }
            return Ok(_response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginRequestDTO loginRequestDTO)
        {
            var loginResponse = await _authService.Login(loginRequestDTO);
            if(loginResponse.User == null)
            {
                _response.IsSuccess=false;
                _response.Message = "User name or Password is incorrect";
                return BadRequest(_response);
            }
            _response.Result = loginResponse;
            return Ok(_response);
        }

        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDTO regRequestDTO)
        {
            var assignRoleSuccess = await _authService.AssignRole(regRequestDTO.Email, regRequestDTO.RoleName.ToUpper());
            if (!assignRoleSuccess)
            {
                _response.IsSuccess = false;
                _response.Message = "Error Encountered";
                return BadRequest(_response);
            }
            return Ok(_response);
        }
    }
}
