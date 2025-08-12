namespace Catalog.API.Products.GetProductById
{
    record GetProductByIdResponse(Product Product);

    public class GetProductByIdEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/{id:guid}", async (Guid Id, ISender sender) =>
            {
              
                    var result = await sender.Send(new GetProductByIdQuery(Id));
                    var response = result.Adapt<GetProductByIdResponse>();
                    return Results.Ok(response);
                
               
            })
            .WithDescription("Get Product By Id")
            .WithTags("Products")
            .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Product By Id");
        }
    }
}
