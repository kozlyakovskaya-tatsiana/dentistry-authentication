using Application.Mapster;
using Domain.Entities;

namespace Application.DTO
{
    public class UserDto : BaseDto<User, UserDto>
    {
        public Guid Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public override void AddCustomMappings()
        {
            SetCustomMappings()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Email, src => src.Email)
                .Map(dest => dest.PhoneNumber, src => src.PhoneNumber);
        }
    }
}