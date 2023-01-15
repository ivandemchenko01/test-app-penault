using AutoMapper;
using TestApp.Domain.Models;
using TestApp.WebApi.Handlers.Models;

namespace TestApp.WebApi.Configuration.MappingProfiles;
public class EstimateProfile : Profile
{
    public EstimateProfile()
    {
        CreateMap<Estimate, EstimateDto>()
            .ReverseMap();
    }
}
