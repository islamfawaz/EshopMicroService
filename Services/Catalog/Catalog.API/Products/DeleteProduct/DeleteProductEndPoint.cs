
namespace Catalog.API.Products.DeleteProduct
{
  //  public record DeleteProductRequest(Guid Id) : ICommand<DeleteProductResponse>;
    public record DeleteProductResponse(bool IsSuccess);
    public class DeleteProductEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/products/{id}", async (Guid Id, ISender sender) =>
            {
                var result =await sender.Send(new DeleteProductCommand(Id));
                var response=result.Adapt<DeleteProductResponse>();
                return Results.Ok(response);
            }).WithDescription("Delete Product")
              .WithTags("Products")
              .Produces<DeleteProductResponse>(StatusCodes.Status200OK)
              .ProducesProblem(StatusCodes.Status400BadRequest)
              .WithSummary("Delete Product");

        }
    }
}
