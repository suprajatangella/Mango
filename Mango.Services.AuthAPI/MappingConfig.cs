using AutoMapper;
using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Models.Dto;


namespace Mango.Services.AuthAPI
{
    public class MappingConfig
    {
        public MappingConfig()
        {
                
        }

        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<RegistrationRequestDTO, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.NormalizedEmail, opt => opt.MapFrom(src => src.Email.ToUpper()))
                .ForMember(dest => dest.PhoneNumberConfirmed, opt => opt.Ignore()); 
                // .ForMember(dest => dest., opt => opt.Ignore());


                config.CreateMap<ApplicationUser, UserDTO>();
            });
            return mappingConfig;    
        }
    }
}
