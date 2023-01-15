using FluentValidation;
using TestApp.WebApi.Queries;

namespace TestApp.WebApi.Validators;
public class GetRatesQueryValidator : AbstractValidator<GetRatesQuery>
{
    public GetRatesQueryValidator()
    {
        RuleFor(query => query.BaseCurrency)
            .NotEmpty()
            .WithMessage("Base currency must be not empty");

        RuleFor(query => query.QuoteCurrency)
            .NotEmpty()
            .WithMessage("Quote currency must be not empty");
    }
}
