using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TestApp.WebApi.Handlers.Models;
using TestApp.WebApi.Queries;
using TestApp.WebApi.Services;

namespace TestApp.WebApi.Handlers;

public class ExchangeHandler
{
    [HttpGet]
    public async Task<IResult> Estimate(
        [FromQuery] decimal inputAmount,
        [FromQuery] string inputCurrency,
        [FromQuery] string outputCurrency,
        [FromServices] ExchangeService service,
        [FromServices] IValidator<EstimateQuery> validator,
        [FromServices] IMapper mapper)
    {
        var query = new EstimateQuery
        {
            InputAmount = inputAmount,
            InputCurrency = inputCurrency,
            OutputCurrency = outputCurrency
        };

        var validationResult = validator.Validate(query);

        if (!validationResult.IsValid)
            return Results.BadRequest(string.Join("\n", validationResult.Errors));

        var estimate = await service.EstimateAsync(query);

        var result = mapper.Map<EstimateDto>(estimate);
        
        return Results.Ok(result);
    }


    [HttpGet]
    public async Task<IResult> GetRates(
        [FromQuery] string baseCurrency,
        [FromQuery] string quoteCurrency,
        [FromServices] ExchangeService service,
        [FromServices] IValidator<GetRatesQuery> validator,
        [FromServices] IMapper mapper
        )
    {
        var query = new GetRatesQuery
        {
            BaseCurrency = baseCurrency,
            QuoteCurrency = quoteCurrency
        };
        
        var validationResult = validator.Validate(query);
        if (!validationResult.IsValid)
            return Results.BadRequest(string.Join("\n", validationResult.Errors));

        var rates = await service.GetRatesAsync(query);

        var result = mapper.Map<List<RateDto>>(rates);

        return Results.Ok(result);
    }
}