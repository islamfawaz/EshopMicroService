
namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price):ICommand<CreateProductResult>;

    public record CreateProductResult(Guid Id);

    // This class implements IValidator<CreateProductCommand> so it trigger first 
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Category).NotEmpty().WithMessage("Category is required");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required");
            RuleFor(x => x.ImageFile).NotEmpty().WithMessage("ImageFile is required");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than zero");
        }

    }
    internal class CreateProductCommandHandler(ILogger<CreateProductCommandHandler> logger, IDocumentSession session) :ICommandHandler<CreateProductCommand,CreateProductResult>
    {
 
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {  
            logger.LogInformation("Createing Product: {Name}", command.Name);
            var product = new Product
            {
                Category = command.Category,
                Description = command.Description,
                ImageFile = command.ImageFile,
                Price = command.Price,
                Name = command.Name

            };

            session.Store(product);
           await session.SaveChangesAsync(cancellationToken);

           return  new CreateProductResult(product.Id);
        }
    }
}
