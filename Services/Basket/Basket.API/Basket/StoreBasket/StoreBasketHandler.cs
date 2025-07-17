
using DicountGrpc;

namespace Basket.API.Basket.StoreBasket
{
    public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;
    public record StoreBasketResult(string UserName);

    public class StoreBasketCommandValidator :AbstractValidator<StoreBasketCommand>
    {
        public StoreBasketCommandValidator()
        {
            RuleFor(x => x.Cart).NotNull().WithMessage("Cart cannot be null.");
            RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("UserName cannot be empty.");
        }
    }

    public class StoreBasketCommandHandler(IBasketRepository reop,DiscountProtoService.DiscountProtoServiceClient discountProto) : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
        {

            await DeductDiscounts(command.Cart, cancellationToken);

            await reop.StoreBasketAsync(command.Cart, cancellationToken);   

            return new StoreBasketResult(command.Cart.UserName);
        }

        private async Task DeductDiscounts(ShoppingCart cart, CancellationToken cancellationToken)
        {
            // This method is not used in the current implementation but can be used for future enhancements.
            foreach (var item in cart.Items)
            {
                var coupon=await discountProto.GetDiscountAsync(new GetDiscountRequest { ProductName = item.ProductName });
                item.Price -= coupon.Amount;
            }
        }
    }
}
