using AutoMapper;
using Library.Data.Models;
using Library.Models.DTOs;

namespace Library.Models.Mapping {
    /// <summary>
    /// AutoMapper Mapping Profile for de/serializing entity models to DTOs
    /// </summary>
    public class EntityMappingProfile : Profile {
        public EntityMappingProfile() {
            CreateMap<Book, BookDto>().ReverseMap();
        }
    }
}