using AutoMapper;
using SAE_APIGestion.Models.DTO;
using SAE_APIGestion.Models.EntityFramework;

namespace SAE_APIGestion.Models.Mapper
{
    public class MapperProfile : Profile
    {

        public MapperProfile() {
            CreateMap<SalleDTO, Salle>()
                .ReverseMap()
                .ForMember(dest => dest.TypeSalle, opt => opt.MapFrom(src => src.TypeSalle));

            CreateMap<TypeSalleDTO, TypeSalle>().ReverseMap();
        }
    }


}
