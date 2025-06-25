
using Microsoft.AspNetCore.Http.HttpResults;

namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductRequest(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price) : ICommand<UpdateProductResponse>;
    public record UpdateProductResponse(bool IsUpdated);


    public class UpdateProductEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/products", async (UpdateProductRequest request, ISender sender) =>
            {
                var command = request.Adapt<UpdateProductCommand>();
                var result =await sender.Send(command);
                var response=result.Adapt<UpdateProductResponse>();

                return Results.Ok(response);
            })
              .WithDisplayName("UpdateProduct")
              .WithTags("Products")
              .Produces<UpdateProductResponse>(StatusCodes.Status200OK)
              .ProducesProblem(StatusCodes.Status400BadRequest)
              .WithSummary("Update Product");
        }
    }
}
