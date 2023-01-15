using AutoMapper;
using TestApp.Domain.Models;
using TestApp.WebApi.Handlers.Models;

namespace TestApp.WebApi.Configuration.MappingProfiles;

public class GetRatesProfile : Profile
{
    public GetRatesProfile()
    {
        CreateMap<Rate, RateDto>()
            .ReverseMap();
    }
}
