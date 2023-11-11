using AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<HouseEntity, HouseDetailDTO>();
        CreateMap<HouseDetailDTO, HouseEntity>();

        CreateMap<BidDTO, BidEntity>();
        CreateMap<BidDTO, BidEntity>();
    }
}