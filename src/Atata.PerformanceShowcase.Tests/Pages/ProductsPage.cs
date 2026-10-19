using Atata;
using Atata.PerformanceShowcase.Tests.Components;
using Atata.PerformanceShowcase.Tests.Domain.Models;

namespace Atata.PerformanceShowcase.Tests.Pages;

using _ = ProductsPage;

[Url("products")]
[WaitForDocumentReadyState]
public sealed class ProductsPage : Page<_>
{
    [FindByClass("table")]
    public Table<ProductRow, _> Products { get; private set; } = null!;

    /// <summary>Deletes a product by its displayed name and waits through the row's trigger configuration.</summary>
    /// <exception cref="InvalidOperationException">Thrown when the requested product is not present.</exception>
    public void RemoveProduct(string productName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(productName);

        ProductRow row = Products.Rows.FirstOrDefault(item => item.Name.Value == productName)
            ?? throw new InvalidOperationException(
                $"Cannot remove product '{productName}' because no matching row exists on the Products page.");

        row.DeleteUsingJsConfirm.Click();
    }

    public IReadOnlyList<Product> ReadProducts() =>
    Products.Rows
        .Select(row => new Product(
            row.Name.Value,
            row.Price.Value,
            row.Amount.Value))
        .ToArray();
}
