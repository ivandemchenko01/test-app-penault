using FluentValidation;
using TestApp.WebApi.Queries;

namespace TestApp.WebApi.Validators;
public class EstimateQueryValidator : AbstractValidator<EstimateQuery>
{
    public EstimateQueryValidator()
    {
        RuleFor(query => query.InputAmount)
            .NotNull()
            .Must(amount => amount > 0)
            .WithMessage("Input amount must be bigger then 0.");

        RuleFor(query => query.InputCurrency)
            .NotEmpty()
            .WithMessage("Input currency must by not empty");

        RuleFor(query => query.OutputCurrency)
            .NotEmpty()
            .WithMessage("Output currency must be not empty");
    }
}
