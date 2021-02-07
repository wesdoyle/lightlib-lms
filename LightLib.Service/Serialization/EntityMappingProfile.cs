using AutoMapper;
using LightLib.Data.Models;
using LightLib.Data.Models.Assets;
using LightLib.Models.DTOs;
using LightLib.Models.DTOs.Assets;

namespace LightLib.Service.Serialization {
    public class EntityMappingProfile : Profile {
        public EntityMappingProfile() {
            CreateMap<Asset, LibraryAssetDto>().ReverseMap();
            CreateMap<Book, BookDto>().ReverseMap();
            CreateMap<DVD, DvdDto>().ReverseMap();
            CreateMap<AudioCd, AudioCdDto>().ReverseMap();
            CreateMap<AudioBook, AudioBookDto>().ReverseMap();
            CreateMap<Periodical, PeriodicalDto>().ReverseMap();
            CreateMap<AvailabilityStatus, StatusDto>().ReverseMap();
            CreateMap<LibraryBranch, LibraryBranchDto>().ReverseMap();
            CreateMap<Patron, PatronDto>().ReverseMap();
            CreateMap<Data.Models.Checkout, CheckoutDto>().ReverseMap();
            CreateMap<CheckoutHistory, CheckoutHistoryDto>().ReverseMap();
            CreateMap<Hold, HoldDto>().ReverseMap();
            CreateMap<LibraryCard, LibraryCardDto>().ReverseMap();
        }
    }
}