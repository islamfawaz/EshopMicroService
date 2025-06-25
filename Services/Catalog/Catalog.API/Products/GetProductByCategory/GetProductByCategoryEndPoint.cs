
namespace Catalog.API.Products.GetProductByCategory
{
    record GetProductByCategoryQueryResponse(IEnumerable<Product> Products);

    public class GetProductByCategoryEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/category/{category}", async (string category, ISender sender) =>
            {
                var result =await sender.Send(new GetProductByCategoryQuery(category));
                var response=result.Adapt<GetProductByCategoryQueryResponse>();

                return Results.Ok(response);  

            })
              .WithDescription("Get Products By Category")
              .WithTags("Products")
              .Produces<GetProductByCategoryQueryResponse>(StatusCodes.Status200OK)
              .ProducesProblem(StatusCodes.Status404NotFound)
              .WithSummary("Get Products By Category");
        }
    }
}
