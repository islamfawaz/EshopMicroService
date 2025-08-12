using FluentValidation;

namespace Ordering.Application.Order.Commands.CreateOrder
{
    public record class CreateOrderCommand(OrderDto Order) 
        :ICommand<CreateOrderResult>;

    public record CreateOrderResult(Guid Id);

    public class CreateOrderCommandValidator:AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x=>x.Order.OrderName).NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.Order.CustomerId).NotNull().WithMessage("Customer Id is required.");
            RuleFor(x => x.Order.OrderItems).NotEmpty().WithMessage("Order Items shoudnt be empty.");
        }
    }


}
