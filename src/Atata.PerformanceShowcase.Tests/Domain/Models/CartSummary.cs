namespace Atata.PerformanceShowcase.Tests.Domain.Models;

public sealed record CartSummary(decimal? TotalPrice, int ProductCount, decimal? TotalAmount)
{
    public static CartSummary FromProducts(IReadOnlyCollection<Product> products) => new(
        TotalPrice: products.Sum(product => product.Price),
        ProductCount: products.Count,
        TotalAmount: products.Sum(product => product.Amount));
}
