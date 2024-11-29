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
                .ForAllMembers(opts => opts.Condition((src, dest, srcValue) => srcValue != null)); //Permet de ne pas override les champs non présent dans le DTO et de perdre des infos

            CreateMap<TypeSalleDTO, TypeSalle>()
                .ReverseMap()
                .ForAllMembers(opts => opts.Condition((src, dest, srcValue) => srcValue != null));
        }
    }


}
