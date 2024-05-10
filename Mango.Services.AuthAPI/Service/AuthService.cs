using AutoMapper;
using Mango.Services.AuthAPI.Data;
using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Models.Dto;
using Mango.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace Mango.Services.AuthAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(AppDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IJwtTokenGenerator jwtTokenGenerator)
        {
             _db = db;
            _userManager= userManager;
            _roleManager= roleManager;
            _jwtTokenGenerator= jwtTokenGenerator;
        }

        public async Task<bool> AssignRole(string email, string roleName)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
            if (user != null)
            {
                if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                {
                    //create role if it does not exist
                    _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
                }
                await _userManager.AddToRoleAsync(user, roleName);

                return true;
            }
            return false;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDTO loginRequestDTO)
        {
            var user= _db.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDTO.Name.ToLower());
            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);

            if (user == null || !isValid)
            {
                return new LoginResponseDto() { User = null, Token = string.Empty };
            }

            //if user was found , Generate JWT Token
            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtTokenGenerator.GenerateToken(user, roles);

            UserDTO userDTO = new()
            {
                Email = user.Email,
                Id = user.Id,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber
            };

            LoginResponseDto loginResponseDto = new LoginResponseDto()
            {
                User = userDTO,
                Token = token
            };

            return loginResponseDto;
        }

        public async Task<string> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            //ApplicationUser user= _mapper.Map<ApplicationUser>(registrationRequestDTO);
            ApplicationUser user = new()
            {
                UserName = registrationRequestDTO.Email,
                Email = registrationRequestDTO.Email,
                NormalizedEmail = registrationRequestDTO.Email.ToUpper(),
                Name = registrationRequestDTO.Name,
                PhoneNumber = registrationRequestDTO.PhoneNumber
            };

            try
            {
                var result = await _userManager.CreateAsync(user, registrationRequestDTO.Password);
                if (result.Succeeded)
                {
                    ApplicationUser userToReturn = _db.ApplicationUsers.First(u => u.UserName == registrationRequestDTO.Email);
                    //UserDTO userDTO = _mapper.Map<UserDTO>(userToReturn);
                    UserDTO userDto = new()
                    {
                        Email = userToReturn.Email,
                        Id = userToReturn.Id,
                        Name = userToReturn.Name,
                        PhoneNumber = userToReturn.PhoneNumber
                    };

                    return "";
                }
                else
                {
                    return result.Errors.FirstOrDefault().Description;
                }


            }
            catch (Exception ex)
            {
                
            }
             return "Error Encountered";
           
        }
    }
}
