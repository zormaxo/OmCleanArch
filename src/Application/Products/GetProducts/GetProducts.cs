using CleanArch.Application.Common.Interfaces;
using CleanArch.Application.Common.Models;
using CleanArch.Domain.Products;

namespace CleanArch.Application.Products.GetProducts;

public record GetProductsQuery : IRequest<Result<List<Product>>> { }

public class GetProductsQueryValidator : AbstractValidator<GetProductsQuery>
{
    public GetProductsQueryValidator() { }
}

public class GetProductsQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetProductsQuery, Result<List<Product>>>
{
    public async Task<Result<List<Product>>> Handle(
        GetProductsQuery request,
        CancellationToken cancellationToken
    )
    {
        return await context.Products.AsNoTracking().ToListAsync(cancellationToken);
    }
}
