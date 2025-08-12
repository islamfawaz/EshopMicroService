

namespace Catalog.API.Products.DeleteProduct
{ 
   public record DeleteProductCommand(Guid Id):ICommand<DeleteProductResult>;
    record DeleteProductResult(bool IsSuccess);

    public class DeleteProductCommandValidator :AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");


        }
    }
    internal class DeleteProductHandler(IDocumentSession session) : ICommandHandler<DeleteProductCommand, DeleteProductResult>
    {
        public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            session.Delete<Product>(command.Id);

            await session.SaveChangesAsync(cancellationToken);

            return new DeleteProductResult(true);
        }
    }
}
