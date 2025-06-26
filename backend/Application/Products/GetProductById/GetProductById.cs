using CleanArch.Application.Common.Interfaces;
using CleanArch.Application.Common.Models;
using CleanArch.Domain.Products;

namespace CleanArch.Application.Products.GetProductById;

public record GetProductByIdQuery(int id) : IRequest<Result<Product>> { }

public class GetProductByIdQueryValidator : AbstractValidator<GetProductByIdQuery>
{
    public GetProductByIdQueryValidator() { }
}

public class GetProductByIdQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetProductByIdQuery, Result<Product>>
{
    public async Task<Result<Product>> Handle(
        GetProductByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var product = await context.Products.FindAsync(
            [request.id],
            cancellationToken: cancellationToken
        );

        if (product is null)
        {
            return Result.Failure<Product>(ProductErrors.NotFound(request.id));
        }

        return product;
    }
}
