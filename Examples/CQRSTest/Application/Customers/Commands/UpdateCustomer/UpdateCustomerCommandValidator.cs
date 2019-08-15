using FluentValidation;

namespace CQRSTest.Application.Customers.Commands.UpdateCustomer
{
    public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
    {
        public UpdateCustomerCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().GreaterThanOrEqualTo(0);
            RuleFor(x => x.Name).MaximumLength(60);
            RuleFor(x => x.Address).MaximumLength(60);
        }
    }
}
