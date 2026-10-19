
using Atata.NUnit;
using Atata.PerformanceShowcase.Tests.Domain.Models;
using Atata.PerformanceShowcase.Tests.Pages;
using Atata.PerformanceShowcase.Tests.TestData.DataProviders;
using NUnit.Framework;

namespace Atata.PerformanceShowcase.Tests.Tests;

[TestFixture]
public sealed class ProductsTestA : AtataTestSuite
{
    private const string ScenarioName = "removeArmchair";

    [Test]
    [Description("Test A: remove the configured product and verify the summary with NUnit assertions.")]
    public void RemoveArmchair_VerifySummaryWithNUnit()
    {
        string productName =
            ProductsTestDataProvider.GetProductName(ScenarioName);

        CartSummary expected =
            ProductsTestDataProvider.GetExpectedSummary(ScenarioName);

        ProductsPage page = Go.To<ProductsPage>();
        page.RemoveProduct(productName);

        IReadOnlyList<Product> remainingProducts = page.ReadProducts();
        CartSummary actual = CartSummary.FromProducts(remainingProducts);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(
                remainingProducts.Select(product => product.Name),
                Does.Not.Contain(productName),
                $"'{productName}' should not remain in the products table.");

            Assert.That(actual.TotalPrice, Is.EqualTo(expected.TotalPrice),
                "Total price of remaining products.");

            Assert.That(actual.ProductCount, Is.EqualTo(expected.ProductCount),
                "Number of remaining product rows.");

            Assert.That(actual.TotalAmount, Is.EqualTo(expected.TotalAmount),
                "Total amount of remaining products.");
        }
    }
}
