using AutoMapper;
using LightLib.Data.Models.Assets;
using LightLib.Models.DTOs;

namespace LightLib.Data.Mapping {
    /// <summary>
    /// AutoMapper Mapping Profile for de/serializing entity models to DTOs
    /// </summary>
    public class EntityMappingProfile : Profile {
        public EntityMappingProfile() {
            CreateMap<Book, BookDto>().ReverseMap();
        }
    }
}