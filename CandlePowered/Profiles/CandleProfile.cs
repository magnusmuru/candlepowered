using AutoMapper;
using CandlePowered.Dtos;
using CandlePowered.Entities;

namespace CandlePowered.Profiles;

public class CandleProfile : Profile
{
    public CandleProfile()
    {
        CreateMap<WebsocketDto, CandleData>();
    }
}