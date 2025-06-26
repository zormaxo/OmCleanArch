using CleanArch.Application.Products.GetProductById;
using CleanArch.Application.Products.GetProducts;
using CleanArch.Web.Extensions;

namespace CleanArch.Web.Endpoints;

public class Products : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this).MapGet(GetProducts).MapGet(GetProductById, "{id}");
    }

    public async Task<IResult> GetProducts(ISender sender)
    {
        var result = await sender.Send(new GetProductsQuery());
        return result.Match(Results.Ok, CustomResults.Problem);
    }

    public async Task<IResult> GetProductById(ISender sender, int id)
    {
        var result = await sender.Send(new GetProductByIdQuery(id));
        return result.Match(Results.Ok, CustomResults.Problem);
    }
}
