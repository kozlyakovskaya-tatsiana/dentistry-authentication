using Application.Mapster;
using Domain.Entities;

namespace Application.DTO
{
    public class RoleDto : BaseDto<Role, RoleDto>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        
        public override void AddCustomMappings()
        {
            SetCustomMappings()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Name);
        }
    }
}