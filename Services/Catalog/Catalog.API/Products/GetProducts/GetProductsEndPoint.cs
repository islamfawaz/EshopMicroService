
namespace Catalog.API.Products.GetProducts
{
    record GetProductRequest(int ? pageNumber = 1, int ? pageSize = 10);
    record GetProductResponse(IEnumerable<Product> Products);
    public class GetProductsEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products", async (HttpContext context,ISender sender) =>
            {
                int pagedNumber=context.Request.Query.ContainsKey("pageNumber") ? int.Parse(context.Request.Query["pageNumber"]!) : 1;

                int pagedSize=context.Request.Query.ContainsKey("pageSize") ? int.Parse(context.Request.Query["pageSize"]!) : 10;

                var query=new GetProductsQuery(pagedNumber, pagedSize); 
                var result =await sender.Send(query);
                var response = result.Adapt<GetProductResponse>();

                return Results.Ok(response);
            }).WithDescription("Get Products")
              .WithTags("Products")
              .Produces<GetProductResponse>(StatusCodes.Status200OK)
              .ProducesProblem(StatusCodes.Status400BadRequest)
              .WithSummary("Get Products");
        }
    }
}
