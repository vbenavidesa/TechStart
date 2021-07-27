using AutoMapper;
using TechStart.Dtos;
using TechStart.Models;

namespace TechStart.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Model to DTO
            CreateMap<Hospital, HospitalDto>();
            CreateMap<Item, ItemDto>();
            CreateMap<ItemVendor, ItemVendorDto>();
            CreateMap<Pharmacy, PharmacyDto>();
            CreateMap<PharmacyInventory, PharmacyInventoryDto>();

            // DTO to Model
            CreateMap<HospitalDto, Hospital>().ForMember(c => c.Id, opt => opt.Ignore());
            CreateMap<ItemDto, Item>().ForMember(c => c.ItemNumber, opt => opt.Ignore());
            CreateMap<ItemVendorDto, ItemVendor>().ForMember(c => c.Id, opt => opt.Ignore());
            CreateMap<PharmacyDto, Pharmacy>().ForMember(c => c.Id, opt => opt.Ignore());
            CreateMap<PharmacyInventoryDto, PharmacyInventory>().ForMember(c => c.Id, opt => opt.Ignore());
        }
    }
}