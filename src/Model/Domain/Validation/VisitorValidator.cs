using FluentValidation;
using FluentValidation.Validators;

namespace Model.Domain.Validation;

public sealed class VisitorValidator : AbstractValidator<Visitor>
{
    public VisitorValidator()
    {
        RuleFor(visitor => visitor.Name)
            .NotEmpty()
            .MaximumLength(250);

        RuleFor(visitor => visitor.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(visitor => visitor.Phone)
            .NotEmpty()
            .Matches(@"^(\+7|8)\d{10}$");
    }
}