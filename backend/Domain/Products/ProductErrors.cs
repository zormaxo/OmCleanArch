namespace CleanArch.Domain.Products;

public static class ProductErrors
{
    public static Error NotFound(int productId) =>
        Error.NotFound(
            "Product.NotFound",
            $"The product with the Id = '{productId}' was not found"
        );
}
